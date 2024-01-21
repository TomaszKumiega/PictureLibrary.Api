using PictureLibrary.Domain.Services;

namespace PictureLibrary.Infrastructure.Services
{
    public class HashAndSaltService : IHashAndSaltService
    {
        public HashAndSalt GetHashAndSalt(string text)
        {
            ArgumentException.ThrowIfNullOrEmpty(text, nameof(text));

            using var hmac = new System.Security.Cryptography.HMACSHA512();

            return new HashAndSalt
            {
                Text = text,
                Hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(text)),
                Salt = hmac.Key
            };
        }
    }
}
