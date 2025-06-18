namespace GymManager.UI.Models;

public class MediatorValidateResponse<TResponse>
{
	public bool IsValid { get; set; }
	public TResponse Model { get; set; }
}