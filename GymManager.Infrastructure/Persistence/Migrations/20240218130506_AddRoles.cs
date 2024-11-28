using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GymManager.Infrastructure.Persistence.Migrations;

/// <inheritdoc />
public partial class AddRoles : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder) => migrationBuilder.InsertData(
			table: "AspNetRoles",
			columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
			values: new object[,]
			{
				{ "ADE32A3F-6149-475A-8155-CFE5D69ACA42", "B50B7D83-F6E6-4DE3-8346-7D0E8501EEB5", "Pracownik", "PRACOWNIK" },
				{ "C854A873-3D75-4973-AD7E-A83C95726133", "C778085D-D407-4936-8A19-350C6817AA5D", "Administrator", "ADMINISTRATOR" },
				{ "EC23C152-A1C5-4D9A-B8D4-FAE62D5F059D", "A1277894-E24D-490E-A3BA-83F9CF5F838D", "Klient", "KLIENT" }
			});

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.DeleteData(
			table: "AspNetRoles",
			keyColumn: "Id",
			keyValue: "ADE32A3F-6149-475A-8155-CFE5D69ACA42");

		migrationBuilder.DeleteData(
			table: "AspNetRoles",
			keyColumn: "Id",
			keyValue: "C854A873-3D75-4973-AD7E-A83C95726133");

		migrationBuilder.DeleteData(
			table: "AspNetRoles",
			keyColumn: "Id",
			keyValue: "EC23C152-A1C5-4D9A-B8D4-FAE62D5F059D");
	}
}
