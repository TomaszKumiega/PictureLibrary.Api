using FluentAssertions;
using MongoDB.Bson;
using Moq;
using PictureLibrary.Application.Command;
using PictureLibrary.Application.Command.CreateTag;
using PictureLibrary.Application.Mapper;
using PictureLibrary.Application.Test.Fakers;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;

namespace PictureLibrary.Application.Test.Command;

public class CreateTagHandlerTest
{
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<ITagRepository> _tagRepositoryMock;
    private readonly Mock<ILibraryRepository> _libraryRepositoryMock;

    private readonly CreateTagHandler _handler;

    public CreateTagHandlerTest()
    {
        _mapperMock = new Mock<IMapper>(MockBehavior.Strict);
        _tagRepositoryMock = new Mock<ITagRepository>(MockBehavior.Strict);
        _libraryRepositoryMock = new Mock<ILibraryRepository>(MockBehavior.Strict);

        _handler = new CreateTagHandler(_mapperMock.Object, _tagRepositoryMock.Object, _libraryRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Throw_NotFoundException_When_User_Doesnt_Own_The_Library()
    {
        var userId = ObjectId.GenerateNewId();
        var libraryId = ObjectId.GenerateNewId();
        var command = new CreateTagCommand(userId.ToString(), libraryId.ToString(), new NewTagDtoFaker());

        _libraryRepositoryMock.Setup(x => x.IsOwner(userId, libraryId))
            .ReturnsAsync(false)
            .Verifiable();

        await Assert.ThrowsAsync<NotFoundException>(async () => await _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_Should_Create_Tag()
    {
        var userId = ObjectId.GenerateNewId();
        var libraryId = ObjectId.GenerateNewId();
        var newTag = new NewTagDtoFaker().Generate();
        var command = new CreateTagCommand(userId.ToString(), libraryId.ToString(), newTag);

        var tagDto = new TagDtoFaker().Generate();

        _libraryRepositoryMock.Setup(x => x.IsOwner(userId, libraryId))
            .ReturnsAsync(true)
            .Verifiable();

        _tagRepositoryMock.Setup(x => x.Add(It.IsAny<Tag>()))
            .Callback<Tag>(x =>
            {
                x.LibraryId.Should().Be(libraryId);
                x.Name.Should().Be(newTag.Name);
                x.ColorHex.Should().Be(newTag.ColorHex);
            })
            .Returns(Task.CompletedTask)
            .Verifiable();

        _mapperMock.Setup(x => x.MapToDto(It.IsAny<Tag>()))
            .Returns(tagDto)
            .Verifiable();

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().Be(tagDto);

        _mapperMock.Verify();
        _tagRepositoryMock.Verify();
        _libraryRepositoryMock.Verify();
    }
}