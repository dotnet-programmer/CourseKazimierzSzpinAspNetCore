// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.Text;
using GymManager.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.WebUtilities;
using Org.BouncyCastle.Ocsp;

namespace GymManager.UI.Areas.Identity.Pages.Account;

public class ConfirmEmailModel(UserManager<ApplicationUser> userManager) : PageModel
{
	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	[TempData]
	public string StatusMessage { get; set; }
	public async Task<IActionResult> OnGetAsync(string userId, string code)
	{
		if (userId == null || code == null)
		{
			return RedirectToPage("/Index");
		}

		var user = await userManager.FindByIdAsync(userId);
		if (user == null)
		{
			return NotFound($"Unable to load user with ID '{userId}'.");
		}

		code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
		//var result = await userManager.ConfirmEmailAsync(user, code);
		//StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
		//return Page();

		await userManager.ConfirmEmailAsync(user, code);

		// TempData[] w ASP.NET to mechanizm służący do przechowywania i przekazywania danych między różnymi akcjami kontrolera (lub między akcjami a widokami) w aplikacji ASP.NET MVC lub Razor Pages.
		// Dane w TempData są dostępne tylko przez jeden cykl żądań (przez jedno przekierowanie) i są usuwane po ich odczytaniu w kolejnym żądaniu, co oznacza, że są one "jednorazowe".  
		// TempData jest słownikiem (kolekcją klucz-wartość), gdzie kluczem jest zazwyczaj ciąg znaków, a wartością dowolny typ obiektu. 
		// TempData jest często używane do przekazywania komunikatów o błędach, sukcesach lub innych informacji, które mają być wyświetlane na stronie po wykonaniu jakiejś akcji. 
		// W rzeczywistości, dane w TempData są przechowywane w sesji użytkownika, ale ich żywotność jest ograniczona do jednego cyklu żądań. 
		TempData["ConfirmedEmail"] = true;

		// przekierowanie do widoku Login
		return Redirect("~/Identity/Account/Login");
	}
}