using MediatR;
using MongoDB.Bson;
using PictureLibrary.Application.Mapper;
using PictureLibrary.Contracts;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Application.Query.GetUser;

public class GetUserHandler(
    IMapper mapper,
    IUserRepository userRepository) 
    : IRequestHandler<GetUserQuery, UserDto>
{
    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        ObjectId userId = ObjectId.Parse(request.UserId);
        User user = userRepository.FindById(userId) ?? throw new NotFoundException();
        
        return await Task.FromResult(mapper.MapToDto(user));
    }
}