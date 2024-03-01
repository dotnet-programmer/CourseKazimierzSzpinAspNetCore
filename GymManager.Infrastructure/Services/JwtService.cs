using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GymManager.Application.Common.Interfaces;
using GymManager.Application.Common.Models.Inovices;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GymManager.Infrastructure.Services;

// JWT - klasa do generowania tokena
// token będzie generowany w warstwie application
public class JwtService : IJwtService
{
	private readonly IConfiguration _configuration;
	private readonly IDateTimeService _dateTimeService;

	public JwtService(IConfiguration configuration, IDateTimeService dateTimeService)
	{
		_configuration = configuration;
		_dateTimeService = dateTimeService;
	}

	public AuthenticateResponse GenerateJwtToken(string userId)
	{
		// pobranie klucza z ustawień
		var key = Encoding.ASCII.GetBytes(_configuration.GetSection("Secret").Value);

		// lista wartości które będą przetrzymywane jako dane w tokenie
		var authClaims = new List<Claim>
		{
			// id użytkownika
			new(ClaimTypes.NameIdentifier, userId),

			// unikalny Id
			new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
		};

		// ustawienie klucza szyfrującego
		var authSigningKey = new SymmetricSecurityKey(key);

		// generowanie tokena
		JwtSecurityToken token = new(
			// data wygaśnięcia - ustawienie jak długo ma być wważny dany token
			expires: _dateTimeService.Now.AddDays(1),

			// dane przechowywane w tokenie
			claims: authClaims,

			// informacje o algorytmie szyfrującym
			signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
			);

		// zwrócenie tokena opakowanego w klasę AuthenticateResponse
		return new AuthenticateResponse { Token = new JwtSecurityTokenHandler().WriteToken(token) };
	}
}