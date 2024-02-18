using System.Reflection;
using GymManager.Application.Common.Interfaces;
using GymManager.Domain.Entities;
using GymManager.Infrastructure.Persistence.Extensions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using File = GymManager.Domain.Entities.File;

namespace GymManager.Infrastructure.Persistence;

// INFO - konfiguracja Identity - zamiast po DbContext trzeba dziedziczyć po IdentityDbContext<ApplicationUser>
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>, IApplicationDbContext
{
	// wszystkie ustawienia przekazywane za pomocą Dependency Injection
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
	{ }

    public DbSet<Address> Addresses { get; set; }
	public DbSet<Client> Clients { get; set; }
	public DbSet<Employee> Employees { get; set; }
	public DbSet<EmployeeEvent> EmployeeEvents { get; set; }
	public DbSet<SettingsPosition> SettingsPositions { get; set; }
	public DbSet<Settings> Settings { get; set; }
	public DbSet<Ticket> Tickets { get; set; }
	public DbSet<TicketType> TicketTypes { get; set; }
	public DbSet<File> Files { get; set; }
	public DbSet<Announcement> Announcements { get; set; }
	public DbSet<Language> Languages { get; set; }
	public DbSet<TicketTypeTranslation> TicketTypeTranslations { get; set; }
	public DbSet<Invoice> Invoices { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		// dodanie wszystkich plików konfiguracyjnych za pomocą reflekcji
		modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

		// wypełnianie bazy danych startowymi wartościami
		modelBuilder.SeedLanguage();
		modelBuilder.SeedTicketType();
		modelBuilder.SeedTicketTypeTranslation();
		modelBuilder.SeedSettings();
		modelBuilder.SeedSettingsPosition();
		modelBuilder.SeedAnnouncement();
		modelBuilder.SeedRoles();

		base.OnModelCreating(modelBuilder);
	}
}