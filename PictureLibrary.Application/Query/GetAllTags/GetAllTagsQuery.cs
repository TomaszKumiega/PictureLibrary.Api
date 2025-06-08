using MediatR;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Query.GetAllTags;

public record GetAllTagsQuery(string UserId, string LibraryId) : IRequest<TagsDto>;