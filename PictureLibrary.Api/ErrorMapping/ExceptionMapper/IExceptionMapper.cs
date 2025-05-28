namespace PictureLibrary.Api.ErrorMapping;

public interface IExceptionMapper
{
    ErrorDetails Map(Exception exception);
}