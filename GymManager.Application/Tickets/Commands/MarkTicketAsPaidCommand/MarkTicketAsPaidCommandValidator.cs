using FluentValidation;
using GymManager.Application.Common.Interfaces;

namespace GymManager.Application.Tickets.Commands.MarkTicketAsPaidCommand;

public class MarkTicketAsPaidCommandValidator : AbstractValidator<MarkTicketAsPaidCommand>
{
	private readonly IHttpContext _httpContext;

	public MarkTicketAsPaidCommandValidator(IHttpContext httpContext)
	{
		_httpContext = httpContext;

		RuleFor(x => x.IsProduction)
			.Must(SendFromValidIpAddress)
			.WithMessage("Przelew24 – not allowed IP");
	}

	// sprawdzenie Ip z którego przychodzi request
	private bool SendFromValidIpAddress(bool isProduction)
	{
		// lista adresów IP z dokumentacji
		List<string> allowedIps =
		[
			"91.216.191.181",
			"91.216.191.182",
			"91.216.191.183",
			"91.216.191.184",
			"91.216.191.185",
			"5.252.202.255"
		];

		if (!isProduction)
		{
			// tak żeby mógł obsługiwać localhosta
			allowedIps.Add("::1");
		}

		return allowedIps.Contains(_httpContext.IpAddress);
	}
}