using GymManager.Application.Common.Interfaces;
using GymManager.Application.Dictionaries;
using GymManager.Application.Users.Extensions;
using MediatR;

namespace GymManager.Application.Employees.Queries.GetEmployeeBasics;

public class GetEmployeeBasicsQueryHandler(IUserRoleManagerService userRoleManagerService) : IRequestHandler<GetEmployeeBasicsQuery, IEnumerable<EmployeeBasicsDto>>
{
	public async Task<IEnumerable<EmployeeBasicsDto>> Handle(GetEmployeeBasicsQuery request, CancellationToken cancellationToken) =>
		(await userRoleManagerService
			.GetUsersInRoleAsync(RolesDict.Employee))
			.Select(x => x.ToEmployeeBasicsDto());
}