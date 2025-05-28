using MediatR;
using MongoDB.Bson;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Application.Command;

public class DeleteTagHandler(
    ITagRepository tagRepository,
    ILibraryRepository libraryRepository) : IRequestHandler<DeleteTagCommand>
{
    public async Task Handle(DeleteTagCommand request, CancellationToken cancellationToken)
    {
        ObjectId userId = ObjectId.Parse(request.UserId);
        ObjectId tagId = ObjectId.Parse(request.TagId);

        var tag = tagRepository.FindById(tagId) ?? throw new NotFoundException();

        bool userOwnsTheLibrary = await libraryRepository.IsOwner(userId, tag.LibraryId);

        if (!userOwnsTheLibrary)
        {
            throw new NotFoundException();
        }

        await tagRepository.Delete(tag);
    }
}