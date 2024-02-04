using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PictureLibrary.Application.Command;
using PictureLibrary.Application.Query;
using PictureLibrary.Contracts;

namespace PictureLibrary.Api.Controllers
{
    [Route("user")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public UsersController(IMediator mediator) 
            : base(mediator)
        {
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] NewUserDto newUser)
        {
            if (newUser == null)
            {
                return BadRequest();
            }

            var command = new RegisterUserCommand(newUser);

            var result = await _mediator.Send(command);

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

            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}
