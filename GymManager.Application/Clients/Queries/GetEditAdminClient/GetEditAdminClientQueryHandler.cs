using GymManager.Application.Common.Interfaces;
using GymManager.Application.Users.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Clients.Queries.GetEditAdminClient;

public class GetEditAdminClientQueryHandler : IRequestHandler<GetEditAdminClientQuery, EditAdminClientVm>
{
	private readonly IApplicationDbContext _context;
	private readonly IRoleManagerService _roleManagerService;
	private readonly IUserRoleManagerService _userRoleManagerService;

	public GetEditAdminClientQueryHandler(
		IApplicationDbContext context,
		IRoleManagerService roleManagerService,
		IUserRoleManagerService userRoleManagerService)
	{
		_context = context;
		_roleManagerService = roleManagerService;
		_userRoleManagerService = userRoleManagerService;
	}

	public async Task<EditAdminClientVm> Handle(GetEditAdminClientQuery request, CancellationToken cancellationToken)
	{
		EditAdminClientVm vm = new()
		{
			Client = (await _context.Users
			.Include(x => x.Client)
			.Include(x => x.Address)
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Id == request.UserId))
			.ToEditAdminClientCommand()
		};

		vm.Client.RoleIds = (await _userRoleManagerService
			.GetRolesAsync(request.UserId))
			.Select(x => x.Id).ToList();

		// wszystkie role z bazy danych
		vm.AvailableRoles = _roleManagerService.GetRoles().ToList();

		return vm;
	}
}