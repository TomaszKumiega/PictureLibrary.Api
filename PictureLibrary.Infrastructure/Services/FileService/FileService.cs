using PictureLibrary.Domain.Services;

namespace PictureLibrary.Infrastructure.Services
{
    public class FileService : IFileService
    {
        private readonly IFileWrapper _fileWrapper;
        private readonly IPathsProvider _pathsProvider;

        public FileService(
            IFileWrapper fileWrapper,
            IPathsProvider pathsProvider)
        {
            _fileWrapper = fileWrapper;
            _pathsProvider = pathsProvider;
        }

        public void AppendFile(string fileName, Stream contentStream)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
            ArgumentNullException.ThrowIfNull(contentStream);

            using var fileStream = _fileWrapper.Open(fileName, FileMode.Append);

            contentStream.Position = 0;
            contentStream.CopyTo(fileStream);
        }

        public void Insert(string fileName, Stream contentStream, long position)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
            ArgumentNullException.ThrowIfNull(contentStream);

            string tempDirectoryPath = _pathsProvider.GetTempDirectoryPath();
            string tempFilePath = Path.Combine(tempDirectoryPath, Path.GetRandomFileName());

            using (var fileStream = _fileWrapper.Open(fileName, FileMode.Open))
            using (var tempFileStream = _fileWrapper.Open(tempFilePath, FileMode.Create))
            {
                while (fileStream.Position < position)
                {
                    tempFileStream.WriteByte((byte)fileStream.ReadByte());
                }

                contentStream.CopyTo(tempFileStream);

                while (fileStream.Position != fileStream.Length - 1)
                {
                    tempFileStream.WriteByte((byte)fileStream.ReadByte());
                }
            }

            _fileWrapper.Delete(fileName);
            _fileWrapper.Copy(tempFilePath, fileName);
            _fileWrapper.Delete(tempFilePath);
        }
    }
}
