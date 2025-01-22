namespace GymManager.Application.Common.Interfaces;

// szyfrowanie danych
public interface IEncryptionService
{
	// szyfrowanie
	string Encrypt(string input);

	// deszyfrowanie
	string Decrypt(string cipherText);
}