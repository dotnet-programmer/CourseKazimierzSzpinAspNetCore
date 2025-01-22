using GymManager.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler(IApplicationDbContext context) : IRequestHandler<DeleteUserCommand>
{
	public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
	{
		var user = await context.Users
			.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

		user.IsDeleted = true;

		await context.SaveChangesAsync(cancellationToken);
	}
}