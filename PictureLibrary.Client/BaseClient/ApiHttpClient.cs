using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using PictureLibrary.Client.Clients.Authorization;
using PictureLibrary.Client.ErrorHandling;
using PictureLibrary.Client.Exceptions;

namespace PictureLibrary.Client.BaseClient
{
    internal class ApiHttpClient(
        HttpClient httpClient,
        IErrorHandler errorHandler,
        IAuthorizationClient authorizationClient) : IApiHttpClient
    {
        public async Task<T> Get<T>(string url) where T : class
        {
            HttpRequestMessage request = new(HttpMethod.Get, url);
            
            request = await AddAuthorizationHeaderWithValidToken(request);
            
            HttpResponseMessage response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                errorHandler.HandleErrorStatusCode(response);
            }

            string responseJson = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(responseJson) ?? throw new InvalidResponseException();
        }

        public async Task<T> Post<T>(string url, object data) where T : class
        {
            HttpRequestMessage request = new(HttpMethod.Post, url);
            
            request = await AddAuthorizationHeaderWithValidToken(request);
            request.Content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                errorHandler.HandleErrorStatusCode(response);
            }

            string responseJson = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(responseJson) ?? throw new InvalidResponseException();
        }

        public async Task<T> Patch<T>(string url, object data) where T : class
        {
            HttpRequestMessage request = new(HttpMethod.Patch, url);

            request = await AddAuthorizationHeaderWithValidToken(request);
            request.Content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                errorHandler.HandleErrorStatusCode(response);
            }

            string responseJson = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(responseJson) ?? throw new InvalidResponseException();
        }

        public async Task Delete(string url)
        {
            HttpRequestMessage request = new(HttpMethod.Delete, url);

            request = await AddAuthorizationHeaderWithValidToken(request);

            HttpResponseMessage response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                errorHandler.HandleErrorStatusCode(response);
            }
        }

        private async Task<HttpRequestMessage> AddAuthorizationHeaderWithValidToken(HttpRequestMessage request)
        {
            await authorizationClient.RefreshTokensIfNecessary();
            var tokens = authorizationClient.GetAuthorizationData();
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokens.AccessToken);

            return request;
        }
    }
}
