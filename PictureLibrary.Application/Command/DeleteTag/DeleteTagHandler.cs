using MediatR;
using MongoDB.Bson;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Application.Command
{
    public class DeleteTagHandler : IRequestHandler<DeleteTagCommand>
    {
        private readonly ITagRepository _tagRepository;
        private readonly ILibraryRepository _libraryRepository;

        public DeleteTagHandler(
            ITagRepository tagRepository,
            ILibraryRepository libraryRepository)
        {
            _tagRepository = tagRepository;
            _libraryRepository = libraryRepository;
        }

        public async Task Handle(DeleteTagCommand request, CancellationToken cancellationToken)
        {
            ObjectId userId = ObjectId.Parse(request.UserId);
            ObjectId tagId = ObjectId.Parse(request.TagId);

            var tag = await _tagRepository.Get(tagId) ?? throw new NotFoundException();

            bool userOwnsTheLibrary = await _libraryRepository.IsOwner(userId, tag.LibraryId);

            if (!userOwnsTheLibrary)
            {
                throw new NotFoundException();
            }

            await _tagRepository.Delete(tag);
        }
    }
}
