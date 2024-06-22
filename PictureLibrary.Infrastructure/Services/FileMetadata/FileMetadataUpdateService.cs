using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Services;

namespace PictureLibrary.Infrastructure.Services
{
    public class FileMetadataUpdateService : IFileMetadataUpdateService
    {
        private readonly IFileService _fileService;

        public FileMetadataUpdateService(IFileService fileService)
        {
            _fileService = fileService;
        }

        public FileMetadata UpdateFileName(FileMetadata fileMetadata, string newFileName)
        {
            ArgumentNullException.ThrowIfNull(fileMetadata, nameof(fileMetadata));
            ArgumentException.ThrowIfNullOrWhiteSpace(newFileName, nameof(newFileName));

            fileMetadata.FilePath = _fileService.ChangeFileName(fileMetadata.FilePath, newFileName);
            fileMetadata.FileName = newFileName;

            return fileMetadata;
        }
    }
}
