namespace GymManager.Domain.Entities;

// nagłówek dla ustawień
// czyli osobno można mieć ustawienia dotyczące konfiguracji email, osobno dotyczące jakiś innych ustawień
// dzięki temu można pogrupować ustawienia powiązane ze sobą tak żeby adminowi było łatwo tym zarządzać
// można różne grupy ustawień wyświetlać na innych zakładkach
public class Settings
{
	public int SettingsId { get; set; }
	public string Description { get; set; }

	// kolejność wyświetlania nagłówków ustawień na formularzu
	public int Order { get; set; }

	// relacja 1:wiele, każde ustawienie ma swoją kolekcję różnych ustawień z danej kategorii
	public ICollection<SettingsPosition> Positions { get; set; } = new HashSet<SettingsPosition>();
}