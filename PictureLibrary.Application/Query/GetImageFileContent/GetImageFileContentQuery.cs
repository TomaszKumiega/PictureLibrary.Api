using MediatR;

namespace PictureLibrary.Application.Query;

public record GetImageFileContentQuery(string UserId, string ImageFileId) : IRequest<GetImageFileContentResult>;