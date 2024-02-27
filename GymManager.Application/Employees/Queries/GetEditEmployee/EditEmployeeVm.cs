using GymManager.Application.Employees.Commands.EditEmployee;
using GymManager.Application.Roles.Queries.GetRoles;

namespace GymManager.Application.Employees.Queries.GetEditEmployee;

public class EditEmployeeVm
{
	public EditEmployeeCommand Employee { get; set; }
	public List<RoleDto> AvailableRoles { get; set; }
	public string EmployeeFullPathImage { get; set; }
}