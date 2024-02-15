namespace GymManager.Domain.Entities;

public class Client
{
	public int ClientId { get; set; }

	// czy konto prywatne czy firmowe
	public bool IsPrivateAccount { get; set; }

	// jeśli konto firmowe, to będzie potrzebny NIP do faktury
	public string NipNumber { get; set; }

	// relacja 1:1, tylko tutaj jest klucz obcy bo to klasa zależna od Usera
	public string UserId { get; set; }
	public ApplicationUser User { get; set; }
}