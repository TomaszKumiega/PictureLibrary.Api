using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using PictureLibrary.Client.Authorization;
using PictureLibrary.Client.ErrorHandling;
using PictureLibrary.Client.Exceptions;
using PictureLibrary.Contracts;

namespace PictureLibrary.Client.Clients.Authorization;

internal class AuthorizationClient(
    HttpClient httpClient,
    IErrorHandler errorHandler,
    AuthorizationDataStore authorizationDataStore) : IAuthorizationClient
{
    public async Task<UserAuthorizationDataDto> Login(LoginUserDto request)
    {
        HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, "auth/login");

        string json = JsonSerializer.Serialize(request);
        httpRequestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
        
        HttpResponseMessage response = await httpClient.SendAsync(httpRequestMessage);
        
        if (!response.IsSuccessStatusCode)
        {
            errorHandler.HandleErrorStatusCode(response);
        }
        
        authorizationDataStore.UserAuthorizationDataDto = await response.Content.ReadFromJsonAsync<UserAuthorizationDataDto>() ?? throw new InvalidResponseException();
        
        return authorizationDataStore.UserAuthorizationDataDto;
    }
    
    public async Task RefreshTokensIfNecessary()
    {
        if (authorizationDataStore.UserAuthorizationDataDto is null)
        {
            throw new Exception("Login first.");
        }

        if (authorizationDataStore.UserAuthorizationDataDto.ExpiryDate < DateTime.UtcNow.AddMinutes(1))
        {
            authorizationDataStore.UserAuthorizationDataDto = await RefreshTokens();
        }
    }
    
    public UserAuthorizationDataDto GetAuthorizationData() => authorizationDataStore.UserAuthorizationDataDto ?? throw new Exception("Login first.");
    
    private async Task<UserAuthorizationDataDto?> RefreshTokens()
    {
        if (authorizationDataStore?.UserAuthorizationDataDto is null)
        {
            throw new InvalidOperationException("No user authorization data.");
        }
        
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "auth/refreshTokens");

        RefreshAuthorizationDataDto refreshAuthorizationDataDto = new RefreshAuthorizationDataDto()
        {
            AccessToken = authorizationDataStore.UserAuthorizationDataDto.AccessToken,
            RefreshToken = authorizationDataStore.UserAuthorizationDataDto.RefreshToken,
        };
        
        string json = JsonSerializer.Serialize(refreshAuthorizationDataDto);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        
        HttpResponseMessage response = await httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            errorHandler.HandleErrorStatusCode(response);
        }

        return await response.Content.ReadFromJsonAsync<UserAuthorizationDataDto>();
    }
}