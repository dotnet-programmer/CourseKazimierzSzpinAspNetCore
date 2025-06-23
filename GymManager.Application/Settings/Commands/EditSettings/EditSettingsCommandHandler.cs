using GymManager.Application.Common.Interfaces;
using MediatR;

namespace GymManager.Application.Settings.Commands.EditSettings;

public class EditSettingsCommandHandler(
	IApplicationDbContext context,
	IAppSettingsService appSettingsService,
	IEmailService emailService
	) : IRequestHandler<EditSettingsCommand>
{
	public async Task Handle(EditSettingsCommand request, CancellationToken cancellationToken)
	{
		foreach (var position in request.Positions)
		{
			var positionToUpdate = context.SettingsPositions.Find(position.Id);
			positionToUpdate.Value = position.Value;
		}

		await context.SaveChangesAsync(cancellationToken);

		await UpdateAppSettingsAsync();
	}

	// odświeżenie ustawień zapisanych w Cahce
	private async Task UpdateAppSettingsAsync()
	{
		await appSettingsService.UpdateValuesAsync(context);
		await emailService.UpdateAsync(appSettingsService);
	}
}