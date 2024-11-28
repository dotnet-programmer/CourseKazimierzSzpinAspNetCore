using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Infrastructure.Persistence.Extensions;

// INFO - role użytkowników
public static class ModelBuilderExtensionsRoles
{
	public static void SeedRoles(this ModelBuilder modelBuilder) =>
		modelBuilder.Entity<IdentityRole>()
		.HasData(
			new IdentityRole
			{
				// Tools -> Create GUID
				Id = "C854A873-3D75-4973-AD7E-A83C95726133",
				Name = "Administrator",
				NormalizedName = "ADMINISTRATOR",
				// Tools -> Create GUID
				ConcurrencyStamp = "C778085D-D407-4936-8A19-350C6817AA5D"
			},
			new IdentityRole
			{
				Id = "EC23C152-A1C5-4D9A-B8D4-FAE62D5F059D",
				Name = "Klient",
				NormalizedName = "KLIENT",
				ConcurrencyStamp = "A1277894-E24D-490E-A3BA-83F9CF5F838D"
			},
			new IdentityRole
			{
				Id = "ADE32A3F-6149-475A-8155-CFE5D69ACA42",
				Name = "Pracownik",
				NormalizedName = "PRACOWNIK",
				ConcurrencyStamp = "B50B7D83-F6E6-4DE3-8346-7D0E8501EEB5"
			});
}