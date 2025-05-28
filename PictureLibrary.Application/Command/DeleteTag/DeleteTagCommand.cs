using MediatR;

namespace PictureLibrary.Application.Command;

public record DeleteTagCommand(string UserId, string TagId) : IRequest;