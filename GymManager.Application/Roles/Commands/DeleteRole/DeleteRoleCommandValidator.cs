using FluentValidation;
using GymManager.Application.Common.Interfaces;

namespace GymManager.Application.Roles.Commands.DeleteRole;

public class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
{
	private readonly IRoleManagerService _roleManagerService;
	private readonly IUserRoleManagerService _userRoleManagerService;

	public DeleteRoleCommandValidator(IRoleManagerService roleManagerService, IUserRoleManagerService userRoleManagerService)
	{
		_roleManagerService = roleManagerService;
		_userRoleManagerService = userRoleManagerService;

		RuleFor(x => x.Id)
			.NotEmpty()
			.MustAsync(BeEmptyRole)
			.WithMessage("Nie można usunąć wybranej roli, ponieważ są do niej przypisani użytkownicy. Jeżeli chcesz usunąć wybraną rolę, to najpierw wypisz z niej wszystkich użytkowników.");
	}

	private async Task<bool> BeEmptyRole(string id, CancellationToken cancellationToken)
	{
		var roleName = (await _roleManagerService.FindByIdAsync(id)).Name;
		var usersInRole = await _userRoleManagerService.GetUsersInRoleAsync(roleName);

		// jeżeli tutaj jest przypisany jakoś użytkownik do tej roli to zwrócony false i wyświetli się błąd walidacji
		return !usersInRole.Any();
	}
}