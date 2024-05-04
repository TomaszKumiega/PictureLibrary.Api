namespace PictureLibrary.Domain.Services
{
    public interface IFileService
    {
        bool Exists(string path);
        Stream OpenFile(string path);
    }
}
