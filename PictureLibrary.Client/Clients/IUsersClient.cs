using PictureLibrary.Client.Model;

namespace PictureLibrary.Client.Clients
{
    public interface IUsersClient
    {
        Task<AuthorizationData> Login(LoginUserRequest request);
    }
}