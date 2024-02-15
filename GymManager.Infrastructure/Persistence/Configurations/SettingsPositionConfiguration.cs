using GymManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManager.Infrastructure.Persistence.Configurations;

public class SettingsPositionConfiguration : IEntityTypeConfiguration<SettingsPosition>
{
	public void Configure(EntityTypeBuilder<SettingsPosition> builder)
	{
		builder.ToTable("SettingsPositions");

		builder.Property(x => x.Key)
			.IsRequired()
			.HasMaxLength(50);

		builder.Property(x => x.Value)
			.HasMaxLength(1000);

		builder.Property(x => x.Description)
			.IsRequired()
			.HasMaxLength(200);

		// konfiguracja relacji 1:wiele, zrobiona dla klasy, w której znajduje się klucz obcy a nie kolekcja
		builder
			.HasOne(x => x.Settings)
			.WithMany(x => x.Positions)
			.HasForeignKey(x => x.SettingsId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}