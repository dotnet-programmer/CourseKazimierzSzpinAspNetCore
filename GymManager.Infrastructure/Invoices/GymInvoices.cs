using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using GymManager.Application.Common.Interfaces;
using GymManager.Application.Common.Models.Inovices;
using GymManager.Application.GymInvoices.Queries.GetPdfGymInvoice;
using GymManager.Application.Invoices.Commands.AddInvoice;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace GymManager.Infrastructure.Invoices;

public class GymInvoices : IGymInvoices
{
	private readonly HttpClient _httpClient;
	private readonly IConfiguration _configuration;
	private readonly ILogger<GymInvoices> _logger;
	private readonly IHttpContext _httpContext;
	private readonly IDateTimeService _dateTimeService;
	private readonly ICurrentUserService _currentUserService;
	private readonly IApplicationDbContext _context;
	private readonly string _baseUrl;

	public GymInvoices(
		HttpClient httpClient,
		IConfiguration configuration,
		ILogger<GymInvoices> logger,
		IHttpContext httpContext,
		IDateTimeService dateTimeService,
		ICurrentUserService currentUserService,
		IApplicationDbContext context)
	{
		_httpClient = httpClient;
		_configuration = configuration;
		_logger = logger;
		_httpContext = httpContext;
		_dateTimeService = dateTimeService;
		_currentUserService = currentUserService;
		_context = context;
		_baseUrl = _configuration.GetValue<string>("GymInvoices:BaseUrl");

		_httpClient.BaseAddress = new Uri(_baseUrl);
		_httpClient.Timeout = new TimeSpan(0, 0, 30);

		ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
	}

	// dodanie nowej faktury poprzez Api
	public async Task AddInvoice(string ticketId)
	{
		// najpierw trzeba ustawić token w nagłówku requesta, bo wszystkie endpointy w Api wymagają zalogowanego użytkownika
		await SetHeader();

		// przygotowanie obiektu który będzie przesłany do Api
		var jsonContent = JsonConvert.SerializeObject(new AddInvoiceCommand { TicketId = ticketId });

		// zmiana jsonContent na StringContent
		var stringContent = new StringContent(jsonContent, UnicodeEncoding.UTF8, "application/json");

		// wywołanie endpointa z Api
		// adres w tym parametrze zostanie doklejony do "_baseUrl"
		var response = await _httpClient.PostAsync("/api/v1/invoices", stringContent);

		// jeśli coś się nie udało to logowanie błędu do pliku i rzucenie wyjątku
		if (!response.IsSuccessStatusCode)
		{
			_logger.LogError(response.RequestMessage.ToString(), null);
			throw new Exception(response.RequestMessage.ToString());
		}
	}

	public async Task<InvoicePdfVm> GetPdfInvoice(int id)
	{
		await SetHeader();

		var response = await _httpClient.GetAsync($"/api/v1/invoices/pdf/{id}");

		if (!response.IsSuccessStatusCode)
		{
			_logger.LogError(response.RequestMessage.ToString(), null);
			throw new Exception(response.RequestMessage.ToString());
		}

		return JsonConvert.DeserializeObject<InvoicePdfVm>(await response.Content.ReadAsStringAsync());
	}

	private async Task SetHeader()
	{
		// wyczyszczenie domyślnego nagłówka requesta
		_httpClient.DefaultRequestHeaders.Clear();

		// sprawdzenie czy user wykonujący request ma już wygenerowany token
		// jeśli user nie ma tokena albo ma ale wygasł, to trzeba wygenerować nowy
		// tokeny będą trzymane w sesji

		// pobierz token z sesji
		var token = _httpContext.Session.GetString("JWToken");

		// jeśli brak tokena w sesji lub wygasł
		if (string.IsNullOrWhiteSpace(token) || TokenHasExpired(token))
		{
			// pobierz Id aktualnie zalogowanego użytkownika w aplikacji MVC
			var userId = _currentUserService.UserId;

			// jeśli nie udało się pobrać Id użytkownika
			if (string.IsNullOrWhiteSpace(userId))
			{
				throw new Exception("unauthorized");
			}

			// pobranie informacji o zalogowanym użytkowniku
			var user = await _context
				.Users
				.Select(x => new { Id = x.Id, UserName = x.UserName, Password = x.PasswordHash })
				.FirstOrDefaultAsync(x => x.Id == userId);

			// wygenerowanie nowego tokena
			token = (await GenerateToken(new AuthenticateRequest { UserName = user.UserName, Password = user.Password })).Token;

			// zapisanie nowego tokena w sesji
			_httpContext.Session.SetString("JWToken", token);
		}

		// przypisanie tokena do headera requesta, dzięki temu można wykonywać zapytania w Api
		_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
	}

	private bool TokenHasExpired(string token)
	{
		if (string.IsNullOrWhiteSpace(token))
		{
			return true;
		}

		// pobranie wartości tokena
		var jwtHandler = new JwtSecurityTokenHandler();
		var jwtToken = jwtHandler.ReadToken(token);
		// tutaj data wygaśnięcia tokena
		var expDate = jwtToken.ValidTo;

		// jeżeli ta data już upłynęła to "true", czyli token wygasł, w przeciwnym razie "false" czyli token dobry
		return expDate < _dateTimeService.Now.AddMinutes(1);
	}

	// w tej metodzie musi być wywołany endpoint z TokenController z WebAppi
	private async Task<AuthenticateResponse> GenerateToken(AuthenticateRequest authenticateRequest)
	{
		var jsonContent = JsonConvert.SerializeObject(authenticateRequest);
		_logger.LogInformation(jsonContent);
		var stringContent = new StringContent(jsonContent, UnicodeEncoding.UTF8, "application/json");

		// wywołanie endpointa z Api
		// adres w tym parametrze zostanie doklejony do "_baseUrl"
		var response = await _httpClient.PostAsync("/api/v1/tokens", stringContent);

		if (!response.IsSuccessStatusCode)
		{
			_logger.LogError(response.RequestMessage.ToString(), null);
			throw new Exception(response.RequestMessage.ToString());
		}

		// typ zwracany z endpointa to AuthenticateResponse zserializowany w Json,
		// dlatego trzeba go deserializować i zwrócić obiekt tej klasy
		return JsonConvert.DeserializeObject<AuthenticateResponse>(await response.Content.ReadAsStringAsync());
	}
}