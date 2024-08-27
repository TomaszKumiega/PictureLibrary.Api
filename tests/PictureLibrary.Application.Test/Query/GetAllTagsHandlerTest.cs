using FluentAssertions;
using MongoDB.Bson;
using Moq;
using PictureLibrary.Application.Mapper;
using PictureLibrary.Application.Query;
using PictureLibrary.Application.Test.Fakers;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;
using PictureLibrary.TestTools.Fakers;

namespace PictureLibrary.Application.Test.Query
{
    public class GetAllTagsHandlerTest
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ITagRepository> _tagRepositoryMock;
        private readonly Mock<ILibraryRepository> _libraryRepositoryMock;
        
        private readonly GetAllTagsHandler _handler;

        public GetAllTagsHandlerTest()
        {
            _mapperMock = new Mock<IMapper>(MockBehavior.Strict);
            _tagRepositoryMock = new Mock<ITagRepository>(MockBehavior.Strict);
            _libraryRepositoryMock = new Mock<ILibraryRepository>(MockBehavior.Strict);

            _handler = new GetAllTagsHandler(_mapperMock.Object, _tagRepositoryMock.Object, _libraryRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_All_Tags()
        {
            var userId = ObjectId.GenerateNewId();
            var libraryId = ObjectId.GenerateNewId();
            var query = new GetAllTagsQuery(userId.ToString(), libraryId.ToString());

            var tags = new TagFaker().Generate(10);
            var tagDto = new TagDtoFaker().Generate();

            _libraryRepositoryMock.Setup(x => x.IsOwner(userId, libraryId))
                .ReturnsAsync(true)
                .Verifiable();

            _tagRepositoryMock.Setup(x => x.GetAll(libraryId))
                .ReturnsAsync(tags)
                .Verifiable();

            _mapperMock.Setup(x => x.MapToDto(It.IsIn<Tag>(tags)))
                .Returns(tagDto)
                .Verifiable();

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Tags.Should().NotBeNullOrEmpty();
            result.Tags.Should().HaveCount(tags.Count);
        }

        [Fact]
        public async Task Handle_Should_Return_Empty_Tags_Collection_When_No_Tags_Are_Found()
        {
            var userId = ObjectId.GenerateNewId();
            var libraryId = ObjectId.GenerateNewId();
            var query = new GetAllTagsQuery(userId.ToString(), libraryId.ToString());

            var tags = Enumerable.Empty<Tag>();
            var tagDto = new TagDtoFaker().Generate();

            _libraryRepositoryMock.Setup(x => x.IsOwner(userId, libraryId))
                .ReturnsAsync(true)
                .Verifiable();

            _tagRepositoryMock.Setup(x => x.GetAll(libraryId))
                .ReturnsAsync(tags)
                .Verifiable();

            _mapperMock.Setup(x => x.MapToDto(It.IsIn(tags)))
                .Returns(tagDto)
                .Verifiable();

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Tags.Should().BeEmpty();
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFoundException_When_User_Doesnt_Own_The_Library()
        {
            var userId = ObjectId.GenerateNewId();
            var libraryId = ObjectId.GenerateNewId();
            var query = new GetAllTagsQuery(userId.ToString(), libraryId.ToString());

            _libraryRepositoryMock.Setup(x => x.IsOwner(userId, libraryId))
                .ReturnsAsync(false)
                .Verifiable();

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
        }
    }
}
