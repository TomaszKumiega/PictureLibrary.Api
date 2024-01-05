using MediatR;
using MongoDB.Bson;
using PictureLibrary.Application.Mapper;
using PictureLibrary.Contracts;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Application.Query
{
    public class GetAllTagsHandler : IRequestHandler<GetAllTagsQuery, TagsDto>
    {
        private readonly IMapper _mapper;
        private readonly ITagRepository _tagRepository;
        private readonly ILibraryRepository _libraryRepository;

        public GetAllTagsHandler(
            IMapper mapper,
            ITagRepository tagRepository,
            ILibraryRepository libraryRepository)
        {
            _mapper = mapper;
            _tagRepository = tagRepository;
            _libraryRepository = libraryRepository;
        }

        public async Task<TagsDto> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
        {
            ObjectId userId = ObjectId.Parse(request.UserId);
            ObjectId libraryId = ObjectId.Parse(request.LibraryId);

            bool userOwnsTheLibrary = await _libraryRepository.IsOwner(userId, libraryId);

            if (!userOwnsTheLibrary)
            {
                throw new NotFoundException();
            }

            var tags = await _tagRepository.GetAll(libraryId);

            var tagDtos = tags.Select(x => _mapper.MapToDto(x));

            return new TagsDto(tagDtos);
        }
    }
}
