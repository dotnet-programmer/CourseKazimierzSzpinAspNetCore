using GymManager.Application.Common.Interfaces;
using GymManager.Application.Roles.Commands.EditRole;
using MediatR;

namespace GymManager.Application.Roles.Queries.GetEditRole;

public class GetEditRoleQueryHandler : IRequestHandler<GetEditRoleQuery, EditRoleCommand>
{
	private readonly IRoleManagerService _roleManagerService;

	public GetEditRoleQueryHandler(IRoleManagerService roleManagerService) => _roleManagerService = roleManagerService;

	public async Task<EditRoleCommand> Handle(GetEditRoleQuery request, CancellationToken cancellationToken)
	{
		var role = await _roleManagerService.FindByIdAsync(request.Id);
		return new EditRoleCommand { Id = role.Id, Name = role.Name };
	}
}