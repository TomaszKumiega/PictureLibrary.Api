using MediatR;
using MongoDB.Bson;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Application.Command
{
    public class DeleteImageFileHandler : IRequestHandler<DeleteImageFileCommand>
    {
        private readonly ILibraryRepository _libraryRepository;
        private readonly IImageFileRepository _imageFileRepository;
        private readonly IFileMetadataRepository _fileMetadataRepository;

        public DeleteImageFileHandler(
            ILibraryRepository libraryRepository,
            IImageFileRepository imageFileRepository, 
            IFileMetadataRepository fileMetadataRepository)
        {
            _libraryRepository = libraryRepository;
            _imageFileRepository = imageFileRepository;
            _fileMetadataRepository = fileMetadataRepository;
        }

        public async Task Handle(DeleteImageFileCommand request, CancellationToken cancellationToken)
        {
            ObjectId ownerId = ObjectId.Parse(request.UserId);
            ObjectId imageFileId = ObjectId.Parse(request.ImageFileId);

            ImageFile imageFile = _imageFileRepository.FindById(imageFileId) ?? throw new NotFoundException();
        
            if (!await _libraryRepository.IsOwner(ownerId, imageFile.LibraryId))
            {
                throw new NotFoundException();
            }

            FileMetadata? metadata = _fileMetadataRepository.FindById(imageFile.FileId);

            if (metadata != null)
            {
                await _fileMetadataRepository.Delete(metadata);
            }

            await _imageFileRepository.Delete(imageFile);
        }
    }
}
