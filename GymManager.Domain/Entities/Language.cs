namespace GymManager.Domain.Entities;

public class Language
{
	public int LanguageId { get; set; }
	public string Name { get; set; }

	// klucz języka
	public string Key { get; set; }

	// relacja 1:wiele, kolekcja tłumaczeń do TicketTypeTranslation
	public ICollection<TicketTypeTranslation> Translations { get; set; } = new HashSet<TicketTypeTranslation>();
}