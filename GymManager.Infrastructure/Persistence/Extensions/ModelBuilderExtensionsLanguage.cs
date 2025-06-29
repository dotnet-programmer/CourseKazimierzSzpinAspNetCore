﻿using GymManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Infrastructure.Persistence.Extensions;

public static class ModelBuilderExtensionsLanguage
{
	public static void SeedLanguage(this ModelBuilder modelBuilder) =>
		modelBuilder.Entity<Language>().HasData(
			new()
			{
				LanguageId = 1,
				Name = "Polski",
				Key = "pl"
			},
			new Language
			{
				LanguageId = 2,
				Name = "Angielski",
				Key = "en"
			});
}