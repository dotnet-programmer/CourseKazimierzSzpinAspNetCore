using GymManager.Domain.Enums;

namespace GymManager.Application.Settings.Queries.Dtos;

public class SettingsPositionDto
{
	public int Id { get; set; }
	public string Key { get; set; }
	public string Value { get; set; }
	public string Description { get; set; }
	public int Order { get; set; }
	public SettingsType Type { get; set; }
	public int SettingsId { get; set; }
}