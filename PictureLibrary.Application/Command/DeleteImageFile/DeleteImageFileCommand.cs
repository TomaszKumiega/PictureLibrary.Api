using MediatR;

namespace PictureLibrary.Application.Command;

public record DeleteImageFileCommand(string UserId, string ImageFileId) : IRequest;