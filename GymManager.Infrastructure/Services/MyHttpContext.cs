using GymManager.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GymManager.Infrastructure.Services;

public class MyHttpContext : IHttpContext
{
	private readonly IHttpContextAccessor _httpContextAccessor;
	private HttpContext Current => _httpContextAccessor.HttpContext;

	public MyHttpContext(IHttpContextAccessor httpContextAccessor) => 
		_httpContextAccessor = httpContextAccessor;

	public string AppBaseUrl => $"{Current.Request.Scheme}://{Current.Request.Host}{Current.Request.PathBase}";

	public string IpAddress => Current.Connection.RemoteIpAddress.ToString();

	// informacje o sesji, potrzebne to trzymania tokena z zewnętrznego Api
	public ISession Session => Current.Session;
}