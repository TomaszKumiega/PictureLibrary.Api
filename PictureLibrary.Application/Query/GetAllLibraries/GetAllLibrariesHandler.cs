using MediatR;
using MongoDB.Bson;
using PictureLibrary.Application.Mapper;
using PictureLibrary.Contracts;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Application.Query
{
    public class GetAllLibrariesHandler(
        IMapper mapper,
        ILibraryRepository libraryRepository) 
        : IRequestHandler<GetAllLibrariesQuery, LibrariesDto>
    {
        public async Task<LibrariesDto> Handle(GetAllLibrariesQuery request, CancellationToken cancellationToken)
        {
            ObjectId userId = ObjectId.Parse(request.UserId);

            IEnumerable<Library> libraries = await libraryRepository.GetAll(userId);

            var libraryDtos = libraries.Select(x => mapper.MapToDto(x));

            return new LibrariesDto(libraryDtos);
        }
    }
}
