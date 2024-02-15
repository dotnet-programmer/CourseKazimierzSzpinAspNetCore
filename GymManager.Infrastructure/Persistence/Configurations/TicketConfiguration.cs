using GymManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManager.Infrastructure.Persistence.Configurations;

public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
	public void Configure(EntityTypeBuilder<Ticket> builder)
	{
		builder.ToTable("Tickets");

		builder.Property(x => x.UserId)
			.IsRequired();

		builder.Property(x => x.TicketTypeId)
			.HasDefaultValue(1);

		// konfiguracja relacji 1:wiele, zrobiona dla klasy, w której znajduje się klucz obcy a nie kolekcja
		builder
			.HasOne(x => x.User)
			.WithMany(x => x.Tickets)
			.HasForeignKey(x => x.UserId)
			.OnDelete(DeleteBehavior.Restrict);

		// konfiguracja relacji 1:wiele, zrobiona dla klasy, w której znajduje się klucz obcy a nie kolekcja
		builder
			.HasOne(x => x.TicketType)
			.WithMany(x => x.Tickets)
			.HasForeignKey(x => x.TicketTypeId)
			.OnDelete(DeleteBehavior.Restrict);

		// konfiguracja relacji 1:1, zrobiona w klasie nadrzędnej, w której nie ma klucza obcego
		builder
			.HasOne(x => x.Invoice)
			.WithOne(x => x.Ticket)
			.HasForeignKey<Invoice>(x => x.TicketId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}