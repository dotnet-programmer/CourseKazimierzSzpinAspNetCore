using GymManager.Application.Invoices.Queries.GetInvoice;
using GymManager.Application.Invoices.Queries.GetInvoices;
using GymManager.Domain.Entities;

namespace GymManager.Application.Invoices.Extensions;

public static class InvoiceExtensions
{
	public static InvoiceBasicsDto ToBasicsDto(this Invoice invoice)
		=> invoice == null
			? null
			: new InvoiceBasicsDto
			{
				Id = invoice.InvoiceId,
				UserId = invoice.UserId,
				CreatedDate = invoice.CreatedDate,
				Title = $"{invoice.InvoiceId}/{invoice.Month}/{invoice.Year}",
				UserName = $"{invoice.User.FirstName} {invoice.User.LastName}",
				Value = invoice.Value
			};

	public static InvoiceDto ToDto(this Invoice invoice)
		=> invoice == null
			? null
			: new InvoiceDto
			{
				Id = invoice.InvoiceId,
				UserId = invoice.UserId,
				CreatedDate = invoice.CreatedDate,
				Title = $"{invoice.InvoiceId}/{invoice.Month}/{invoice.Year}",
				UserName = $"{invoice.User.FirstName} {invoice.User.LastName}",
				Value = invoice.Value,
				MethodOfPayments = invoice.MethodOfPayment,
				TicketId = invoice.TicketId,
				PositionName = invoice.Ticket.TicketType.TicketTypeEnum.ToString(),
				UserEmail = invoice.User.Email,
				UserCity = $"{invoice.User.Address.ZipCode} {invoice.User.Address.City}",
				UserStreet = $"{invoice.User.Address.Street} {invoice.User.Address.StreetNumber}"
			};
}