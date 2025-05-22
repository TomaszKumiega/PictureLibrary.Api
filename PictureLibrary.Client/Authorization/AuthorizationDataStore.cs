using PictureLibrary.Contracts;

namespace PictureLibrary.Client.Authorization;

public record AuthorizationDataStore()
{
    public UserAuthorizationDataDto? UserAuthorizationDataDto { get; set; }
}