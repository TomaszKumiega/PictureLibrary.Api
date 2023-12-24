﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using PictureLibrary.Application.Query;

namespace PictureLibrary.Api.Controllers
{
    [Route("library")]
    [ApiController]
    public class LibraryController(IMediator mediator) : ControllerBase(mediator)
    {
        [Route("get")]
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
    }
}
