using Microsoft.AspNetCore.Mvc;

namespace GymManager.UI.Components;

// INFO - komponent
public class MainCarouselViewComponent : ViewComponent
{
	public async Task<IViewComponentResult> InvokeAsync(string priority)
	{
		// jakaś logika
		// pobranie z bazy danych
		int timeInterval = 1500;
		return View(timeInterval);
	}
}