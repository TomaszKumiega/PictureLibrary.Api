using PictureLibrary.Client.BaseClient;
using PictureLibrary.Client.FileUpload;
using PictureLibrary.Client.Requests;
using PictureLibrary.Contracts;
using PictureLibrary.Contracts.Images;

namespace PictureLibrary.Client.Clients.Images;

internal class ImagesClient(IApiHttpClient apiHttpClient, IImageFileUpload imageFileUpload) : IImagesClient
{
    public async Task<FileCreatedResult> CreateImageFile(UploadFileRequest request)
    {
        return await imageFileUpload.CreateImageFile(apiHttpClient, request);
    }

    public async Task DeleteImageFile(string imageFileId)
    {
        await apiHttpClient.Delete($"image/delete?imageFileId={imageFileId}");
    }

    public async Task<ImageFileDto> UpdateImageFile(string imageFileId, UpdateImageFileDto request)
    {
        return await apiHttpClient.Patch<ImageFileDto>($"image/update?imageFileId={imageFileId}", request);
    }

    public async Task<ImageFilesDto> GetAllImageFiles(string libraryId)
    {
        return await apiHttpClient.Get<ImageFilesDto>($"image/all?libraryId={libraryId}"); 
    }

    public async Task<Stream> DownloadImageFile(string imageFileId)
    {
        return await apiHttpClient.GetFile($"image/content?imageFileId={imageFileId}");
    }
}