using FluentValidation;
using PictureLibrary.Contracts;

namespace PictureLibrary.Application.DtoValidators
{
    public class NewUserValidator : AbstractValidator<NewUserDto>
    {
        public NewUserValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Password).NotEmpty().MinimumLength(7);
            When(x => x.Email != null, () =>
            {
                RuleFor(x => x.Email).EmailAddress();
            });
        }
    }
}
