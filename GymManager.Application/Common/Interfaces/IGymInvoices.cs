
using GymManager.Application.GymInvoices.Queries.GetPdfGymInvoice;

namespace GymManager.Application.Common.Interfaces;

public interface IGymInvoices
{
	// na podstawie Id karnetu zostanie utworzona nowa faktura
	Task AddInvoice(string ticketId);
	Task<InvoicePdfVm> GetPdfInvoice(int id);
}