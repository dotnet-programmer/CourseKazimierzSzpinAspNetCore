// dyrektywa w JavaScript, kt�ra w��cza tryb �cis�y (strict mode) dla danego skryptu lub funkcji
"use strict";

// zmienne var maj� zasi�g funkcyjny (funkcja jako blok) lub zasi�g globalny (je�li deklaracja jest poza funkcj�)
// zmienne let maj� zasi�g blokowy (np.wewn�trz p�tli if, for itp.).

// podpi�cie pod klas� - hub
// adres taki jak w Program.cs - app.MapHub<NotificationUserHub>("/NotificationUserHub");
// dodatkowo trzeba przekaza� userId, czyli Id u�ytkownika, kt�ry jest zalogowany
// zmienna userId jest zadeklarowana w pliku _Layout.cshtml i tam jest pobierana jej warto��, dlatego w HTML musi by� powy�ej wywo�ania tego skryptu: <script src="~/js/usernotification.js"></script>
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
		// "signalRConnectionId" - ukryte pole o unikalnym Id utworzone w _Layout.cshtml
		document.getElementById("signalRConnectionId").innerHTML = connectionId;
	})
});