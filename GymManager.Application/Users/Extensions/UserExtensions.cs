using GymManager.Application.Clients.Commands.EditAdminClient;
using GymManager.Application.Clients.Commands.EditClient;
using GymManager.Application.Clients.Queries.GetClient;
using GymManager.Application.Clients.Queries.GetClientsBasics;
using GymManager.Application.Employees.Commands.EditEmployee;
using GymManager.Application.Employees.Queries.GetEmployeeBasics;
using GymManager.Domain.Entities;
using GymManager.Domain.Enums;

namespace GymManager.Application.Users.Extensions;

public static class UserExtensions
{
	public static ClientDto ToClientDto(this ApplicationUser user) 
		=> user == null ?
		null :
		new ClientDto
		{
			Id = user.Id,
			City = user.Address?.City,
			Country = user.Address?.Country,
			Street = user.Address?.Street,
			StreetNumber = user.Address?.StreetNumber,
			ZipCode = user.Address?.ZipCode,
			Email = user.Email,
			FirstName = user.FirstName,
			LastName = user.LastName,
			NipNumber = user.Client?.NipNumber,
			IsPrivateAccount = user.Client?.IsPrivateAccount ?? true
		};

	public static EditClientCommand ToEditClientCommand(this ApplicationUser user) 
		=> user == null ?
		null :
		new EditClientCommand
		{
			UserId = user.Id,
			City = user.Address?.City,
			Country = user.Address?.Country,
			Street = user.Address?.Street,
			StreetNumber = user.Address?.StreetNumber,
			ZipCode = user.Address?.ZipCode,
			Email = user.Email,
			FirstName = user.FirstName,
			LastName = user.LastName,
			NipNumber = user.Client?.NipNumber,
			IsPrivateAccount = user.Client?.IsPrivateAccount ?? true
		};

	public static ClientBasicsDto ToClientBasicsDto(this ApplicationUser user) 
		=> user == null ?
		null :
		new ClientBasicsDto
		{
			Id = user.Id,
			Email = user.Email,
			Name = !string.IsNullOrWhiteSpace(user.FirstName) || !string.IsNullOrWhiteSpace(user.LastName) ?
				$"{user.FirstName} {user.LastName}" :
				"-",
			IsDeleted = user.IsDeleted
		};

	public static EditAdminClientCommand ToEditAdminClientCommand(this ApplicationUser user)
		=> user == null
			? null
			: new EditAdminClientCommand
			{
				UserId = user.Id,
				City = user.Address?.City,
				Country = user.Address?.Country,
				Street = user.Address?.Street,
				StreetNumber = user.Address?.StreetNumber,
				ZipCode = user.Address?.ZipCode,
				Email = user.Email,
				FirstName = user.FirstName,
				LastName = user.LastName,
				NipNumber = user.Client?.NipNumber,
				IsPrivateAccount = user.Client?.IsPrivateAccount ?? true
			};

	public static EmployeeBasicsDto ToEmployeeBasicsDto(this ApplicationUser user)
		=> user == null
			? null
			: new EmployeeBasicsDto
			{
				Id = user.Id,
				Email = user.Email,
				Name = !string.IsNullOrWhiteSpace(user.FirstName) || !string.IsNullOrWhiteSpace(user.LastName) ?
					$"{user.FirstName} {user.LastName}"
					: "-",
				IsDeleted = user.IsDeleted
			};

	public static EditEmployeeCommand ToEmployee(this ApplicationUser user)
		=> user == null
			? null
			: new EditEmployeeCommand
			{
				City = user.Address?.City,
				Country = user.Address?.Country,
				Street = user.Address?.Street,
				StreetNumber = user.Address?.StreetNumber,
				ZipCode = user.Address?.ZipCode,
				Email = user.Email,
				FirstName = user.FirstName,
				LastName = user.LastName,
				Id = user.Id,
				DateOfDismissal = user.Employee?.DateOfDismissal,
				DateOfEmployment = user.Employee?.DateOfEmployment ?? DateTime.MinValue,
				ImageUrl = user.Employee?.ImageUrl,
				PositionId = (int?)user.Employee?.Position ?? (int)Position.Receptionist,
				Salary = user.Employee?.Salary ?? 0,
				WebsiteRaw = user.Employee?.WebsiteRaw,
				WebsiteUrl = user.Employee?.WebsiteUrl
			};
}