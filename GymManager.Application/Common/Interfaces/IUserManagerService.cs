namespace GymManager.Application.Common.Interfaces;

// nakładka na metody z Identity
public interface IUserManagerService
{
	Task<string> CreateAsync(string email, string password, string role);
}