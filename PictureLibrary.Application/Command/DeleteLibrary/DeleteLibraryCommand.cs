using MediatR;

namespace PictureLibrary.Application.Command.DeleteLibrary;

public record DeleteLibraryCommand(string UserId, string LibraryId) : IRequest;