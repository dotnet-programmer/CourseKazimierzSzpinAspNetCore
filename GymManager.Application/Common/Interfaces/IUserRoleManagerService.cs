using GymManager.Domain.Entities;

namespace GymManager.Application.Common.Interfaces;

public interface IUserRoleManagerService
{
	// na podstawie nazwy roli zwróci listę użytkowników należących do tej roli
	Task<IEnumerable<ApplicationUser>> GetUsersInRoleAsync(string roleName);
}