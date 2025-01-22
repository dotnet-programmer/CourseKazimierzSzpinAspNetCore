using GymManager.Application.Roles.Queries.GetRoles;

namespace GymManager.Application.Common.Interfaces;

// abstrakcja / nakładka na RoleManager z Identity
public interface IRoleManagerService
{
	IEnumerable<RoleDto> GetRoles();
	Task CreateAsync(string roleName);
	Task UpdateAsync(RoleDto role);
	Task<RoleDto> FindByIdAsync(string id);
	Task DeleteAsync(string id);
}