using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PictureLibrary.Application.Command;
using PictureLibrary.Application.Command.DeleteUser;
using PictureLibrary.Application.Command.RegsiterUser;
using PictureLibrary.Application.Command.UpdateUser;
using PictureLibrary.Application.Query;
using PictureLibrary.Application.Query.GetUser;
using PictureLibrary.Contracts;

namespace PictureLibrary.Api.Controllers;

[Route("user")]
[ApiController]
public class UsersController(IMediator mediator) : ControllerBase(mediator)
{
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] NewUserDto? newUser)
    {
        if (newUser == null)
        {
            return BadRequest();
        }

        var command = new RegisterUserCommand(newUser);

        var result = await Mediator.Send(command);

        return Ok(result);
    }

    [HttpGet("get")]
    public async Task<IActionResult> Get()
    {
        string? userId = GetUserId();

        if (userId == null)
        {
            return Unauthorized();
        }

        var query = new GetUserQuery(userId);

        var result = await Mediator.Send(query);

        return Ok(result);
    }

    [HttpPatch("update")]
    public async Task<IActionResult> Update([FromBody] UpdateUserDto? user)
    {
        string? userId = GetUserId();

        if (userId == null)
        {
            return Unauthorized();
        }

        if (user == null)
        {
            return BadRequest();
        }

        var command = new UpdateUserCommand(userId, user);

        var result = await Mediator.Send(command);

        return Ok(result);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete()
    {
        string? userId = GetUserId();

        if (userId == null)
        {
            return Unauthorized();
        }

        var command = new DeleteUserCommand(userId);

        await Mediator.Send(command);

        return Ok();
    }
}