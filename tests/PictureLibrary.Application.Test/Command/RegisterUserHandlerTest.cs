using FluentAssertions;
using Moq;
using PictureLibrary.Application.Command;
using PictureLibrary.Application.Mapper;
using PictureLibrary.Application.Test.Fakers;
using PictureLibrary.Domain.Entities;
using PictureLibrary.Domain.Exceptions;
using PictureLibrary.Domain.Repositories;
using PictureLibrary.Domain.Services;
using System.Text;

namespace PictureLibrary.Application.Test.Command
{
    public class RegisterUserHandlerTest
    {
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IHashAndSaltService> _hashAndSaltService;

        private readonly RegisterUserHandler _handler;

        public RegisterUserHandlerTest()
        {
            _mapper = new Mock<IMapper>(MockBehavior.Strict);
            _userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
            _hashAndSaltService = new Mock<IHashAndSaltService>(MockBehavior.Strict);

            _handler = new RegisterUserHandler(_mapper.Object, _userRepository.Object, _hashAndSaltService.Object);
        }

        [Fact]
        public async Task Handle_Should_Throw_AlreadyExistsException_When_Username_Exists()
        {
            var user = new NewUserDtoFaker().Generate();
            var command = new RegisterUserCommand(user);

            _userRepository.Setup(x => x.UsernameExists(user.Username))
                .Returns(true)
                .Verifiable();

            await Assert.ThrowsAsync<AlreadyExistsException>(() => _handler.Handle(command, CancellationToken.None));

            _userRepository.Verify();
        }

        [Fact]
        public async Task Handle_Should_Throw_AlreadyExistsException_When_Email_Exists()
        {
            var user = new NewUserDtoFaker().Generate();
            var command = new RegisterUserCommand(user);
            user.Email = "email@example.com";

            _userRepository.Setup(x => x.UsernameExists(user.Username))
                .Returns(false)
                .Verifiable();

            _userRepository.Setup(x => x.EmailExists(user.Email))
                .Returns(true)
                .Verifiable();

            await Assert.ThrowsAsync<AlreadyExistsException>(() => _handler.Handle(command, CancellationToken.None));

            _userRepository.Verify();
        }

        [Fact]
        public async Task Handle_Should_Create_And_Add_User()
        {
            User? user = null;
            var newUserDto = new NewUserDtoFaker().Generate();
            var userDto = new UserDtoFaker().Generate();
            var command = new RegisterUserCommand(newUserDto);
            var passwordHashAndSalt = new HashAndSalt
            {
                Text = newUserDto.Password,
                Hash = Encoding.UTF8.GetBytes("dapujhadgpubpqerpqe"),
                Salt = Encoding.UTF8.GetBytes("135ojbbqpdu0gpb3p"),
            };

            _hashAndSaltService.Setup(x => x.GetHashAndSalt(command.NewUser.Password))
                .Returns(passwordHashAndSalt)
                .Verifiable();

            _userRepository.Setup(x => x.UsernameExists(newUserDto.Username))
                .Returns(false)
                .Verifiable();

            _userRepository.Setup(x => x.EmailExists(newUserDto.Email))
                .Returns(false)
                .Verifiable();

            _userRepository.Setup(x => x.Add(It.IsAny<User>()))
                .Callback<User>(u =>
                {
                    user = u;
                    u.Should().NotBeNull();
                    u.Username.Should().Be(newUserDto.Username);
                    u.Email.Should().Be(newUserDto.Email);
                    u.PasswordHash.Should().BeEquivalentTo(passwordHashAndSalt.Hash);
                    u.PasswordSalt.Should().BeEquivalentTo(passwordHashAndSalt.Salt);
                })
                .Returns(Task.CompletedTask)
                .Verifiable();

            _mapper.Setup(x => x.MapToDto(It.IsAny<User>()))
                .Callback<User>(u => u.Should().BeEquivalentTo(user))
                .Returns(userDto)
                .Verifiable();

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.Should().Be(userDto);

            _hashAndSaltService.Verify();
            _userRepository.Verify();
            _mapper.Verify();
        }
    }
}
