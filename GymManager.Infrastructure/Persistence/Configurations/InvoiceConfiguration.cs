using GymManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManager.Infrastructure.Persistence.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
	public void Configure(EntityTypeBuilder<Invoice> builder)
	{
		builder.ToTable("Invoices");

		builder.Property(x => x.MethodOfPayment)
			.IsRequired()
			.HasMaxLength(20);

		builder.Property(x => x.UserId)
			.IsRequired();

		builder.Property(x => x.TicketId)
			.IsRequired();

		builder.Property(x => x.Value)
			.HasPrecision(18, 2);

		// konfiguracja relacji 1:wiele, zrobiona dla klasy, w której znajduje się klucz obcy a nie kolekcja
		builder
			.HasOne(x => x.User)
			.WithMany(x => x.Invoices)
			.HasForeignKey(x => x.UserId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}