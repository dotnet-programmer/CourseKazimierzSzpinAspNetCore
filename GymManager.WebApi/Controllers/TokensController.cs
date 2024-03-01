using GymManager.Application.Users.Queries.GetUserToken;
using Microsoft.AspNetCore.Mvc;

namespace GymManager.WebApi.Controllers;

[ApiVersion("1")]
[ApiExplorerSettings(GroupName = "v1")]
public class TokensController : BaseApiController
{
	// JWT - autoryzacja użytkownika - generowanie tokena
	[HttpPost]
	public async Task<IActionResult> Generate(GetUserTokenQuery command)
	{
		// generowanie tokena
		var token = await Mediator.Send(command);

		// zwrócenie tokena - Ok lub BadRequest
		return token != null ? Ok(token) : BadRequest(new { message = "Username or password is incorrect" });
	}
}