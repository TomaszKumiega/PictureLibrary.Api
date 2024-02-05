using MediatR;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Command
{
    public record UpdateUserCommand(string UserId, UpdateUserDto UserDto) : IRequest<UserDto>;
}
