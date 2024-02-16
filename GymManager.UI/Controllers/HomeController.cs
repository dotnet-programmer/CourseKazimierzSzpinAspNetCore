using Microsoft.AspNetCore.Mvc;

namespace GymManager.UI.Controllers;

// INFO - routing - można go definiować nad nazwą kontrolera
// wtedy dotyczy wszystkich akcji w kontrolerze, które nie mają nadpisanego routingu
//[Route("test/[controller]/[action]")]
public class HomeController : BaseController
{
	private readonly ILogger _logger;

	// konstruktor - dependency injection - wstrzyknięcie klasy NLoga
	public HomeController(ILogger<HomeController> logger)
	{
		_logger = logger;
	}

	// widok strona główna
	public IActionResult Index()
	{
		return View();
	}

	// widok polityka prywatności
	public IActionResult Privacy()
	{
		return View();
	}

	// widok kontakt
	public IActionResult Contact()
	{
		return View();
	}

	// testowa akcja Index
	//public async Task<IActionResult> Index()
	//{
	//	// symulacja błędu
	//	//throw new Exception("Nieobsłużony błąd!");

	//	// INFO - wywołanie kwerendy
	//	// parametr to nazwa kwerendy, czyli klasa Query
	//	//var ticket = await Mediator.Send(new GetTicketByIdQuery { TicketId = 1 });

	//	// INFO - wywołanie kwerendy
	//	//await Mediator.Send(new AddTicketCommand { Name = "Ticket1" });

	//	// INFO - użycie NLog
	//	//_logger.LogInformation("LogInformation");
	//	//_logger.LogError(new Exception("LogError"), null);

	//	return View();
	//}

	// INFO - routing - definiowanie ścieżki nad konkretną akcją

	// przekazanie w jaki sposób ma zostać przekazany ten parametr
	// czyli nie ma jawnego przypisania wartości do parametru - www.adres.pl/home/index2?test=wartosc
	// tylko adres będzie wyglądał www.adres.pl/wartosc
	//[Route("{test}")]

	// tutaj adres będzie wyglądał www.adres.pl/indextest/wartosc
	//[Route("indextest{test}")]

	// oczekiwanie że w adresie zostanie przekazana też nazwa kontrolera i nazwa akcji
	// tutaj adres będzie wyglądał www.adres.pl/home/index2/wartosc
	//[Route("[controller]/[action]/{test}")]

	// Zamiast atrybutu Route można w zależności od typu akcji użyć od razu HttpGet
	//[HttpGet("[controller]/[action]/{test}")]

	// jeżeli routing jest ustawiony dla całego kontrolera i jednocześnie dla akcji i ten nie zaczyna się od slash - "/",
	// to adres musi zawierać całą ścieżke powstałą przez połączenie tych adresów
	// przykład: kontroler - [Route("test/[controller]/[action]")], akcja - [HttpGet("[controller]/[action]/{test}")]
	// efekt: www.adres.pl/test/home/index2/home/index2/wartosc

	// jeżeli routing jest ustawiony dla całego kontrolera i jednocześnie dla akcji i ten zaczyna się od slash - "/",
	// to adres nie musi zawierać całej ścieżki i nie łączy tych adresów
	// przykład: kontroler - [Route("test/[controller]/[action]")], akcja - [HttpGet("/[controller]/[action]/{test}")]
	// efekt: www.adres.pl/test/home/index2/wartosc
	//public IActionResult Index2(string test)
	//{
	//	return View("Index");
	//}

	// to jest nie potrzebne bo jest własna obsługa błędów
	//[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	//public IActionResult Error()
	//{
	//	return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
	//}
}