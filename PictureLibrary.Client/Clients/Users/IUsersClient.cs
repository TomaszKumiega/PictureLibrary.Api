using PictureLibrary.Client.Model;
using PictureLibrary.Contracts;

namespace PictureLibrary.Client.Clients.Users
{
    public interface IUsersClient
    {
        Task<AuthorizationData> Login(LoginUserDto request);
        Task<UserDto> Register(NewUserDto request);
        Task<UserDto> GetUser(AuthorizationData authorizationData);
        Task<UserDto> UpdateUser(UpdateUserDto request, AuthorizationData authorizationData);
        Task DeleteUser(AuthorizationData authorizationData);
    }
}