using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace GymManager.Application.Employees.Commands.EditEmployee;

public class EditEmployeeCommand : IRequest
{
	public string Id { get; set; }

	[Required(ErrorMessage = "Pole 'Adres e-mail' jest wymagane")]
	[DisplayName("Adres e-mail")]
	[EmailAddress(ErrorMessage = "Pole 'Adres e-mail nie jest prawidłowym adresem e-mail'")]
	public string Email { get; set; }

	[Required(ErrorMessage = "Pole 'Imię' jest wymagane")]
	[DisplayName("Imię")]
	public string FirstName { get; set; }

	[Required(ErrorMessage = "Pole 'Nazwisko' jest wymagane")]
	[DisplayName("Nazwisko")]
	public string LastName { get; set; }

	[Required(ErrorMessage = "Pole 'Kraj' jest wymagane")]
	[DisplayName("Kraj")]
	public string Country { get; set; }

	[Required(ErrorMessage = "Pole 'Miejscowość' jest wymagane")]
	[DisplayName("Miejscowość")]
	public string City { get; set; }

	[Required(ErrorMessage = "Pole 'Ulica' jest wymagane")]
	[DisplayName("Ulica")]
	public string Street { get; set; }

	[Required(ErrorMessage = "Pole 'Numer domu' jest wymagane")]
	[DisplayName("Numer domu")]
	public string StreetNumber { get; set; }

	[Required(ErrorMessage = "Pole 'Kod pocztowy' jest wymagane")]
	[DisplayName("Kod pocztowy")]
	public string ZipCode { get; set; }

	[DisplayName("Data zatrudnienia")]
	[Required(ErrorMessage = "Pole 'Data zatrudnienia' jest wymagane")]
	public DateTime DateOfEmployment { get; set; }

	[DisplayName("Data zwolnienia")]
	public DateTime? DateOfDismissal { get; set; }

	[DisplayName("Wynagrodzenie")]
	[Required(ErrorMessage = "Pole 'Wynagrodzenie' jest wymagane")]
	public decimal Salary { get; set; }

	[DisplayName("Stanowisko")]
	[Required(ErrorMessage = "Pole 'Stanowisko' jest wymagane")]
	public int PositionId { get; set; }

	[DisplayName("Zdjęcie profilowe")]
	public string ImageUrl { get; set; }

	[DisplayName("Adres URL strony profilowej")]
	public string WebsiteUrl { get; set; }

	[DisplayName("Strona profilowa")]
	public string WebsiteRaw { get; set; }

	[DisplayName("Wybrane role")]
	public List<string> RoleIds { get; set; } = [];
}