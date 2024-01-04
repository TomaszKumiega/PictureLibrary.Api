using MediatR;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Query
{
    public record GetAllLibrariesQuery(string UserId) : IRequest<LibrariesDto>;
}
