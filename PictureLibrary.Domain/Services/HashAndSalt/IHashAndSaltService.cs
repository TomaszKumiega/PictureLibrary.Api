namespace PictureLibrary.Domain.Services;

public interface IHashAndSaltService
{
    HashAndSalt GetHashAndSalt(string text);
    bool Verify(HashAndSalt hashAndSalt);
}