namespace GymManager.Application.Tickets.Queries.GetPdfTicket;

public class TicketPdfVm
{
	// unikalne ID
	public string Handle { get; set; }

	public string FileName { get; set; }
	public byte[] PdfContent { get; set; }
}