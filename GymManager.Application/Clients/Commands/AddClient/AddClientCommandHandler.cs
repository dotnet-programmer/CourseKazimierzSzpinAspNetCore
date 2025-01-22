using GymManager.Application.Common.Interfaces;
using GymManager.Application.Dictionaries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Clients.Commands.AddClient;

public class AddClientCommandHandler(
	IApplicationDbContext context,
	IUserManagerService userManagerService,
	IDateTimeService dateTimeService) : IRequestHandler<AddClientCommand>
{
	public async Task Handle(AddClientCommand request, CancellationToken cancellationToken)
	{
		if (request.IsPrivateAccount)
		{
			request.NipNumber = null;
		}

		// stworzenie nowego klienta z podstawowymi danymi w bazie i zwrócenie jego ID
		var userId = await userManagerService.CreateAsync(request.Email, request.Password, RolesDict.Client);

		// wybranie z bazy nowo utworzonego klienta
		var user = await context.Users
			.Include(x => x.Client)
			.Include(x => x.Address)
			.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

		// ustawienie jego właściwości z pól formularza
		user.FirstName = request.FirstName;
		user.LastName = request.LastName;
		user.RegisterDateTime = dateTimeService.Now;
		// jeśli pracownik dodaje nowego klienta to ten ma od razu potwierdzony adres
		user.EmailConfirmed = true;

		user.Client = new Domain.Entities.Client
		{
			IsPrivateAccount = request.IsPrivateAccount,
			NipNumber = request.NipNumber,
			UserId = userId
		};

		user.Address = new Domain.Entities.Address
		{
			Country = request.Country,
			City = request.City,
			Street = request.Street,
			ZipCode = request.ZipCode,
			StreetNumber = request.StreetNumber,
			UserId = userId
		};

		await context.SaveChangesAsync(cancellationToken);
	}
}