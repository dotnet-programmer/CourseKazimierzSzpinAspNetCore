using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using GymManager.Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace GymManager.UI.Extensions;

public static class IDataTablesRequestExtensions
{
	// zwróc numer strony
	public static int GetPageNumber(this IDataTablesRequest request)
		=> request.Start / request.Length + 1;

	// zwróć informację o tym w jaki sposób posortować dane
	public static string GetOrderInfo(this IDataTablesRequest request)
	{
		var columnSort = request?.Columns?.FirstOrDefault(x => x.Sort != null);

		// te informacje są potrzebne żeby zbudować sortowanie za pomocą Dynamic LINQ w Handlerze
		var columnName = string.Empty;
		var orderDirection = string.Empty;

		if (columnSort != null)
		{
			columnName = columnSort.Name;
			orderDirection = columnSort.Sort.Direction == SortDirection.Ascending ? "asc" : "desc";
		}

		return $"{columnName} {orderDirection}";
	}

	// na podstawie modelu zostanie zwrócony odpowiedni JSON do widoku z tabelą DaaTables
	public static IActionResult GetResponse<T>(this IDataTablesRequest request, PaginatedList<T> paginatedList)
	{
		// typ DataTablesResponse jest w pakiecie NuGet - DataTables.AspNet.AspNetCore
		var response = DataTablesResponse.Create(request, paginatedList.TotalCount, paginatedList.TotalCount, paginatedList.Items);
		return new DataTablesJsonResult(response, true);
	}
}