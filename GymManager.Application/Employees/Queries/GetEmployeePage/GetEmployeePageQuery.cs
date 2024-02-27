using MediatR;

namespace GymManager.Application.Employees.Queries.GetEmployeePage;

// tutaj opisane jakie parametry są przyjmowane i jaki typ zwracany (TResponse)
public class GetEmployeePageQuery : IRequest<EmployeePageDto>
{
	public string Url { get; set; }
}