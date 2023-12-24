using Microsoft.AspNetCore.Mvc;

namespace PictureLibrary.Api.Controllers
{
    [Route("/library")]
    [ApiController]
    public class LibraryController : ControllerBase
    {
        public LibraryController()
        {
            
        }

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

            //TODO: get library

            return Ok();
        }
    }
}
