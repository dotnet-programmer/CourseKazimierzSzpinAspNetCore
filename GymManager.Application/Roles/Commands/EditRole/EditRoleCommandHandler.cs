using GymManager.Application.Common.Interfaces;
using GymManager.Application.Roles.Queries.GetRoles;
using MediatR;

namespace GymManager.Application.Roles.Commands.EditRole;

public class EditRoleCommandHandler(IRoleManagerService roleManagerService) : IRequestHandler<EditRoleCommand, Unit>
{
	public async Task<Unit> Handle(EditRoleCommand request, CancellationToken cancellationToken)
	{
		await roleManagerService.UpdateAsync(new RoleDto { Id = request.Id, Name = request.Name });
		return Unit.Value;
	}
}