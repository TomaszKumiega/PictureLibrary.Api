using MediatR;
using PictureLibrary.Contracts;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;
using PictureLibrary.Domain.Services;

namespace PictureLibrary.Application.Command
{
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, UserAuthorizationDataDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IHashAndSaltService _hashAndSaltService;
        private readonly IAuthorizationDataService _authorizationDataService;
        private readonly IAuthorizationDataRepository _authorizationDataRepository;

        public LoginUserHandler(
            IUserRepository userRepository,
            IHashAndSaltService hashAndSaltService,
            IAuthorizationDataService authorizationDataService,
            IAuthorizationDataRepository authorizationDataRepository)
        {
            _userRepository = userRepository;
            _hashAndSaltService = hashAndSaltService;
            _authorizationDataService = authorizationDataService;
            _authorizationDataRepository = authorizationDataRepository;
        }

        public async Task<UserAuthorizationDataDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            User user = _userRepository.GetByUsername(request.LoginDto.Username) ?? throw new NotFoundException();

            HashAndSalt hashAndSalt = new()
            {
                Text = request.LoginDto.Password,
                Hash = user.PasswordHash,
                Salt = user.PasswordSalt
            };

            if (!_hashAndSaltService.Verify(hashAndSalt))
            {
                throw new NotFoundException();
            }

            AuthorizationData authorizationData = _authorizationDataService.GenerateAuthorizationData(user);

            authorizationData = await _authorizationDataRepository.UpsertForUser(authorizationData);

            return new UserAuthorizationDataDto
            {
                UserId = user.Id.ToString(),
                AccessToken = authorizationData.AccessToken,
                RefreshToken = authorizationData.RefreshToken,
                ExpiryDate = authorizationData.ExpiryDate
            };
        }
    }
}
