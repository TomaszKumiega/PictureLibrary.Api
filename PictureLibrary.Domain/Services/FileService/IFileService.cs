namespace PictureLibrary.Domain.Services
{
    public interface IFileService
    {
        void AppendFile(string fileName, Stream contentStream);
        void Insert(string fileName, Stream contentStream, long position);
    }
}
