using PictureLibrary.Client.BaseClient;
using PictureLibrary.Contracts;

namespace PictureLibrary.Client.Clients.Users;

internal class UsersClient(IApiHttpClient client) : IUsersClient
{
    public async Task<UserDto> Register(NewUserDto request)
    {
        return await client.Post<UserDto>("user/register", request);
    }

    public async Task<UserDto> GetUser()
    {
        return await client.Get<UserDto>($"user/get");
    }

    public async Task<UserDto> UpdateUser(UpdateUserDto request)
    {
        return await client.Patch<UserDto>("user/update", request);
    }

    public async Task DeleteUser()
    {
        await client.Delete("user/delete");
    }
}