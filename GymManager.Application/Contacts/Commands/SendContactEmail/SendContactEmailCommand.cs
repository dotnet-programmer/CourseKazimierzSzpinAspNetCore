using System.ComponentModel.DataAnnotations;
using MediatR;

namespace GymManager.Application.Contacts.Commands.SendContactEmail;

// model dla formularza w widoku Contact.cshtml
public class SendContactEmailCommand : IRequest<Unit>
{
	// walidacja danych za pomocą DataAnnotation
	[Required(ErrorMessage = "Pole 'Imię i Nazwisko' jest wymagane.")]
	public string Name { get; set; }

	[Required(ErrorMessage = "Pole 'Adres e-mail' jest wymagane.")]
	[EmailAddress(ErrorMessage = "Pole 'Adres e-mail' nie jest prawidłowym adresem e-mail")]
	public string Email { get; set; }

	[Required(ErrorMessage = "Pole 'Tytuł wiadomości' jest wymagane.")]
	public string Title { get; set; }

	[Required(ErrorMessage = "Pole 'Wiadomość' jest wymagane.")]
	public string Message { get; set; }

	// właściwość dla ReCaptcha
	public string AntySpamResult { get; set; }
}