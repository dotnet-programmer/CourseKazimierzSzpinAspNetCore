using GymManager.Domain.Enums;

namespace GymManager.Domain.Entities;

// na podstawie tych pozycji ustawień będą tworzone różne formularze,
// dlatego trzeba zdefioniować typ dla każdej pozycji żeby było wiadomo jaką kontrolkę trzeba załadować
public class SettingsPosition
{
	public int SettingsPositionId { get; set; }
	public string Key { get; set; }
	public string Value { get; set; }
	public string Description { get; set; }
	public SettingsType Type { get; set; }

	// do wyświetlania w określonej kolejności
	public int Order { get; set; }

	// relacja 1:wiele, każde ustawienie ma swoją kolekcję różnych ustawień z danej kategorii
	public int SettingsId { get; set; }
	public Settings Settings { get; set; }
}