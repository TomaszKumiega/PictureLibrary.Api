using PictureLibrary.Contracts;

namespace PictureLibrary.Client.Clients.Authorization;

public interface IAuthorizationClient
{
    Task<UserAuthorizationDataDto> Login(LoginUserDto request);
    internal Task RefreshTokensIfNecessary();
    internal UserAuthorizationDataDto GetAuthorizationData();
}