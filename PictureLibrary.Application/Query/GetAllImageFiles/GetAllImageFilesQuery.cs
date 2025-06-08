using MediatR;
using PictureLibrary.Contracts.Images;

namespace PictureLibrary.Application.Query.GetAllImageFiles;

public record GetAllImageFilesQuery(string UserId, string LibraryId) : IRequest<ImageFilesDto>;