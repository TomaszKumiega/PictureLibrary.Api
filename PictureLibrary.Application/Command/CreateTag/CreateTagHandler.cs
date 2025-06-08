using MediatR;
using MongoDB.Bson;
using PictureLibrary.Application.Mapper;
using PictureLibrary.Contracts;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Application.Command.CreateTag;

public class CreateTagHandler(
    IMapper mapper,
    ITagRepository tagRepository,
    ILibraryRepository libraryRepository) : IRequestHandler<CreateTagCommand, TagDto>
{
    public async Task<TagDto> Handle(CreateTagCommand request, CancellationToken cancellationToken)
    {
        ObjectId userId = ObjectId.Parse(request.UserId);
        ObjectId libraryId = ObjectId.Parse(request.LibraryId);

        bool userOwnsTheLibrary = await libraryRepository.IsOwner(userId, libraryId);

        if (!userOwnsTheLibrary)
        {
            throw new NotFoundException();
        }

        var tag = new Tag
        {
            Id = ObjectId.GenerateNewId(),
            LibraryId = libraryId,
            Name = request.NewTagDto.Name,
            ColorHex = request.NewTagDto.ColorHex,
        };

        await tagRepository.Add(tag);

        return mapper.MapToDto(tag);
    }
}