namespace GymManager.Application.Invoices.Queries.GetInvoice;

public class InvoiceDto
{
	public int Id { get; set; }
	public string Title { get; set; }
	public DateTime CreatedDate { get; set; }
	public string MethodOfPayments { get; set; }
	public decimal Value { get; set; }
	public string UserName { get; set; }
	public string UserEmail { get; set; }
	public string UserStreet { get; set; }
	public string UserCity { get; set; }
	public string UserId { get; set; }
	public string TicketId { get; set; }
	public string PositionName { get; set; }
}