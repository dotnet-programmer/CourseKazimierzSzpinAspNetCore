using MediatR;

namespace GymManager.Application.Employees.Queries.GetEmployeeBasics;

public class GetEmployeeBasicsQuery : IRequest<IEnumerable<EmployeeBasicsDto>>
{
}