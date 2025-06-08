using MediatR;
using PictureLibrary.Contracts;
using PictureLibrary.Contracts.Results;

namespace PictureLibrary.Application.Command.CreateImageUploadSession;

public record CreateImageUploadSessionCommand(
    string UserId, 
    CreateImageUploadSessionDto CreateUploadSessionDto) : IRequest<CreateImageUploadSessionResult>;