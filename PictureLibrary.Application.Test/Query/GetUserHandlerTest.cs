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
    public class GetUserHandlerTest
    {
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IUserRepository> _userRepository;

        private readonly GetUserHandler _handler;

        public GetUserHandlerTest()
        {
            _mapper = new Mock<IMapper>(MockBehavior.Strict);
            _userRepository = new Mock<IUserRepository>(MockBehavior.Strict);

            _handler = new GetUserHandler(_mapper.Object, _userRepository.Object);
        }

        [Fact]
        public async Task Handle_Should_Return_UserDto()
        {
            var userId = ObjectId.GenerateNewId().ToString();
            var user = new UserFaker().Generate();
            var userDto = new UserDtoFaker().Generate();

            var query = new GetUserQuery(userId);

            _userRepository.Setup(x => x.FindById(ObjectId.Parse(userId)))
                .Returns(user)
                .Verifiable();

            _mapper.Setup(x => x.MapToDto(user))
                .Returns(userDto)
                .Verifiable();

            var result = await _handler.Handle(query, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().Be(userDto);

            _mapper.Verify();
            _userRepository.Verify();
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFoundException_When_User_Is_Null()
        {
            var userId = ObjectId.GenerateNewId().ToString();

            _userRepository.Setup(x => x.FindById(ObjectId.Parse(userId)))
                .Returns((User?)null)
                .Verifiable();

            await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(new GetUserQuery(userId), CancellationToken.None));
        
            _userRepository.Verify();
        }
    }
}
