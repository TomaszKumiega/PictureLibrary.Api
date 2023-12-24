using MediatR;
using PictureLibrary.Application.Dto;

namespace PictureLibrary.Application.Query
{
    public record GetLibraryQuery(string UserId, string LibraryId) : IRequest<LibraryDto>;
}
