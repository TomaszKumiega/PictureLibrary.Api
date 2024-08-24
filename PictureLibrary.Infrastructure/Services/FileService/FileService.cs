using PictureLibrary.Domain.Services;
using System.Drawing;

namespace PictureLibrary.Infrastructure.Services
{
    public class FileService(
        IFileWrapper fileWrapper,
        IPathsProvider pathsProvider) : IFileService
    {
        public void AppendFile(string fileName, Stream contentStream)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
            ArgumentNullException.ThrowIfNull(contentStream);

            using var fileStream = fileWrapper.Open(fileName, FileMode.Append);

            contentStream.Position = 0;
            contentStream.CopyTo(fileStream);
        }

        public string ChangeFileName(string filePath, string newFileName)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(filePath);

            string fileDirectoryPath = Path.GetDirectoryName(filePath)
                ?? throw new ArgumentException("File path is incorrect.");
            string newFilePath = Path.Combine(fileDirectoryPath, newFileName);
            
            File.Move(filePath, newFilePath);

            return newFilePath;
        }

        public void Insert(string fileName, Stream contentStream, long position)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(fileName);
            ArgumentNullException.ThrowIfNull(contentStream);

            string tempDirectoryPath = pathsProvider.GetTempDirectoryPath();
            string tempFilePath = Path.Combine(tempDirectoryPath, Path.GetRandomFileName());

            using (var fileStream = fileWrapper.Open(fileName, FileMode.Open))
            using (var tempFileStream = fileWrapper.Open(tempFilePath, FileMode.Create))
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

            fileWrapper.Delete(fileName);
            fileWrapper.Copy(tempFilePath, fileName);
            fileWrapper.Delete(tempFilePath);
        }
    }
}
