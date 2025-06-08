using MediatR;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Command.CreateTag;

public record CreateTagCommand(string UserId, string LibraryId, NewTagDto NewTagDto) : IRequest<TagDto>;