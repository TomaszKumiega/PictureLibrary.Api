using PictureLibrary.Client.Model;

namespace PictureLibrary.Client.Clients
{
    public class UsersClient(IApiHttpClient client) : IUsersClient
    {
        public async Task<AuthorizationData> Login(LoginUserRequest request)
        {
            return await client.Post<AuthorizationData>("auth/login", request);
        }
    }
}
