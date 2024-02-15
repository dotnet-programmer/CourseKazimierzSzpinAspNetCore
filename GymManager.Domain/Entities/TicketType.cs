using GymManager.Domain.Enums;

namespace GymManager.Domain.Entities;

// typ karnetu - dniowy / miesięczny / roczny itp
public class TicketType
{
	public int TicketTypeId { get; set; }
	public decimal Price { get; set; }

	// dokładny typ karnetu
	public TicketTypeEnum TicketTypeEnum { get; set; }

	// relacja 1:wiele, typ karnetu może być na wielu karnetach
	public ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();

	// relacja 1:wiele, nazwa typu karnetu w różnych wersjach językowych
	public ICollection<TicketTypeTranslation> Translations { get; set; } = new HashSet<TicketTypeTranslation>();
}