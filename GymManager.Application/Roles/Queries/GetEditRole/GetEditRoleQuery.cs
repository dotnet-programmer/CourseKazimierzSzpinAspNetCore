using GymManager.Application.Roles.Commands.EditRole;
using MediatR;

namespace GymManager.Application.Roles.Queries.GetEditRole;

public class GetEditRoleQuery : IRequest<EditRoleCommand>
{
	public string Id { get; set; }
}