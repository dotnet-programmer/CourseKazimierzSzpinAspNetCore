using GymManager.Application.Common.Interfaces;
using MediatR;

namespace GymManager.Application.Roles.Commands.DeleteRole;

public class DeleteRoleCommandHandler(IRoleManagerService roleManagerService) : IRequestHandler<DeleteRoleCommand, Unit>
{
	public async Task<Unit> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
	{
		await roleManagerService.DeleteAsync(request.Id);
		return Unit.Value;
	}
}