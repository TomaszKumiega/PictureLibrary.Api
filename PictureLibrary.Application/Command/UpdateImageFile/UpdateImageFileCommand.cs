using MediatR;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Command.UpdateImageFile;

public record UpdateImageFileCommand(
    string UserId, 
    string ImageFileId, 
    UpdateImageFileDto Dto) : IRequest<ImageFileDto>;