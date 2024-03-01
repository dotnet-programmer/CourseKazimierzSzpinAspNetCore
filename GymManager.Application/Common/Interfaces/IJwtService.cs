using GymManager.Application.Common.Models.Inovices;

namespace GymManager.Application.Common.Interfaces;

public interface IJwtService
{
	// na podstawie Id użytkownika będzie generowany token, który będzie zwracany opakowany w klasie AuthenticateResponse
	AuthenticateResponse GenerateJwtToken(string userId);
}