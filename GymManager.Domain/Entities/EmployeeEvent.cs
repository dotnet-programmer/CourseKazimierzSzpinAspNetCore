namespace GymManager.Domain.Entities;

// lista zdarzeń - pozycje na grafiku w kalendarzu
public class EmployeeEvent
{
	public int EmployeeEventId { get; set; }
	public DateTime Start { get; set; }
	public DateTime? End { get; set; }
	public bool IsFullDay { get; set; }
	public DateTime CreatedDate { get; set; }

	// relacja 1:wiele
	public string UserId { get; set; }
	public ApplicationUser User { get; set; }
}