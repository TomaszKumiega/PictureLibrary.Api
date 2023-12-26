using MediatR;
using MongoDB.Bson;
using PictureLibrary.Application.Mapper;
using PictureLibrary.Contracts;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Application.Command
{
    public class CreateLibraryHandler : IRequestHandler<CreateLibraryCommand, LibraryDto>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly ILibraryRepository _libraryRepository;

        public CreateLibraryHandler(
            IMapper mapper,
            IUserRepository userRepository,
            ILibraryRepository libraryRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _libraryRepository = libraryRepository;
        }

        public async Task<LibraryDto> Handle(CreateLibraryCommand request, CancellationToken cancellationToken)
        {
            ObjectId userId = ObjectId.Parse(request.UserId);

            bool userExists = _userRepository.Query().Any(x => x.Id == userId);
            if (!userExists)
            {
                throw new NotFoundException();
            }

            Library library = new()
            {
                Id = ObjectId.GenerateNewId(),
                OwnerId = userId,
                Name = request.NewLibrary.Name,
                Description = request.NewLibrary.Description,
            };

            await _libraryRepository.Add(library);

            return _mapper.MapToDto(library);
        }
    }
}
