using GymManager.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GymManager.Infrastructure.Services;

// statyczny słownik dla danych pobieranych z bazy danych żeby nie pobierać za każdym razem tych samych danych,
// statyczny, czyli każdy użytkownik będzie pracował na tych samych danych,
// jeśli podział na użytkowników, to trzeba dodać wrappera i filtrować dane po jakimś Id użytkownika,
// w tym przypadku będą przechowywane wszystkie rekordy z tabeli SettingsPosition
public class AppSettingsService : IAppSettingsService
{
	// słownik na dane z tabeli SettingsPosition
	private readonly Dictionary<string, string> _appSettings = [];

	#region blokada wątku dla 1 użytkownika
	// do tego serwisu mogą odnosić się w tym samym czasie różni użytkownicy z różnych wątków,
	// dlatego zrobić słownik bezpieczny wątkowo

	// tylko 1 użytkownik w 1 czasie ma dostęp do słownika
	private static readonly SemaphoreSlim _semaphore = new(1, 1);

	// metody pomocnicze
	// jeżeli ktoś chce pobrać dane to wywołuje metodę WaitAsync, co blokuje dostęp do tego miejsca innym użytkownikom
	private async Task Wait() => await _semaphore.WaitAsync();

	// jak ktoś dane już pobierze to wywołuje metodę Release, co zwalnia blokadę wątku
	private void Release() => _semaphore.Release();
	#endregion

	// pobieranie wartości ze słownika na podstawie klucza
	public async Task<string> GetValueByKeyAsync(string key)
	{
		try
		{
			await Wait();
			return _appSettings[key];
		}
		finally
		{
			Release();
		}
	}

	// aktualizacja słownika danymi z bazy danych
	public async Task Update(IApplicationDbContext context)
	{
		try
		{
			await Wait();

			_appSettings.Clear();

			var settingsPositions = await context.SettingsPositions
				.AsNoTracking()
				.ToListAsync();

			foreach (var position in settingsPositions)
			{
				_appSettings.Add(position.Key, position.Value);
			}
		}
		finally
		{
			Release();
		}
	}
}