namespace GymManager.Application.Employees.Queries.GetEmployeeBasics;

public class EmployeeBasicsDto
{
	public string Id { get; set; }
	public string Email { get; set; }
	public string Name { get; set; }
	public bool IsDeleted { get; set; }
}