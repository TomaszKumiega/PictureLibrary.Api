using FluentValidation;
using PictureLibrary.Contracts;
using System.Text.RegularExpressions;

namespace PictureLibrary.Application.DtoValidators;

public partial class NewTagValidator : AbstractValidator<NewTagDto>
{
    public NewTagValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.ColorHex).NotEmpty().Must(x => x != null && HexRegex().IsMatch(x));
    }

    [GeneratedRegex("^#[0-9a-fA-F]{6}$")]
    private static partial Regex HexRegex();
}