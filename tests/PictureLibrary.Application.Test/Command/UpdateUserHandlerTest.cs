using FluentAssertions;
using MongoDB.Bson;
using Moq;
using PictureLibrary.Application.Command;
using PictureLibrary.Application.Command.UpdateUser;
using PictureLibrary.Application.Mapper;
using PictureLibrary.Application.Test.Fakers;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;
using PictureLibrary.TestTools.Fakers;

namespace PictureLibrary.Application.Test.Command;

public class UpdateUserHandlerTest
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;

    private readonly UpdateUserHandler _handler;

    public UpdateUserHandlerTest()
    {
        _mapperMock = new Mock<IMapper>(MockBehavior.Strict);
        _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);

        _handler = new UpdateUserHandler(_mapperMock.Object, _userRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Update_User()
    {
        ObjectId userId = ObjectId.GenerateNewId();

        var user = new UserFaker().Generate();
        var userDto = new UserDtoFaker().Generate();
        var updateUserDto = new UpdateUserDtoFaker().Generate();

        var command = new UpdateUserCommand(userId.ToString(), updateUserDto);

        _userRepositoryMock.Setup(x => x.FindById(userId))
            .Returns(user)
            .Verifiable();

        _userRepositoryMock.Setup(x => x.Update(user))
            .Callback((User u) =>
            {
                u.Email.Should().Be(updateUserDto.Email);
                u.Username.Should().Be(updateUserDto.Username);
            })
            .Returns(Task.CompletedTask)
            .Verifiable();

        _mapperMock.Setup(x => x.MapToDto(user))
            .Returns(userDto)
            .Verifiable();

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().Be(userDto);
    }

    [Fact]
    public async Task Handle_Should_Throw_NotFoundException()
    {
        ObjectId userId = ObjectId.GenerateNewId();

        var updateUserDto = new UpdateUserDtoFaker().Generate();

        var command = new UpdateUserCommand(userId.ToString(), updateUserDto);

        _userRepositoryMock.Setup(x => x.FindById(userId))
            .Returns((User?)null)
            .Verifiable();

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));

        _userRepositoryMock.Verify();
    }
}