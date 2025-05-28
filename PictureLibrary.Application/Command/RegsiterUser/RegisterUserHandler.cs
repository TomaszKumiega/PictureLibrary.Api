using MediatR;
using MongoDB.Bson;
using PictureLibrary.Application.Mapper;
using PictureLibrary.Contracts;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;
using PictureLibrary.Domain.Services;

namespace PictureLibrary.Application.Command;

public class RegisterUserHandler(
    IMapper mapper,
    IUserRepository userRepository,
    IHashAndSaltService hashAndSaltService) : IRequestHandler<RegisterUserCommand, UserDto>
{
    public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        if (userRepository.UsernameExists(request.NewUser.Username))
        {
            throw new AlreadyExistsException(Resources.UsernameAlreadyExists);
        }

        if (!string.IsNullOrEmpty(request.NewUser.Email) && userRepository.EmailExists(request.NewUser.Email))
        {
            throw new AlreadyExistsException(Resources.EmailAlreadyExists);
        }

        var passwordHashAndSalt = hashAndSaltService.GetHashAndSalt(request.NewUser.Password);

        var user = new User
        {
            Id = ObjectId.GenerateNewId(),
            Username = request.NewUser.Username,
            Email = request.NewUser.Email,
            PasswordHash = passwordHashAndSalt.Hash,
            PasswordSalt = passwordHashAndSalt.Salt
        };

        await userRepository.Add(user);

        return mapper.MapToDto(user);
    }
}