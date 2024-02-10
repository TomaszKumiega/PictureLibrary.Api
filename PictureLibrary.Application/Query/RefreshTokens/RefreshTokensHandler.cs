using MediatR;
using PictureLibrary.Application.Mapper;
using PictureLibrary.Contracts;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Repositories;
using PictureLibrary.Domain.Services;

namespace PictureLibrary.Application.Query
{
    public class RefreshTokensHandler : IRequestHandler<RefreshTokensQuery, UserAuthorizationDataDto>
    {
        private readonly IMapper _mapper;
        private readonly IAuthorizationDataService _authorizationDataService;
        private readonly IAuthorizationDataRepository _authorizationDataRepository;

        public RefreshTokensHandler(
            IMapper mapper,
            IAuthorizationDataService authorizationDataService,
            IAuthorizationDataRepository authorizationDataRepository)
        {
            _mapper = mapper;
            _authorizationDataService = authorizationDataService;
            _authorizationDataRepository = authorizationDataRepository;
        }

        public async Task<UserAuthorizationDataDto> Handle(RefreshTokensQuery request, CancellationToken cancellationToken)
        {
            AuthorizationData authorizationData = await _authorizationDataService.RefreshAuthorizationData(
                request.AuthorizationDataDto.AccessToken, 
                request.AuthorizationDataDto.RefreshToken);

            authorizationData = await _authorizationDataRepository.UpsertForUser(authorizationData);
        
            return _mapper.MapToDto(authorizationData);
        }
    }
}
