using MediatR;
using MongoDB.Bson;
using PictureLibrary.Application.Mapper;
using PictureLibrary.Contracts;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Repositories;
using PictureLibrary.Domain.Services;

namespace PictureLibrary.Application.Command
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, UserDto>
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IHashAndSaltService _hashAndSaltService;

        public RegisterUserHandler(
            IMapper mapper, 
            IUserRepository userRepository,
            IHashAndSaltService hashAndSaltService)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _hashAndSaltService = hashAndSaltService;
        }

        public async Task<UserDto> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var passwordHashAndSalt = _hashAndSaltService.GetHashAndSalt(request.NewUser.Password);

            var user = new User
            {
                Id = ObjectId.GenerateNewId(),
                Username = request.NewUser.Username,
                Email = request.NewUser.Email,
                PasswordHash = passwordHashAndSalt.Hash,
                PasswordSalt = passwordHashAndSalt.Salt
            };

            await _userRepository.Add(user);

            return _mapper.MapToDto(user);
        }
    }
}
