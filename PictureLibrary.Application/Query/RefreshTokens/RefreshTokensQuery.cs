using MediatR;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Query;

public record RefreshTokensQuery(RefreshAuthorizationDataDto AuthorizationDataDto) : IRequest<UserAuthorizationDataDto>;