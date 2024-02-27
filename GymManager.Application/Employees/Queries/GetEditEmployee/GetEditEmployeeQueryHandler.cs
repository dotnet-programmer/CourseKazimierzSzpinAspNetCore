using GymManager.Application.Common.Interfaces;
using GymManager.Application.Users.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Employees.Queries.GetEditEmployee;

public class GetEditEmployeeQueryHandler : IRequestHandler<GetEditEmployeeQuery, EditEmployeeVm>
{
	private readonly IApplicationDbContext _context;
	private readonly IRoleManagerService _roleManagerService;
	private readonly IUserRoleManagerService _userRoleManagerService;

	public GetEditEmployeeQueryHandler(
		IApplicationDbContext context,
		IRoleManagerService roleManagerService,
		IUserRoleManagerService userRoleManagerService)
	{
		_context = context;
		_roleManagerService = roleManagerService;
		_userRoleManagerService = userRoleManagerService;
	}

	public async Task<EditEmployeeVm> Handle(GetEditEmployeeQuery request, CancellationToken cancellationToken)
	{
		EditEmployeeVm vm = new()
		{
			Employee = (await _context.Users
			.Include(x => x.Employee)
			.Include(x => x.Address)
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Id == request.UserId))
			.ToEmployee()
		};

		vm.Employee.RoleIds = (await _userRoleManagerService
			.GetRolesAsync(request.UserId))
			.Select(x => x.Id).ToList();

		vm.AvailableRoles = _roleManagerService.GetRoles().ToList();

		if (!string.IsNullOrWhiteSpace(vm.Employee.ImageUrl))
		{
			vm.EmployeeFullPathImage = $"/Content/Files/{vm.Employee.ImageUrl}";
		}

		return vm;
	}
}