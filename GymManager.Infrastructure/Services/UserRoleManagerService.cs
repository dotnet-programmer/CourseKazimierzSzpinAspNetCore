using GymManager.Application.Common.Interfaces;
using GymManager.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace GymManager.Infrastructure.Services;

public class UserRoleManagerService : IUserRoleManagerService
{
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly RoleManager<IdentityRole> _roleManager;

	public UserRoleManagerService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
	{
		_userManager = userManager;
		_roleManager = roleManager;
	}

	// na podstawie nazwy roli zwróci listę użytkowników należących do tej roli
	public async Task<IEnumerable<ApplicationUser>> GetUsersInRoleAsync(string roleName) => 
		await _userManager.GetUsersInRoleAsync(roleName);

	public async Task<IEnumerable<IdentityRole>> GetRolesAsync(string userId)
	{
		List<IdentityRole> roles = [];
		// info o użytkowniku
		var user = await _userManager.FindByIdAsync(userId);
		// nazwy ról tego użytkownika
		var roleNames = await _userManager.GetRolesAsync(user);
		// uzupełnienie listy informacjami o rolach
		foreach (var role in roleNames)
		{
			roles.Add(await _roleManager.FindByNameAsync(role));
		}

		return roles;
	}

	public async Task AddToRoleAsync(string userId, string roleName)
	{
		var user = await _userManager.FindByIdAsync(userId);
		var result = await _userManager.AddToRoleAsync(user, roleName);

		if (!result.Succeeded)
		{
			throw new Exception(string.Join(". ", result.Errors.Select(x => x.Description)));
		}
	}

	public async Task RemoveFromRoleAsync(string userId, string roleName)
	{
		var user = await _userManager.FindByIdAsync(userId);
		var result = await _userManager.RemoveFromRoleAsync(user, roleName);

		if (!result.Succeeded)
		{
			throw new Exception(string.Join(". ", result.Errors.Select(x => x.Description)));
		}
	}
}