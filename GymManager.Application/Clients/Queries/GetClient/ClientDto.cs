namespace GymManager.Application.Clients.Queries.GetClient;

public class ClientDto
{
	public string Id { get; set; }
	public string Email { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public string Password { get; set; }
	public string ConfirmPassword { get; set; }
	public bool IsPrivateAccount { get; set; }
	public string NipNumber { get; set; }
	public string Country { get; set; }
	public string City { get; set; }
	public string Street { get; set; }
	public string StreetNumber { get; set; }
	public string ZipCode { get; set; }
	public DateTime RegisterDateTime { get; set; }
	public List<string> RoleIds { get; set; } = [];
}