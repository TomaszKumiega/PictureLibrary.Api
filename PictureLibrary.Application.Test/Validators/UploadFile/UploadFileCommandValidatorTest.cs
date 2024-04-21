using FluentValidation.TestHelper;
using MongoDB.Bson;
using PictureLibrary.Application.Command;
using System.Net.Http.Headers;

namespace PictureLibrary.Application.Test.Validators.UploadFile
{
    public class UploadFileCommandValidatorTest
    {
        private readonly UploadFileCommandValidator _validator;

        public UploadFileCommandValidatorTest()
        {
            _validator = new UploadFileCommandValidator();
        }

        [Fact]
        public void Should_Have_Error_When_UserId_Is_Empty()
        {
            var command = new UploadFileCommand(string.Empty, new ContentRangeHeaderValue(20,100,1000));

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Fact]
        public void Should_Have_Error_When_UserId_Is_Null()
        {
            var command = new UploadFileCommand(null, new ContentRangeHeaderValue(20, 100, 1000));

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Fact]
        public void Should_Have_Error_When_ContentRange_Is_Null()
        {
            var command = new UploadFileCommand(ObjectId.GenerateNewId().ToString(), null);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(x => x.ContentRange);
        }
    }
}
