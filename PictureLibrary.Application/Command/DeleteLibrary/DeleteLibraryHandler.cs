using MediatR;
using MongoDB.Bson;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Application.Command
{
    public class DeleteLibraryHandler : IRequestHandler<DeleteLibraryCommand>
    {
        private readonly ILibraryRepository _libraryRepository;

        public DeleteLibraryHandler(ILibraryRepository libraryRepository)
        {
            _libraryRepository = libraryRepository;
        }

        public async Task Handle(DeleteLibraryCommand request, CancellationToken cancellationToken)
        {
            ObjectId userId = ObjectId.Parse(request.UserId);
            ObjectId libraryId = ObjectId.Parse(request.LibraryId);

            var library = await _libraryRepository.Get(userId, libraryId) ?? throw new NotFoundException();

            await _libraryRepository.Delete(library);
        }
    }
}
