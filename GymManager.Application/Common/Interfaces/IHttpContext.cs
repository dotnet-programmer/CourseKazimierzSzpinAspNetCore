namespace GymManager.Application.Common.Interfaces;

public interface IHttpContext
{
	string AppBaseUrl { get; }
	string IpAddress { get; }
}