using System.Drawing;
using System.Drawing.Imaging;
using GymManager.Application.Common.Interfaces;
using QRCoder;

namespace GymManager.Infrastructure.Services;

public class QrCodeGenerator : IQrCodeGenerator
{
	// message to wiadomość, która zostanie zamieniona na kod QR
	// zwraca zdjęcie QR w formacie Stream
	public string Get(string message)
	{
		QRCodeGenerator qrGenerator = new();
		QRCodeData qRCodeData = qrGenerator.CreateQrCode(message, QRCodeGenerator.ECCLevel.Q);
		QRCode qRCode = new(qRCodeData);
		Bitmap bitmap = qRCode.GetGraphic(20);

		using (MemoryStream ms = new())
		{
			bitmap.Save(ms, ImageFormat.Png);
			var byteImage = ms.ToArray();
			return "data:image/png;base64," + Convert.ToBase64String(byteImage);
		}
	}
}