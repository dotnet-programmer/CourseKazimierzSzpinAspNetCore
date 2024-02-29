namespace GymManager.Application.Invoices.Queries.GetInvoices;

public class InvoiceBasicsDto
{
	public int Id { get; set; }
	public string Title { get; set; }
	public decimal Value { get; set; }
	public string UserName { get; set; }
	public DateTime CreatedDate { get; set; }
	public string UserId { get; set; }
}