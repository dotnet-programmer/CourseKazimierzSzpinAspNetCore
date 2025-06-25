using System.Security.Cryptography;
using GymManager.Application.Common.Interfaces;

namespace GymManager.Infrastructure.Encryption;

public class EncryptionService(KeyInfo keyInfo) : IEncryptionService
{
	// szyfrowanie tekstu
	public string Encrypt(string input)
		=> Convert.ToBase64String(EncryptStringToBytesAes(input, keyInfo.Key, keyInfo.Iv));

	// deszyfrowanie zaszyfrowanego tekstu
	public string Decrypt(string cipherText)
		=> DecryptStringFromBytesAes(Convert.FromBase64String(cipherText), keyInfo.Key, keyInfo.Iv);

	private static byte[] EncryptStringToBytesAes(string plainText, byte[] key, byte[] iv)
	{
		if (plainText == null || plainText.Length <= 0)
		{
			throw new ArgumentNullException(nameof(plainText));
		}

		if (key == null || key.Length <= 0)
		{
			throw new ArgumentNullException(nameof(key));
		}

		if (iv == null || iv.Length <= 0)
		{
			throw new ArgumentNullException(nameof(iv));
		}

		byte[] encrypted;

		using (var aesAlg = Aes.Create())
		{
			aesAlg.Key = key;
			aesAlg.IV = iv;

			var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
			using (var msEncrypt = new MemoryStream())
			{
				using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
				{
					using (var swEncrypt = new StreamWriter(csEncrypt))
					{
						swEncrypt.Write(plainText);
					}
					encrypted = msEncrypt.ToArray();
				}
			}
		}

		return encrypted;
	}

	private static string DecryptStringFromBytesAes(byte[] cipherText, byte[] key, byte[] iv)
	{
		if (cipherText == null || cipherText.Length <= 0)
		{
			throw new ArgumentNullException(nameof(cipherText));
		}

		if (key == null || key.Length <= 0)
		{
			throw new ArgumentNullException(nameof(key));
		}

		if (iv == null || iv.Length <= 0)
		{
			throw new ArgumentNullException(nameof(iv));
		}

		string plaintext;
		using (var aesAlg = Aes.Create())
		{
			aesAlg.Key = key;
			aesAlg.IV = iv;

			var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
			using (var msDecrypt = new MemoryStream(cipherText))
			{
				using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
				{
					using (var srDecrypt = new StreamReader(csDecrypt))
					{
						plaintext = srDecrypt.ReadToEnd();
					}
				}
			}
		}

		return plaintext;
	}
}