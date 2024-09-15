using PictureLibrary.Client.Model;

namespace PictureLibrary.Client.BaseClient
{
    public interface IApiHttpClient
    {
        Task<T> Get<T>(string url, AuthorizationData? authorizationData = null) where T : class;
        Task<T> Post<T>(string url, object data, AuthorizationData? authorizationData = null) where T : class;
        Task<T> Patch<T>(string url, object data, AuthorizationData? authorizationData = null) where T : class;
        Task Delete(string url, AuthorizationData? authorizationData = null);
    }
}