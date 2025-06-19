using GymManager.Application.Common.Interfaces;
using MediatR;

namespace GymManager.Application.Roles.Queries.GetRoles;

// <1, 2> - 1 to typ parametru (czyli ta sama nazwa bez Handle) + typ zwracany, a 2 to typ zwracany
// zarządzanie rolami jest w Identity, czyli jest już dostęp do wszystkich funkcji związanych z rolami
public class GetRolesQueryHandler(IRoleManagerService roleManagerService) : IRequestHandler<GetRolesQuery, IEnumerable<RoleDto>>
{
	// zarządzanie rolami jest w Identity, czyli jest już dostęp do wszystkich funkcji związanych z rolami
	// żeby nie używać tych metod bezpośrednio z RoleManager została stworzona abstrakcja i serwis
	public async Task<IEnumerable<RoleDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken) 
		=> roleManagerService.GetRoles();
}