﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using GymManager.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace GymManager.UI.Areas.Identity.Pages.Account;

public class LoginModel(SignInManager<ApplicationUser> signInManager, ILogger<LoginModel> logger) : PageModel
{
	private readonly ILogger<LoginModel> _logger = logger;

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	[BindProperty]
	public InputModel Input { get; set; }

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public IList<AuthenticationScheme> ExternalLogins { get; set; }

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public string ReturnUrl { get; set; }

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	[TempData]
	public string ErrorMessage { get; set; }

	/// <summary>
	///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
	///     directly from your code. This API may change or be removed in future releases.
	/// </summary>
	public class InputModel
	{
		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		[Required(ErrorMessage = "Pole 'Adres e-mail' jest wymagane.")]
		[EmailAddress(ErrorMessage = "Nieprawiłowy adres e-mail.")]
		public string Email { get; set; }

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		[Required(ErrorMessage = "Pole 'Hasło' jest wymagane.")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		[Display(Name = "Remember me?")]
		public bool RememberMe { get; set; }
	}

	// wywołanie przy wczytaniu okna
	public async Task OnGetAsync(string returnUrl = null)
	{
		if (!string.IsNullOrEmpty(ErrorMessage))
		{
			ModelState.AddModelError(string.Empty, ErrorMessage);
		}

		returnUrl ??= Url.Content("~/");

		// Clear the existing external cookie to ensure a clean login process
		await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

		ExternalLogins = (await signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

		ReturnUrl = returnUrl;
	}

	// wywołanie po kliknięciu przycisku logowania
	public async Task<IActionResult> OnPostAsync(string returnUrl = null)
	{
		returnUrl ??= Url.Content("~/Client/Dashboard");

		//ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

		if (ModelState.IsValid)
		{
			string incorrectLogin = "Nieprawidłowe dane logowania.";

			// This doesn't count login failures towards account lockout
			// To enable password failures to trigger account lockout, set lockoutOnFailure: true
			var result = await signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);

			if (result.Succeeded)
			{
				var user = await signInManager.UserManager.FindByEmailAsync(Input.Email);
				if (user.IsDeleted)
				{
					await signInManager.SignOutAsync();
					ModelState.AddModelError("Input.Email", incorrectLogin);
					return Page();
				}

				_logger.LogInformation("User logged in.");
				return LocalRedirect(returnUrl);
			}
			//if (result.RequiresTwoFactor)
			//            {
			//                return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
			//            }
			//if (result.IsLockedOut)
			//            {
			//                _logger.LogWarning("User account locked out.");
			//                return RedirectToPage("./Lockout");
			//            }
			// else
			// {
			// obok adresu email będzie taki komunikat o błędzie
			ModelState.AddModelError("Input.Email", incorrectLogin);
			//return Page();
			//}
		}

		// If we got this far, something failed, redisplay form
		return Page();
	}
}