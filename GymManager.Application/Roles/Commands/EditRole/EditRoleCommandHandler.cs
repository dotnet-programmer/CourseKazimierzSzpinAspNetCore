using GymManager.Application.Common.Interfaces;
using GymManager.Application.Roles.Queries.GetRoles;
using MediatR;

namespace GymManager.Application.Roles.Commands.EditRole;

public class EditRoleCommandHandler : IRequestHandler<EditRoleCommand>
{
	private readonly IRoleManagerService _roleManagerService;

	public EditRoleCommandHandler(IRoleManagerService roleManagerService) => _roleManagerService = roleManagerService;

	public async Task Handle(EditRoleCommand request, CancellationToken cancellationToken)
	{
		await _roleManagerService.UpdateAsync(new RoleDto { Id = request.Id, Name = request.Name });

		return;
	}
}