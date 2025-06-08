using FluentAssertions;
using MongoDB.Bson;
using Moq;
using PictureLibrary.Application.Command;
using PictureLibrary.Application.Command.DeleteTag;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;
using PictureLibrary.TestTools.Fakers;

namespace PictureLibrary.Application.Test.Command;

public class DeleteTagHandlerTest
{
    Mock<ITagRepository> _tagRepositoryMock;
    Mock<ILibraryRepository> _libraryRepositoryMock;

    private readonly DeleteTagHandler _handler;

    public DeleteTagHandlerTest()
    {
        _tagRepositoryMock = new Mock<ITagRepository>(MockBehavior.Strict);
        _libraryRepositoryMock = new Mock<ILibraryRepository>(MockBehavior.Strict);

        _handler = new DeleteTagHandler(_tagRepositoryMock.Object, _libraryRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Delete_Tag()
    {
        var userId = ObjectId.GenerateNewId();
        var tagId = ObjectId.GenerateNewId();
        var tag = new TagFaker().Generate();

        var command = new DeleteTagCommand(userId.ToString(), tagId.ToString());

        _tagRepositoryMock.Setup(x => x.FindById(tagId))
            .Returns(tag)
            .Verifiable();

        _libraryRepositoryMock.Setup(x => x.IsOwner(userId, tag.LibraryId))
            .ReturnsAsync(true)
            .Verifiable();

        _tagRepositoryMock.Setup(x => x.Delete(tag))
            .Callback<Tag>(x =>
            {
                x.Should().Be(tag);
            })
            .Returns(Task.CompletedTask)
            .Verifiable();

        await _handler.Handle(command, CancellationToken.None);

        _tagRepositoryMock.Verify();
        _libraryRepositoryMock.Verify();
    }

    [Fact]
    public async Task Handle_Should_Throw_NotFoundException_When_Tag_Doesnt_Exist()
    {
        var userId = ObjectId.GenerateNewId();
        var tagId = ObjectId.GenerateNewId();

        var command = new DeleteTagCommand(userId.ToString(), tagId.ToString());

        _tagRepositoryMock.Setup(x => x.FindById(tagId))
            .Returns((Tag?)null)
            .Verifiable();

        await Assert.ThrowsAsync<NotFoundException>(async () => await _handler.Handle(command, CancellationToken.None));

        _tagRepositoryMock.Verify();
    }

    [Fact]
    public async Task Handle_Should_Throw_NotFoundException_When_User_Doesnt_Own_The_Library()
    {
        var userId = ObjectId.GenerateNewId();
        var tagId = ObjectId.GenerateNewId();
        var tag = new TagFaker().Generate();

        var command = new DeleteTagCommand(userId.ToString(), tagId.ToString());

        _tagRepositoryMock.Setup(x => x.FindById(tagId))
            .Returns(tag)
            .Verifiable();

        _libraryRepositoryMock.Setup(x => x.IsOwner(userId, tag.LibraryId))
            .ReturnsAsync(false)
            .Verifiable();

        await Assert.ThrowsAsync<NotFoundException>(async () => await _handler.Handle(command, CancellationToken.None));

        _tagRepositoryMock.Verify();
        _libraryRepositoryMock.Verify();
    }
}