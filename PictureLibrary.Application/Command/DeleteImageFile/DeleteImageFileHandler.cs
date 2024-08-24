using MediatR;
using MongoDB.Bson;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Application.Command
{
    public class DeleteImageFileHandler(
        ILibraryRepository libraryRepository,
        IImageFileRepository imageFileRepository,
        IFileMetadataRepository fileMetadataRepository) : IRequestHandler<DeleteImageFileCommand>
    {
        public async Task Handle(DeleteImageFileCommand request, CancellationToken cancellationToken)
        {
            ObjectId ownerId = ObjectId.Parse(request.UserId);
            ObjectId imageFileId = ObjectId.Parse(request.ImageFileId);

            ImageFile imageFile = imageFileRepository.FindById(imageFileId) ?? throw new NotFoundException();
        
            if (!await libraryRepository.IsOwner(ownerId, imageFile.LibraryId))
            {
                throw new NotFoundException();
            }

            FileMetadata? metadata = fileMetadataRepository.FindById(imageFile.FileId);

            if (metadata != null)
            {
                await fileMetadataRepository.Delete(metadata);
            }

            await imageFileRepository.Delete(imageFile);
        }
    }
}
