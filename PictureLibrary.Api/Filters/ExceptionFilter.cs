using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PictureLibrary.Api.ErrorMapping;
using PictureLibrary.Api.ErrorMapping.ExceptionMapper;

namespace PictureLibrary.Api.Filters;

public class ExceptionFilter(IExceptionMapper exceptionMapper) : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        ErrorDetails errorDetails = exceptionMapper.Map(context.Exception);

        context.Result = new ObjectResult(errorDetails)
        {
            StatusCode = errorDetails.StatusCode,
        };
    }
}