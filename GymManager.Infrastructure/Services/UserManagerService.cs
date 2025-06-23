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

	// ten interfejs nie zostanie poprawnie wstrzyknięty, dlatego nie ma go w kontruktorze, 
	// logika odpowiedzialna za EmailStore jest w metodzie GetEmailStore()
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
		ApplicationUser newUser = new();
		await _userStore.SetUserNameAsync(newUser, email, CancellationToken.None);
		await _userEmailStore.SetEmailAsync(newUser, email, CancellationToken.None);
		var result = await _userManager.CreateAsync(newUser, password);

		if (!result.Succeeded)
		{
			foreach (var item in result.Errors)
			{
				throw new ValidationException([new(item.Code, item.Description)]);
			}
		}

		if (!string.IsNullOrWhiteSpace(role))
		{
			await _userManager.AddToRoleAsync(newUser, role);
		}

		return await _userManager.GetUserIdAsync(newUser);
	}

	private IUserEmailStore<ApplicationUser> GetEmailStore()
		=> _userManager.SupportsUserEmail ?
			(IUserEmailStore<ApplicationUser>)_userStore :
			throw new NotSupportedException("The default UI requires a user store with email support");
}