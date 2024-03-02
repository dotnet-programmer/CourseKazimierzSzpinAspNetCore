using GymManager.Application.Common.Events;
using GymManager.Application.Common.Interfaces;
using GymManager.Application.Tickets.Events;

namespace GymManager.Application.GymInvoices.EventHandler;

// klasa implementująca IEventHandler, czyli klasa której metody zostaną wywołane po opublikowaniu eventa

// Czyli handler, który zostanie podpięty pod zdarzenie TicketPaidEvent.
// jego metoda HandleAsync zostanie wywołana gdy faktura zostanie opłacona,
// wtedy zostanie również dodana nowa faktura
public class AddInvoiceHandler : IEventHandler<TicketPaidEvent>
{
	private readonly IGymInvoices _gymInvoices;

	public AddInvoiceHandler(IGymInvoices gymInvoices) 
		=> _gymInvoices = gymInvoices;

	// jeśli event TicketPaidEvent zostanie opublikowany to wykona się ta metoda
	// w tym przypadku doda się nowa faktura za pomocą WebApi
	public async Task HandleAsync(TicketPaidEvent @event) 
		=> await _gymInvoices.AddInvoice(@event.TicketId, @event.UserId);
}