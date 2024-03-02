// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using GymManager.Application.Common.Interfaces;
using GymManager.Application.Dictionaries;
using GymManager.Domain.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace GymManager.UI.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IUserEmailStore<ApplicationUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailService _emailService;
		private readonly IDateTimeService _dateTimeService;
		private readonly IBackgroundWorkerQueue _backgroundWorkerQueue;

		public RegisterModel(
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            SignInManager<ApplicationUser> signInManager,
            ILogger<RegisterModel> logger,
			IEmailService emailService,
			IDateTimeService dateTimeService,
			IBackgroundWorkerQueue backgroundWorkerQueue)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
			_emailService = emailService;
			_dateTimeService = dateTimeService;
			_backgroundWorkerQueue = backgroundWorkerQueue;
		}

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
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

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
			[EmailAddress(ErrorMessage = "Nieprawidłowy adres e-mail.")]
			[Display(Name = "Email")]
			public string Email { get; set; }

			/// <summary>
			///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
			///     directly from your code. This API may change or be removed in future releases.
			/// </summary>
			[Required(ErrorMessage = "Pole 'Hasło' jest wymagane.")]
			[StringLength(100, ErrorMessage = "Hasło musi mieć co najmniej {2} znaków i nie więcej niż {1} znaków długości.", MinimumLength = 8)]
			[DataType(DataType.Password)]
			[Display(Name = "Password")]
			public string Password { get; set; }

			/// <summary>
			///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
			///     directly from your code. This API may change or be removed in future releases.
			/// </summary>
			[DataType(DataType.Password)]
			[Display(Name = "Confirm password")]
			[Compare("Password", ErrorMessage = "Hasło i Potwierdzone hasło są różne.")]
			public string ConfirmPassword { get; set; }
		}

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

		// ta akcja wykona się po kliknięciu "Rejestruj"
		public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            
			// logowanie zewnętrznymi kontami np google czy facebook, tutaj nie będzie tej opcji
			//ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
				// tworzenie użytkownika - nowa instancja ApplicationUser
                var user = CreateUser();
				user.RegisterDateTime = _dateTimeService.Now;

				// ustawianie nazwy użytkownika
                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);

				// ustawianie emaila użytkownika
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

				// _userManager tworzy użytkownika w bazie danych i zwraca jakąś odpowiedź
				var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);

					// każdy użytkownik na starcie ma rolę klienta
					await _userManager.AddToRoleAsync(user, RolesDict.Klient);

					// generowanie kodu potwierdzającego wysyłanego emailem
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
					
					// ustawienie kodowania
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

					 // generowanie linka zwrotnego, który potwierdzi email
					 var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

					// wysłanie emaila z potwierdzeniem konta

					// to jest domyślna wysyłka, ale w tym projekcie jest własny EmailService i z niego będą wysyłane maile
					//await _emailSender.SendEmailAsync(Input.Email, "Confirm your email", $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
					_backgroundWorkerQueue.QueueBackgroundWorkItem(async x =>
						{
							await _emailService.SendAsync(
							"Potwierdź e-mail",
							$"Aby potwierdzić utworzone konto kliknij w link: <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>kliknij tutaj</a>",
							Input.Email);
						},
						$"Aktywacja konta. E-mail: {Input.Email}"
					);

					// jeśli wszystko się powiedzie, to przekierowanie do widoku z info że należy potwierdzić adres email
					return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
