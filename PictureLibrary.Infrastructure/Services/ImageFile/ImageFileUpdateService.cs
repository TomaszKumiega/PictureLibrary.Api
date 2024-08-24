using MongoDB.Bson;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Services;

namespace PictureLibrary.Infrastructure.Services
{
    public class ImageFileUpdateService(
        IFileService fileService,
        IImageFileFullInformationProvider imageFileFullInfoProvider) : IImageFileUpdateService
    {
        public UpdateImageFileResult UpdateImageFile(
            ImageFile imageFile, 
            FileMetadata fileMetadata, 
            UpdateImageFileData updateImageFileData)
        {
            fileMetadata = UpdateImageFileName(fileMetadata, updateImageFileData.FileName);
            imageFile = UpdateTagIds(imageFile, updateImageFileData.TagIds);
            imageFile = UpdateLibraryId(imageFile, updateImageFileData.LibraryId);

            FullImageFileInformation fullInformation = imageFileFullInfoProvider.GetFullImageFileInformation(imageFile, fileMetadata);

            return new UpdateImageFileResult(imageFile, fileMetadata, fullInformation);
        }

        private FileMetadata UpdateImageFileName(FileMetadata fileMetadata, string? fileName)
        {
            if (fileName != null)
            {
                string newFilePath = fileService.ChangeFileName(fileMetadata.FilePath, fileName);

                fileMetadata.FilePath = newFilePath;
                fileMetadata.FileName = fileName;
            }

            return fileMetadata;
        }
        private static ImageFile UpdateTagIds(ImageFile imageFile, IEnumerable<string>? tagIds)
        {
            if (tagIds != null)
            {
                imageFile.TagIds = tagIds.Select(ObjectId.Parse);
            }

            return imageFile;
        }
        private static ImageFile UpdateLibraryId(ImageFile imageFile, string? libraryId)
        {
            if (libraryId != null)
            {
                imageFile.LibraryId = ObjectId.Parse(libraryId);
            }

            return imageFile;
        }
    }
}
