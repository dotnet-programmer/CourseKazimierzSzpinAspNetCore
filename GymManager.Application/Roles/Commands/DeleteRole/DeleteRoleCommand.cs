using MediatR;

namespace GymManager.Application.Roles.Commands.DeleteRole;

public class DeleteRoleCommand : IRequest<Unit>
{
	public string Id { get; set; }
}