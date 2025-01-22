using MediatR;

namespace GymManager.Application.Users.Commands.DeleteUser;

public class DeleteUserCommand : IRequest
{
	public string Id { get; set; }
}
