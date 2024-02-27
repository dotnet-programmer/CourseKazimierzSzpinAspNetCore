using GymManager.Application.Common.Interfaces;
using GymManager.Application.Dictionaries;
using GymManager.Application.Users.Extensions;
using MediatR;

namespace GymManager.Application.Employees.Queries.GetEmployeeBasics;

public class GetEmployeeBasicsQueryHandler : IRequestHandler<GetEmployeeBasicsQuery, IEnumerable<EmployeeBasicsDto>>
{
	private readonly IUserRoleManagerService _userRoleManagerService;

	public GetEmployeeBasicsQueryHandler(IUserRoleManagerService userRoleManagerService) => _userRoleManagerService = userRoleManagerService;

	public async Task<IEnumerable<EmployeeBasicsDto>> Handle(GetEmployeeBasicsQuery request, CancellationToken cancellationToken) =>
		(await _userRoleManagerService
			.GetUsersInRoleAsync(RolesDict.Pracownik))
			.Select(x => x.ToEmployeeBasicsDto());
}