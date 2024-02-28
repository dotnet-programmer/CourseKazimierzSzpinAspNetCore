using GymManager.Application.Common.Interfaces;
using MediatR;

namespace GymManager.Application.Settings.Commands.EditSettings;

public class EditSettingsCommandHandler : IRequestHandler<EditSettingsCommand>
{
	private readonly IApplicationDbContext _context;
	private readonly IAppSettingsService _appSettingsService;
	private readonly IEmailService _emailService;

	public EditSettingsCommandHandler(
		IApplicationDbContext context,
		IAppSettingsService appSettingsService,
		IEmailService emailService)
	{
		_context = context;
		_appSettingsService = appSettingsService;
		_emailService = emailService;
	}

	public async Task Handle(EditSettingsCommand request, CancellationToken cancellationToken)
	{
		foreach (var position in request.Positions)
		{
			var positionToUpdate = _context.SettingsPositions.Find(position.Id);
			positionToUpdate.Value = position.Value;
		}

		await _context.SaveChangesAsync(cancellationToken);

		await UpdateAppSettings();

		return;
	}

	// odświeżenie ustawień zapisanych w Cahce
	private async Task UpdateAppSettings()
	{
		await _appSettingsService.Update(_context);
		await _emailService.Update(_appSettingsService);
	}
}