using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PictureLibrary.Application.Command;
using PictureLibrary.Application.Command.LoginUser;
using PictureLibrary.Application.Query;
using PictureLibrary.Application.Query.RefreshTokens;
using PictureLibrary.Contracts;

namespace PictureLibrary.Api.Controllers;

[Route("auth")]
[ApiController]
[AllowAnonymous]
public class AuthenticationController(IMediator mediator) : ControllerBase(mediator)
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto? loginUser)
    {
        if (loginUser == null)
        {
            return BadRequest();
        }

        var command = new LoginUserCommand(loginUser);

        var result = await Mediator.Send(command);

        return Ok(result);
    }

    [HttpPost("refreshTokens")]
    public async Task<IActionResult> RefreshTokens([FromBody] RefreshAuthorizationDataDto? userAuthorizationDataDto)
    {
        if (userAuthorizationDataDto == null)
        {
            return BadRequest();
        }
        
        var query = new RefreshTokensQuery(userAuthorizationDataDto);

        var result = await Mediator.Send(query);

        return Ok(result);
    }
}