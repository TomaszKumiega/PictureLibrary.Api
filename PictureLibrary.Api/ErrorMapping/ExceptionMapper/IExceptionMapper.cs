namespace PictureLibrary.Api.ErrorMapping.ExceptionMapper;

public interface IExceptionMapper
{
    ErrorDetails Map(Exception exception);
}