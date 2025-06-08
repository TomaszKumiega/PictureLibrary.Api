using MediatR;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Command.RegsiterUser;

public record RegisterUserCommand(NewUserDto NewUser) : IRequest<UserDto>;