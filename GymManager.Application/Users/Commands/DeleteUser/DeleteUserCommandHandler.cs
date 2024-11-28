using GymManager.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Users.Commands.DeleteUser;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
{
	private readonly IApplicationDbContext _context;

	public DeleteUserCommandHandler(IApplicationDbContext context) =>
		_context = context;

	public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
	{
		var user = await _context.Users
			.FirstOrDefaultAsync(x => x.Id == request.Id);

		user.IsDeleted = true;

		await _context.SaveChangesAsync(cancellationToken);

		return;
	}
}