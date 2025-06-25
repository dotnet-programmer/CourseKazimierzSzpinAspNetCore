// dyrektywa w JavaScript, która w³¹cza tryb œcis³y (strict mode) dla danego skryptu lub funkcji
"use strict";

// zmienne var maj¹ zasiêg funkcyjny (funkcja jako blok) lub zasiêg globalny (jeœli deklaracja jest poza funkcj¹)
// zmienne let maj¹ zasiêg blokowy (np.wewn¹trz pêtli if, for itp.).

// podpiêcie pod klasê - hub
// adres taki jak w Program.cs - app.MapHub<NotificationUserHub>("/NotificationUserHub");
// dodatkowo trzeba przekazaæ userId, czyli Id u¿ytkownika, który jest zalogowany
// zmienna userId jest zadeklarowana w pliku _Layout.cshtml i tam jest pobierana jej wartoœæ, dlatego w HTML musi byæ powy¿ej wywo³ania tego skryptu: <script src="~/js/usernotification.js"></script>
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
		// "signalRConnectionId" - ukryte pole o unikalnym Id utworzone w _Layout.cshtml
		document.getElementById("signalRConnectionId").innerHTML = connectionId;
	})
});