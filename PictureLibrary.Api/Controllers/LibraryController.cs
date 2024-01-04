using MediatR;
using Microsoft.AspNetCore.Mvc;
using PictureLibrary.Application.Command;
using PictureLibrary.Application.Query;
using PictureLibrary.Contracts.Library;

namespace PictureLibrary.Api.Controllers
{
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

            var result = await _mediator.Send(query);

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

            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] NewLibraryDto newLibrary)
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

            var result = await _mediator.Send(command);

            return Ok(result);
        }
    }
}
