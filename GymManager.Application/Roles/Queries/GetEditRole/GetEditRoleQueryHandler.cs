using GymManager.Application.Common.Interfaces;
using GymManager.Application.Roles.Commands.EditRole;
using MediatR;

namespace GymManager.Application.Roles.Queries.GetEditRole;

public class GetEditRoleQueryHandler(IRoleManagerService roleManagerService) : IRequestHandler<GetEditRoleQuery, EditRoleCommand>
{
	public async Task<EditRoleCommand> Handle(GetEditRoleQuery request, CancellationToken cancellationToken)
	{
		var role = await roleManagerService.FindByIdAsync(request.Id);
		return new EditRoleCommand { Id = role.Id, Name = role.Name };
	}
}