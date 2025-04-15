using LoanShark.Domain;
using LoanShark.Repository;
using LoanShark.Service;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LoanShark.Tests.Service
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> mockUserRepository;
        private readonly UserService userService;

        public UserServiceTests()
        {
            mockUserRepository = new Mock<IUserRepository>();

            // Injecting mock through constructor not supported here since your UserService creates its own
            // So to test properly, you'd want to refactor UserService to allow injection (for now, we can use InternalsVisibleTo or partial mocks if needed)
            userService = new UserServiceTestable(mockUserRepository.Object); // custom subclass for testing
        }

        [Fact]
        public async Task CheckCnp_WhenCnpIsTooShort_ReturnsErrorMessage()
        {
            var result = await userService.CheckCnp("12345678901");

            Assert.Equal("CNP must have 13 characters", result);
        }

        [Fact]
        public async Task CheckCnp_WhenCnpIsNonDigit_ReturnsErrorMessage()
        {
            var result = await userService.CheckCnp("1234ABCD56789");

            Assert.Equal("CNP must contain only digits", result);
        }
        [Fact]
        public async Task CheckCnp_WhenCnpExistsAndDifferentUser_ReturnsErrorMessage()
        {
            var cnp = "1234567890123";
            var existingUser = new User(
                2,
                new Cnp(cnp),
                "Test",
                "User",
                new Email("test@example.com"),
                new PhoneNumber("0712345678"),
                new HashedPassword("Secret123@"));

            UserSession.Instance.SetUserData("id_user", "1");
            mockUserRepository.Setup(repo => repo.GetUserByCnp(cnp)).ReturnsAsync(existingUser);

            var userService = new UserService(mockUserRepository.Object); // inject the mock

            var result = await userService.CheckCnp(cnp);

            Assert.Equal("CNP already in use", result);
        }

        [Fact]
        public async Task CheckEmail_WhenEmailIsInvalid_ReturnsErrorMessage()
        {
            var result = await userService.CheckEmail("invalidEmail");
            Assert.Equal("Invalid email address", result);
        }

        [Fact]

        public async Task CheckEmail_WhenEmailAlreadyInUse_ReturnsErrorMessage()
        {
            // Arrange
            var email = "test@example.com";
            var existingUser = new User(
                2,
                new Cnp("1234567890123"),
                "Jane",
                "Doe",
                new Email(email),
                new PhoneNumber("0712345678"),
                new HashedPassword("Secret123@"));

            UserSession.Instance.SetUserData("id_user", "1"); // Current user is ID 1

            mockUserRepository.Setup(repo => repo.GetUserByEmail(email))
                              .ReturnsAsync(existingUser);

            var userService = new UserService(mockUserRepository.Object);

            // Act
            var result = await userService.CheckEmail(email);

            // Assert
            Assert.Equal("Email already in use", result);
        }


        [Fact]
        public async Task CheckPhoneNumber_WhenInvalidPrefix_ReturnsErrorMessage()
        {
            var result = await userService.CheckPhoneNumber("0612345678");
            Assert.Equal("Phone number must start with 07", result);
        }
        [Fact]
        public async Task CheckPhoneNumber_WhenInvalidLength_ReturnsErrorMessage()
        {
            var result = await userService.CheckPhoneNumber("071234567");
            Assert.Equal("Phone number must have 10 characters", result);
        }
        [Fact]
        public async Task CheckPhoneNumber_WhenInvalidCharacters_ReturnsErrorMessage()
        {
            var result = await userService.CheckPhoneNumber("07123ABCD8");
            Assert.Equal("Phone number must contain only digits", result);
        }
        [Fact]
        public async Task CheckPhoneNumber_WhenAlreadyInUse_ReturnsErrorMessage()
        {
            // Arrange
            var phoneNumber = "0712345678";
            var existingUser = new User(
                2,
                new Cnp("1234567890123"),
                "Test",
                "User",
                new Email("test@example.com"),
                new PhoneNumber(phoneNumber),
                new HashedPassword("Secret123@"));

            UserSession.Instance.SetUserData("id_user", "1");

            mockUserRepository.Setup(repo => repo.GetUserByPhoneNumber(phoneNumber))
                              .ReturnsAsync(existingUser);

            var userService = new UserService(mockUserRepository.Object);

            // Act
            var result = await userService.CheckPhoneNumber(phoneNumber);

            // Assert
            Assert.Equal("Phone number already in use", result);
        }

        private User CreateTestUser(int id = 1, string password = "Secret123@")
        {
            return new User(
                id,
                new Cnp("1234567890123"),
                "John",
                "Doe",
                new Email("john@example.com"),
                new PhoneNumber("0712345678"),
                new HashedPassword(password) // this hashes the password and stores salt
            );
        }



        [Fact]
        public async Task DeleteUser_WhenUserDoesNotExist_ReturnsErrorMessage()
        {
            // Arrange
            mockUserRepository.Setup(repo => repo.GetUserById(It.IsAny<int>()))
                              .ReturnsAsync((User?)null);

            UserSession.Instance.SetUserData("id_user", "1");

            var userService = new UserService(mockUserRepository.Object);

            // Act
            var result = await userService.DeleteUser("somepassword");

            // Assert
            Assert.Equal("User doesn't exist", result);
        }

        [Fact]
        public async Task DeleteUser_WhenPasswordIsWrong_ReturnsWrongPasswordMessage()
        {
            // Arrange
            var actualUser = CreateTestUser(password: "correctPassword@123");

            mockUserRepository.Setup(repo => repo.GetUserById(It.IsAny<int>()))
                              .ReturnsAsync(actualUser);

            UserSession.Instance.SetUserData("id_user", "1");

            var userService = new UserService(mockUserRepository.Object);

            // Act
            var result = await userService.DeleteUser("wrongPassword");

            // Assert
            Assert.Equal("Wrong password", result);
        }

        [Fact]
        public async Task DeleteUser_WhenPasswordIsCorrect_DeletesUserAndReturnsSuccess()
        {
            // Arrange
            var actualUser = CreateTestUser(password: "CorrectPass123!");

            mockUserRepository.Setup(repo => repo.GetUserById(It.IsAny<int>()))
                              .ReturnsAsync(actualUser);

            mockUserRepository.Setup(repo => repo.DeleteUser())
                              .ReturnsAsync(true);

            UserSession.Instance.SetUserData("id_user", "1");

            var userService = new UserService(mockUserRepository.Object);

            // Act
            var result = await userService.DeleteUser("CorrectPass123!");

            // Assert
            Assert.Equal("Succes", result);
            mockUserRepository.Verify(repo => repo.DeleteUser(), Times.Once);
        }
    }

    public class UserServiceTestable : UserService
    {
        public UserServiceTestable(IUserRepository mockRepo)
        {
            var repoField = typeof(UserService).GetField("userRepository", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            repoField?.SetValue(this, mockRepo);
        }
    }
}
