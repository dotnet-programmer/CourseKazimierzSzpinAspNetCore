namespace GymManager.Application.Tickets.Queries.GetPrintTicket;

public class PrintTicketDto
{
	public string Id { get; set; }
	public string QrCodeId { get; set; }
	public string FullName { get; set; }
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
	public string Image { get; set; }
	public string CompanyContactPhone { get; set; }
	public string CompanyContactEmail { get; set; }
}