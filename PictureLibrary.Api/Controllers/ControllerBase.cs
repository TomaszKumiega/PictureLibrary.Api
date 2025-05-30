using System.Net.Http.Headers;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace PictureLibrary.Api.Controllers;

public class ControllerBase(IMediator mediator) : Controller
{
    protected IMediator Mediator => mediator;

    protected string? GetUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    protected static ContentRangeHeaderValue? GetContentRange(string contentRange)
    {
        if (string.IsNullOrEmpty(contentRange))
        {
            return null;
        }

        return ContentRangeHeaderValue.TryParse(contentRange, out var contentRangeHeaderValue)
            ? contentRangeHeaderValue
            : null;
    }
}