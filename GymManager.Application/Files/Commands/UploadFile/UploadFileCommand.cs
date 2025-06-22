using MediatR;
using Microsoft.AspNetCore.Http;

namespace GymManager.Application.Files.Commands.UploadFile;

// z kontrolera zostaną przekazane wybrane pliki
public class UploadFileCommand : IRequest<Unit>
{
	public IEnumerable<IFormFile> Files { get; set; }
}