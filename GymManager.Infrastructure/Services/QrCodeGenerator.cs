using GymManager.Application.Common.Interfaces;
using QRCoder;

namespace GymManager.Infrastructure.Services;

public class QrCodeGenerator : IQrCodeGenerator
{
	// message to wiadomość, która zostanie zamieniona na kod QR
	// zwraca zdjęcie QR w formacie Stream
	public string Get(string message)
	{
		using (QRCodeGenerator qrGenerator = new())
		using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(message, QRCodeGenerator.ECCLevel.Q))
		using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
		{
			byte[] qrCodeImage = qrCode.GetGraphic(20);
			return "data:image/png;base64," + Convert.ToBase64String(qrCodeImage);
		}
	}
}