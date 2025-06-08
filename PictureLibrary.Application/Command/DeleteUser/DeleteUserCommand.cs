using MediatR;

namespace PictureLibrary.Application.Command.DeleteUser;

public record DeleteUserCommand(string UserId) : IRequest;