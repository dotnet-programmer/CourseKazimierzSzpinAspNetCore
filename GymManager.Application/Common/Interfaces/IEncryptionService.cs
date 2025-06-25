namespace GymManager.Application.Common.Interfaces;

// szyfrowanie danych
public interface IEncryptionService
{
	// szyfrowanie tekstu
	string Encrypt(string input);

	// deszyfrowanie zaszyfrowanego tekstu
	string Decrypt(string cipherText);
}