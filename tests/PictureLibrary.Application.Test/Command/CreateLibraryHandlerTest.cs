using FluentAssertions;
using MongoDB.Bson;
using Moq;
using PictureLibrary.Application.Command;
using PictureLibrary.Application.Command.CreateLibrary;
using PictureLibrary.Application.Mapper;
using PictureLibrary.Application.Test.Fakers;
using PictureLibrary.Contracts;
using PictureLibrary.Contracts.Library;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;
using PictureLibrary.TestTools.Fakers;

namespace PictureLibrary.Application.Test.Command;

public class CreateLibraryHandlerTest
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ILibraryRepository> _libraryRepositoryMock;

    private readonly CreateLibraryHandler _handler;

    public CreateLibraryHandlerTest()
    {
        _mapperMock = new Mock<IMapper>(MockBehavior.Strict);
        _userRepositoryMock = new Mock<IUserRepository>(MockBehavior.Strict);
        _libraryRepositoryMock = new Mock<ILibraryRepository>(MockBehavior.Strict);

        _handler = new CreateLibraryHandler(_mapperMock.Object, _userRepositoryMock.Object, _libraryRepositoryMock.Object);    
    }

    [Fact]
    public async Task Handle_Should_Throw_NotFoundException_When_User_Doesnt_Exist()
    {
        var command = new CreateLibraryCommand(ObjectId.GenerateNewId().ToString(), null!);

        _userRepositoryMock.Setup(r => r.Query())
            .Returns(new List<User>().AsQueryable())
            .Verifiable();

        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_Should_CreateLibrary()
    {
        NewLibraryDto newLibrary = new NewLibraryDtoFaker().Generate();
        LibraryDto libraryDto = new LibraryDtoFaker().Generate();
        IQueryable<User> userQueryable = new UserFaker().Generate(10).AsQueryable();
        ObjectId userId = userQueryable.Select(x => x.Id).First();

        var request = new CreateLibraryCommand(userId.ToString(), newLibrary);

        _mapperMock.Setup(m => m.MapToDto(It.IsAny<Library>()))
            .Callback((Library l) =>
            {
                l.Name.Should().Be(newLibrary.Name);
                l.Description.Should().Be(newLibrary.Description);
            })
            .Returns(libraryDto)
            .Verifiable();

        _userRepositoryMock.Setup(r => r.Query())
            .Returns(userQueryable)
            .Verifiable();

        _libraryRepositoryMock.Setup(r => r.Add(It.IsAny<Library>()))
            .Callback((Library l) =>
            {
                l.Name.Should().Be(newLibrary.Name);
                l.Description.Should().Be(newLibrary.Description);
                l.OwnerId.Should().Be(userId);
            })
            .Returns(Task.CompletedTask)
            .Verifiable();

        var result = await _handler.Handle(request, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().Be(libraryDto);

        _mapperMock.Verify();
        _userRepositoryMock.Verify();
        _libraryRepositoryMock.Verify();
    }
}