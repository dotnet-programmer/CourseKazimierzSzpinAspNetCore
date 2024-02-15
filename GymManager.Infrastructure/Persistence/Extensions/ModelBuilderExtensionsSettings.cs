using GymManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Infrastructure.Persistence.Extensions;

public static class ModelBuilderExtensionsSettings
{
	public static void SeedSettings(this ModelBuilder modelBuilder) =>
		modelBuilder.Entity<Settings>().HasData(
			new Settings
			{
				SettingsId = 1,
				Description = "Ogólne",
				Order = 1
			},
			new Settings
			{
				SettingsId = 2,
				Description = "E-mail",
				Order = 2
			});
}