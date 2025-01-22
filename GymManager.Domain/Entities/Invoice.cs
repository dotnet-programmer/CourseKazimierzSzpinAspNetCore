namespace GymManager.Domain.Entities;

public class Invoice
{
	public int InvoiceId { get; set; }
	public int Month { get; set; }
	public int Year { get; set; }
	public decimal Value { get; set; }

	public string MethodOfPayment { get; set; }
	public DateTime CreatedDate { get; set; }

	// relacja 1:wiele, każda faktura powiązana z konkretnym użytkownikiem
	public string UserId { get; set; }
	public ApplicationUser User { get; set; }

	// relacja 1:1, każda faktura powiązana z konkretnym karnetem, klucz tutaj bo to klasa zależna, musi istnieć jakiś karnet żeby zrobić fakturę dla niego
	public string TicketId { get; set; }
	public Ticket Ticket { get; set; }
}