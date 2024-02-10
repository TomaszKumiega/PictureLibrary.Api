using FluentAssertions;
using MongoDB.Bson;
using Moq;
using PictureLibrary.Application.Mapper;
using PictureLibrary.Application.Query;
using PictureLibrary.Application.Test.Fakers;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Repositories;
using PictureLibrary.TestTools.Fakers;

namespace PictureLibrary.Application.Test.Query
{
    public class GetAllLibrariesHandlerTest
    {
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<ILibraryRepository> _libraryRepository;

        private readonly GetAllLibrariesHandler _handler;

        public GetAllLibrariesHandlerTest()
        {
            _mapper = new Mock<IMapper>(MockBehavior.Strict);
            _libraryRepository = new Mock<ILibraryRepository>(MockBehavior.Strict);

            _handler = new GetAllLibrariesHandler(_mapper.Object, _libraryRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_Get_All_Libraries()
        {
            var userId = ObjectId.GenerateNewId();
            var query = new GetAllLibrariesQuery(userId.ToString());

            var libraries = new LibraryFaker().Generate(10);
            var libraryDto = new LibraryDtoFaker().Generate();

            _libraryRepository.Setup(x => x.GetAll(userId))
                .ReturnsAsync(libraries)
                .Verifiable();

            _mapper.Setup(x => x.MapToDto(It.IsIn<Library>(libraries)))
                .Returns(libraryDto)
                .Verifiable();

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Libraries.Should().NotBeNullOrEmpty();
            result.Libraries.Should().HaveCount(libraries.Count);
        }
    }
}
