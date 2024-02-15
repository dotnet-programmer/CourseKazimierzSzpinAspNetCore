using GymManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Infrastructure.Persistence.Extensions;

public static class ModelBuilderExtensionsAnnouncement
{
	public static void SeedAnnouncement(this ModelBuilder modelBuilder) => 
		modelBuilder.Entity<Announcement>().HasData(
			new Announcement
			{
				AnnouncementId = 1,
				Date = new DateTime(2022, 1, 12),
				Description = "Kod promocyjny na suplementy w sklepie abc = rabat12."
			},
			new Announcement
			{
				AnnouncementId = 2,
				Date = new DateTime(2022, 1, 15),
				Description = "W najbliższą niedzielę siłownia jest otwarta do godsziny 24:00."
			},
			new Announcement
			{
				AnnouncementId = 3,
				Date = new DateTime(2022, 1, 16),
				Description = "Jutrzejsze zajęcia crossfit zostały odwołane - przepraszamy."
			},
			new Announcement
			{
				AnnouncementId = 4,
				Date = new DateTime(2022, 1, 20),
				Description = "Zatrudnimy trenera personalnego."
			},
			new Announcement
			{
				AnnouncementId = 5,
				Date = new DateTime(2022, 1, 24),
				Description = "Od przyszłego miesiąca możesz kupić karnet dla dwóch osób w cenie jednego."
			},
			new Announcement
			{
				AnnouncementId = 6,
				Date = new DateTime(2021, 12, 7),
				Description = "Zatrudnimy recepcjonistę."
			},
			new Announcement
			{
				AnnouncementId = 7,
				Date = new DateTime(2021, 12, 5),
				Description = "Kod promocyjny na suplementy w sklepie abc = rabat12."
			},
			new Announcement
			{
				AnnouncementId = 8,
				Date = new DateTime(2021, 12, 4),
				Description = "W poprzednim miesiącu zrobiłeś aż 12 treningów - gratulacje."
			},
			new Announcement
			{
				AnnouncementId = 9,
				Date = new DateTime(2021, 12, 3),
				Description = "Jutrzejsze zajęcia crossfit zostały odwołane - przepraszamy."
			},
			new Announcement
			{
				AnnouncementId = 10,
				Date = new DateTime(2021, 12, 3),
				Description = "Kod promocyjny na suplementy w sklepie abc = rabat12."
			},
			new Announcement
			{
				AnnouncementId = 11,
				Date = new DateTime(2021, 12, 2),
				Description = "Odbierz kod zniżkowy na suplementacje w recepcji."
			},
			new Announcement
			{
				AnnouncementId = 12,
				Date = new DateTime(2021, 12, 1),
				Description = "Wesołych świąt."
			});
}