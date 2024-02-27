using GymManager.Application.Common.Interfaces;
using GymManager.Application.Dictionaries;
using GymManager.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Employees.Commands.AddEmployee;

public class AddEmployeeCommandHandler : IRequestHandler<AddEmployeeCommand>
{
	private readonly IApplicationDbContext _context;
	private readonly IUserManagerService _userManagerService;
	private readonly IDateTimeService _dateTimeService;

	public AddEmployeeCommandHandler(
		IApplicationDbContext context,
		IUserManagerService userManagerService,
		IDateTimeService dateTimeService)
	{
		_context = context;
		_userManagerService = userManagerService;
		_dateTimeService = dateTimeService;
	}

	public async Task Handle(AddEmployeeCommand request, CancellationToken cancellationToken)
	{
		var userId = await _userManagerService.CreateAsync(request.Email, request.Password, RolesDict.Pracownik);

		var user = await _context.Users
			.Include(x => x.Employee)
			.Include(x => x.Address)
			.FirstOrDefaultAsync(x => x.Id == userId);

		user.FirstName = request.FirstName;
		user.LastName = request.LastName;
		user.RegisterDateTime = _dateTimeService.Now;
		user.EmailConfirmed = true;

		user.Employee = new Domain.Entities.Employee
		{
			UserId = userId,
			Salary = request.Salary,
			ImageUrl = request.ImageUrl,
			DateOfEmployment = request.DateOfEmployment,
			Position = (Position)request.PositionId,
			WebsiteRaw = request.WebsiteRaw,
			WebsiteUrl = request.WebsiteUrl
		};

		user.Address = new Domain.Entities.Address
		{
			Country = request.Country,
			City = request.City,
			Street = request.Street,
			ZipCode = request.ZipCode,
			StreetNumber = request.StreetNumber,
			UserId = userId
		};

		await _context.SaveChangesAsync(cancellationToken);
		return;
	}
}