using Microsoft.AspNetCore.Http;

namespace GymManager.Application.Common.Interfaces;

public interface IHttpContext
{
	string AppBaseUrl { get; }
	string IpAddress { get; }

	// informacje o sesji, potrzebne to trzymania tokena z zewnętrznego Api
	ISession Session { get; }
}