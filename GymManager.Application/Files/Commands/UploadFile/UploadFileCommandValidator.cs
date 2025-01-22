using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace GymManager.Application.Files.Commands.UploadFile;

// przez to, że modelem jest lista obiektów IFormFile, trzeba zdefiniować osobną klasę, która będzie walidować osobno każdy typ
public class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
{
	public UploadFileCommandValidator() => 
		RuleForEach(x => x.Files).SetValidator(new IFormValidator());
}

public class IFormValidator : AbstractValidator<IFormFile>
{
	private readonly string[] _extensions = [".PDF", ".JPG", ".PNG", ".JPEG", ".ICO"];

	public IFormValidator()
	{
		// wielkość ustawiana w bajtach, 2mb to ok. 2 000 000 bajtów
		RuleFor(x => x.Length)
			.LessThan(2000000).WithMessage("Wybrany plik jest zbyt duży");

		RuleFor(x => x.FileName)
			.Must(ValidName).WithMessage("Nieprawidłowa nazwa pliku")
			.Must(ValidExtensions).WithMessage("Nieprawidłowe rozszerzenie pliku")
			.Must(x => x.Length < 200).WithMessage("Zbyt długa nazwa pliku");
	}

	private bool ValidName(string fileName)
	{
		var dotCount = fileName.Where(x => x == '.').Count();

		if (dotCount > 1 || fileName.Contains('\\') || fileName.Contains('/') || fileName.Contains(':') || fileName.Contains(' '))
		{
			return false;
		}

		return true;
	}

	private bool ValidExtensions(string fileName) => 
		_extensions.Contains(Path.GetExtension(fileName).ToUpper());
}