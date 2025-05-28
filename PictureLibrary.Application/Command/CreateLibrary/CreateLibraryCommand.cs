using MediatR;
using PictureLibrary.Contracts;
using PictureLibrary.Contracts.Library;

namespace PictureLibrary.Application.Command;

public record CreateLibraryCommand(string UserId, NewLibraryDto NewLibrary) : IRequest<LibraryDto>;