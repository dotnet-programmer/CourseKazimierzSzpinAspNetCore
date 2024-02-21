using MediatR;

namespace GymManager.Application.Clients.Queries.GetClient;

// przekazanie Id zalogowanego użytkownika i na tej podstawie zostaną zwrócone dane tego użytkownika
public class GetClientQuery : IRequest<ClientDto>
{
	public string UserId { get; set; }
}