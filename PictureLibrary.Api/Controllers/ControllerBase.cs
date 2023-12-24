using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace PictureLibrary.Api.Controllers
{
    public class ControllerBase : Controller
    {
        protected readonly IMediator _mediator;

        public ControllerBase(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
