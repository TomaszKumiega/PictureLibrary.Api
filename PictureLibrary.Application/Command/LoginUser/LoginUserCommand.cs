using MediatR;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Command
{
    public record LoginUserCommand(LoginUserDto LoginDto) : IRequest<UserAuthorizationDataDto>;
}
