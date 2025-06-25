using System.Security.Cryptography;

namespace GymManager.Infrastructure.Encryption;

// klasa do generowania nowych kluczy do szyfrowania
public class KeyInfo
{
	public byte[] Key { get; }
	public byte[] Iv { get; }

	public string KeyString
		=> Convert.ToBase64String(Key);

	public string IVString
		=> Convert.ToBase64String(Iv);

	// konstruktor bezparametrowy generuje nowy klucz i wektor inicjalizacyjny
	public KeyInfo()
	{
		using (var myAes = Aes.Create())
		{
			Key = myAes.Key;
			Iv = myAes.IV;
		}
	}

	// konstruktor przyjmujący klucz i wektor inicjalizacyjny w postaci base64 i konwertowujący je na tablice bajtów
	public KeyInfo(string key, string iv)
	{
		Key = Convert.FromBase64String(key);
		Iv = Convert.FromBase64String(iv);
	}
}