namespace GymManager.Domain.Entities;

// ogłoszenia wyświetlane na panelu ogłoszeń
public class Announcement
{
	public int AnnouncementId { get; set; }
	public DateTime Date { get; set; }
	public string Description { get; set; }
}