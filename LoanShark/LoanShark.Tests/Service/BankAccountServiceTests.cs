using LoanShark.Domain;
using LoanShark.Repository;
using LoanShark.Service;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace LoanShark.Tests
{
    public class BankAccountServiceTests
    {
        [Fact]
        public async Task GetUserBankAccounts_ReturnsCorrectAccounts()
        {
            // Arrange
            var mockRepo = new Mock<IBankAccountRepository>();
            var userId = 1;
            var expectedAccounts = new List<BankAccount>
            {
                new BankAccount("RO01SEUP0000000001", "RON", 0, false, userId, "Cont RON", 1000, 200, 10)
            };

            mockRepo.Setup(r => r.GetBankAccountsByUserId(userId))
                    .ReturnsAsync(expectedAccounts);

            var service = new BankAccountService(mockRepo.Object);

            // Act
            var result = await service.GetUserBankAccounts(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("RON", result[0].Currency);
        }

        [Fact]
        public async Task CheckIBANExists_ReturnsTrueIfExists()
        {
            // Arrange
            var mockRepo = new Mock<IBankAccountRepository>();
            var iban = "RO02SEUP1234567890";
            var accounts = new List<BankAccount>
            {
                new BankAccount(iban, "RON", 0, false, 1, "Cont", 1000, 200, 10)
            };

            mockRepo.Setup(r => r.GetAllBankAccounts())
                    .ReturnsAsync(accounts);

            var service = new BankAccountService(mockRepo.Object);

            // Act
            var result = await service.CheckIBANExists(iban);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task RemoveBankAccount_ReturnsTrueOnSuccess()
        {
            // Arrange
            var mockRepo = new Mock<IBankAccountRepository>();
            var iban = "RO03SEUP0000000002";

            mockRepo.Setup(r => r.RemoveBankAccount(iban))
                    .ReturnsAsync(true);

            var service = new BankAccountService(mockRepo.Object);

            // Act
            var result = await service.RemoveBankAccount(iban);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateBankAccount_ReturnsTrueWhenSuccessful()
        {
            // Arrange
            var mockRepo = new Mock<IBankAccountRepository>();
            var iban = "RO04SEUP0000000003";

            mockRepo.Setup(r => r.UpdateBankAccount(iban, It.IsAny<BankAccount>()))
                    .ReturnsAsync(true);

            var service = new BankAccountService(mockRepo.Object);

            // Act
            var result = await service.UpdateBankAccount(iban, "New Name", 1500, 300, 5, true);

            // Assert
            Assert.True(result);
        }
    }
}
