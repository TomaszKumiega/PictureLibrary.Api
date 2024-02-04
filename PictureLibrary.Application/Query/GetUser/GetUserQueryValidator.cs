using FluentValidation;

namespace PictureLibrary.Application.Query
{
    public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
    {
        public GetUserQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
