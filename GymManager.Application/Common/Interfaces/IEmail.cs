namespace GymManager.Application.Common.Interfaces;

// interfejs odpowiedzialny za wysyłkę emaili
public interface IEmail
{
	// wysłanie wiadomości email, implementacja klasy konkretnej znajduje się w Infrastructure -> Services
	Task SendAsync(string subject, string body, string to, string attachmentPath = null);
}