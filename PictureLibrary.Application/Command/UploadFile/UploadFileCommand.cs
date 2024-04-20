using MediatR;
using PictureLibrary.Contracts;
using System.Net.Http.Headers;

namespace PictureLibrary.Application.Command
{
    public record UploadFileCommand(string UserId, ContentRangeHeaderValue ContentRange) : IRequest<UploadFileResult>;
}
