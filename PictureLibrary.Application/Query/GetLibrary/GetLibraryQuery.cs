using MediatR;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Query;

public record GetLibraryQuery(string UserId, string LibraryId) : IRequest<LibraryDto>;