using MediatR;
using PictureLibrary.Contracts;
using PictureLibrary.Contracts.Results;

namespace PictureLibrary.Application.Command
{
    public record CreateImageUploadSessionCommand(string UserId, CreateUploadSessionDto CreateUploadSessionDto) : IRequest<CreateImageUploadSessionResult>;
}
