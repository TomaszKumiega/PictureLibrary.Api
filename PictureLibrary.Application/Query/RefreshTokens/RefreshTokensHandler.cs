using MediatR;
using PictureLibrary.Application.Mapper;
using PictureLibrary.Contracts;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Repositories;
using PictureLibrary.Domain.Services;

namespace PictureLibrary.Application.Query.RefreshTokens;

public class RefreshTokensHandler(
    IMapper mapper, 
    IAuthorizationDataService authorizationDataService, 
    IAuthorizationDataRepository authorizationDataRepository) 
    : IRequestHandler<RefreshTokensQuery, UserAuthorizationDataDto>
{
    public async Task<UserAuthorizationDataDto> Handle(RefreshTokensQuery request, CancellationToken cancellationToken)
    {
        AuthorizationData authorizationData = await authorizationDataService.RefreshAuthorizationData(
            request.AuthorizationDataDto.AccessToken, 
            request.AuthorizationDataDto.RefreshToken);

        authorizationData = await authorizationDataRepository.UpsertForUser(authorizationData);
        
        return mapper.MapToDto(authorizationData);
    }
}