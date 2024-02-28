using GymManager.Domain.Enums;

namespace GymManager.Domain.Entities;

// na podstawie tych pozycji ustawień będą tworzone różne formularze,
// dlatego trzeba zdefioniować typ dla każdej pozycji żeby było wiadomo jaką kontrolkę trzeba załadować
public class SettingsPosition
{
	public int SettingsPositionId { get; set; }

	// klucz danej opcji
	public string Key { get; set; }

	// wartość danej opcji
	public string Value { get; set; }

	public string Description { get; set; }

	// typ danej opcji, dzięki temu wiadomo jakie dane powinny być przypisane do tej pozycji (czyli typ inputu)
	// dzięki tej właściwości można wszystkie ustawienia trzymać w 1 tabeli i edytować na formularzu
	public SettingsType Type { get; set; }

	// do wyświetlania w określonej kolejności na formularzu
	public int Order { get; set; }

	// relacja 1:wiele, każde ustawienie ma swoją kolekcję różnych ustawień z danej kategorii
	// Id nagłówka, do którego będzie przypisana grupa ustawień,
	// czyli osobno można mieć ustawienia dotyczące konfiguracji email, osobno dotyczące jakiś innych ustawień
	// dzięki temu można pogrupować ustawienia powiązane ze sobą tak żeby adminowi było łatwo tym zarządzać
	// można różne grupy ustawień wyświetlać na innych zakładkach
	public int SettingsId { get; set; }
	public Settings Settings { get; set; }
}