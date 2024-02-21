using GymManager.Application.Clients.Queries.GetClient;
using GymManager.Domain.Entities;

namespace GymManager.Application.Users.Extensions;

public static class UserExtensions
{
	public static ClientDto ToClientDto(this ApplicationUser user) =>
		user == null ?
		null :
		new ClientDto
		{
			Id = user.Id,
			City = user.Address?.City,
			Country = user.Address?.Country,
			Street = user.Address?.Street,
			StreetNumber = user.Address?.StreetNumber,
			ZipCode = user.Address?.ZipCode,
			Email = user.Email,
			FirstName = user.FirstName,
			LastName = user.LastName,
			NipNumber = user.Client?.NipNumber,
			IsPrivateAccount = user.Client?.IsPrivateAccount ?? true
		};
}