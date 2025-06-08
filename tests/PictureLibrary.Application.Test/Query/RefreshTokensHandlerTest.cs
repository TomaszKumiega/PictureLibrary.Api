using FluentAssertions;
using Moq;
using PictureLibrary.Application.Mapper;
using PictureLibrary.Application.Query;
using PictureLibrary.Application.Query.RefreshTokens;
using PictureLibrary.Application.Test.Fakers;
using PictureLibrary.Domain.Repositories;
using PictureLibrary.Domain.Services;
using PictureLibrary.TestTools.Fakers;

namespace PictureLibrary.Application.Test.Query;

public class RefreshTokensHandlerTest
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IAuthorizationDataService> _authorizationDataServiceMock;
    private readonly Mock<IAuthorizationDataRepository> _authorizationDataRepositoryMock;

    private readonly RefreshTokensHandler _handler;

    public RefreshTokensHandlerTest()
    {
        _mapperMock = new Mock<IMapper>(MockBehavior.Strict);
        _authorizationDataServiceMock = new Mock<IAuthorizationDataService>(MockBehavior.Strict);
        _authorizationDataRepositoryMock = new Mock<IAuthorizationDataRepository>(MockBehavior.Strict);

        _handler = new RefreshTokensHandler(_mapperMock.Object, _authorizationDataServiceMock.Object, _authorizationDataRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Return_UserAuthorizationDataDto()
    {
        var refreshAuthDataDto = new RefreshAuthorizationDataDtoFaker().Generate();
        var authorizationData = new AuthorizationDataFaker().Generate();
        var authorizationDataDto = new UserAuthorizationDataDtoFaker().Generate();

        _authorizationDataServiceMock.Setup(x => x.RefreshAuthorizationData(refreshAuthDataDto.AccessToken, refreshAuthDataDto.RefreshToken))
            .ReturnsAsync(authorizationData)
            .Verifiable();
            
        _authorizationDataRepositoryMock.Setup(x => x.UpsertForUser(authorizationData))
            .ReturnsAsync(authorizationData)
            .Verifiable();
            
        _mapperMock.Setup(x => x.MapToDto(authorizationData))
            .Returns(authorizationDataDto)
            .Verifiable();
            
        var result = await _handler.Handle(new RefreshTokensQuery(refreshAuthDataDto), new CancellationToken());
            
        result.Should().Be(authorizationDataDto);

        _mapperMock.Verify();
        _authorizationDataServiceMock.Verify();
        _authorizationDataRepositoryMock.Verify();
    }
}