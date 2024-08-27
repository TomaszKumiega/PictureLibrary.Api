using FluentValidation.TestHelper;
using MongoDB.Bson;
using PictureLibrary.Application.Query;

namespace PictureLibrary.Application.Test.Validators.GetUser
{
    public class GetUserQueryValidatorTest
    {
        private readonly GetUserQueryValidator _validator;

        public GetUserQueryValidatorTest()
        {
            _validator = new GetUserQueryValidator();
        }

        [Fact]
        public void Should_Have_Error_When_UserId_Is_Empty()
        {
            var query = new GetUserQuery(string.Empty);

            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Fact]
        public void Should_Have_Error_When_UserId_Is_Null()
        {
            var query = new GetUserQuery(null!);

            var result = _validator.TestValidate(query);

            result.ShouldHaveValidationErrorFor(x => x.UserId);
        }

        [Fact]
        public void Should_Not_Have_Error_When_UserId_Is_Not_Empty()
        {
            var query = new GetUserQuery(ObjectId.GenerateNewId().ToString());

            var result = _validator.TestValidate(query);

            result.ShouldNotHaveValidationErrorFor(x => x.UserId);
        }
    }
}
