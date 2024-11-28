using GymManager.Domain.Entities;
using GymManager.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Infrastructure.Persistence.Extensions;

public static class ModelBuilderExtensionsTicketType
{
	public static void SeedTicketType(this ModelBuilder modelBuilder) =>
		modelBuilder.Entity<TicketType>().HasData(
			new TicketType
			{
				TicketTypeId = 1,
				Price = 10,
				TicketTypeEnum = TicketTypeEnum.Single
			},
			new TicketType
			{
				TicketTypeId = 2,
				Price = 25,
				TicketTypeEnum = TicketTypeEnum.Weekly
			},
			new TicketType
			{
				TicketTypeId = 3,
				Price = 100,
				TicketTypeEnum = TicketTypeEnum.Monthly
			},
			new TicketType
			{
				TicketTypeId = 4,
				Price = 1000,
				TicketTypeEnum = TicketTypeEnum.Annual
			});
}