﻿using System.Net;

namespace GymManager.WebApi.Middlewares;

// Globalna obsługa wyjątków
public class ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
{
	public async Task Invoke(HttpContext context)
	{
		try
		{
			await next.Invoke(context);
		}
		catch (Exception exception)
		{
			logger.LogError(exception, "GymManager Request: Nieobsłużony wyjątek - Request {Name}", context.Request.Path);

			await HandleExceptionAsync(context, exception).ConfigureAwait(false);
		}
	}

	private Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		context.Response.ContentType = "application/json";
		context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
		return context.Response.WriteAsJsonAsync(new { error = exception.Message });
	}
}