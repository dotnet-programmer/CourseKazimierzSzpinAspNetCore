using GymManager.Application.Common.Interfaces;
using GymManager.Application.Dictionaries;
using MediatR;

namespace GymManager.Application.Contacts.Commands.SendContactEmail;

public class SendContactEmailCommandHandler : IRequestHandler<SendContactEmailCommand>
{
	private readonly IEmail _email;
	private readonly IAppSettingsService _appSettings;

	// wstrzyknięcie klasy Email odpowiedzialnej za wysyłkę emaili
	public SendContactEmailCommandHandler(IEmail email, IAppSettingsService appSettings)
	{
		_email = email;
		_appSettings = appSettings;
	}

	// wysłanie email do administratora poprzez formularz Contact.cshtml
	public async Task Handle(SendContactEmailCommand request, CancellationToken cancellationToken)
	{
		string body =
			$"Nazwa: {request.Name}.<br><br>" +
			$"E-mail nadawcy: {request.Email}.<br><br>" +
			$"Tytuł wiadomości: {request.Title}.<br><br>" +
			$"Wiadomość: {request.Message}.<br><br>" +
			$"Wysłano z: GymManager.";

		await _email.SendAsync(
			$"Wiadomość z GymManager: {request.Title}",
			body,
			await _appSettings.Get(SettingsDict.AdminEmail));

		return;
	}
}