using Microsoft.AspNetCore.Identity;

namespace GymManager.Infrastructure.Identity;

// INFO - konfiguracja Identity - komunikaty poprawności danych
internal class LocalizedIdentityErrorDescriber : IdentityErrorDescriber
{
	private const string Password = "Password";
	private const string Email = "Email";

	// jeśli różne hasła to wyświetli się ten błąd
	public override IdentityError PasswordMismatch() => new()
	{
		Code = Password,
		Description = "Hasło i Potwierdzone hasło są różne."
	};

	public override IdentityError PasswordRequiresDigit() => new()
	{
		Code = Password,
		Description = "Hasło musi zawierać przynajmniej 1 cyfrę."
	};

	public override IdentityError PasswordRequiresLower() => new()
	{
		Code = Password,
		Description = "Hasło musi zawierać przynajmniej 1 małą literę."
	};

	public override IdentityError PasswordRequiresNonAlphanumeric() => new()
	{
		Code = Password,
		Description = "Hasło musi zawierać przynajmniej 1 znak specjalny (np. $, #, & itp.)."
	};

	public override IdentityError PasswordRequiresUpper() => new()
	{
		Code = Password,
		Description = "Hasło musi zawierać przynajmniej 1 dużą literę."
	};

	public override IdentityError PasswordTooShort(int length) => new()
	{
		Code = Password,
		Description = "Hasło musi mieć co najmniej 8 znaków i nie więcej niż 100 znaków."
	};

	public override IdentityError DuplicateUserName(string userName) => new()
	{
		Code = Email,
		Description = "Wybrany email jest już zajęty."
	};
}