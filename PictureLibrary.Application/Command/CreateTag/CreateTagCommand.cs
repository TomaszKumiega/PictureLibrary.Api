using MediatR;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Command;

public record CreateTagCommand(string UserId, string LibraryId, NewTagDto NewTagDto) : IRequest<TagDto>;