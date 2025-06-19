using GymManager.Application.Common.Exceptions;
using GymManager.Application.Common.Interfaces;
using GymManager.Application.Roles.Queries.GetRoles;
using Microsoft.AspNetCore.Identity;

namespace GymManager.Infrastructure.Services;

// abstrakcja / nakładka na RoleManager z Identity

// walidacja przeniesiona do klasy DeleteRoleCommandValidator w warstwie Application obok komend, dlatego IUserRoleManagerService już nie jest tutaj potrzebne
//public class RoleManagerService(RoleManager<IdentityRole> roleManager, IUserRoleManagerService userRoleManagerService) : IRoleManagerService
public class RoleManagerService(RoleManager<IdentityRole> roleManager) : IRoleManagerService
{
	public IEnumerable<RoleDto> GetRoles() 
		=> roleManager.Roles
			.Select(x => new RoleDto { Id = x.Id, Name = x.Name })
			.ToList();

	public async Task CreateAsync(string roleName)
	{
		await ValidateRoleNameAsync(roleName);
		var result = await roleManager.CreateAsync(new IdentityRole(roleName));
		ThrowIfNotSucceeded(result);
	}

	public async Task UpdateAsync(RoleDto role)
	{
		// pobranie roli która będzie aktualizowana
		var roleDb = await roleManager.FindByIdAsync(role.Id);

		// jeśli nazwa się zmieniła, to trzeba sprawdzić czy jest poprawna
		if (roleDb.Name != role.Name)
		{
			await ValidateRoleNameAsync(role.Name);
		}

		roleDb.Name = role.Name;
		var result = await roleManager.UpdateAsync(roleDb);
		ThrowIfNotSucceeded(result);
	}

	public async Task<RoleDto> FindByIdAsync(string id)
	{
		var role = await roleManager.FindByIdAsync(id);

		return role == null ?
			throw new Exception($"Brak roli o podanym id: {id}.") :
			new RoleDto { Id = role.Id, Name = role.Name };
	}

	public async Task DeleteAsync(string id)
	{
		var roleFromDbToDelete = await roleManager.FindByIdAsync(id);

		// walidacja przeniesiona do klasy DeleteRoleCommandValidator
		//await ValidateRoleToDelete(roleDb.Name);

		var result = await roleManager.DeleteAsync(roleFromDbToDelete);
		ThrowIfNotSucceeded(result);
	}

	private async Task ValidateRoleNameAsync(string roleName)
	{
		if (await roleManager.RoleExistsAsync(roleName))
		{
			throw new ValidationException([new("Name", $"Rola o nazwie '{roleName}' już istnieje.")]);
		}
	}

	private void ThrowIfNotSucceeded(IdentityResult result)
	{
		if (!result.Succeeded)
		{
			throw new Exception(string.Join(". ", result.Errors.Select(x => x.Description)));
		}
	}

	// walidacja przeniesiona do klasy DeleteRoleCommandValidator w warstwie Application obok komend
	//private async Task ValidateRoleToDelete(string roleName)
	//{
	//	var userInRole = await userRoleManagerService.GetUsersInRoleAsync(roleName);
	//	if (userInRole.Any())
	//	{
	//		throw new ValidationException(new List<ValidationFailure> 
	//		{ 
	//			new("Name", $"Nie można usunąć wybranej roli, ponieważ są do niej przypisani użytkownicy. Jeżeli chcesz usunąć wybraną rolę, to najpierw wypisz z niej wszystkich użytkowników.") 
	//		});
	//	}
	//}
}