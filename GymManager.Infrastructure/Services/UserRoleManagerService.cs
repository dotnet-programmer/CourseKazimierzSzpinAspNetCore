using GymManager.Application.Common.Interfaces;
using GymManager.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace GymManager.Infrastructure.Services;

public class UserRoleManagerService : IUserRoleManagerService
{
	private readonly UserManager<ApplicationUser> _userManager;

	public UserRoleManagerService(UserManager<ApplicationUser> userManager) => 
		_userManager = userManager;

	// na podstawie nazwy roli zwróci listę użytkowników należących do tej roli
	public async Task<IEnumerable<ApplicationUser>> GetUsersInRoleAsync(string roleName) => 
		await _userManager.GetUsersInRoleAsync(roleName);
}