using Microsoft.AspNetCore.Identity;

namespace GymManager.Domain.Entities;

// INFO - konfiguracja Identity - ApplicationUser musi dziedziczyć po IdentityUser
public class ApplicationUser : IdentityUser
{
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public DateTime RegisterDateTime { get; set; }
	public bool IsDeleted { get; set; }

	// 3x relacje 1:1, to jest klasa nadrzędna,
	// klucze obce w tabelach zależnych od Usera - np. nie doda się adresu bez użytkownika
	public Address Address { get; set; }
	// jeśeli użytkownik jest klientem to tutaj będą dodatkowe informacje jakich potrzebuje klient
	public Client Client { get; set; }
	// jeśeli użytkownik jest pracownikiem to tutaj będą dodatkowe informacje jakich potrzebuje pracownik
	public Employee Employee { get; set; }

	// relacja 1:wiele, lista karnetów
	public ICollection<Ticket> Tickets { get; set; } = new HashSet<Ticket>();

	// relacja 1:wiele, lista faktur
	public ICollection<Invoice> Invoices { get; set; } = new HashSet<Invoice>();

	// relacja 1:wiele, lista zdarzeń - pozycje na grafiku w kalendarzu
	public ICollection<EmployeeEvent> EmployeeEvents { get; set; } = new HashSet<EmployeeEvent>();
}