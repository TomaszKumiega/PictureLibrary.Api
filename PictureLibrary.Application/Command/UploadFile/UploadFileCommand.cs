using System.Net.Http.Headers;
using MediatR;

namespace PictureLibrary.Application.Command.UploadFile;

public record UploadFileCommand(
    string UserId,
    string UploadSessionId, 
    ContentRangeHeaderValue ContentRange, 
    Stream ContentStream) : IRequest<UploadFileResult>;