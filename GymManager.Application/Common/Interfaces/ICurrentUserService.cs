namespace GymManager.Application.Common.Interfaces;

// INFO - Pobieranie informacji o zalogowanym użytkowniku bez zapytań na bazie danych
public interface ICurrentUserService
{
	string UserId { get; }
	string UserName { get; }
}