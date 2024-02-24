using MediatR;

namespace GymManager.Application.Files.Queries.GetFiles;

public class GetFilesQuery : IRequest<IEnumerable<FileDto>>
{
}