using MediatR;
using MongoDB.Bson;
using PictureLibrary.Application.Mapper;
using PictureLibrary.Contracts;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Application.Query.GetLibrary;

public class GetLibraryHandler(
    IMapper mapper,
    ILibraryRepository libraryRepository) 
    : IRequestHandler<GetLibraryQuery, LibraryDto>
{
    public async Task<LibraryDto> Handle(GetLibraryQuery request, CancellationToken cancellationToken)
    {
        ObjectId userId = ObjectId.Parse(request.UserId);
        ObjectId libraryId = ObjectId.Parse(request.LibraryId);

        Library library = await libraryRepository.Get(userId, libraryId)
                          ?? throw new NotFoundException();

        return mapper.MapToDto(library);
    }
}