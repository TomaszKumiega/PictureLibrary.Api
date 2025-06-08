using MediatR;

namespace PictureLibrary.Application.Command.DeleteImageFile;

public record DeleteImageFileCommand(string UserId, string ImageFileId) : IRequest;