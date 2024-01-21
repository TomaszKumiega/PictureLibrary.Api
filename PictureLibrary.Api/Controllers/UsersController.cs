using MediatR;
using Microsoft.AspNetCore.Mvc;

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
    }
}
