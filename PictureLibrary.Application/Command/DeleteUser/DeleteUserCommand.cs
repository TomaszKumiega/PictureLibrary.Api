using MediatR;

namespace PictureLibrary.Application.Command
{
    public record DeleteUserCommand(string UserId) : IRequest;
}
