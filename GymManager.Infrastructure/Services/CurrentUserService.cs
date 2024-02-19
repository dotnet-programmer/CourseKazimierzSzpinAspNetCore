using System.Security.Claims;
using GymManager.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GymManager.Infrastructure.Services;

// INFO - Pobieranie informacji o zalogowanym użytkowniku bez zapytań na bazie danych
public class CurrentUserService : ICurrentUserService
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	// IHttpContextAccessor - dzięki niemu można odnieść się do kontekstu i na jego podstawie pobrać ID i nazwę zalogowanego użytkownika
	public CurrentUserService(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

	// dostęp do danych użytkownika polega na odczytaniu wartości z poświadczeń HttpContext 
	// jeżeli użytkownik jest zalogowany to jest bezpośredni dostęp do niego w każdym requeście
	public string UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
	public string UserName => _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
}