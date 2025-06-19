using System.Security.Claims;
using GymManager.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace GymManager.Infrastructure.Services;

// Pobieranie informacji o zalogowanym użytkowniku bez zapytań na bazie danych
// dzięki IHttpContextAccessor można odnieść się do bieżącego kontekstu HTTP i na tej podstawie pobierać informacje o użytkowniku
public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
	// dostęp do danych użytkownika polega na odczytaniu wartości z poświadczeń HttpContext
	// jeżeli użytkownik jest zalogowany to jest bezpośredni dostęp do niego w każdym requeście
	public string UserId
		=> httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

	public string UserName
		=> httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
}