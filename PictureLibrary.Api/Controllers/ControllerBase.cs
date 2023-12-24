using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace PictureLibrary.Api.Controllers
{
    public class ControllerBase : Controller
    {
        protected string? GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
