using MediatR;
using MongoDB.Bson;
using PictureLibrary.Application.Mapper;
using PictureLibrary.Contracts;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Application.Query.GetAllTags;

public class GetAllTagsHandler(
    IMapper mapper,
    ITagRepository tagRepository,
    ILibraryRepository libraryRepository) 
    : IRequestHandler<GetAllTagsQuery, TagsDto>
{
    public async Task<TagsDto> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
    {
        ObjectId userId = ObjectId.Parse(request.UserId);
        ObjectId libraryId = ObjectId.Parse(request.LibraryId);

        bool userOwnsTheLibrary = await libraryRepository.IsOwner(userId, libraryId);

        if (!userOwnsTheLibrary)
        {
            throw new NotFoundException();
        }

        var tags = await tagRepository.GetAll(libraryId);

        var tagDtos = tags.Select(x => mapper.MapToDto(x));

        return new TagsDto(tagDtos);
    }
}