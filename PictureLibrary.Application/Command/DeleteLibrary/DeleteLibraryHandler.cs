using MediatR;
using MongoDB.Bson;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Application.Command.DeleteLibrary;

public class DeleteLibraryHandler(ILibraryRepository libraryRepository) : IRequestHandler<DeleteLibraryCommand>
{
    public async Task Handle(DeleteLibraryCommand request, CancellationToken cancellationToken)
    {
        ObjectId userId = ObjectId.Parse(request.UserId);
        ObjectId libraryId = ObjectId.Parse(request.LibraryId);

        var library = await libraryRepository.Get(userId, libraryId) ?? throw new NotFoundException();

        await libraryRepository.Delete(library);
    }
}