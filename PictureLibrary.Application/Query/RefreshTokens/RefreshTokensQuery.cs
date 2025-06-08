using MediatR;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.Query.RefreshTokens;

public record RefreshTokensQuery(RefreshAuthorizationDataDto AuthorizationDataDto) : IRequest<UserAuthorizationDataDto>;