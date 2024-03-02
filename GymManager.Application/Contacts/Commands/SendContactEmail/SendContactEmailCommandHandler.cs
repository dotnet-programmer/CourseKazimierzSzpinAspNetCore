using GymManager.Application.Common.Interfaces;
using GymManager.Application.Dictionaries;
using MediatR;

namespace GymManager.Application.Contacts.Commands.SendContactEmail;

public class SendContactEmailCommandHandler : IRequestHandler<SendContactEmailCommand>
{
	private readonly IEmailService _email;
	private readonly IAppSettingsService _appSettings;
	private readonly IBackgroundWorkerQueue _backgroundWorkerQueue;

	// wstrzyknięcie klasy Email odpowiedzialnej za wysyłkę emaili
	public SendContactEmailCommandHandler(
		IEmailService email,
		IAppSettingsService appSettings,
		IBackgroundWorkerQueue backgroundWorkerQueue)
	{
		_email = email;
		_appSettings = appSettings;
		_backgroundWorkerQueue = backgroundWorkerQueue;
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

		// wywołanie bez użycia zadań w tle
		//await _email.SendAsync(
		//	$"Wiadomość z GymManager: {request.Title}",
		//	body,
		//	await _appSettings.Get(SettingsDict.AdminEmail));

		// wywołanie komendy w tle z użyciem zadań w tle
		_backgroundWorkerQueue.QueueBackgroundWorkItem(async x =>
			{
				await _email.SendAsync(
				$"Wiadomość z GymManager: {request.Title}",
				body,
				await _appSettings.Get(SettingsDict.AdminEmail));
			},
			$"Kontakt. E-mail: {request.Email}"
		);

		return;
	}
}