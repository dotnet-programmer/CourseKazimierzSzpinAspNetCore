namespace GymManager.Application.Charts.Queries.Dtos;

// każdy wykres np. słupkowy będzie miał różne słupki, a każdy słupek będzie miał etykietę, liczbę danych, kolor, kolor obramowania
public class ChartPositionDto
{
	// etykieta
	public string Label { get; set; }

	// liczba danych
	public int Data { get; set; }

	// kolor
	public string Color { get; set; }

	// kolor obramowania
	public string BorderColor { get; set; }
}