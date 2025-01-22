using GymManager.Application.Common.Interfaces;
using GymManager.Application.Settings.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Settings.Queries.GetSettings;

public class GetSettingsQueryHandler(IApplicationDbContext context) : IRequestHandler<GetSettingsQuery, IList<SettingsDto>>
{
	public async Task<IList<SettingsDto>> Handle(GetSettingsQuery request, CancellationToken cancellationToken)
		=> await context.Settings
			.AsNoTracking()
			.Include(x => x.Positions.OrderBy(y => y.Order))
			.OrderBy(x => x.Order)
			.Select(x => x.ToDto())
			.ToListAsync(cancellationToken);
}