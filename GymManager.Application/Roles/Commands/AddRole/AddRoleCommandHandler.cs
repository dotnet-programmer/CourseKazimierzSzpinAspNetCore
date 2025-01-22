using GymManager.Application.Common.Interfaces;
using MediatR;

namespace GymManager.Application.Roles.Commands.AddRole;

public class AddRoleCommandHandler(IRoleManagerService roleManagerService) : IRequestHandler<AddRoleCommand>
{
	public async Task Handle(AddRoleCommand request, CancellationToken cancellationToken) => 
		await roleManagerService.CreateAsync(request.Name);
}