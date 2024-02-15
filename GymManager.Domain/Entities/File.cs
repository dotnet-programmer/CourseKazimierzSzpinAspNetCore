namespace GymManager.Domain.Entities;

// klasa dla plików przechowywanych w bazie dancyh
public class File
{
	public int FileId { get; set; }
	public string Name { get; set; }

	// rozmiar pliku w bajtach
	public long Bytes { get; set; }
	public string Description { get; set; }
}