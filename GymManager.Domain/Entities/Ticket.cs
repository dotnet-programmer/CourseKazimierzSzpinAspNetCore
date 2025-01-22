namespace GymManager.Domain.Entities;

public class Ticket
{
	public string TicketId { get; set; }
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
	public decimal Price { get; set; }
	public bool IsPaid { get; set; }

	// Token generowany przez system płatności
	public string Token { get; set; }
	public DateTime CreatedDate { get; set; }

	// relacja 1:1, każda faktura powiązana z konkretnym karnetem
	public Invoice Invoice { get; set; }

	// relacja 1:wiele, typ karnetu może być na wielu karnetach
	public int TicketTypeId { get; set; }
	public TicketType TicketType { get; set; }

	// relacja 1:wiele
	public string UserId { get; set; }
	public ApplicationUser User { get; set; }
}