using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Services;

namespace PictureLibrary.Infrastructure.Services
{
    public class ImageFileFullInformationProvider : IImageFileFullInformationProvider
    {
        public FullImageFileInformation GetFullImageFileInformation(ImageFile imageFile, FileMetadata fileMetadata)
        {
            return new FullImageFileInformation
            {
                Id = imageFile.Id.ToString(),
                LibraryId = imageFile.LibraryId.ToString(),
                FileName = fileMetadata.FileName,
                TagIds = imageFile.TagIds?.Select(x => x.ToString()),
                Base64Thumbnail = string.Empty, //TODO: dodanie pobierania ikonki
            };
        }
    }
}
