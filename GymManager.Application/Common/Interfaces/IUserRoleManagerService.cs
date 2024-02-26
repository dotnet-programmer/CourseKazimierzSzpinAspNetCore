using GymManager.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace GymManager.Application.Common.Interfaces;

public interface IUserRoleManagerService
{
	// na podstawie nazwy roli zwróci listę użytkowników należących do tej roli
	Task<IEnumerable<ApplicationUser>> GetUsersInRoleAsync(string roleName);
	Task<IEnumerable<IdentityRole>> GetRolesAsync(string userId);
	Task AddToRoleAsync(string userId, string roleName);
	Task RemoveFromRoleAsync(string userId, string roleName);
}