using MediatR;
using MongoDB.Bson;
using PictureLibrary.Application.Mapper;
using PictureLibrary.Contracts;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Application.Command
{
    public class CreateTagHandler : IRequestHandler<CreateTagCommand, TagDto>
    {
        private readonly IMapper _mapper;
        private readonly ITagRepository _tagRepository;
        private readonly ILibraryRepository _libraryRepository;

        public CreateTagHandler(
            IMapper mapper,
            ITagRepository tagRepository,
            ILibraryRepository libraryRepository)
        {
            _mapper = mapper;
            _tagRepository = tagRepository;
            _libraryRepository = libraryRepository;
        }

        public async Task<TagDto> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
            ObjectId userId = ObjectId.Parse(request.UserId);
            ObjectId libraryId = ObjectId.Parse(request.LibraryId);

            bool userOwnsTheLibrary = await _libraryRepository.IsOwner(userId, libraryId);

            if (!userOwnsTheLibrary)
            {
                throw new NotFoundException();
            }

            var tag = new Tag
            {
                Id = ObjectId.GenerateNewId(),
                LibraryId = libraryId,
                Name = request.NewTagDto.Name,
                ColorHex = request.NewTagDto.ColorHex,
            };

            await _tagRepository.Add(tag);

            return _mapper.MapToDto(tag);
        }
    }
}
