using MediatR;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Command
{
    public record UpdateUserCommand(UpdateUserDto UserDto) : IRequest<UserDto>;
}
