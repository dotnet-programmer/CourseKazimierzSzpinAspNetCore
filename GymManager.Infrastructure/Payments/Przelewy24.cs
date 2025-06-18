using System.Net;
using System.Security.Cryptography;
using System.Text;
using GymManager.Application.Common.Interfaces;
using GymManager.Application.Common.Models.Payments;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Formatting = Newtonsoft.Json.Formatting;

namespace GymManager.Infrastructure.Payments;
public class Przelewy24 : IPrzelewy24
{
	private readonly HttpClient _httpClient;
	private readonly IConfiguration _configuration;
	private readonly ILogger _logger;
	private readonly IEncryptionService _encryptionService;
	private string _crc;
	private string _userName;
	private string _userSecret;
	private string _baseUrl;
	private JsonSerializerSettings _jsonSettings;

	// użycie HttpClient - najlepsze użycie to wstrzyknięcie przez konstruktor
	// + w Dependency Injection dodać services.AddHttpClient<IPrzelewy24, Przelewy24>(); - to powoduje użycie fabryki HttpClient
	public Przelewy24(
		HttpClient httpClient,
		IConfiguration configuration,
		ILogger<Przelewy24> logger,
		IEncryptionService encryptionService)
	{
		_httpClient = httpClient;
		_configuration = configuration;
		_logger = logger;
		_encryptionService = encryptionService;
		GetConfiguration();
		InitHttpClient();
		InitJsonSettings();

		//ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
	}

	public async Task<P24TransactionResponse> NewTransactionAsync(P24TransactionRequest data)
	{
		data.MerchantId = int.Parse(_userName);
		data.PosId = int.Parse(_userName);

		string signString = $"{{\"sessionId\":\"{data.SessionId}\",\"merchantId\":{data.MerchantId},\"amount\":{data.Amount},\"currency\":\"{data.Currency}\",\"crc\":\"{_crc}\"}}";
		data.Sign = GenerateSign(signString);

		string jsonContent = JsonConvert.SerializeObject(data, _jsonSettings);
		StringContent stringContent = new(jsonContent, UnicodeEncoding.UTF8, "application/json");

		var response = await _httpClient.PostAsync("/api/v1/transaction/register", stringContent);
		if (!response.IsSuccessStatusCode)
		{
			_logger.LogError(response.RequestMessage.ToString(), null);
		}

		return JsonConvert.DeserializeObject<P24TransactionResponse>(await response.Content.ReadAsStringAsync());
	}

	public async Task<P24TestAccessResponse> TestConnectionAsync()
	{
		var response = await _httpClient.GetAsync("/api/v1/testAccess");
		if (!response.IsSuccessStatusCode)
		{
			_logger.LogError(response.RequestMessage.ToString(), null);
		}

		return JsonConvert.DeserializeObject<P24TestAccessResponse>(await response.Content.ReadAsStringAsync());
	}

	private void GetConfiguration()
	{
		_crc = _configuration.GetValue<string>("Przelewy24:Crc");
		_userName = _configuration.GetValue<string>("Przelewy24:UserName");
		_userSecret = _encryptionService.Decrypt(_configuration.GetValue<string>("Przelewy24:UserSecret"));
		_baseUrl = _configuration.GetValue<string>("Przelewy24:BaseUrl");
	}

	private void InitHttpClient()
	{
		_httpClient.BaseAddress = new Uri(_baseUrl);
		_httpClient.Timeout = new TimeSpan(0, 0, 30);
		_httpClient.DefaultRequestHeaders.Clear();
		_httpClient.DefaultRequestHeaders.Authorization = new("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_userName}:{_userSecret}")));
	}

	private void InitJsonSettings() =>
		_jsonSettings = new JsonSerializerSettings
		{
			ContractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() },
			Formatting = Formatting.Indented,
		};

	private string GenerateSign(string signString)
	{
		using (SHA384 sha384Hash = SHA384.Create())
		{
			byte[] sourceBytes = Encoding.UTF8.GetBytes(signString);
			byte[] hashBytes = sha384Hash.ComputeHash(sourceBytes);
			string hash = BitConverter.ToString(hashBytes).Replace("-", "");
			return hash.ToLower();
		}
	}

	public async Task<P24TransactionVerifyResponse> TransactionVerifyAsync(P24TransactionVerifyRequest data)
	{
		string signString = $"{{\"sessionId\":\"{data.SessionId}\",\"orderId\":{data.OrderId},\"amount\":{data.Amount},\"currency\":\"{data.Currency}\",\"crc\":\"{_crc}\"}}";
		data.Sign = GenerateSign(signString);
		string jsonContent = JsonConvert.SerializeObject(data, _jsonSettings);
		StringContent stringContent = new(jsonContent, UnicodeEncoding.UTF8, "application/json");
		var response = await _httpClient.PutAsync("/api/v1/transaction/verify", stringContent);
		if (!response.IsSuccessStatusCode)
		{
			_logger.LogError(response.RequestMessage.ToString(), null);
		}
		return JsonConvert.DeserializeObject<P24TransactionVerifyResponse>(await response.Content.ReadAsStringAsync());
	}
}
