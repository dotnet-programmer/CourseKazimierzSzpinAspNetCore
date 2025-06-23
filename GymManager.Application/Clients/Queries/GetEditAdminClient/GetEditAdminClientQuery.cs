using MediatR;

namespace GymManager.Application.Clients.Queries.GetEditAdminClient;

// jeśli admin chce edytować dane jakiegoś klienta to przekazuje Id użyszkodnika i zostanie wyświetlony formularz bazujący na EditAdminClientVm
public class GetEditAdminClientQuery : IRequest<EditAdminClientVm>
{
	public string UserId { get; set; }
}