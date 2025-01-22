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