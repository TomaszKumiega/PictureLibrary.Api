using FluentValidation.TestHelper;
using MongoDB.Bson;
using PictureLibrary.Application.Command;
using PictureLibrary.Application.Command.CreateLibrary;
using PictureLibrary.Application.DtoValidators;
using PictureLibrary.Contracts.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PictureLibrary.Application.Test.Validators.CreateLibrary;

public class CreateLibraryCommandValidatorTest
{
    private readonly CreateLibraryCommandValidator _validator;

    public CreateLibraryCommandValidatorTest()
    {
        _validator = new CreateLibraryCommandValidator(new NewLibraryValidator());
    }

    [Fact]
    public void Validate_Have_Errors_When_UserId_Is_Empty()
    {
        var command = new CreateLibraryCommand("", new NewLibraryDto()
        {
            Name = "Name"
        });

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void Validate_Have_Errors_When_UserId_Is_Null()
    {
        var command = new CreateLibraryCommand(null!, new NewLibraryDto()
        {
            Name = "Name"
        });

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void Validate_Have_Errors_When_UserId_Is_Whitespace()
    {
        var command = new CreateLibraryCommand(" ", new NewLibraryDto()
        {
            Name = "Name"
        });

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void Validate_Doesnt_Have_Errors_When_Command_Is_Valid()
    {
        var command = new CreateLibraryCommand(ObjectId.GenerateNewId().ToString(), new NewLibraryDto()
        {
            Name = "Name"
        });

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void Validate_Have_Errors_When_UserId_Is_Not_ObjectId()
    {
        var command = new CreateLibraryCommand("test", new NewLibraryDto()
        {
            Name = "Name"
        });

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void Validate_Have_Errors_When_NewLibrary_Is_Null()
    {
        var command = new CreateLibraryCommand(ObjectId.GenerateNewId().ToString(), null!);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.NewLibrary);
    }
}