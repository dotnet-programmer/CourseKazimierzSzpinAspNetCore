using MediatR;

namespace GymManager.Application.Clients.Queries.GetEditAdminClient;

public class GetEditAdminClientQuery : IRequest<EditAdminClientVm>
{
	public string UserId { get; set; }
}