namespace GymManager.Application.Invoices.Queries.GetPdfInvoice;

// view model bo jest kilka właściwości do przekazania poza samym modelem
public class InvoicePdfVm
{
	// unikalne Id
	public string Handle { get; set; }

	// nazwa pliku pdf
	public string FileName { get; set; }

	// pdf jako tablica bajtów
	public byte[] PdfContent { get; set; }
}