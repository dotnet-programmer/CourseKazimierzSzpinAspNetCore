using GymManager.Application.Common.Interfaces;
using GymManager.Application.Dictionaries;
using GymManager.Application.Users.Extensions;
using MediatR;

namespace GymManager.Application.Clients.Queries.GetClientsBasics;

public class GetClientsBasicsQueryHandler(IUserRoleManagerService userRoleManagerService) : IRequestHandler<GetClientsBasicsQuery, IEnumerable<ClientBasicsDto>>
{
	// pobranie wszystkich klientów,
	// dzięki Identity można to pobrać z już istniejącej metody, która jest w user manager o nazwie GetUsersInRoleAsync
	// czyli pobranie wszystkich userów którzy należą do danej roli
	public async Task<IEnumerable<ClientBasicsDto>> Handle(GetClientsBasicsQuery request, CancellationToken cancellationToken) =>
		(await userRoleManagerService
			.GetUsersInRoleAsync(RolesDict.Client))
			.Select(x => x.ToClientBasicsDto());
}