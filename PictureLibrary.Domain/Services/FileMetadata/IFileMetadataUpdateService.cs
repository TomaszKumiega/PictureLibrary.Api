using PictureLibrary.Domain.Entities;

namespace PictureLibrary.Domain.Services;

public interface IFileMetadataUpdateService
{
    FileMetadata UpdateFileName(FileMetadata fileMetadata, string newFileName);
}