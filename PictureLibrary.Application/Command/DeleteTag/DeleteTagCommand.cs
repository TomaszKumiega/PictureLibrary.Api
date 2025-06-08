using MediatR;

namespace PictureLibrary.Application.Command.DeleteTag;

public record DeleteTagCommand(string UserId, string TagId) : IRequest;