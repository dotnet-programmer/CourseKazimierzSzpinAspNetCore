using GymManager.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Employees.Queries.GetEmployeePage;

public class GetEmployeePageQueryHandler : IRequestHandler<GetEmployeePageQuery, EmployeePageDto>
{
	private readonly IApplicationDbContext _context;

	public GetEmployeePageQueryHandler(IApplicationDbContext context) => 
		_context = context;

	public async Task<EmployeePageDto> Handle(GetEmployeePageQuery request, CancellationToken cancellationToken)
	{
		var employee = await _context
			.Users
			.AsNoTracking()
			.Include(x => x.Employee)
			.Where(x => x.Employee != null)
			.Select(x => new { IsDeleted = x.IsDeleted, Url = x.Employee.WebsiteUrl, Content = x.Employee.WebsiteRaw })
			// pobierz pierwszy wpis gdzie użytkownik nie jest usunięty i adres url = adres url z requesta (czyli z obiektu Query)
			.FirstOrDefaultAsync(x => !x.IsDeleted && x.Url == request.Url);

		return new EmployeePageDto { Content = employee.Content };
	}
}