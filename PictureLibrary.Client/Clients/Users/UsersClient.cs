using PictureLibrary.Client.Model;
using PictureLibrary.Client.Requests;

namespace PictureLibrary.Client.Clients.Users
{
    public class UsersClient(IApiHttpClient client) : IUsersClient
    {
        public async Task<AuthorizationData> Login(LoginUserRequest request)
        {
            return await client.Post<AuthorizationData>("auth/login", request);
        }

        public async Task<User> Register(AddUserRequest request)
        {
            return await client.Post<User>("user/register", request);
        }

        public async Task<User> GetUser(AuthorizationData authorizationData)
        {
            return await client.Get<User>($"user/get", authorizationData);
        }

        public async Task<User> UpdateUser(UpdateUserRequest request, AuthorizationData authorizationData)
        {
            return await client.Patch<User>("user/update", request, authorizationData);
        }

        public async Task DeleteUser(AuthorizationData authorizationData)
        {
            await client.Get<User>("user/delete", authorizationData);
        }
    }
}
