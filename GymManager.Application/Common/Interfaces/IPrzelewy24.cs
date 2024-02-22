using GymManager.Application.Common.Models.Payments;

namespace GymManager.Application.Common.Interfaces;

public interface IPrzelewy24
{
	Task<P24TestAccessResponse> TestConnectionAsync();
	Task<P24TransactionResponse> NewTransactionAsync(P24TransactionRequest data);
}