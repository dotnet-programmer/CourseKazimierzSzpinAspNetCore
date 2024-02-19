using GymManager.Application.Roles.Queries.GetRoles;

namespace GymManager.Application.Common.Interfaces;

// INFO - abstrakcja / nakładka na RoleManager z Identity
public interface IRoleManagerService
{
	IEnumerable<RoleDto> GetRoles();
}