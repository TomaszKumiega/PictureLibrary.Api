using PictureLibrary.Domain.Services;

namespace PictureLibrary.Infrastructure.Services;

public class HashAndSaltService : IHashAndSaltService
{
    public HashAndSalt GetHashAndSalt(string text)
    {
        ArgumentException.ThrowIfNullOrEmpty(text);

        using var hmac = new System.Security.Cryptography.HMACSHA512();

        return new HashAndSalt
        {
            Text = text,
            Hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(text)),
            Salt = hmac.Key
        };
    }

    public bool Verify(HashAndSalt hashAndSalt)
    {
        if (hashAndSalt == null ||
            string.IsNullOrEmpty(hashAndSalt.Text) ||
            hashAndSalt.Hash == null ||
            hashAndSalt.Salt == null ||
            hashAndSalt.Hash.Length == 0 ||
            hashAndSalt.Salt.Length == 0)
        {
            throw new ArgumentException("Invalid hash and salt parameter." , nameof(hashAndSalt));
        }

        using var hmac = new System.Security.Cryptography.HMACSHA512(hashAndSalt.Salt);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(hashAndSalt.Text));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != hashAndSalt.Hash[i])
            {
                return false;
            }
        }

        return true;
    }
}