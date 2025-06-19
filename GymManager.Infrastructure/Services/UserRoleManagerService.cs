using GymManager.Application.Common.Interfaces;
using GymManager.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace GymManager.Infrastructure.Services;

public class UserRoleManagerService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : IUserRoleManagerService
{
	// na podstawie nazwy roli zwróci listę użytkowników należących do tej roli
	public async Task<IEnumerable<ApplicationUser>> GetUsersInRoleAsync(string roleName)
		=> await userManager.GetUsersInRoleAsync(roleName);

	public async Task<IEnumerable<IdentityRole>> GetRolesAsync(string userId)
	{
		List<IdentityRole> roles = [];

		// info o użytkowniku
		var user = await userManager.FindByIdAsync(userId);

		// nazwy ról tego użytkownika
		var roleNames = await userManager.GetRolesAsync(user);

		// uzupełnienie listy informacjami o rolach
		foreach (var role in roleNames)
		{
			roles.Add(await roleManager.FindByNameAsync(role));
		}

		return roles;
	}

	public async Task AddToRoleAsync(string userId, string roleName)
	{
		var user = await userManager.FindByIdAsync(userId);
		var result = await userManager.AddToRoleAsync(user, roleName);
		ThrowIfNotSucceeded(result);
	}

	public async Task RemoveFromRoleAsync(string userId, string roleName)
	{
		var user = await userManager.FindByIdAsync(userId);
		var result = await userManager.RemoveFromRoleAsync(user, roleName);
		ThrowIfNotSucceeded(result);
	}

	private void ThrowIfNotSucceeded(IdentityResult result)
	{
		if (!result.Succeeded)
		{
			throw new Exception(string.Join(". ", result.Errors.Select(x => x.Description)));
		}
	}
}