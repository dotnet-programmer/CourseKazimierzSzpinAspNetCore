using GymManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManager.Infrastructure.Persistence.Configurations;

public class EmployeeEventConfiguration : IEntityTypeConfiguration<EmployeeEvent>
{
	public void Configure(EntityTypeBuilder<EmployeeEvent> builder)
	{
		builder.ToTable("EmployeeEvents");

		builder.Property(x => x.UserId)
			.IsRequired();

		// konfiguracja relacji 1:wiele, zrobiona dla klasy, w której znajduje się klucz obcy a nie kolekcja
		builder
			.HasOne(x => x.User)
			.WithMany(x => x.EmployeeEvents)
			.HasForeignKey(x => x.UserId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}