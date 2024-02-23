namespace GymManager.Application.Charts.Queries.Dtos;

// model, na podstawie którego będą wyświetlane wykresy
public class ChartDto
{
	// etykieta
	public string Label { get; set; }

	// lista z pozycjami
	public List<ChartPositionDto> Positions { get; set; } = [];
}