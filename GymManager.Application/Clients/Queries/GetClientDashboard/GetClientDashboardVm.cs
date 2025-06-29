﻿using GymManager.Application.Announcements.Queries.Dtos;
using GymManager.Application.Charts.Queries.Dtos;
using GymManager.Application.Common.Models;

namespace GymManager.Application.Clients.Queries.GetClientDashboard;

public class GetClientDashboardVm
{
	public PaginatedList<AnnouncementDto> Announcements { get; set; }
	public string Email { get; set; }
	public DateTime? TicketEndDate { get; set; }

	// dla 1 wykresu zawsze będzie 1 model (czyli dla każdego wykresu potrzebna osobna właściwość)
	// wykres na temat liczby treningów
	public ChartDto TrainingCountChart { get; set; }

	// statystyki najlepszych treningów
	public ChartDto TheBestTrainingsChart { get; set; }
}