"use strict";

// podpi�cie pod klas� - hub
var connection = new signalR.HubConnectionBuilder().withUrl("/NotificationUserHub?userId=" + userId).build();

// podpi�cie pod metod� "sendToUser", czyli metod�, kt�ra jest wywo�ywana w UserNotificationService - SendNotification()
// czyli gdy ta metoda zostanie wywo�ana to zostanie wy�wietlona notyfikacja za pomoc� toastr
connection.on("sendToUser", (message) => {
	toastr.success(message);
});

// podpi�cie pod metod� GetConnectionId() z NotificationUserHub
// czyli zawsze gdy ten skrypt zostanie za�adowany to zostanie zapisana informacja o Id usera i Id po��czenia
connection.start().catch(function (err) {
	return console.error(err.toString());
}).then(function () {
	// "GetConnectionId" - nazwa funkcji z NotificationUserHub
	connection.invoke("GetConnectionId").then(function (connectionId) {
		// "signalRConnectionId" - ukryte pole o unikalnym Id
		document.getElementById("signalRConnectionId").innerHTML = connectionId;
	})
});