using GymManager.Application.Settings.Queries.Dtos;

namespace GymManager.Application.Settings.Queries.GetSettings;

public class SettingsDto
{
	public int Id { get; set; }
	public string Description { get; set; }
	public int Order { get; set; }

	public IList<SettingsPositionDto> Positions { get; set; } = [];
}