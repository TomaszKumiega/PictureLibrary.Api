using MediatR;
using Microsoft.AspNetCore.Mvc;
using PictureLibrary.Application.Command;
using PictureLibrary.Application.Command.CreateLibrary;
using PictureLibrary.Application.Command.DeleteLibrary;
using PictureLibrary.Application.Command.UpdateLibrary;
using PictureLibrary.Application.Query;
using PictureLibrary.Application.Query.GetAllLibraries;
using PictureLibrary.Application.Query.GetLibrary;
using PictureLibrary.Contracts;
using PictureLibrary.Contracts.Library;

namespace PictureLibrary.Api.Controllers;

[Route("library")]
[ApiController]
public class LibraryController(IMediator mediator) : ControllerBase(mediator)
{
    [HttpGet("get")]
    public async Task<IActionResult> Get([FromQuery] string id)
    {
        string? userId = GetUserId();

        if (userId == null)
        {
            return Unauthorized();
        }

        if (string.IsNullOrEmpty(id)) 
        {
            return BadRequest();
        }

        var query = new GetLibraryQuery(userId, id);

        var result = await Mediator.Send(query);

        return Ok(result);
    }

    [HttpGet("getall")]
    public async Task<IActionResult> GetAll()
    {
        string? userId = GetUserId();

        if (userId == null)
        {
            return Unauthorized();
        }

        var query = new GetAllLibrariesQuery(userId);

        var result = await Mediator.Send(query);

        return Ok(result);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] NewLibraryDto? newLibrary)
    {
        string? userId = GetUserId();

        if (userId == null)
        {
            return Unauthorized();
        }

        if (newLibrary is null)
        {
            return BadRequest();
        }

        var command = new CreateLibraryCommand(userId, newLibrary);

        var result = await Mediator.Send(command);

        return Created("create", result);
    }

    [HttpPatch("update")]
    public async Task<IActionResult> Update(
        [FromQuery] string libraryId,
        [FromBody] UpdateLibraryDto? library)
    {
        string? userId = GetUserId();

        if (userId == null)
        {
            return Unauthorized();
        }

        if (library is null)
        {
            return BadRequest();
        }

        var command = new UpdateLibraryCommand(userId, libraryId, library);

        var result = await Mediator.Send(command);

        return Ok(result);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> Delete([FromQuery] string id)
    {
        string? userId = GetUserId();

        if (userId == null)
        {
            return Unauthorized();
        }

        if (string.IsNullOrEmpty(id))
        {
            return BadRequest();
        }

        var command = new DeleteLibraryCommand(userId, id);

        await Mediator.Send(command);

        return Ok();
    }
}