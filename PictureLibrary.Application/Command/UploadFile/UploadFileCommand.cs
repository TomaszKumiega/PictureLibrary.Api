using MediatR;
using System.Net.Http.Headers;

namespace PictureLibrary.Application.Command
{
    public record UploadFileCommand(
        string UserId,
        string UploadSessionId, 
        ContentRangeHeaderValue ContentRange, 
        Stream ContentStream) : IRequest<UploadFileResult>;
}
