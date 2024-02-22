namespace GymManager.Application.Common.Interfaces;

public interface IQrCodeGenerator
{
	string Get(string message);
}