using MediatR;
using PictureLibrary.Contracts.Results;

namespace PictureLibrary.Application.Query
{
    public record GetAllLibrariesQuery(string UserId) : IRequest<GetAllLibrariesResult>;
}
