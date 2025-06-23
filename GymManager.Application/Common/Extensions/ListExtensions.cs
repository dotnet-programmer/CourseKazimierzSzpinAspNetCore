namespace GymManager.Application.Common.Extensions;

public static class ListExtensions
{
	// metoda rozszerzająca do porównywania ról między sobą
	// T - typ generyczny
	// TKey - klucz po którym będą porównywane listy
	public static List<T> Except<T, TKey>(this List<T> items, List<T> other, Func<T, TKey> getKeyFunc)
		=> items
			.GroupJoin(other, getKeyFunc, getKeyFunc, (item, tempItems) => new { item, tempItems })
			.SelectMany(x => x.tempItems.DefaultIfEmpty(), (x, temp) => new { x, temp })
			.Where(x => ReferenceEquals(null, x.temp) || x.temp.Equals(default(T)))
			.Select(x => x.x.item)
			.ToList();
}