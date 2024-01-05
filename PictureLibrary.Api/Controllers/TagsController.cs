using MediatR;
using Microsoft.AspNetCore.Mvc;

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
    }
}
