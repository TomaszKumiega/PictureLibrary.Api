using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;
using PictureLibrary.Domain.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PictureLibrary.Infrastructure.Services
{
    public class AuthorizationDataService : IAuthorizationDataService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthorizationDataRepository _authorizationDataRepository;

        public AuthorizationDataService(
            IUserRepository userRepository,
            IAuthorizationDataRepository authorizationDataRepository)
        {

            _userRepository = userRepository;
            _authorizationDataRepository = authorizationDataRepository;
        }

        public AuthorizationData GenerateAuthorizationData(User user, string privateKey)
        {
            var (accessToken, expiryDate) = GenerateAccessToken(user, privateKey);

            return new()
            {
                Id = ObjectId.GenerateNewId(),
                UserId = user.Id,
                AccessToken = accessToken,
                RefreshToken = GenerateRefreshToken(),
                ExpiryDate = expiryDate,
            };
        }

        public async Task<AuthorizationData> RefreshAuthorizationData(
            TokenValidationParameters tokenValidationParams, 
            string accessToken, 
            string refreshToken, 
            string privateKey)
        {
            var handler = new JwtSecurityTokenHandler();
            var validationResult = await handler.ValidateTokenAsync(accessToken, tokenValidationParams);

            if (!validationResult.IsValid)
                throw new InvalidTokenException();

            var id = (string)validationResult.Claims[ClaimTypes.NameIdentifier];
            ObjectId userId = ObjectId.Parse(id);

            var tokens = _authorizationDataRepository.GetByUserId(userId);
            var user = await _userRepository.GetById(userId);

            if (tokens?.RefreshToken != refreshToken || user == null)
                throw new InvalidTokenException();

            return GenerateAuthorizationData(user, privateKey);
        }

        private static (string accessToken, DateTime expiryDate) GenerateAccessToken(User user, string privateKey)
        {
            DateTime expires = DateTime.UtcNow.AddHours(1);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(privateKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(ClaimTypes.Email, user.Email ?? string.Empty),
                    new(ClaimTypes.Name, user.Username),
                }),

                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return (tokenHandler.WriteToken(token), expires);
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            return Convert.ToBase64String(randomNumber);
        }
    }
}
