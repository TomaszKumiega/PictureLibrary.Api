using PictureLibrary.Client.Requests;
using PictureLibrary.Contracts;
using PictureLibrary.Contracts.Images;

namespace PictureLibrary.Client.Clients.Images;

public interface IImagesClient
{
    Task<FileCreatedResult> CreateImageFile(UploadFileRequest request);

    Task DeleteImageFile(string imageFileId);
    
    Task<ImageFileDto> UpdateImageFile(string imageFileId, UpdateImageFileDto request);

    Task<ImageFilesDto> GetAllImageFiles(string libraryId);
    
    Task<Stream> DownloadImageFile(string imageFileId);
}