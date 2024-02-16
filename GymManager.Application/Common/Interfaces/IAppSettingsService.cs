namespace GymManager.Application.Common.Interfaces;

// interfejs potrzebny żeby udostępnić metody z warstwy Infrastructure w warstwie Application
public interface IAppSettingsService
{
	// pobieranie wartości ze słownika na podstawie klucza
	Task<string> Get(string key);

	// aktualizacja słownika danymi z bazy danych
	Task Update(IApplicationDbContext context);
}