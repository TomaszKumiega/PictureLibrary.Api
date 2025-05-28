using PictureLibrary.Domain.Entities;

namespace PictureLibrary.Domain.Services;

public interface IImageFileUpdateService
{
    public UpdateImageFileResult UpdateImageFile(ImageFile imageFile, FileMetadata fileMetadata, UpdateImageFileData updateImageFileData);
}