using GymManager.Application.Common.Interfaces;
using GymManager.Application.Dictionaries;
using MailKit.Net.Smtp;
using MimeKit;

namespace GymManager.Infrastructure.Services;

// INFO - implementacja wysyłki email
public class EmailService : IEmailService
{
	private int _port;
	private string _hostSmtp;
	private string _senderEmail;
	private string _senderEmailPassword;
	private string _senderName;
	private string _senderLogin;

	// pola wypełniane danymi pobranymi z serwisu IAppSettingsService
	public async Task Update(IAppSettingsService appSettingsService)
	{
		_port = Convert.ToInt32(await appSettingsService.Get(SettingsDict.Port));
		_hostSmtp = await appSettingsService.Get(SettingsDict.HostSmtp);
		_senderEmail = await appSettingsService.Get(SettingsDict.SenderEmail);
		_senderEmailPassword = await appSettingsService.Get(SettingsDict.SenderEmailPassword);
		_senderName = await appSettingsService.Get(SettingsDict.SenderName);
		_senderLogin = await appSettingsService.Get(SettingsDict.SenderLogin);
	}

	// ogólna metoda wysyłająca emaile, używa NuGet - MailKit (instalowany w Infrastructure)
	public async Task SendAsync(string subject, string body, string to, string attachmentPath = null)
	{
		MimeMessage message = new();

		message.From.Add(new MailboxAddress(_senderName, _senderEmail));
		message.To.Add(MailboxAddress.Parse(to));
		message.Subject = subject;

		BodyBuilder builder = new()
		{
			HtmlBody = @$"
					<html>
						<head></head>
						<body>
							<div style=""font-size: 16px; padding: 10px; font-family: Arial; line-height: 1.4;"">
								{body}
							</div>
						</body>
					</html>"
		};

		// sprawdzanie czy dodano załącznik
		if (!string.IsNullOrEmpty(attachmentPath))
		{
			builder.Attachments.Add(attachmentPath);
		}

		message.Body = builder.ToMessageBody();

		using (SmtpClient client = new())
		{
			await client.ConnectAsync(_hostSmtp, _port, MailKit.Security.SecureSocketOptions.Auto);

			// niektórzy zewnętrzni dostawcy żądają loginu a inni całego maila
			await client.AuthenticateAsync(!string.IsNullOrWhiteSpace(_senderLogin) ? _senderLogin : _senderEmail, _senderEmailPassword);

			await client.SendAsync(message);
			await client.DisconnectAsync(true);
		}
	}
}