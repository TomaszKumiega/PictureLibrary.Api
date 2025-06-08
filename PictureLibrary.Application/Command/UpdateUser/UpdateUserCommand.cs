using MediatR;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Command.UpdateUser;

public record UpdateUserCommand(string UserId, UpdateUserDto UserDto) : IRequest<UserDto>;