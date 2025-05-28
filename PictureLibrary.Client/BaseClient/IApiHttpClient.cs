using System.Net.Http.Headers;
using PictureLibrary.Client.Results;

namespace PictureLibrary.Client.BaseClient;

internal interface IApiHttpClient
{
    Task<T> Get<T>(string url) where T : class;
    Task<T> Post<T>(string url, object data) where T : class;
    Task<T> Patch<T>(string url, object data) where T : class;
    Task Delete(string url);
    Task<Stream> GetFile(string url);
    Task<FileUploadResult> UploadFile(string url, Memory<byte> content, RangeHeaderValue range);
}