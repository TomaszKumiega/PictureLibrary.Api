using MediatR;
using Microsoft.AspNetCore.Mvc;
using PictureLibrary.Application.Command;
using PictureLibrary.Contracts;

namespace PictureLibrary.Api.Controllers
{
    [Route("image")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        public ImageController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("createUploadSession")]
        public async Task<IActionResult> CreateUploadSession(
            [FromBody] CreateImageUploadSessionDto createUploadSessionDto)
        {
            string? userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var command = new CreateImageUploadSessionCommand(userId, createUploadSessionDto);

            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPost("uploadFile")]
        public async Task<IActionResult> UploadFile()
        {
            string? userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            var contentRange = GetContentRange();

            if (contentRange == null)
            {
                return BadRequest();
            }

            var command = new UploadFileCommand(userId, contentRange);

            var result = await _mediator.Send(command);

            return result.IsUploadFinished
                ? Created("/image/get", result)
                : Accepted(result);
        }
    }
}
