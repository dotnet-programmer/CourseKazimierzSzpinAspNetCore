namespace GymManager.Application.Common.Interfaces;

// INFO - szyfrowanie danych
public interface IEncryptionService
{
	// szyfrowanie
	string Encrypt(string input);

	// deszyfrowanie
	string Decrypt(string cipherText);
}