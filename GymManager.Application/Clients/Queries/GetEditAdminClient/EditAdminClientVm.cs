using GymManager.Application.Clients.Commands.EditAdminClient;
using GymManager.Application.Roles.Queries.GetRoles;

namespace GymManager.Application.Clients.Queries.GetEditAdminClient;

public class EditAdminClientVm
{
	public EditAdminClientCommand Client { get; set; }
	public List<RoleDto> AvailableRoles { get; set; }
}