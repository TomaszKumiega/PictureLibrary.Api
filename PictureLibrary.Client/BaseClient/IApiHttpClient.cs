namespace PictureLibrary.Client.BaseClient;

internal interface IApiHttpClient
{
    Task<T> Get<T>(string url) where T : class;
    Task<T> Post<T>(string url, object data) where T : class;
    Task<T> Patch<T>(string url, object data) where T : class;
    Task Delete(string url);
}