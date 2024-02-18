using Microsoft.AspNetCore.Mvc;

namespace GymManager.UI.Extensions;

// INFO - dynamiczna aktywacja zakładki
public static class IUrlHelperExtensions
{
	public static string MakeActiveClass(this IUrlHelper urlHelper, string controller, string action)
	{
		try
		{
			// nazwa klasy która będzie dopisywana do elementu HTML
			var result = "active";

			// pobranie nazwy kontrolera który został wywołany
			var controllerName = urlHelper.ActionContext.RouteData.Values["controller"].ToString();

			// pobranie nazwy akcji która została wywołana
			var methodName = urlHelper.ActionContext.RouteData.Values["action"].ToString();

			if (string.IsNullOrEmpty(controllerName))
			{
				return null;
			}

			// pobranie nazwy akcji z parametru przekazanego do metody, bo kontroler jest 1 ale akcji może być kilka
			var actions = action.Split(',')?
				.ToList()
				.Select(x => x.ToUpper());

			// TODO - albo to przez wykonaniem Split, albo tu chyba powinno być sprawdzanie actions
			if (action == null || !action.Any())
			{
				return null;
			}

			// porównanie czy nazwy się zgadzają
			if (controllerName.Equals(controller, StringComparison.OrdinalIgnoreCase))
			{
				// sprawdzenie czy jest taka akcja w parametrze
				if (actions.Contains(methodName.ToUpper()))
				{
					return result;
				}
			}

			return null;
		}
		catch (Exception)
		{
			return null;
		}
	}
}