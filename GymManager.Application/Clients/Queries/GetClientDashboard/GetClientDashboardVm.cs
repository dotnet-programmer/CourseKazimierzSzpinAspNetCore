using GymManager.Application.Announcements.Queries.Dtos;
using GymManager.Application.Common.Models;

namespace GymManager.Application.Clients.Queries.GetClientDashboard;

public class GetClientDashboardVm
{
	public PaginatedList<AnnouncementDto> Announcements { get; set; }
	public string Email { get; set; }
	public DateTime? TicketEndDate { get; set; }
}