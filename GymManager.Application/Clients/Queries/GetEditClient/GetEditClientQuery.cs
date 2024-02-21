using GymManager.Application.Clients.Commands.EditClient;
using MediatR;

namespace GymManager.Application.Clients.Queries.GetEditClient;

// do pobrania danych o edytowanym kliencie i wyświetlenie ich na formularzu
// EditClientCommand - zwracany model, 
public class GetEditClientQuery : IRequest<EditClientCommand>
{
	public string UserId { get; set; }
}