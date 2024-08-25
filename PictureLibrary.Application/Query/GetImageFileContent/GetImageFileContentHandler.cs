using MediatR;
using MongoDB.Bson;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;
using PictureLibrary.Domain.Services;

namespace PictureLibrary.Application.Query
{
    public class GetImageFileContentHandler(
        IFileService fileService,
        ILibraryRepository libraryRepository,
        IImageFileRepository imageFileRepository,
        IFileMetadataRepository fileMetadataRepository) : IRequestHandler<GetImageFileContentQuery, GetImageFileContentResult>
    {
        public async Task<GetImageFileContentResult> Handle(GetImageFileContentQuery request, CancellationToken cancellationToken)
        {
            ObjectId userId = ObjectId.Parse(request.UserId);
            ObjectId imageFileId = ObjectId.Parse(request.ImageFileId);

            ImageFile imageFile = imageFileRepository.GetImageFile(imageFileId) ?? throw new NotFoundException();

            bool userOwnsLibrary = await libraryRepository.IsOwner(userId, imageFile.LibraryId);

            if (!userOwnsLibrary)
            {
                throw new NotFoundException();
            }

            FileMetadata fileMetadata = fileMetadataRepository.GetFileMetadata(imageFile.FileId) ?? throw new NotFoundException();

            Stream fileContent = fileService.OpenFile(fileMetadata.FilePath);
            string contentType = fileService.GetFileMimeType(fileMetadata.FilePath);

            return new GetImageFileContentResult(fileContent, contentType);
        }
    }
}
