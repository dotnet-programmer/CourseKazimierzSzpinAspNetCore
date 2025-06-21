using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace GymManager.Application.Clients.Commands.EditClient;

// komenda zapisujaca zmiany w bazie
// wspólny model wykorzystany w kwerendzie do pobrania danych a później w komendzie do zapisu danych
// będzie tylko 1 model żeby nie tworzyć osobno modeli dla EditClient i GetEditClient bo wtedy modele byłyby podwójne i identyczne
public class EditClientCommand : IRequest<Unit>
{
	public string UserId { get; set; }

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

	[DisplayName("Czy osoba prywatna (bez faktury)?")]
	public bool IsPrivateAccount { get; set; }

	[DisplayName("NIP")]
	public string NipNumber { get; set; }

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
}