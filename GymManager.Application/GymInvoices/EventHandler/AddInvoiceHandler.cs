using GymManager.Application.Common.Events;
using GymManager.Application.Common.Interfaces;
using GymManager.Application.Tickets.Events;

namespace GymManager.Application.GymInvoices.EventHandler;

// eventy
// 6. Klasa implementująca IEventHandler, czyli klasa której metody zostaną wywołane po opublikowaniu eventa.
// Czyli handler, który zostanie podpięty pod zdarzenie TicketPaidEvent
// (jeśli ten event zostanie opublikowany to wykona się metoda HandleAsync, w tym przypadku doda się nowa faktura za pomocą WebApi).
public class AddInvoiceHandler(IGymInvoices gymInvoices) : IEventHandler<TicketPaidEvent>
{
	public async Task HandleAsync(TicketPaidEvent @event)
		=> await gymInvoices.AddInvoice(@event.TicketId, @event.UserId);
}