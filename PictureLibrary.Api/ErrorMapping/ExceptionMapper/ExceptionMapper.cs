using FluentValidation;
using PictureLibrary.Domain.Exceptions;

namespace PictureLibrary.Api.ErrorMapping
{
    public class ExceptionMapper : IExceptionMapper
    {
        public ErrorDetails Map(Exception exception)
        {
            return exception switch
            {
                ValidationException e => new ErrorDetails
                {
                    StatusCode = 400,
                    ErrorCode = ErrorCode.ValidationError,
                    Message = e.Message
                },
                NotFoundException e => new ErrorDetails
                {
                    StatusCode = 404,
                    ErrorCode = ErrorCode.NotFound,
                    Message = e.Message
                },
                AlreadyExistsException e => new ErrorDetails
                {
                    StatusCode = 409,
                    ErrorCode = ErrorCode.AlreadyExists,
                    Message = e.Message
                },
                _ => new ErrorDetails
                {
                    StatusCode = 500,
                    Message = "An error occurred"
                }
            };
        }
    }
}
