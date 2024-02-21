using GymManager.Application.Common.Models;

namespace GymManager.Application.Common.Extensions;

// rozszerzenie ułatwiające wywołanie klasy PaginatedList
public static class IQueryableExtensions
{
	// metoda ułatwiająca zamienianie listy IQueryable na odpowiednia stronę w tabeli (paginacja danych)
	public static async Task<PaginatedList<T>> PaginatedListAsync<T>(this IQueryable<T> queryable, int pageNumber, int pageSize) =>
		await PaginatedList<T>.CreateAsync(queryable, pageNumber, pageSize);
}