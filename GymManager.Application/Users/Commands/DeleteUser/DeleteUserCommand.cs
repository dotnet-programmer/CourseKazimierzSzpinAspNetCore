using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace GymManager.Application.Users.Commands.DeleteUser;
public class DeleteUserCommand : IRequest
{
	public string Id { get; set; }
}
