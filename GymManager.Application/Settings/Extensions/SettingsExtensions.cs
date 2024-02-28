using GymManager.Application.Settings.Queries.Dtos;
using GymManager.Application.Settings.Queries.GetSettings;
using GymManager.Domain.Entities;

namespace GymManager.Application.Settings.Extensions;

public static class SettingsExtensions
{
	public static SettingsPositionDto ToDto(this SettingsPosition position)
		=> position == null
			? null
			: new SettingsPositionDto
			{
				Id = position.SettingsPositionId,
				Description = position.Description,
				Key = position.Key,
				Type = position.Type,
				Value = position.Value
			};

	public static SettingsDto ToDto(this Domain.Entities.Settings settings)
		=> settings == null
			? null
			: new SettingsDto
			{
				Id = settings.SettingsId,
				Description = settings.Description,
				Order = settings.Order,
				Positions = settings.Positions.Select(x => x.ToDto())?.ToList(),
			};
}