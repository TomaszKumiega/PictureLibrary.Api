using MediatR;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Command.UpdateTag;

public record UpdateTagCommand(string UserId, string TagId, UpdateTagDto UpdateTagDto) : IRequest<TagDto>;