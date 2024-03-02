"use strict";

// podpiêcie pod klasê - hub
var connection = new signalR.HubConnectionBuilder().withUrl("/NotificationUserHub?userId=" + userId).build();

// podpiêcie pod metodê "sendToUser", czyli metodê, która jest wywo³ywana w UserNotificationService - SendNotification()
// czyli gdy ta metoda zostanie wywo³ana to zostanie wyœwietlona notyfikacja za pomoc¹ toastr
connection.on("sendToUser", (message) => {
	toastr.success(message);
});

// podpiêcie pod metodê GetConnectionId() z NotificationUserHub
// czyli zawsze gdy ten skrypt zostanie za³adowany to zostanie zapisana informacja o Id usera i Id po³¹czenia
connection.start().catch(function (err) {
	return console.error(err.toString());
}).then(function () {
	// "GetConnectionId" - nazwa funkcji z NotificationUserHub
	connection.invoke("GetConnectionId").then(function (connectionId) {
		// "signalRConnectionId" - ukryte pole o unikalnym Id
		document.getElementById("signalRConnectionId").innerHTML = connectionId;
	})
});