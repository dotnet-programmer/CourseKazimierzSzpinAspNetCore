using Microsoft.AspNetCore.Mvc;

namespace GymManager.Application.Common.Models;

public class FileGeneratorParams
{
	public ActionContext Context { get; set; }

	// szablon-widok na podstawie którego będzie generowany PDF
	public string ViewTemplate { get; set; }

	// model, czyli dane, które mają być na tym widoku
	public object Model { get; set; }
}