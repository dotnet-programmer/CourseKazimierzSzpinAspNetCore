using FluentValidation;

namespace GymManager.Application.Contacts.Commands.SendContactEmail;

// dodawanie nowej walidacji danych
// wskazanie, że tutaj będą zasady walidacyjne do pól z komendy SendContactEmailCommand
public class SendContactEmailCommandValidator : AbstractValidator<SendContactEmailCommand>
{
	// wzięte w komentarz, bo stosowana inna walidacja - DataAnnotation
	//// w konstruktorze tworzy się różne zasady dla różnych pól
	//public SendContactEmailCommandValidator()
	//{
	//	RuleFor(x => x.Name)
	//		.NotEmpty()
	//		.WithMessage("Pole 'Imię i Nazwisko' jest wymagane.");
	//}
}