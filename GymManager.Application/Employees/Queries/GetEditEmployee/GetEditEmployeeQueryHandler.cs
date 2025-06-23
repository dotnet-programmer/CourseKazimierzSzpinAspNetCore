using GymManager.Application.Common.Interfaces;
using GymManager.Application.Users.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Employees.Queries.GetEditEmployee;

public class GetEditEmployeeQueryHandler(
	IApplicationDbContext context,
	IRoleManagerService roleManagerService,
	IUserRoleManagerService userRoleManagerService
	) : IRequestHandler<GetEditEmployeeQuery, EditEmployeeVm>
{
	public async Task<EditEmployeeVm> Handle(GetEditEmployeeQuery request, CancellationToken cancellationToken)
	{
		EditEmployeeVm vm = new()
		{
			Employee = (await context.Users
			.Include(x => x.Employee)
			.Include(x => x.Address)
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken))
			.ToEmployee(),

			AvailableRoles = roleManagerService.GetRoles().ToList(),
		};

		vm.Employee.RoleIds = (await userRoleManagerService
			.GetRolesAsync(request.UserId))
			.Select(x => x.Id)
			.ToList();

		if (!string.IsNullOrWhiteSpace(vm.Employee.ImageUrl))
		{
			vm.EmployeeFullPathImage = $"/Content/Files/{vm.Employee.ImageUrl}";
		}

		return vm;
	}
}