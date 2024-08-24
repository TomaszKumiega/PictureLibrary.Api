using MediatR;
using MongoDB.Bson;
using PictureLibrary.Application.Mapper;
using PictureLibrary.Contracts;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Application.Command
{
    public class UpdateLibraryHandler(
        IMapper mapper,
        ILibraryRepository libraryRepository) : IRequestHandler<UpdateLibraryCommand, LibraryDto>
    {
        private readonly IMapper _mapper = mapper;
        private readonly ILibraryRepository _libraryRepository = libraryRepository;

        public async Task<LibraryDto> Handle(UpdateLibraryCommand request, CancellationToken cancellationToken)
        {
            ObjectId userId = ObjectId.Parse(request.UserId);
            ObjectId libraryId = ObjectId.Parse(request.LibraryId);

            Library library = await _libraryRepository.Get(userId, libraryId)
                ?? throw new NotFoundException();

            library.Name = request.Library.Name;
            library.Description = request.Library.Description;

            await _libraryRepository.Update(library);

            return _mapper.MapToDto(library);
        }
    }
}
