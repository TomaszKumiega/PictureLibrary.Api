using MediatR;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Query;

public record GetAllTagsQuery(string UserId, string LibraryId) : IRequest<TagsDto>;