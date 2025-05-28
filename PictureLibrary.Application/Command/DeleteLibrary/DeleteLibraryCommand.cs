using MediatR;

namespace PictureLibrary.Application.Command;

public record DeleteLibraryCommand(string UserId, string LibraryId) : IRequest;