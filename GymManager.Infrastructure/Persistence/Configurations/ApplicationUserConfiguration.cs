﻿using GymManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManager.Infrastructure.Persistence.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
	public void Configure(EntityTypeBuilder<ApplicationUser> builder)
	{
		builder.Property(x => x.FirstName)
			.HasMaxLength(100);

		builder.Property(x => x.LastName)
			.HasMaxLength(100);

		// konfiguracja kluczy obcych z relacji 1:1,
		// zrobiona tutaj w klasie nadrzędnej, w której nie ma kluczy obcych,
		// żeby mieć konfiguracje wszystkich klas zależnych w 1 miejscu
		builder
			.HasOne(x => x.Address)
			.WithOne(x => x.User)
			.HasForeignKey<Address>(x => x.UserId)
			.OnDelete(DeleteBehavior.Restrict);

		builder
			.HasOne(x => x.Client)
			.WithOne(x => x.User)
			.HasForeignKey<Client>(x => x.UserId)
			.OnDelete(DeleteBehavior.Restrict);

		builder
			.HasOne(x => x.Employee)
			.WithOne(x => x.User)
			.HasForeignKey<Employee>(x => x.UserId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}