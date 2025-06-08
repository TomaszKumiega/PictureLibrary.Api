using MediatR;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Query.GetAllLibraries;

public record GetAllLibrariesQuery(string UserId) : IRequest<LibrariesDto>;