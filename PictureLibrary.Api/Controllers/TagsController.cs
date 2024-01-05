﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using PictureLibrary.Application.Command;
using PictureLibrary.Application.Query;
using PictureLibrary.Contracts;

namespace PictureLibrary.Api.Controllers
{
    [Route("tag")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        public TagsController(IMediator mediator) 
            : base(mediator)
        {
        }

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

            await _mediator.Send(command);

            return Ok();
        }
    }
}
