using MediatR;
using MongoDB.Bson;
using PictureLibrary.Application.Mapper;
using PictureLibrary.Contracts;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Application.Command
{
    public class UpdateTagHandler : IRequestHandler<UpdateTagCommand, TagDto>
    {
        private readonly IMapper _mapper;
        private readonly ITagRepository _tagRepository;
        private readonly ILibraryRepository _libraryRepository;

        public UpdateTagHandler(
            IMapper mapper,
            ITagRepository tagRepository,
            ILibraryRepository libraryRepository)
        {
            _mapper = mapper;
            _tagRepository = tagRepository;
            _libraryRepository = libraryRepository;
        }

        public async Task<TagDto> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
        {
            ObjectId userId = ObjectId.Parse(request.UserId);
            ObjectId tagId = ObjectId.Parse(request.TagId);

            var tag = _tagRepository.FindById(tagId) ?? throw new NotFoundException();

            bool userOwnsTheLibrary = await _libraryRepository.IsOwner(userId, tag.LibraryId);

            if (!userOwnsTheLibrary)
            {
                throw new NotFoundException();
            }

            tag.Name = request.UpdateTagDto.Name;
            tag.ColorHex = request.UpdateTagDto.ColorHex;

            await _tagRepository.Update(tag);

            return _mapper.MapToDto(tag);
        }
    }
}
