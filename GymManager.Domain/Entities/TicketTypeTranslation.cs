namespace GymManager.Domain.Entities;

// tłumaczenia dla nazw typu karnetu
public class TicketTypeTranslation
{
	public int TicketTypeTranslationId { get; set; }
	public string Name { get; set; }

	// relacja 1:wiele, 
	public int LanguageId { get; set; }
	public Language Language { get; set; }

	// relacja 1:wiele, nazwa typu karnetu w różnych wersjach językowych
	public int TicketTypeId { get; set; }
	public TicketType TicketType { get; set; }
}