using MediatR;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Command.UpdateLibrary;

public record UpdateLibraryCommand(string UserId, string LibraryId, UpdateLibraryDto Library) : IRequest<LibraryDto>;