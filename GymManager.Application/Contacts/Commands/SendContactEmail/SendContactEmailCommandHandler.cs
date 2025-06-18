using GymManager.Application.Common.Interfaces;
using GymManager.Application.Dictionaries;
using MediatR;

namespace GymManager.Application.Contacts.Commands.SendContactEmail;

public class SendContactEmailCommandHandler(
	IEmailService email,
	IAppSettingsService appSettings,
	IBackgroundWorkerQueue backgroundWorkerQueue) : IRequestHandler<SendContactEmailCommand, Unit>
{
	// wysłanie email do administratora poprzez formularz Contact.cshtml
	public async Task<Unit> Handle(SendContactEmailCommand request, CancellationToken cancellationToken)
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
		//	await _appSettings.GetValueByKeyAsync(SettingsDict.AdminEmail));

		// wywołanie komendy w tle z użyciem zadań w tle
		backgroundWorkerQueue.QueueBackgroundWorkItem(async x =>
			{
				await email.SendAsync(
					$"Wiadomość z GymManager: {request.Title}",
					body,
					await appSettings.GetValueByKeyAsync(SettingsDict.AdminEmail));
			},
			$"Kontakt. E-mail: {request.Email}"
		);
		return Unit.Value;
	}
}