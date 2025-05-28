using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace PictureLibrary.Api.Configuration;

public class PictureLibraryTokenValidationParameters : TokenValidationParameters
{
    public PictureLibraryTokenValidationParameters(IConfiguration configuration)
    {
        ValidateIssuer = true;
        ValidateAudience = true;
        ValidateLifetime = true;
        ValidateIssuerSigningKey = true;
        ValidIssuer = configuration["JwtIssuer"] ?? string.Empty;
        ValidAudience = configuration["JwtAudience"] ?? string.Empty;
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("TokenPrivateKey")!));
        LogValidationExceptions = true;
        ClockSkew = TimeSpan.Zero;
    }
}