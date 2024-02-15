using GymManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Infrastructure.Persistence.Extensions;

internal static class ModelBuilderExtensionsTicketTypeTranslation
{
	public static void SeedTicketTypeTranslation(this ModelBuilder modelBuilder) =>
		modelBuilder.Entity<TicketTypeTranslation>().HasData(
			new TicketTypeTranslation
			{
				TicketTypeTranslationId = 1,
				LanguageId = 1,
				Name = "Jednorazowy",
				TicketTypeId = 1
			},
			new TicketTypeTranslation
			{
				TicketTypeTranslationId = 2,
				LanguageId = 2,
				Name = "Single",
				TicketTypeId = 1
			},
			new TicketTypeTranslation
			{
				TicketTypeTranslationId = 3,
				LanguageId = 1,
				Name = "Tygodniowy",
				TicketTypeId = 2
			},
			new TicketTypeTranslation
			{
				TicketTypeTranslationId = 4,
				LanguageId = 2,
				Name = "Weekly",
				TicketTypeId = 2
			},
			new TicketTypeTranslation
			{
				TicketTypeTranslationId = 5,
				LanguageId = 1,
				Name = "Miesięczny",
				TicketTypeId = 3
			},
			new TicketTypeTranslation
			{
				TicketTypeTranslationId = 6,
				LanguageId = 2,
				Name = "Monthly",
				TicketTypeId = 3
			},
			new TicketTypeTranslation
			{
				TicketTypeTranslationId = 7,
				LanguageId = 1,
				Name = "Roczny",
				TicketTypeId = 4
			},
			new TicketTypeTranslation
			{
				TicketTypeTranslationId = 8,
				LanguageId = 2,
				Name = "Annual",
				TicketTypeId = 4
			});
}