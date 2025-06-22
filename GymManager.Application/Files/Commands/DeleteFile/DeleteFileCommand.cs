using MediatR;

namespace GymManager.Application.Files.Commands.DeleteFile;

public class DeleteFileCommand : IRequest<Unit>
{
	public int Id { get; set; }
}