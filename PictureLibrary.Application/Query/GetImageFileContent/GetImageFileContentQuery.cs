using MediatR;

namespace PictureLibrary.Application.Query.GetImageFileContent;

public record GetImageFileContentQuery(string UserId, string ImageFileId) : IRequest<GetImageFileContentResult>;