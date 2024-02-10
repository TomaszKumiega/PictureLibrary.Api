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
    public class UpdateLibraryHandlerTest
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILibraryRepository> _libraryRepositoryMock;

        private readonly UpdateLibraryHandler _handler;

        public UpdateLibraryHandlerTest()
        {
            _mapperMock = new Mock<IMapper>(MockBehavior.Strict);
            _libraryRepositoryMock = new Mock<ILibraryRepository>(MockBehavior.Strict);

            _handler = new UpdateLibraryHandler(_mapperMock.Object, _libraryRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_Should_Update_Library()
        {
            ObjectId userId = ObjectId.GenerateNewId();
            ObjectId libraryId = ObjectId.GenerateNewId();

            var library = new LibraryFaker().Generate();
            var libraryDto = new LibraryDtoFaker().Generate();
            var updateLibraryDto = new UpdateLibraryDtoFaker().Generate();

            var command = new UpdateLibraryCommand(userId.ToString(), libraryId.ToString(), updateLibraryDto);

            _libraryRepositoryMock.Setup(x => x.Get(userId, libraryId))
                .ReturnsAsync(library)
                .Verifiable();

            _libraryRepositoryMock.Setup(x => x.Update(library))
                .Callback((Library lib) =>
                {
                    lib.Name.Should().Be(updateLibraryDto.Name);
                    lib.Description.Should().Be(updateLibraryDto.Description);
                })
                .Returns(Task.CompletedTask)
                .Verifiable();

            _mapperMock.Setup(x => x.MapToDto(library))
                .Returns(libraryDto)
                .Verifiable();

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().Be(libraryDto);

            _mapperMock.Verify();
            _libraryRepositoryMock.Verify();
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFoundException_When_Library_Doesnt_Exist()
        {
            ObjectId userId = ObjectId.GenerateNewId();
            ObjectId libraryId = ObjectId.GenerateNewId();

            var updateLibraryDto = new UpdateLibraryDtoFaker().Generate();

            var command = new UpdateLibraryCommand(userId.ToString(), libraryId.ToString(), updateLibraryDto);

            _libraryRepositoryMock.Setup(x => x.Get(userId, libraryId))
                .ReturnsAsync((Library?)null)
                .Verifiable();

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));

            _libraryRepositoryMock.Verify();
        }
    }
}
