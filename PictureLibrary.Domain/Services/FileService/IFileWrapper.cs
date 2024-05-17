namespace PictureLibrary.Domain.Services
{
    public interface IFileWrapper
    {
        Stream Open(string path, FileMode mode);
        void Delete(string path);
        void Copy(string sourcePath, string destinationPath);
    }
}
