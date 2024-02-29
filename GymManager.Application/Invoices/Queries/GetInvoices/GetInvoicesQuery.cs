using MediatR;

namespace GymManager.Application.Invoices.Queries.GetInvoices;

// zwraca wszystkie faktury danego użytkownika
public class GetInvoicesQuery : IRequest<IEnumerable<InvoiceBasicsDto>>
{
	public string UserId { get; set; }
}