using GymManager.Application.Common.Interfaces;
using GymManager.Application.Dictionaries;
using GymManager.Application.Users.Extensions;
using MediatR;

namespace GymManager.Application.Clients.Queries.GetClientsBasics;

public class GetClientsBasicsQueryHandler : IRequestHandler<GetClientsBasicsQuery, IEnumerable<ClientBasicsDto>>
{
	private readonly IUserRoleManagerService _userRoleManagerService;

	public GetClientsBasicsQueryHandler(IUserRoleManagerService userRoleManagerService) =>
		_userRoleManagerService = userRoleManagerService;

	// pobranie wszystkich klientów,
	// dzięki Identity można to pobrać z już istniejącej metody, która jst w user manager o nazwie GetUsersInRoleAsync
	// czyli pobranie wszystkich userów którzy należą do danej roli
	public async Task<IEnumerable<ClientBasicsDto>> Handle(GetClientsBasicsQuery request, CancellationToken cancellationToken) =>
		(await _userRoleManagerService
			.GetUsersInRoleAsync(RolesDict.Klient))
			.Select(x => x.ToClientBasicsDto());
}