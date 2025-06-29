﻿using GymManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManager.Infrastructure.Persistence.Configurations;

public class TicketTypeConfiguration : IEntityTypeConfiguration<TicketType>
{
	public void Configure(EntityTypeBuilder<TicketType> builder)
	{
		builder.ToTable("TicketTypes");

		builder.Property(x => x.Price)
			.HasPrecision(18, 2);
	}
}