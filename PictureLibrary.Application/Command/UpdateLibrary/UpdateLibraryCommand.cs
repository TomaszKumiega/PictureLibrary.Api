using MediatR;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Command
{
    public record UpdateLibraryCommand(string UserId, string LibraryId, UpdateLibraryDto Library) : IRequest<LibraryDto>;
}
