using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManager.Application.Common.Interfaces;
using GymManager.Application.Roles.Queries.GetRoles;
using Microsoft.AspNetCore.Identity;

namespace GymManager.Infrastructure.Services;

// INFO - abstrakcja / nakładka na RoleManager z Identity
public class RoleManagerService : IRoleManagerService
{
	private readonly RoleManager<IdentityRole> _roleManager;

	public RoleManagerService(RoleManager<IdentityRole> roleManager)
	{
		_roleManager = roleManager;
	}

	public IEnumerable<RoleDto> GetRoles() => 
		_roleManager.Roles
		// zmiana z typu IdentityRole na ty RoleDto, może jakaś metoda rozszerzjąca .ToDto() i na odwrót?
			.Select(x => new RoleDto { Id = x.Id, Name = x.Name })
			.ToList();
}