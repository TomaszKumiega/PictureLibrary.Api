using MediatR;
using MongoDB.Bson;
using PictureLibrary.Application.Mapper;
using PictureLibrary.Contracts;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Application.Query
{
    public class GetAllLibrariesHandler : IRequestHandler<GetAllLibrariesQuery, LibrariesDto>
    {
        private readonly IMapper _mapper;
        private readonly ILibraryRepository _libraryRepository;

        public GetAllLibrariesHandler(
            IMapper mapper,
            ILibraryRepository libraryRepository)
        {
            _mapper = mapper;
            _libraryRepository = libraryRepository;
        }

        public async Task<LibrariesDto> Handle(GetAllLibrariesQuery request, CancellationToken cancellationToken)
        {
            ObjectId userId = ObjectId.Parse(request.UserId);

            IEnumerable<Library> libraries = await _libraryRepository.GetAll(userId);

            var libraryDtos = libraries.Select(x => _mapper.MapToDto(x));

            return new LibrariesDto(libraryDtos);
        }
    }
}
