using FluentValidation.Results;
using GymManager.Application.Common.Exceptions;
using GymManager.Application.Common.Interfaces;
using GymManager.Application.Roles.Queries.GetRoles;
using Microsoft.AspNetCore.Identity;

namespace GymManager.Infrastructure.Services;

// INFO - abstrakcja / nakładka na RoleManager z Identity
public class RoleManagerService : IRoleManagerService
{
	private readonly RoleManager<IdentityRole> _roleManager;

	public RoleManagerService(RoleManager<IdentityRole> roleManager) => _roleManager = roleManager;

	public IEnumerable<RoleDto> GetRoles() =>
		_roleManager.Roles
			// zmiana z typu IdentityRole na ty RoleDto, może jakaś metoda rozszerzjąca .ToDto() i na odwrót?
			.Select(x => new RoleDto { Id = x.Id, Name = x.Name })
			.ToList();

	public async Task CreateAsync(string roleName)
	{
		await ValidateRoleName(roleName);

		var result = await _roleManager.CreateAsync(new IdentityRole(roleName));

		if (!result.Succeeded)
		{
			throw new Exception(string.Join(". ", result.Errors.Select(x => x.Description)));
		}
	}

	public async Task UpdateAsync(RoleDto role)
	{
		// pobranie roli która będzie aktualizowana
		var roleDb = await _roleManager.FindByIdAsync(role.Id);

		// jeśli nazwa się zmieniła, to trzeba sprawdzić czy jest poprawna
		if (roleDb.Name != role.Name)
		{
			await ValidateRoleName(role.Name);
		}

		roleDb.Name = role.Name;

		var result = await _roleManager.UpdateAsync(roleDb);

		if (!result.Succeeded)
		{
			throw new Exception(string.Join(". ", result.Errors.Select(x => x.Description)));
		}
	}

	public async Task<RoleDto> FindByIdAsync(string id)
	{
		var role = await _roleManager.FindByIdAsync(id);

		if (role == null)
		{
			throw new Exception($"Brak roli o podanym id: {id}.");
		}

		return new RoleDto { Id = role.Id, Name = role.Name };
	}

	private async Task ValidateRoleName(string roleName)
	{
		if (await _roleManager.RoleExistsAsync(roleName))
		{
			throw new ValidationException(new List<ValidationFailure> { new("Name", $"Rola o nazwie '{roleName}' już istnieje.") });
		}
	}
}