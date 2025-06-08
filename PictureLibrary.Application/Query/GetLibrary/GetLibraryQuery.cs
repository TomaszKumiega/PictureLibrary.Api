using MediatR;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Query.GetLibrary;

public record GetLibraryQuery(string UserId, string LibraryId) : IRequest<LibraryDto>;