using PictureLibrary.Domain.Entities;

namespace PictureLibrary.Domain.Services
{
    public interface IImageFileFullInformationProvider
    {
        public FullImageFileInformation GetFullImageFileInformation(ImageFile imageFile, FileMetadata fileMetadata);
    }
}
