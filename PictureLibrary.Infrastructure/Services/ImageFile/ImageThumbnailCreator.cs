using PictureLibrary.Domain.Services;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace PictureLibrary.Infrastructure.Services;

public class ImageThumbnailCreator : IImageThumbnailCreator
{
    public async Task<string> GetBase64Thumbnail(string filePath)
    {
        using Image image = await Image.LoadAsync(filePath);
        using MemoryStream thumbnailMemoryStream = new();

        image.Mutate(x => x.Resize(50, 50));
        await image.SaveAsync(thumbnailMemoryStream, new JpegEncoder());

        byte[] thumbnailBytes = thumbnailMemoryStream.ToArray();

        return Convert.ToBase64String(thumbnailBytes);
    }
}