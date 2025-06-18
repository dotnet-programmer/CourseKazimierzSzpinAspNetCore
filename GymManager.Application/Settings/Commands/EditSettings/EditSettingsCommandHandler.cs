using GymManager.Application.Common.Interfaces;
using MediatR;

namespace GymManager.Application.Settings.Commands.EditSettings;

public class EditSettingsCommandHandler(
	IApplicationDbContext context,
	IAppSettingsService appSettingsService,
	IEmailService emailService) : IRequestHandler<EditSettingsCommand>
{
	private readonly IApplicationDbContext _context = context;
	private readonly IAppSettingsService _appSettingsService = appSettingsService;
	private readonly IEmailService _emailService = emailService;

	public async Task Handle(EditSettingsCommand request, CancellationToken cancellationToken)
	{
		foreach (var position in request.Positions)
		{
			var positionToUpdate = _context.SettingsPositions.Find(position.Id);
			positionToUpdate.Value = position.Value;
		}

		await _context.SaveChangesAsync(cancellationToken);

		await UpdateAppSettingsAsync();
	}

	// odświeżenie ustawień zapisanych w Cahce
	private async Task UpdateAppSettingsAsync()
	{
		await _appSettingsService.UpdateValuesAsync(_context);
		await _emailService.UpdateAsync(_appSettingsService);
	}
}