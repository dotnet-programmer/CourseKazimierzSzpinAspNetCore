using MediatR;

namespace GymManager.Application.Roles.Commands.DeleteRole;

public class DeleteRoleCommand : IRequest
{
	public string Id { get; set; }
}