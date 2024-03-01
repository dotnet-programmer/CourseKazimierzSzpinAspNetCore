using GymManager.Application.Common.Models.Inovices;
using MediatR;

namespace GymManager.Application.Users.Queries.GetUserToken;

// kwerenda do pobierania tokena na podstawie danych przekazanych w requeście
// przekazywane nazwa usera i zaszyfrowane hasło
// zwracany token w klasie AuthenticateResponse
public class GetUserTokenQuery : IRequest<AuthenticateResponse>
{
	public string UserName { get; set; }
	public string Password { get; set; }
}