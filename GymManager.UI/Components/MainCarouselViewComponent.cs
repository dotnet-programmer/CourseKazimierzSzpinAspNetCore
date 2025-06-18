using GymManager.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GymManager.UI.Components;

// komponent to zaawansowany widok częściowy, który może mieć swoją logikę, całkowicie oddzieloną od kontrolera
// jego logika jest dedykowana tylko dla danego komponentu
// dzięki temu można ich używać w wielu miejscach i nie trzeba za każdym razem dodawać logiki do kontrolera

// konwencja jest taka, że w folderze "Components" nazwa komponentu zaczyna się od "Main" i kończy na "ViewComponent"
// klasa musi dziedziczyć po ViewComponent
// W Views/Shared musi być folder o nazwie "Components", w nim nowy folder o nazwie komponentu, a tam widok o nazwie "Default.cshtml"
// w pliku _ViewImports.cshtml musi być dodany namespace do komponentu, np. @addTagHelper *, GymManager.UI

// użycie komponentu:
//<vc:main-carousel priority="2" />
//main-carousel - nazwa komponentu
//priority="2" - przekazanie parametru do wywołania funkcji InvokeAsync komponentu 

public class MainCarouselViewComponent : ViewComponent
{
	// można przekazać jakieś parametry do komponentu
	public async Task<IViewComponentResult> InvokeAsync(string priority)
	{
		// jakaś logika, np. pobranie z bazy danych
		// int timeInterval = 1500;
		return View();
	}
}