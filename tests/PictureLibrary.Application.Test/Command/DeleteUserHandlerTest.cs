using FluentAssertions;
using MongoDB.Bson;
using Moq;
using PictureLibrary.Application.Command;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;
using PictureLibrary.TestTools.Fakers;

namespace PictureLibrary.Application.Test.Command;

public class DeleteUserHandlerTest
{
    private readonly Mock<IUserRepository> _userRepositoryMock;

    private readonly DeleteUserHandler _handler;

    public DeleteUserHandlerTest()
    {
        _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);

        _handler = new DeleteUserHandler(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Delete_User()
    {
        ObjectId userId = ObjectId.GenerateNewId();
        User user = new UserFaker().Generate();

        var command = new DeleteUserCommand(userId.ToString());

        _userRepositoryMock.Setup(x => x.FindById(userId))
            .Returns(user)
            .Verifiable();

        _userRepositoryMock.Setup(x => x.Delete(user))
            .Callback<User>(u => u.Should().Be(user))
            .Returns(Task.CompletedTask)
            .Verifiable();

        await _handler.Handle(command, CancellationToken.None);

        _userRepositoryMock.Verify();
    }

    [Fact]
    public async Task Handle_Should_Throw_NotFoundException()
    {
        ObjectId userId = ObjectId.GenerateNewId();

        var command = new DeleteUserCommand(userId.ToString());

        _userRepositoryMock.Setup(x => x.FindById(userId))
            .Returns((User?)null)
            .Verifiable();

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));

        _userRepositoryMock.Verify();
    }
}