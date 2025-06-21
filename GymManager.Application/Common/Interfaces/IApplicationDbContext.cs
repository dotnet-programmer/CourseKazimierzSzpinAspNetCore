using GymManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using File = GymManager.Domain.Entities.File;

namespace GymManager.Application.Common.Interfaces;

public interface IApplicationDbContext : IDisposable
{
	DbSet<Address> Addresses { get; set; }
	DbSet<Client> Clients { get; set; }
	DbSet<Employee> Employees { get; set; }
	DbSet<EmployeeEvent> EmployeeEvents { get; set; }
	DbSet<SettingsPosition> SettingsPositions { get; set; }
	DbSet<Domain.Entities.Settings> Settings { get; set; }
	DbSet<Ticket> Tickets { get; set; }
	DbSet<TicketType> TicketTypes { get; set; }
	DbSet<File> Files { get; set; }
	DbSet<Announcement> Announcements { get; set; }
	DbSet<Language> Languages { get; set; }
	DbSet<TicketTypeTranslation> TicketTypeTranslations { get; set; }
	DbSet<Invoice> Invoices { get; set; }

	// implementacja tego DbSeta jest w Identity, dlatego nie ma go w konkretnej klasie ApplicationDbContext
	DbSet<ApplicationUser> Users { get; set; }

	// ta klasa jest wewnątrz DbContext
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}