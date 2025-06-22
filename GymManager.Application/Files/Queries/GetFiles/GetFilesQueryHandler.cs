using GymManager.Application.Common.Interfaces;
using GymManager.Application.Files.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Files.Queries.GetFiles;

public class GetFilesQueryHandler(IApplicationDbContext context) : IRequestHandler<GetFilesQuery, IEnumerable<FileDto>>
{
	public async Task<IEnumerable<FileDto>> Handle(GetFilesQuery request, CancellationToken cancellationToken)
		=> await context.Files
			.AsNoTracking()
			.OrderByDescending(x => x.FileId)
			.Select(x => x.ToDto())
			.ToListAsync(cancellationToken);
}