using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Net.Http.Headers;
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

        protected ContentRangeHeaderValue? GetContentRange()
        {
            if (!Request.Headers.TryGetValue("Content-Range", out StringValues contentRangeHeaders))
            {
                string? contentRange = contentRangeHeaders.First();

                if (string.IsNullOrEmpty(contentRange))
                {
                    return null;
                }

                return ContentRangeHeaderValue.TryParse(contentRange, out var contentRangeHeaderValue)
                    ? contentRangeHeaderValue
                    : null;
            }

            return null;
        }

    }
}
