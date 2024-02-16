namespace GymManager.Application.Common.Interfaces;

// interfejs odpowiedzialny za wysyłkę emaili
public interface IEmailService
{
	// wypełnia pola w klasie danymi pobranymi z serwisu IAppSettingsService
	Task Update(IAppSettingsService appSettingsService);

	// wysłanie wiadomości email, implementacja klasy konkretnej znajduje się w Infrastructure -> Services
	Task SendAsync(string subject, string body, string to, string attachmentPath = null);
}