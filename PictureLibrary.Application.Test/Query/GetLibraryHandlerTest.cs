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
    public class GetLibraryHandlerTest
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILibraryRepository> _libraryRepositoryMock;

        private readonly GetLibraryHandler _handler;

        public GetLibraryHandlerTest()
        {
            _mapperMock = new Mock<IMapper>(MockBehavior.Strict);
            _libraryRepositoryMock = new Mock<ILibraryRepository>(MockBehavior.Strict);

            _handler = new GetLibraryHandler(_mapperMock.Object, _libraryRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFoundException_When_Library_Was_Not_Found()
        {
            ObjectId userId = ObjectId.GenerateNewId();
            ObjectId libraryId = ObjectId.GenerateNewId();

            var query = new GetLibraryQuery(userId.ToString(), libraryId.ToString());

            _libraryRepositoryMock.Setup(x => x.Get(userId, libraryId))
                .ReturnsAsync((Library?)null)
                .Verifiable();

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));

            _libraryRepositoryMock.Verify();
        }

        [Fact]
        public async Task Handle_Should_Return_LibraryDto()
        {
            ObjectId userId = ObjectId.GenerateNewId();
            ObjectId libraryId = ObjectId.GenerateNewId();

            var library = new LibraryFaker().Generate();
            var libraryDto = new LibraryDtoFaker().Generate();

            var query = new GetLibraryQuery(userId.ToString(), libraryId.ToString());

            _libraryRepositoryMock.Setup(x => x.Get(userId, libraryId))
                .ReturnsAsync(library)
                .Verifiable();

            _mapperMock.Setup(x => x.MapToDto(library))
                .Callback((Library library) =>
                {
                    library.Should().Be(library);
                })
                .Returns(libraryDto)
                .Verifiable();

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().Be(libraryDto);

            _libraryRepositoryMock.Verify();
            _mapperMock.Verify();
        }
    }
}
