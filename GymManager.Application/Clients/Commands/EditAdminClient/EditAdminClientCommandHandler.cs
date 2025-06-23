using GymManager.Application.Common.Extensions;
using GymManager.Application.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Clients.Commands.EditAdminClient;

public class EditAdminClientCommandHandler(
	IApplicationDbContext context,
	IUserRoleManagerService userRoleManagerService,
	IRoleManagerService roleManagerService
	) : IRequestHandler<EditAdminClientCommand, Unit>
{
	public async Task<Unit> Handle(EditAdminClientCommand request, CancellationToken cancellationToken)
	{
		if (request.IsPrivateAccount)
		{
			request.NipNumber = null;
		}

		var user = await context.Users
			.Include(x => x.Client)
			.Include(x => x.Address)
			.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

		user.FirstName = request.FirstName;
		user.LastName = request.LastName;

		//if (user.Client == null)
		//{
		//	user.Client = new Domain.Entities.Client();
		//}
		user.Client ??= new Domain.Entities.Client();
		user.Client.IsPrivateAccount = request.IsPrivateAccount;
		user.Client.NipNumber = request.NipNumber;
		user.Client.UserId = request.UserId;

		//if (user.Address == null)
		//{
		//	user.Address = new Domain.Entities.Address();
		//}
		user.Address ??= new Domain.Entities.Address();
		user.Address.Country = request.Country;
		user.Address.City = request.City;
		user.Address.Street = request.Street;
		user.Address.ZipCode = request.ZipCode;
		user.Address.StreetNumber = request.StreetNumber;
		user.Address.UserId = request.UserId;

		await context.SaveChangesAsync(cancellationToken);

		if (request.RoleIds != null && request.RoleIds.Any())
		{
			await UpdateRoles(request.RoleIds, request.UserId);
		}

		return Unit.Value;
	}

	private async Task UpdateRoles(List<string> newRoleIds, string userId)
	{
		// pobierz wszystkie role
		var roles = roleManagerService.GetRoles().Select(x => new IdentityRole { Id = x.Id, Name = x.Name });

		// sprawdź stare role użytkownika, czyli które ma obecnie przed zapisaniem do bazy danych
		var oldRoles = await GetOldRoles(userId);

		// sprawdzanie nowych ról
		var newRoles = GetNewRoles(newRoleIds, roles);

		// usunięcie z bazy danych wcześniejszych ról użytkownika, które były a teraz ich nie ma
		await RemoveOldRoles(userId, oldRoles, newRoles);

		// dodaj nowe role, których wcześniej nie było a teraz są
		await AddNewRoles(userId, oldRoles, newRoles);
	}

	private async Task<List<IdentityRole>> GetOldRoles(string userId)
		=> (await userRoleManagerService.GetRolesAsync(userId)).ToList();

	private List<IdentityRole> GetNewRoles(List<string> newRoleIds, IEnumerable<IdentityRole> roles)
	{
		List<IdentityRole> newRoles = [];

		foreach (var roleId in newRoleIds)
		{
			newRoles.Add(new IdentityRole { Id = roleId, Name = roles.FirstOrDefault(x => x.Id == roleId).Name });
		}

		return newRoles;
	}

	private async Task RemoveOldRoles(string userId, List<IdentityRole> oldRoles, List<IdentityRole> newRoles)
	{
		var roleToRemove = oldRoles.Except(newRoles, x => x.Id);

		foreach (var role in roleToRemove)
		{
			await userRoleManagerService.RemoveFromRoleAsync(userId, role.Name);
		}
	}

	private async Task AddNewRoles(string userId, List<IdentityRole> oldRoles, List<IdentityRole> newRoles)
	{
		var roleToAdd = newRoles.Except(oldRoles, x => x.Id);

		foreach (var role in roleToAdd)
		{
			await userRoleManagerService.AddToRoleAsync(userId, role.Name);
		}
	}
}