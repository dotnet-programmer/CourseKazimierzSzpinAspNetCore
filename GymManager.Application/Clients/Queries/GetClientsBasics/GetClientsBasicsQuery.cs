using MediatR;

namespace GymManager.Application.Clients.Queries.GetClientsBasics;

public class GetClientsBasicsQuery : IRequest<IEnumerable<ClientBasicsDto>>
{
}