using GymManager.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Clients.Commands.EditClient;

// komenda zapisujaca zmiany w bazie
public class EditClientCommandHandler : IRequestHandler<EditClientCommand>
{
	private readonly IApplicationDbContext _context;

	public EditClientCommandHandler(IApplicationDbContext context) =>
		_context = context;

	public async Task Handle(EditClientCommand request, CancellationToken cancellationToken)
	{
		if (request.IsPrivateAccount)
		{
			request.NipNumber = null;
		}

		// pobranie użytkownika który będzie aktualizowany
		var user = await _context.Users
			.Include(x => x.Client)
			.Include(x => x.Address)
			.FirstOrDefaultAsync(x => x.Id == request.Id);

		user.FirstName = request.FirstName;
		user.LastName = request.LastName;

		// jeśli dane klienta jeszcze nie są uzupełnione, to zainicjalizuj je nowym obiektem
		if (user.Client == null)
		{
			user.Client = new Domain.Entities.Client();
		}
		user.Client.IsPrivateAccount = request.IsPrivateAccount;
		user.Client.NipNumber = request.NipNumber;
		user.Client.UserId = request.Id;

		// jeśli dane adresowe jeszcze nie są uzupełnione, to zainicjalizuj je nowym obiektem
		if (user.Address == null)
		{
			user.Address = new Domain.Entities.Address();
		}
		user.Address.Country = request.Country;
		user.Address.City = request.City;
		user.Address.Street = request.Street;
		user.Address.ZipCode = request.ZipCode;
		user.Address.StreetNumber = request.StreetNumber;
		user.Address.UserId = request.Id;

		await _context.SaveChangesAsync(cancellationToken);

		return;
	}
}