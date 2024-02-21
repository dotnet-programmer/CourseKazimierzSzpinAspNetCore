using Microsoft.EntityFrameworkCore;

namespace GymManager.Application.Common.Models;

// klasa generyczna, używana dla dowolnych typów gdy będzie potrzebne stronicowanie w konkretnych kwerendach
public class PaginatedList<T>
{
	public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
	{
		PageIndex = pageIndex;
		TotalPages = (int)Math.Ceiling(count / (double)pageSize);
		TotalCount = count;
		Items = items;
	}

	// lista generyczna z elementami listy
	public List<T> Items { get; }
	public int PageIndex { get; }
	public int TotalPages { get; }
	public int TotalCount { get; }
	public bool HasPreviousPage => PageIndex > 1;
	public bool HasNextPage => PageIndex < TotalPages;

	// metoda statyczna która będzie tworzyła tą listę
	public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
	{
		var count = await source.CountAsync();

		// pobieranie konkretnych danych z odpowiedniej strony
		var items = await source
			.Skip((pageIndex - 1) * pageSize)
			.Take(pageSize)
			.ToListAsync();

		return new PaginatedList<T>(items, count, pageIndex, pageSize);
	}
}