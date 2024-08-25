using MediatR;
using MongoDB.Bson;
using PictureLibrary.Application.Mapper;
using PictureLibrary.Contracts;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;
using PictureLibrary.Domain.Services;

namespace PictureLibrary.Application.Command
{
    public class UpdateImageFileHandler(
        IMapper mapper,
        ILibraryRepository libraryRepository,
        IImageFileRepository imageFileRepository,
        IImageFileUpdateService imageFileUpdateService,
        IFileMetadataRepository fileMetadataRepository) : IRequestHandler<UpdateImageFileCommand, ImageFileDto>
    {
        public async Task<ImageFileDto> Handle(UpdateImageFileCommand request, CancellationToken cancellationToken)
        {
            ObjectId userId = ObjectId.Parse(request.UserId);
            ObjectId imageFileId = ObjectId.Parse(request.ImageFileId);

            ImageFile imageFile = imageFileRepository.FindById(imageFileId) ?? throw new NotFoundException();
            FileMetadata fileMetadata = fileMetadataRepository.FindById(imageFile.FileId) ?? throw new NotFoundException();

            await EnsureUserOwnsTheLibrary(userId, imageFile.LibraryId);

            UpdateImageFileData updateImageFileData = mapper.MapToUpdateImageFileData(request.Dto);
            UpdateImageFileResult result = imageFileUpdateService.UpdateImageFile(imageFile, fileMetadata, updateImageFileData);

            await fileMetadataRepository.Update(result.FileMetadata);
            await imageFileRepository.Update(result.ImageFile);

            return mapper.MapToDto(result.ImageFile);
        }

        private async Task EnsureUserOwnsTheLibrary(ObjectId userId, ObjectId libraryId)
        {
            bool isUserOwnerOfTheLibrary = await libraryRepository.IsOwner(userId, libraryId);
            if (!isUserOwnerOfTheLibrary)
            {
                throw new NotFoundException();
            }
        }
    }
}
