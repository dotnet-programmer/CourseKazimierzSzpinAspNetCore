using MediatR;

namespace GymManager.Application.Clients.Queries.GetClientDashboard;

public class GetClientDashboardQuery : IRequest<GetClientDashboardVm>
{
	public int PageNumber { get; set; } = 1;
	public int PageSize { get; set; } = 10;
	public string UserId { get; set; }
}