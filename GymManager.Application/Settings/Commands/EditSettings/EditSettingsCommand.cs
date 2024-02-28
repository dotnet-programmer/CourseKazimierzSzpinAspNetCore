using GymManager.Application.Settings.Queries.Dtos;
using MediatR;

namespace GymManager.Application.Settings.Commands.EditSettings;

public class EditSettingsCommand : IRequest
{
	public List<SettingsPositionDto> Positions { get; set; } = [];
}