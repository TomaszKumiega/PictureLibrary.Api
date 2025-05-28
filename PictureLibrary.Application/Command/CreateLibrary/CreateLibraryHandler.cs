using MediatR;
using MongoDB.Bson;
using PictureLibrary.Application.Mapper;
using PictureLibrary.Contracts;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Application.Command;

public class CreateLibraryHandler(
    IMapper mapper,
    IUserRepository userRepository,
    ILibraryRepository libraryRepository) : IRequestHandler<CreateLibraryCommand, LibraryDto>
{
    public async Task<LibraryDto> Handle(CreateLibraryCommand request, CancellationToken cancellationToken)
    {
        ObjectId userId = ObjectId.Parse(request.UserId);

        bool userExists = userRepository.Query().Any(x => x.Id == userId);
        if (!userExists)
        {
            throw new NotFoundException();
        }

        Library library = new()
        {
            Id = ObjectId.GenerateNewId(),
            OwnerId = userId,
            Name = request.NewLibrary.Name,
            Description = request.NewLibrary.Description,
        };

        await libraryRepository.Add(library);

        return mapper.MapToDto(library);
    }
}