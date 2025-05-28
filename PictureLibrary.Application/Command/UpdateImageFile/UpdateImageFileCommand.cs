using MediatR;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Command;

public record UpdateImageFileCommand(
    string UserId, 
    string ImageFileId, 
    UpdateImageFileDto Dto) : IRequest<ImageFileDto>;