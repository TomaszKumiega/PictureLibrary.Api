using MediatR;
using PictureLibrary.Contracts;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;
using PictureLibrary.Domain.Services;

namespace PictureLibrary.Application.Command;

public class LoginUserHandler(
    IUserRepository userRepository,
    IHashAndSaltService hashAndSaltService,
    IAuthorizationDataService authorizationDataService,
    IAuthorizationDataRepository authorizationDataRepository) 
    : IRequestHandler<LoginUserCommand, UserAuthorizationDataDto>
{
    public async Task<UserAuthorizationDataDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        User user = userRepository.GetByUsername(request.LoginDto.Username) ?? throw new NotFoundException();

        HashAndSalt hashAndSalt = new()
        {
            Text = request.LoginDto.Password,
            Hash = user.PasswordHash,
            Salt = user.PasswordSalt
        };

        if (!hashAndSaltService.Verify(hashAndSalt))
        {
            throw new NotFoundException();
        }

        AuthorizationData authorizationData = authorizationDataService.GenerateAuthorizationData(user);

        authorizationData = await authorizationDataRepository.UpsertForUser(authorizationData);

        return new UserAuthorizationDataDto
        {
            UserId = user.Id.ToString(),
            AccessToken = authorizationData.AccessToken,
            RefreshToken = authorizationData.RefreshToken,
            ExpiryDate = authorizationData.ExpiryDate
        };
    }
}