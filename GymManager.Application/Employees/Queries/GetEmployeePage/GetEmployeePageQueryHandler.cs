using GymManager.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Employees.Queries.GetEmployeePage;

public class GetEmployeePageQueryHandler(IApplicationDbContext context) : IRequestHandler<GetEmployeePageQuery, EmployeePageDto>
{
	public async Task<EmployeePageDto> Handle(GetEmployeePageQuery request, CancellationToken cancellationToken)
	{
		var employee = await context.Users
			.AsNoTracking()
			.Include(x => x.Employee)
			.Where(x => x.Employee != null)
			.Select(x => new { IsDeleted = x.IsDeleted, Url = x.Employee.WebsiteUrl, Content = x.Employee.WebsiteRaw })
			// pobierz pierwszy wpis gdzie użytkownik nie jest usunięty i adres url = adres url z requesta (czyli z obiektu Query)
			.FirstOrDefaultAsync(x => !x.IsDeleted && x.Url == request.Url, cancellationToken);

		return new EmployeePageDto { Content = employee.Content };
	}
}