namespace PictureLibrary.Domain.Services
{
    public interface IImageThumbnailCreator
    {
        /// <summary>
        /// Creates 50x50 thumbnail in base64 format.
        /// </summary>
        /// <returns></returns>
        public string GetBase64Thumbnail(string filePath);
    }
}
