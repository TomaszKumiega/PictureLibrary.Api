using PictureLibrary.Domain.Services;

namespace PictureLibrary.Infrastructure.Services
{
    public class FileWrapper : IFileWrapper
    {
        public void Copy(string sourcePath, string destinationPath)
        {
            File.Copy(sourcePath, destinationPath);
        }

        public void Delete(string path)
        {
            File.Delete(path);
        }

        public Stream Open(string path, FileMode mode)
        {
            return File.Open(path, mode);
        }
    }
}
