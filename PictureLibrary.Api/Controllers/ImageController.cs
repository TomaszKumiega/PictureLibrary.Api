﻿using MediatR;
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
        public async Task<IActionResult> UploadFile([FromQuery] string uploadSessionId)
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

            var command = new UploadFileCommand(userId, uploadSessionId, contentRange, Request.Body);

            var result = await _mediator.Send(command);

            return result.IsUploadFinished
                ? Created("/image/get", result.Value)
                : Accepted(result.Value);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete([FromQuery] string imageFileId)
        {
            string? userId = GetUserId();

            if (userId == null)
            {
                return Unauthorized();
            }

            if (string.IsNullOrEmpty(imageFileId))
            {
                return BadRequest();
            }

            var command = new DeleteImageFileCommand(userId, imageFileId);

            await _mediator.Send(command);

            return Ok();
        }
    }
}
