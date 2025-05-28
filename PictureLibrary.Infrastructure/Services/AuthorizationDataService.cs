using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using PictureLibrary.Domain.Configuration;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;
using PictureLibrary.Domain.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PictureLibrary.Infrastructure.Services;

public class AuthorizationDataService(
    IAppSettings appSettings,
    IUserRepository userRepository,
    IAuthorizationDataRepository authorizationDataRepository) : IAuthorizationDataService
{
    public AuthorizationData GenerateAuthorizationData(User user)
    {
        var (accessToken, expiryDate) = GenerateAccessToken(user, appSettings.TokenPrivateKey);

        return new()
        {
            Id = ObjectId.GenerateNewId(),
            UserId = user.Id,
            AccessToken = accessToken,
            RefreshToken = GenerateRefreshToken(),
            ExpiryDate = expiryDate,
        };
    }

    public async Task<AuthorizationData> RefreshAuthorizationData(string accessToken, string refreshToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var validationResult = await handler.ValidateTokenAsync(accessToken, GetTokenValidationParameters());

        if (!validationResult.IsValid)
        {
            throw new InvalidTokenException();
        }

        var id = (string)validationResult.Claims[ClaimTypes.NameIdentifier];
        ObjectId userId = ObjectId.Parse(id);

        var tokens = authorizationDataRepository.GetByUserId(userId);
        var user = userRepository.FindById(userId);

        if (tokens?.RefreshToken != refreshToken || user == null)
        {
            throw new InvalidTokenException();
        }

        return GenerateAuthorizationData(user);
    }

    private TokenValidationParameters GetTokenValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = appSettings.JwtIssuer,
            ValidAudience = appSettings.JwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.TokenPrivateKey)),
        };
    }

    private (string accessToken, DateTime expiryDate) GenerateAccessToken(User user, string privateKey)
    {
        DateTime expires = DateTime.UtcNow.AddHours(1);
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(privateKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email ?? string.Empty),
                new(ClaimTypes.Name, user.Username),
            ]),
            Issuer = appSettings.JwtIssuer,
            Audience = appSettings.JwtAudience,
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