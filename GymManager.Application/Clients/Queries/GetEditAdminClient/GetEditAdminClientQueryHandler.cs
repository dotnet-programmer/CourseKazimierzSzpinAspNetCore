using GymManager.Application.Common.Interfaces;
using GymManager.Application.Users.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Clients.Queries.GetEditAdminClient;

public class GetEditAdminClientQueryHandler(
	IApplicationDbContext context,
	IRoleManagerService roleManagerService,
	IUserRoleManagerService userRoleManagerService) : IRequestHandler<GetEditAdminClientQuery, EditAdminClientVm>
{
	public async Task<EditAdminClientVm> Handle(GetEditAdminClientQuery request, CancellationToken cancellationToken)
	{
		EditAdminClientVm vm = new()
		{
			Client = (await context.Users
				.Include(x => x.Client)
				.Include(x => x.Address)
				.AsNoTracking()
				.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken))
				.ToEditAdminClientCommand(),

			// wszystkie role z bazy danych
			AvailableRoles = roleManagerService.GetRoles().ToList(),
		};

		vm.Client.RoleIds = (await userRoleManagerService
			.GetRolesAsync(request.UserId))
			.Select(x => x.Id)
			.ToList();

		//// wszystkie role z bazy danych
		//vm.AvailableRoles = roleManagerService.GetRoles().ToList();

		return vm;
	}
}