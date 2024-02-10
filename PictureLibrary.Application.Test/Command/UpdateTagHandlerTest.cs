using FluentAssertions;
using MongoDB.Bson;
using Moq;
using PictureLibrary.Application.Command;
using PictureLibrary.Application.Mapper;
using PictureLibrary.Application.Test.Fakers;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;
using PictureLibrary.TestTools.Fakers;

namespace PictureLibrary.Application.Test.Command
{
    public class UpdateTagHandlerTest
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ITagRepository> _tagRepositoryMock;
        private readonly Mock<ILibraryRepository> _libraryRepositoryMock;

        private readonly UpdateTagHandler _handler;

        public UpdateTagHandlerTest()
        {
            _mapperMock = new Mock<IMapper>(MockBehavior.Strict);
            _tagRepositoryMock = new Mock<ITagRepository>(MockBehavior.Strict);
            _libraryRepositoryMock = new Mock<ILibraryRepository>(MockBehavior.Strict);

            _handler = new UpdateTagHandler(_mapperMock.Object, _tagRepositoryMock.Object, _libraryRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFoundException_When_User_Doesnt_Own_The_Library()
        {
            var userId = ObjectId.GenerateNewId();
            var libraryId = ObjectId.GenerateNewId();
            var command = new UpdateTagCommand(userId.ToString(), libraryId.ToString(), new UpdateTagDtoFaker().Generate());
            var tag = new TagFaker().Generate();

            _tagRepositoryMock.Setup(x => x.Get(It.IsAny<ObjectId>()))
                .ReturnsAsync(tag);

            _libraryRepositoryMock.Setup(x => x.IsOwner(userId, tag.LibraryId))
                .ReturnsAsync(false)
                .Verifiable();

            await Assert.ThrowsAsync<NotFoundException>(async () => await _handler.Handle(command, CancellationToken.None));

            _libraryRepositoryMock.Verify();
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFoundException_When_Tag_Doesnt_Exist()
        {
            var userId = ObjectId.GenerateNewId();
            var libraryId = ObjectId.GenerateNewId();
            var command = new UpdateTagCommand(userId.ToString(), libraryId.ToString(), new UpdateTagDtoFaker().Generate());

            _tagRepositoryMock.Setup(x => x.Get(It.IsAny<ObjectId>()))
                .ReturnsAsync((Tag?)null)
                .Verifiable();

            await Assert.ThrowsAsync<NotFoundException>(async () => await _handler.Handle(command, CancellationToken.None));

            _tagRepositoryMock.Verify();
        }

        [Fact]
        public async Task Handle_Should_Update_Tag()
        {
            var userId = ObjectId.GenerateNewId();
            var tagId = ObjectId.GenerateNewId();
            
            var tag = new TagFaker().Generate();
            var command = new UpdateTagCommand(userId.ToString(), tagId.ToString(), new UpdateTagDtoFaker().Generate());

            var tagDto = new TagDtoFaker().Generate();

            _libraryRepositoryMock.Setup(x => x.IsOwner(userId, tag.LibraryId))
                .ReturnsAsync(true)
                .Verifiable();

            _tagRepositoryMock.Setup(x => x.Get(tagId))
                .ReturnsAsync(tag)
                .Verifiable();

            _tagRepositoryMock.Setup(x => x.Update(It.IsAny<Tag>()))
                .Callback<Tag>(x =>
                {
                    x.Id.Should().Be(tag.Id);
                    x.LibraryId.Should().Be(tag.LibraryId);
                    x.Name.Should().Be(command.UpdateTagDto.Name);
                    x.ColorHex.Should().Be(command.UpdateTagDto.ColorHex);
                })
                .Returns(Task.CompletedTask)
                .Verifiable();

            _mapperMock.Setup(x => x.MapToDto(tag))
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
}
