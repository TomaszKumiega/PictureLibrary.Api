using PictureLibrary.Client.BaseClient;
using PictureLibrary.Client.Model;
using PictureLibrary.Contracts;

namespace PictureLibrary.Client.Clients.Users
{
    internal class UsersClient(IApiHttpClient client) : IUsersClient
    {
        public async Task<AuthorizationData> Login(LoginUserDto request)
        {
            return await client.Post<AuthorizationData>("auth/login", request);
        }

        public async Task<UserDto> Register(NewUserDto request)
        {
            return await client.Post<UserDto>("user/register", request);
        }

        public async Task<UserDto> GetUser(AuthorizationData authorizationData)
        {
            return await client.Get<UserDto>($"user/get", authorizationData);
        }

        public async Task<UserDto> UpdateUser(UpdateUserDto request, AuthorizationData authorizationData)
        {
            return await client.Patch<UserDto>("user/update", request, authorizationData);
        }

        public async Task DeleteUser(AuthorizationData authorizationData)
        {
            await client.Delete("user/delete", authorizationData);
        }
    }
}
