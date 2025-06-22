using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace GymManager.UI.Extensions;

public static class TempDataExtensions
{
	// serializacja
	public static void Put<T>(this ITempDataDictionary tempData, string key, T value) where T : class
		=> tempData[key] = JsonConvert.SerializeObject(value);

	// deserializacja
	public static T Get<T>(this ITempDataDictionary tempData, string key) where T : class
	{
		tempData.TryGetValue(key, out object obj);
		return obj == null ? null : JsonConvert.DeserializeObject<T>((string)obj);
	}
}