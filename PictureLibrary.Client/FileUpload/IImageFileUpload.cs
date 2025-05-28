using PictureLibrary.Client.BaseClient;
using PictureLibrary.Client.Requests;
using PictureLibrary.Contracts;

namespace PictureLibrary.Client.FileUpload;

internal interface IImageFileUpload
{
    Task<FileCreatedResult> CreateImageFile(IApiHttpClient apiHttpClient, UploadFileRequest request);
}