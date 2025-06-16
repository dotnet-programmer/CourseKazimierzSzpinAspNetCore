using MediatR;

namespace GymManager.Application.Tickets.Queries.GetTicketById;

// Kwerenda - Query
// klasa Query jest to klasa, która będzie parametrem w metodzie, która zostanie wykonana przez QueryHandler
// musi implementować interfejs IRequest<TResult>
// - typ IRequest jest w NuGet -> MediatR
// - TResult - typ który ma zostać zwrócony z kwerendy
public class GetTicketByIdQuery : IRequest<TicketDto>
{
	// tutaj definiuje się parametry które zostaną przekazane do metody Handle() w QueryHandler
	// czyli jest to po prostu lista argumentów przekazywana do metody
	// w tym przypadku zostanie przekazane Id a zostanie zwrócony TicketDto
	public int TicketId { get; set; }
}

// Kwerenda - QueryHandler
// zgodnie z konwencją ta klasa będzie zawsze miała w nazwie przyrostek QueryHandler
// musi implementować interfejs IRequestHandler<T, TResult>
// T - klasa implementująca interfejs IRequest
// TResult - klasa, która zostanie zwrócona, jesto to obiekt DTO
public class GetTicketByIdQueryHandler : IRequestHandler<GetTicketByIdQuery, TicketDto>
{
	// tutaj operuje się na obiektach DAO, ale zwraca się obiekt DTO
	// na podstawie Id pobieranie danych z bazy danych i zwrócenie Ticketu który był wyszukiwany
	public async Task<TicketDto> Handle(GetTicketByIdQuery request, CancellationToken cancellationToken)
		=> new() { TicketId = request.TicketId, Name = "Name" };
}