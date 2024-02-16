using MediatR;

namespace GymManager.Application.Contacts.Commands.SendContactEmail;

// model dla formularza w widoku Contact.cshtml
public class SendContactEmailCommand : IRequest
{
	public string Name { get; set; }
	public string Email { get; set; }
	public string Title { get; set; }
	public string Message { get; set; }
}