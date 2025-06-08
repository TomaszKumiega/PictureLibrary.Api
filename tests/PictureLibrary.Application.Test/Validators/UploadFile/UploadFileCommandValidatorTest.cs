using FluentValidation.TestHelper;
using MongoDB.Bson;
using PictureLibrary.Application.Command;
using System.Net.Http.Headers;
using PictureLibrary.Application.Command.UploadFile;

namespace PictureLibrary.Application.Test.Validators.UploadFile;

public class UploadFileCommandValidatorTest
{
    private readonly UploadFileCommandValidator _validator;

    public UploadFileCommandValidatorTest()
    {
        _validator = new UploadFileCommandValidator();
    }

    //TODO: Add tests

}