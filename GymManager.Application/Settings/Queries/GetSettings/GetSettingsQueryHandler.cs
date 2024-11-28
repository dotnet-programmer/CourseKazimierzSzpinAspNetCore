using GymManager.Application.Common.Interfaces;
using GymManager.Application.Settings.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Settings.Queries.GetSettings;

public class GetSettingsQueryHandler : IRequestHandler<GetSettingsQuery, IList<SettingsDto>>
{
	private readonly IApplicationDbContext _context;

	public GetSettingsQueryHandler(IApplicationDbContext context) =>
		_context = context;

	public async Task<IList<SettingsDto>> Handle(GetSettingsQuery request, CancellationToken cancellationToken)
		=> await _context.Settings
			.AsNoTracking()
			.Include(x => x.Positions.OrderBy(y => y.Order))
			.OrderBy(x => x.Order)
			.Select(x => x.ToDto())
			.ToListAsync();
}