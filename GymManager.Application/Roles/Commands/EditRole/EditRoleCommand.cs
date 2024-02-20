using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace GymManager.Application.Roles.Commands.EditRole;

public class EditRoleCommand : IRequest
{
	public string Id { get; set; }

	[Required(ErrorMessage = "Pole 'Nazwa' jest wymagane.")]
	[DisplayName("Nazwa")]
	public string Name { get; set; }
}