using GymManager.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Files.Commands.DeleteFile;

public class DeleteFileCommandHandler(IApplicationDbContext context, IFileManagerService fileManagerService) : IRequestHandler<DeleteFileCommand>
{
	// usuwanie najpierw z serwera a później z bazy danych
	public async Task Handle(DeleteFileCommand request, CancellationToken cancellationToken)
	{
		var fileDb = await context.Files.FirstOrDefaultAsync(x => x.FileId == request.Id, cancellationToken);
		fileManagerService.Delete(fileDb.Name);
		context.Files.Remove(fileDb);
		await context.SaveChangesAsync(cancellationToken);
	}
}