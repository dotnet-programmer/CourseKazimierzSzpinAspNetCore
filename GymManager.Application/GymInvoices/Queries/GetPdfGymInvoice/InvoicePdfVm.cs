namespace GymManager.Application.GymInvoices.Queries.GetPdfGymInvoice;

public class InvoicePdfVm
{
	public string Handle { get; set; }
	public string FileName { get; set; }
	public byte[] PdfContent { get; set; }
}