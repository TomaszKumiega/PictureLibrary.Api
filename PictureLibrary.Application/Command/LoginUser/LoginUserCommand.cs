using MediatR;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Command.LoginUser;

public record LoginUserCommand(LoginUserDto LoginDto) : IRequest<UserAuthorizationDataDto>;