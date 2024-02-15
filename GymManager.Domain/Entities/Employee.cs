using GymManager.Domain.Enums;

namespace GymManager.Domain.Entities;

public class Employee
{
	public int EmployeeId { get; set; }
	public DateTime DateOfEmployment { get; set; }
	public DateTime? DateOfDismissal { get; set; }
	public decimal Salary { get; set; }
	public Position Position { get; set; }

	// adres obrazka przypisanego do pracownika
	public string ImageUrl { get; set; }

	// adres strony profilowej użytkownika
	public string WebsiteUrl { get; set; }

	// strona profilowa użytkownika w HTML, która będzie generowana dynamicznie w programie 
	public string WebsiteRaw { get; set; }

	// relacja 1:1, tylko tutaj jest klucz obcy bo to klasa zależna od Usera
	public string UserId { get; set; }
	public ApplicationUser User { get; set; }
}