using MediatR;

namespace GymManager.Application.Roles.Queries.GetRoles;

// w <> typ zwracany, będzie zwracana kolekcja elementów, dlatego IEnumerable<RoleDto>
public class GetRolesQuery : IRequest<IEnumerable<RoleDto>>
{
	// brak parametrów, bo zwracanie wszystkich elementów w tabeli
}