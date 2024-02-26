using GymManager.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Clients.Commands.EditAdminClient;

public class EditAdminClientCommandHandler : IRequestHandler<EditAdminClientCommand>
{
	private readonly IApplicationDbContext _context;
	private readonly IUserRoleManagerService _userRoleManagerService;
	private readonly IRoleManagerService _roleManagerService;

	public EditAdminClientCommandHandler(
		IApplicationDbContext context,
		IUserRoleManagerService userRoleManagerService,
		IRoleManagerService roleManagerService)
	{
		_context = context;
		_userRoleManagerService = userRoleManagerService;
		_roleManagerService = roleManagerService;
	}

	//public async Task<Unit> Handle(EditAdminClientCommand request, CancellationToken cancellationToken)
	public async Task Handle(EditAdminClientCommand request, CancellationToken cancellationToken)
	{
		if (request.IsPrivateAccount)
		{
			request.NipNumber = null;
		}

		var user = await _context.Users
			.Include(x => x.Client)
			.Include(x => x.Address)
			.FirstOrDefaultAsync(x => x.Id == request.Id);

		user.FirstName = request.FirstName;
		user.LastName = request.LastName;

		//if (user.Client == null)
		//{
		//	user.Client = new Domain.Entities.Client();
		//}
		user.Client ??= new Domain.Entities.Client();

		user.Client.IsPrivateAccount = request.IsPrivateAccount;
		user.Client.NipNumber = request.NipNumber;
		user.Client.UserId = request.Id;

		//if (user.Address == null)
		//{
		//	user.Address = new Domain.Entities.Address();
		//}
		user.Address ??= new Domain.Entities.Address();

		user.Address.Country = request.Country;
		user.Address.City = request.City;
		user.Address.Street = request.Street;
		user.Address.ZipCode = request.ZipCode;
		user.Address.StreetNumber = request.StreetNumber;
		user.Address.UserId = request.Id;

		await _context.SaveChangesAsync(cancellationToken);

		//	return Unit.Value;
		return;
	}
}