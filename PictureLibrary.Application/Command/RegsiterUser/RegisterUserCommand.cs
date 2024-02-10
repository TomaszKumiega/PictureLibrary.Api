using MediatR;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Command
{
    public record RegisterUserCommand(NewUserDto NewUser) : IRequest<UserDto>;
}
