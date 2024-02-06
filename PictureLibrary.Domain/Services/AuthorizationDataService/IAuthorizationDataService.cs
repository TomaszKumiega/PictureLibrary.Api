using Microsoft.IdentityModel.Tokens;
using PictureLibrary.Domain.Entities;

namespace PictureLibrary.Domain.Services
{
    public interface IAuthorizationDataService
    {
        /// <summary>
        /// Generates tokens for specified user.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        AuthorizationData GenerateAuthorizationData(User user);

        /// <summary>
        /// Generates new tokens.
        /// </summary>
        /// <param name="tokenValidationParams"></param>
        /// <param name="accessToken">Expired access token</param>
        /// <param name="refreshToken">Refresh token</param>
        /// <returns></returns>
        Task<AuthorizationData> RefreshAuthorizationData(TokenValidationParameters tokenValidationParams, string accessToken, string refreshToken);
    }
}
