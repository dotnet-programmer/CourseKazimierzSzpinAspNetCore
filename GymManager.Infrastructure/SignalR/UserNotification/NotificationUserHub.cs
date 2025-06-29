﻿using GymManager.Application.Common.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace GymManager.Infrastructure.SignalR.UserNotification;

// klasa - hub - przez którą będą przechodzić wszystkie połączenia
// tutaj są podpięcia pod metody wywoływane z poziomu JavaScript
// takie połączenie między JavaScript a C#
// żeby to zadziałało trzeba dodać w projekcie UI w Program.cs: app.MapHub<NotificationUserHub>("/NotificationUserHub");
public class NotificationUserHub(IUserConnectionManager userConnectionManager) : Hub
{
	// jeśli user wejdzie na stronę to potrzebna jest o tym informacja,
	public string GetConnectionId()
	{
		// odwołanie do kontekstu
		var httpContext = Context.GetHttpContext();

		// z requesta trzeba pobrać informację o Id usera
		var userId = httpContext.Request.Query["userId"];

		// wywołanie metody z IUserConnectionManager
		// jeśli ktoś wejdzie na stronę, to zapisz informacje o Id usera oraz IdConnection z contextu
		userConnectionManager.KeepUserConnection(userId, Context.ConnectionId);

		// zwróć Id contextu
		return Context.ConnectionId;
	}

	// jeśli user wyjdzie ze strony to również jest o tym info
	public override async Task OnDisconnectedAsync(Exception exception)
	{
		// pobierz Id połączenia z contextu
		var connectionId = Context.ConnectionId;

		// wywołanie metody z IUserConnectionManager
		// jeśli ktoś opuszcza stronę to trzeba usunąć informacje z listy
		userConnectionManager.RemoveUserConnection(connectionId);
	}
}