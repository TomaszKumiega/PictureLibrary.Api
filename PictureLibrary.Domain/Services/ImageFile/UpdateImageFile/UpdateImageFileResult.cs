using PictureLibrary.Domain.Entities;

namespace PictureLibrary.Domain.Services
{
    public record UpdateImageFileResult(ImageFile ImageFile, FileMetadata FileMetadata, FullImageFileInformation FullImageFileInformation);
}
