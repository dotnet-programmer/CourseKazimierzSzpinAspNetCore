using GymManager.Application.Common.Models.Payments;

namespace GymManager.Application.Common.Interfaces;

public interface IPrzelewy24
{
	// do przetestowania poprawności ustawień
	Task<P24TestAccessResponse> TestConnectionAsync();

	// do utworzenia nowej transakcji
	Task<P24TransactionResponse> NewTransactionAsync(P24TransactionRequest data);

	// weryfikacja transakcji
	Task<P24TransactionVerifyResponse> TransactionVerifyAsync(P24TransactionVerifyRequest data);
}