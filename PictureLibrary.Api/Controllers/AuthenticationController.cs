using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PictureLibrary.Application.Command;
using PictureLibrary.Application.Query;
using PictureLibrary.Contracts;

namespace PictureLibrary.Api.Controllers
{
    [Route("auth")]
    [ApiController]
    [AllowAnonymous]
    public class AuthenticationController(IMediator mediator) : ControllerBase(mediator)
    {
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserDto loginUser)
        {
            if (loginUser == null)
            {
                return BadRequest();
            }

            var command = new LoginUserCommand(loginUser);

            var result = await _mediator.Send(command);

            return Ok(result);
        }

        [HttpPost("refreshTokens")]
        public async Task<IActionResult> RefreshTokens([FromBody] RefreshAuthorizationDataDto userAuthorizationDataDto)
        {
            var query = new RefreshTokensQuery(userAuthorizationDataDto);

            var result = await _mediator.Send(query);

            return Ok(result);
        }
    }
}
