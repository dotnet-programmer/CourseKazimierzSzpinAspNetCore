using GymManager.Application.Common.Exceptions;
using GymManager.Application.Common.Interfaces;
using GymManager.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace GymManager.Infrastructure.Services;

// nakładka na metody z Identity
public class UserManagerService : IUserManagerService
{
	private readonly UserManager<ApplicationUser> _userManager;
	private readonly IUserStore<ApplicationUser> _userStore;
	private readonly IUserEmailStore<ApplicationUser> _userEmailStore;

	public UserManagerService(UserManager<ApplicationUser> userManager, IUserStore<ApplicationUser> userStore)
	{
		_userManager = userManager;
		_userStore = userStore;
		_userEmailStore = GetEmailStore();
	}

	// metoda tworząca nowego użytkownika w bazie danych, używane metody z Identity
	public async Task<string> CreateAsync(string email, string password, string role)
	{
		ApplicationUser user = new();
		await _userStore.SetUserNameAsync(user, email, CancellationToken.None);
		await _userEmailStore.SetEmailAsync(user, email, CancellationToken.None);
		var result = await _userManager.CreateAsync(user, password);

		if (!result.Succeeded)
		{
			foreach (var item in result.Errors)
			{
				throw new ValidationException([new(item.Code, item.Description)]);
			}
		}

		if (!string.IsNullOrWhiteSpace(role))
		{
			await _userManager.AddToRoleAsync(user, role);
		}

		return await _userManager.GetUserIdAsync(user);
	}

	private IUserEmailStore<ApplicationUser> GetEmailStore() =>
		_userManager.SupportsUserEmail ?
			(IUserEmailStore<ApplicationUser>)_userStore :
			throw new NotSupportedException("The default UI requires a user store with email support");
}