using System.Security.Claims;
using GymManager.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GymManager.Infrastructure.Services;

// Pobieranie informacji o zalogowanym użytkowniku bez zapytań na bazie danych
public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
	private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

	// dostęp do danych użytkownika polega na odczytaniu wartości z poświadczeń HttpContext 
	// jeżeli użytkownik jest zalogowany to jest bezpośredni dostęp do niego w każdym requeście
	public string UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

	public string UserName => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
}