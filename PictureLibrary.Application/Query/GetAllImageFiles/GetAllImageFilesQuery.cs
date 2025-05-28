using MediatR;
using PictureLibrary.Contracts.Images;

namespace PictureLibrary.Application.Query;

public record GetAllImageFilesQuery(string UserId, string LibraryId) : IRequest<ImageFilesDto>;