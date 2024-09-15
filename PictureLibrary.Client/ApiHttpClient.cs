using PictureLibrary.Client.ErrorHandling;
using PictureLibrary.Client.Exceptions;
using PictureLibrary.Client.Model;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace PictureLibrary.Client
{
    public class ApiHttpClient(HttpClient httpClient, IErrorHandler errorHandler)
    {
        public async Task<T?> Get<T>(string url, AuthorizationData? authorizationData = null) where T : class
        {
            HttpRequestMessage request = new(HttpMethod.Get, url);

            if (authorizationData != null)
            {
                if (!IsTokenValid(authorizationData))
                {
                    authorizationData = await RefreshTokens(authorizationData);
                }

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authorizationData.AccessToken);
            }

            HttpResponseMessage response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                errorHandler.HandleErrorStatusCode(response);
            }

            string responseJson = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(responseJson);
        }

        public async Task<T?> Post<T>(string url, object data, AuthorizationData? authorizationData = null) where T : class
        {
            HttpRequestMessage request = new(HttpMethod.Post, url);

            if (authorizationData != null)
            {
                if (!IsTokenValid(authorizationData))
                {
                    authorizationData = await RefreshTokens(authorizationData);
                }

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authorizationData.AccessToken);
            }

            request.Content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");

            HttpResponseMessage response = await httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                errorHandler.HandleErrorStatusCode(response);
            }

            string responseJson = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<T>(responseJson);
        }

        private async Task<AuthorizationData> RefreshTokens(AuthorizationData authorizationData)
        {
            var refreshTokensRequest = new RefreshAuthorizationDataRequest()
            { 
                AccessToken = authorizationData.AccessToken, 
                RefreshToken = authorizationData.RefreshToken 
            };

            return await Post<AuthorizationData>("auth/refreshTokens", refreshTokensRequest) ?? throw new AuthorizationFailedException();
        }

        private static bool IsTokenValid(AuthorizationData authorizationData)
        {
            return authorizationData.ExpiryDate > DateTime.UtcNow.AddMinutes(1);
        }
    }
}
