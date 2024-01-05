using MediatR;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Command
{
    public record UpdateLibraryCommand(string UserId, LibraryDto Library) : IRequest<LibraryDto>;
}
