namespace GymManager.Application.Common.Extensions;

public static class StringExtensions
{
	public static string TakeFirstNChar(this string model, int maxLength)
	{
		if (string.IsNullOrEmpty(model))
		{
			return string.Empty;
		}

		if (model.Length <= maxLength)
		{
			return model;
		}

		return model.Substring(0, maxLength - 3) + "...";
	}
}