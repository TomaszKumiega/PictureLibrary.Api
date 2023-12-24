using MediatR;
using MongoDB.Bson;
using PictureLibrary.Application.Mapper;
using PictureLibrary.Contracts;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Application.Query
{
    public class GetLibraryHandler : IRequestHandler<GetLibraryQuery, LibraryDto>
    {
        private readonly IMapper _mapper;
        private readonly ILibraryRepository _libraryRepository;

        public GetLibraryHandler(
            IMapper mapper,
            ILibraryRepository libraryRepository)
        {
            _mapper = mapper;
            _libraryRepository = libraryRepository;
        }

        public async Task<LibraryDto> Handle(GetLibraryQuery request, CancellationToken cancellationToken)
        {
            ObjectId userId = ObjectId.Parse(request.UserId);
            ObjectId libraryId = ObjectId.Parse(request.LibraryId);

            Library library = await _libraryRepository.Get(userId, libraryId)
                ?? throw new NotFoundException();

            return _mapper.MapToDto(library);
        }
    }
}
