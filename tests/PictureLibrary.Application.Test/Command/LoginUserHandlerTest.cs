using FluentAssertions;
using MongoDB.Bson;
using Moq;
using PictureLibrary.Application.Command;
using PictureLibrary.Application.Command.LoginUser;
using PictureLibrary.Application.Test.Fakers;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;
using PictureLibrary.Domain.Services;
using PictureLibrary.TestTools.Fakers;

namespace PictureLibrary.Application.Test.Command;

public class LoginUserHandlerTest
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IHashAndSaltService> _hashAndSaltServiceMock;
    private readonly Mock<IAuthorizationDataService> _authorizationDataServiceMock;
    private readonly Mock<IAuthorizationDataRepository> _authorizationDataRepositoryMock;

    private readonly LoginUserHandler _handler;
    
    public LoginUserHandlerTest()
    {
        _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
        _hashAndSaltServiceMock = new Mock<IHashAndSaltService>(MockBehavior.Strict);
        _authorizationDataServiceMock = new Mock<IAuthorizationDataService>(MockBehavior.Strict);
        _authorizationDataRepositoryMock = new Mock<IAuthorizationDataRepository>(MockBehavior.Strict);

        _handler = new LoginUserHandler(
            _userRepositoryMock.Object, 
            _hashAndSaltServiceMock.Object, 
            _authorizationDataServiceMock.Object, 
            _authorizationDataRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_UserAuthorizationDataDto()
    {
        var loginUser = new LoginUserDtoFaker().Generate();
        var user = new UserFaker().Generate();
        var authorizationData = new AuthorizationData
        {
            Id = ObjectId.GenerateNewId(),
            UserId = user.Id,
            AccessToken = "accessToken",
            RefreshToken = "refreshToken",
            ExpiryDate = DateTime.Now
        };

        _userRepositoryMock.Setup(x => x.GetByUsername(loginUser.Username))
            .Returns(user)
            .Verifiable();

        _hashAndSaltServiceMock.Setup(x => x.Verify(It.IsAny<HashAndSalt>()))
            .Returns(true)
            .Verifiable();

        _authorizationDataServiceMock.Setup(x => x.GenerateAuthorizationData(user))
            .Returns(authorizationData)
            .Verifiable();

        _authorizationDataRepositoryMock.Setup(x => x.UpsertForUser(authorizationData))
            .ReturnsAsync(authorizationData)
            .Verifiable();

        var result = await _handler.Handle(new LoginUserCommand(loginUser), new CancellationToken());

        result.Should().NotBeNull();
        result.UserId.Should().Be(user.Id.ToString());
        result.AccessToken.Should().BeEquivalentTo(authorizationData.AccessToken);
        result.RefreshToken.Should().BeEquivalentTo(authorizationData.RefreshToken);
        result.ExpiryDate.Should().Be(authorizationData.ExpiryDate);

        _userRepositoryMock.Verify();
        _hashAndSaltServiceMock.Verify();
        _authorizationDataServiceMock.Verify();
    }

    [Fact]
    public async Task Handle_Should_Throw_NotFoundException_When_User_Doesnt_Exist()
    {
        var loginUser = new LoginUserDtoFaker().Generate();

        _userRepositoryMock.Setup(x => x.GetByUsername(loginUser.Username))
            .Returns((User?)null)
            .Verifiable();

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(new LoginUserCommand(loginUser), new CancellationToken()));

        _userRepositoryMock.Verify();
    }

    [Fact]
    public async Task Handle_Should_Throw_NotFoundException_When_Password_Is_Invalid()
    {
        var loginUser = new LoginUserDtoFaker().Generate();
        var user = new UserFaker().Generate();

        _userRepositoryMock.Setup(x => x.GetByUsername(loginUser.Username))
            .Returns(user)
            .Verifiable();

        _hashAndSaltServiceMock.Setup(x => x.Verify(It.IsAny<HashAndSalt>()))
            .Returns(false)
            .Verifiable();

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(new LoginUserCommand(loginUser), new CancellationToken()));

        _userRepositoryMock.Verify();
        _hashAndSaltServiceMock.Verify();
    }
}