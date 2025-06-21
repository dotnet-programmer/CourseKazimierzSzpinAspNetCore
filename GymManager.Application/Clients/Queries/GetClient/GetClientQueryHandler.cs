using GymManager.Application.Common.Interfaces;
using GymManager.Application.Users.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Clients.Queries.GetClient;

internal class GetClientQueryHandler(IApplicationDbContext context) : IRequestHandler<GetClientQuery, ClientDto>
{
	public async Task<ClientDto> Handle(GetClientQuery request, CancellationToken cancellationToken)
		=> (await context.Users
			.Include(x => x.Client)
			.Include(x => x.Address)
			.AsNoTracking()
			.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken))
			.ToClientDto();
}