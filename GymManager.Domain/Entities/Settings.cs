namespace GymManager.Domain.Entities;

public class Settings
{
	public int SettingsId { get; set; }
	public string Description { get; set; }

	// nagłówek dla ustawień
	public int Order { get; set; }

	// relacja 1:wiele, każde ustawienie ma swoją kolekcję różnych ustawień z danej kategorii
	public ICollection<SettingsPosition> Positions { get; set; } = new HashSet<SettingsPosition>();
}