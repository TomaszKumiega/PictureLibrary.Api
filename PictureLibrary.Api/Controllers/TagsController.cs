using MediatR;
using Microsoft.AspNetCore.Mvc;
using PictureLibrary.Application.Command;
using PictureLibrary.Application.Query;
using PictureLibrary.Contracts;

namespace PictureLibrary.Api.Controllers
{
    [Route("tag")]
    [ApiController]
    public class TagsController(IMediator mediator) : ControllerBase(mediator)
    {
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll([FromQuery] string libraryId)
        {
            string? userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            if (string.IsNullOrEmpty(libraryId))
            {
                return BadRequest();
            }

            var query = new GetAllTagsQuery(userId, libraryId);

            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(
            [FromQuery] string libraryId,
            [FromBody] NewTagDto newTagDto)
        {
            string? userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            if (string.IsNullOrEmpty(libraryId)
                || newTagDto == null)
            {
                return BadRequest();
            }

            var command = new CreateTagCommand(userId, libraryId, newTagDto);

            var tagDto = await _mediator.Send(command);

            return Created("create", tagDto);
        }

        [HttpPatch("update")]
        public async Task<IActionResult> Update(
            [FromQuery] string tagId,
            [FromBody] UpdateTagDto tagDto)
        {
            string? userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            if (string.IsNullOrEmpty(tagId)
                || tagDto == null)
            {
                return BadRequest();
            }

            var command = new UpdateTagCommand(userId, tagId, tagDto);

            var response = await _mediator.Send(command);

            return Ok(response);
        }

        [HttpDelete("delete")]  
        public async Task<IActionResult> Delete(
            [FromQuery] string tagId)
        {
            string? userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            if (string.IsNullOrEmpty(tagId))
            {
                return BadRequest();
            }

            var command = new DeleteTagCommand(userId, tagId);

            await _mediator.Send(command);

            return Ok();
        }

    }
}
