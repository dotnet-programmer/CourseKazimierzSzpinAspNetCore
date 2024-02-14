using MediatR;

namespace GymManager.Application.Tickets.Queries.GetTicketById;

// INFO - Kwerenda - QueryHandler
// zgodnie z konwencją ta klasa będzie zawsze miała w nazwie przyrostek QueryHandler
// musi implementować interfejs IRequestHandler<T, TResult>
// T - klasa implementująca interfejs IRequest
// TResult - klasa, która zostanie zwrócona, jesto to obiekt DTO
public class GetTicketByIdQueryHandler : IRequestHandler<GetTicketByIdQuery, TicketDto>
{
	public async Task<TicketDto> Handle(GetTicketByIdQuery request, CancellationToken cancellationToken) =>
		// tutaj operuje się na obiektach DAO, ale zwraca się obiekt DTO
		// na podstawie Id pobieranie danych z bazy danych i zwrócenie Ticketu który był wyszukiwany
		new() { TicketId = request.TicketId, Name = "Name" };
}