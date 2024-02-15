namespace GymManager.Domain.Entities;

public class Address
{
	public int AddressId { get; set; }
	public string Country { get; set; }
	public string City { get; set; }
	public string Street { get; set; }
	public string StreetNumber { get; set; }
	public string ZipCode { get; set; }

	// relacja 1:1, tylko tutaj jest klucz obcy bo to klasa zależna od Usera
	public string UserId { get; set; }
	public ApplicationUser User { get; set; }
}