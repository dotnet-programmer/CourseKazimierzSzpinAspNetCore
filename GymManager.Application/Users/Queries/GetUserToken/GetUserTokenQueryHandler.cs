using GymManager.Application.Common.Interfaces;
using GymManager.Application.Common.Models.Inovices;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Users.Queries.GetUserToken;

// kwerenda do pobierania tokena na podstawie danych przekazanych w requeście
public class GetUserTokenQueryHandler : IRequestHandler<GetUserTokenQuery, AuthenticateResponse>
{
	private readonly IApplicationDbContext _context;
	private readonly IJwtService _jwtService;

	public GetUserTokenQueryHandler(IApplicationDbContext context, IJwtService jwtService)
	{
		_context = context;
		_jwtService = jwtService;
	}

	public async Task<AuthenticateResponse> Handle(GetUserTokenQuery request, CancellationToken cancellationToken)
	{
		// sprawdzenie czy w bazie danych istnieje użytkownik o podanej nazwie i haśle przekazanym w requeście
		var user = await _context
			.Users
			.Select(x => new { x.Id, x.UserName, x.PasswordHash })
			.FirstOrDefaultAsync(x => x.UserName == request.UserName && x.PasswordHash == request.Password);

		// jeśli dane się zgadzają, to zwrócenie nowego tokena
		return user != null ? _jwtService.GenerateJwtToken(user.Id) : null;
	}
}