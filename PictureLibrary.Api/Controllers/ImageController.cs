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
    }
}
