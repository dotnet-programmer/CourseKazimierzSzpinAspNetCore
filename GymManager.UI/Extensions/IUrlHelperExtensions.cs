using Microsoft.AspNetCore.Mvc;

namespace GymManager.UI.Extensions;

// dynamiczna aktywacja zakładki
public static class IUrlHelperExtensions
{
	// przekazywanie nazwy kontrolera i akcji;
	// jeżeli kontroler i akcja są takie same jak w obecnym adresie wywołania (URL) to zwróci klasę "active" 
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

			if (action == null || !action.Any())
			{
				return null;
			}

			// porównanie czy nazwy kontrolerów się zgadzają
			// controllerName - nazwa kontrolera z adresu URL
			// controller - przekazany parametr
			if (controllerName.Equals(controller, StringComparison.OrdinalIgnoreCase))
			{
				// sprawdzenie czy lista akcji przekazanych w parametrze zawiera nazwę metody z URL
				// methodName - nazwa akcji z adresu URL
				// actions - lista akcji przekazana jako parametr
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