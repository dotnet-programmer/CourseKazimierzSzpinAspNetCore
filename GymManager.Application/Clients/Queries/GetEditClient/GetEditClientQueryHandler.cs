using GymManager.Application.Clients.Commands.EditClient;
using GymManager.Application.Common.Interfaces;
using GymManager.Application.Users.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Clients.Queries.GetEditClient;

// do pobrania danych o edytowanym kliencie i wyświetlenie ich na formularzu
public class GetEditClientQueryHandler(IApplicationDbContext context) : IRequestHandler<GetEditClientQuery, EditClientCommand>
{
	public async Task<EditClientCommand> Handle(GetEditClientQuery request, CancellationToken cancellationToken)
		=> (await context.Users
			.Include(x => x.Client)
			.Include(x => x.Address)
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken))
			.ToEditClientCommand();
}