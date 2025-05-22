using PictureLibrary.Contracts;

namespace PictureLibrary.Client.Clients.Users
{
    public interface IUsersClient
    {
        Task<UserDto> Register(NewUserDto request);
        Task<UserDto> GetUser();
        Task<UserDto> UpdateUser(UpdateUserDto request);
        Task DeleteUser();
    }
}