namespace GymManager.Application.EmployeeEvents.Queries.GetEmployeeEvents;

// zdarzenia które będą wyświetlane na kalendarzu
public class EmployeeEventDto
{
	public int Id { get; set; }
	public string Title { get; set; }
	public DateTime Start { get; set; }
	public DateTime? End { get; set; }
	public string ThemeColor { get; set; }
	public bool IsFullDay { get; set; }
	public string UserId { get; set; }
}