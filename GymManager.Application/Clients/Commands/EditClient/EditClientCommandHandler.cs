using GymManager.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Clients.Commands.EditClient;

// komenda zapisujaca zmiany w bazie
public class EditClientCommandHandler(IApplicationDbContext context) : IRequestHandler<EditClientCommand, Unit>
{
	public async Task<Unit> Handle(EditClientCommand request, CancellationToken cancellationToken)
	{
		if (request.IsPrivateAccount)
		{
			request.NipNumber = null;
		}

		// pobranie użytkownika który będzie aktualizowany
		var userFromDbToUpdate = await context.Users
			.Include(x => x.Client)
			.Include(x => x.Address)
			.FirstOrDefaultAsync(x => x.Id == request.UserId, cancellationToken);

		userFromDbToUpdate.FirstName = request.FirstName;
		userFromDbToUpdate.LastName = request.LastName;

		// jeśli dane klienta jeszcze nie są uzupełnione, to zainicjalizuj je nowym obiektem
		userFromDbToUpdate.Client ??= new Domain.Entities.Client();
		
		userFromDbToUpdate.Client.IsPrivateAccount = request.IsPrivateAccount;
		userFromDbToUpdate.Client.NipNumber = request.NipNumber;
		userFromDbToUpdate.Client.UserId = request.UserId;

		// jeśli dane adresowe jeszcze nie są uzupełnione, to zainicjalizuj je nowym obiektem
		userFromDbToUpdate.Address ??= new Domain.Entities.Address();

		userFromDbToUpdate.Address.Country = request.Country;
		userFromDbToUpdate.Address.City = request.City;
		userFromDbToUpdate.Address.Street = request.Street;
		userFromDbToUpdate.Address.ZipCode = request.ZipCode;
		userFromDbToUpdate.Address.StreetNumber = request.StreetNumber;
		userFromDbToUpdate.Address.UserId = request.UserId;

		await context.SaveChangesAsync(cancellationToken);
		return Unit.Value;
	}
}