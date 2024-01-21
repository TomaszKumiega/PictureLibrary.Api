using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PictureLibrary.Application.Command;
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
    }
}
