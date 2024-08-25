using PictureLibrary.Domain.Services;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace PictureLibrary.Infrastructure.Services
{
    public class ImageThumbnailCreator : IImageThumbnailCreator
    {
        public string GetBase64Thumbnail(string filePath)
        {
            using Image image = Image.Load(filePath);
            using MemoryStream thumbnailMemoryStream = new();

            image.Mutate(x => x.Resize(50, 50));
            image.Save(thumbnailMemoryStream, new JpegEncoder());

            byte[] thumbnailBytes = thumbnailMemoryStream.ToArray();

            return Convert.ToBase64String(thumbnailBytes);
        }
    }
}
