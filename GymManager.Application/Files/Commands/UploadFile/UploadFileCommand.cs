using MediatR;
using Microsoft.AspNetCore.Http;

namespace GymManager.Application.Files.Commands.UploadFile;

public class UploadFileCommand : IRequest
{
	public IEnumerable<IFormFile> Files { get; set; }
}