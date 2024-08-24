using MediatR;
using MongoDB.Bson;
using PictureLibrary.Application.Mapper;
using PictureLibrary.Contracts;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Repositories;
using PictureLibrary.Domain.Services;

namespace PictureLibrary.Application.Command
{
    public class RegisterUserHandler(
        IMapper mapper,
        IUserRepository userRepository,
        IHashAndSaltService hashAndSaltService) : IRequestHandler<RegisterUserCommand, UserDto>
    {
        public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
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
}
