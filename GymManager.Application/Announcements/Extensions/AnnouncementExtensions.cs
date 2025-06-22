using GymManager.Application.Announcements.Queries.Dtos;
using GymManager.Domain.Entities;

namespace GymManager.Application.Announcements.Extensions;

public static class AnnouncementExtensions
{
	public static AnnouncementDto ToDto(this Announcement announcement)
		=> announcement == null ?
		null :
		new AnnouncementDto
		{
			Date = announcement.Date,
			Description = announcement.Description
		};
}