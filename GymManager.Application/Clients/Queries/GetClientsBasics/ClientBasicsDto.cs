namespace GymManager.Application.Clients.Queries.GetClientsBasics;

// model który jest wyświetlany na tabeli
public class ClientBasicsDto
{
	public string Id { get; set; }
	public string Email { get; set; }
	public string Name { get; set; }
	public bool IsDeleted { get; set; }
}