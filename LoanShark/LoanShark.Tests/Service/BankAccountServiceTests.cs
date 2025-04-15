using LoanShark.Domain;
using LoanShark.Repository;
using LoanShark.Service;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.System;
using Xunit;

namespace LoanShark.Tests
{
    public class BankAccountServiceTests
    {
        private readonly Mock<IBankAccountRepository> mockRepo;
        private readonly BankAccountService service;

        public BankAccountServiceTests()
        {
            mockRepo = new Mock<IBankAccountRepository>();
            service = new BankAccountService(mockRepo.Object);
        }
        [Fact]
        public async Task GetUserBankAccounts_ReturnsCorrectAccounts()
        {
            // Arrange
            var userId = 1;
            var expectedAccounts = new List<BankAccount>
            {
                new BankAccount("RO01SEUP0000000001", "RON", 0, false, userId, "Cont RON", 1000, 200, 10)
            };

            mockRepo.Setup(r => r.GetBankAccountsByUserId(userId))
                    .ReturnsAsync(expectedAccounts);

            // Act
            var result = await service.GetUserBankAccounts(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("RON", result[0].Currency);
            Assert.Equal("RO01SEUP0000000001", result[0].Iban);
            Assert.Equal("Cont RON", result[0].Name);
            Assert.Equal(1000, result[0].DailyLimit);
            Assert.Equal(200, result[0].MaximumPerTransaction);
            Assert.Equal(10, result[0].MaximumNrTransactions);
            Assert.Equal(0, result[0].Balance);
            Assert.False(result[0].Blocked);

        }

        [Fact]
        public async Task GetUserBankAccounts_UserIdNotFound_ReturnsNull()
        {
            // Arrange
            int testUserId = 999;

            mockRepo.Setup(repo => repo.GetBankAccountsByUserId(testUserId))
                    .ReturnsAsync((List<BankAccount>?)null);

            // Act
            var result = await service.GetUserBankAccounts(testUserId);

            // Assert
            Assert.Null(result);
        }


        [Fact]
        public async Task FindBankAccount_ReturnsCorrectBankAccount()
        {
            // Arrange
            var iban = "RO00SEUP1234567890123456";
            var expectedAccount = new BankAccount(
                iban: iban,
                currency: "EUR",
                balance: 1000m,
                blocked: false,
                userID: 1,
                name: "Teo",
                dailyLimit: 1500m,
                maximumPerTransaction: 300m,
                maximumNrTransactions: 5
            );

            mockRepo.Setup(r => r.GetBankAccountByIBAN(iban))
                    .ReturnsAsync(expectedAccount);

            // Act
            var result = await service.FindBankAccount(iban);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedAccount.Iban, result!.Iban);

            mockRepo.Verify(r => r.GetBankAccountByIBAN(iban), Times.Once);
        }

        [Fact]
        public async Task FindBankAccount_ReturnsNull_WhenIbanNotFound()
        {
            // Arrange
            var iban = "RO00SEUP0000000000000000";
            mockRepo.Setup(r => r.GetBankAccountByIBAN(iban))
                    .ReturnsAsync((BankAccount?)null);

            // Act
            var result = await service.FindBankAccount(iban);

            // Assert
            Assert.Null(result);
            mockRepo.Verify(r => r.GetBankAccountByIBAN(iban), Times.Once);
        }

        [Fact]
        public async Task GetCurrencies_ReturnsListOfCurrencies()
        {
            // Arrange
            var expectedCurrencies = new List<string> { "EUR", "USD", "RON" };

            mockRepo.Setup(r => r.GetCurrencies())
                    .ReturnsAsync(expectedCurrencies);

            // Act
            var result = await service.GetCurrencies();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCurrencies.Count, result.Count);
            Assert.Contains("EUR", result);
            Assert.Contains("USD", result);
            Assert.Contains("RON", result);

            mockRepo.Verify(r => r.GetCurrencies(), Times.Once);
        }

        [Fact]
        public async Task VerifyUserCredentials_ReturnsTrue_WhenPasswordIsCorrect()
        {
            // Arrange
            var email = "teo@example.com";
            var originalPassword = "Password123!";
            var passwordObj = new HashedPassword(originalPassword);

            var hashed = passwordObj.GetHashedPassword();
            var salt = passwordObj.GetSalt();

            mockRepo.Setup(r => r.GetCredentials(email))
                    .ReturnsAsync(new List<string> { hashed, salt });

            // Act
            var result = await service.VerifyUserCredentials(email, originalPassword);

            // Assert
            Assert.True(result);
            mockRepo.Verify(r => r.GetCredentials(email), Times.Once);
        }

        [Fact]
        public async Task VerifyUserCredentials_ReturnsFalse_WhenPasswordIsIncorrect()
        {
            // Arrange
            var email = "teo@example.com";
            var correctPassword = "Password123!";
            var wrongPassword = "WrongPassword1!";

            var passwordObj = new HashedPassword(correctPassword);
            var hashed = passwordObj.GetHashedPassword();
            var salt = passwordObj.GetSalt();

            mockRepo.Setup(r => r.GetCredentials(email))
                    .ReturnsAsync(new List<string> { hashed, salt });

            // Act
            var result = await service.VerifyUserCredentials(email, wrongPassword);

            // Assert
            Assert.False(result);
            mockRepo.Verify(r => r.GetCredentials(email), Times.Once);
        }

        [Fact]
        public async Task CheckIBANExists_ReturnsTrueIfExists()
        {
            // Arrange
            var iban = "RO02SEUP1234567890";
            var accounts = new List<BankAccount>
            {
                new BankAccount(iban, "RON", 0, false, 1, "Cont", 1000, 200, 10)
            };

            mockRepo.Setup(r => r.GetAllBankAccounts())
                    .ReturnsAsync(accounts);

            // Act
            var result = await service.CheckIBANExists(iban);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task CheckIBANExists_IBANNotFound_ReturnsFalse()
        {

            var accounts = new List<BankAccount>
            {
                new BankAccount("RO01SEUP0000000001", "RON", 0, false, 6, "Cont RON", 1000, 200, 10),
                 new BankAccount("RO01SEUP0000000002", "RON", 0, false, 9, "Cont EUR", 100, 20, 10)
            };

            mockRepo.Setup(repo => repo.GetAllBankAccounts())
                    .ReturnsAsync(accounts);

            string ibanToCheck = "RO00XXXX0000000000000000";

            // Act
            var result = await service.CheckIBANExists(ibanToCheck);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GenerateIBAN_GeneratesUniqueIbanWithCorrectFormat()
        {
            // Act
            string iban = await service.GenerateIBAN();

            // Assert
            Assert.NotNull(iban);
            Assert.StartsWith("RO", iban);
            Assert.Equal(24, iban.Length); // RO + 2 + SEUP (4) + 16 = 24
            Assert.Matches(@"^RO\d{2}SEUP[A-Z0-9]{16}$", iban);
        }

        [Fact]
        public async Task CreateBankAccount_CreatesCorrectBankAccountAndReturnsTrue()
        {
            // Arrange
            var userId = 123;
            var name = "Test Account";
            var currency = "EUR";

            BankAccount captured = null;

            mockRepo.Setup(r => r.AddBankAccount(It.IsAny<BankAccount>()))
                    .Callback<BankAccount>(acc => captured = acc)
                    .ReturnsAsync(true);

            // Act
            var result = await service.CreateBankAccount(userId, name, currency);

            // Assert
            Assert.True(result);
            Assert.NotNull(captured);
            Assert.Equal(userId, captured.UserID);
            Assert.Equal(name, captured.Name);
            Assert.Equal(currency, captured.Currency);
            Assert.Equal(0.0m, captured.Balance);
            Assert.Equal(1000.0m, captured.DailyLimit);
            Assert.Equal(200.0m, captured.MaximumPerTransaction);
            Assert.Equal(10, captured.MaximumNrTransactions);
            Assert.False(captured.Blocked);
        }

        [Fact]
        public async Task CreateBankAccount_FailsButCreatesCorrectBankAccountObject()
        {
            // Arrange
            var userId = 123;
            var name = "Failing Account";
            var currency = "USD";

            BankAccount captured = null;

            mockRepo.Setup(r => r.AddBankAccount(It.IsAny<BankAccount>()))
                    .Callback<BankAccount>(acc => captured = acc)
                    .ReturnsAsync(false);

            // Act
            var result = await service.CreateBankAccount(userId, name, currency);

            // Assert
            Assert.False(result);
            Assert.NotNull(captured);
            Assert.Equal(userId, captured.UserID);
            Assert.Equal(name, captured.Name);
        }



        [Fact]
        public async Task RemoveBankAccount_ReturnsTrueOnSuccess()
        {
            // Arrange
            var iban = "RO03SEUP0000000002";

            mockRepo.Setup(r => r.RemoveBankAccount(iban))
                    .ReturnsAsync(true);

            // Act
            var result = await service.RemoveBankAccount(iban);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task RemoveBankAccount_ReturnsFalseWhenRemovalFails()
        {
            // Arrange
            var iban = "RO03SEUP0000000002";

            mockRepo.Setup(r => r.RemoveBankAccount(iban))
                    .ReturnsAsync(false); // simulăm eșecul

            // Act
            var result = await service.RemoveBankAccount(iban);

            // Assert
            Assert.False(result);
        }


        [Fact]
        public async Task UpdateBankAccount_SendsCorrectUpdatedData()
        {
            // Arrange
            var iban = "RO04SEUP0000000003";
            BankAccount capturedAccount = null;

            mockRepo.Setup(r => r.UpdateBankAccount(iban, It.IsAny<BankAccount>()))
                    .Callback<string, BankAccount>((_, acc) => capturedAccount = acc)
                    .ReturnsAsync(true);

            // Act
            var result = await service.UpdateBankAccount(iban, "New Name", 1500, 300, 5, true);

            // Assert
            Assert.True(result);
            Assert.NotNull(capturedAccount);
            Assert.Equal("New Name", capturedAccount.Name);
            Assert.Equal(1500, capturedAccount.DailyLimit);
            Assert.Equal(300, capturedAccount.MaximumPerTransaction);
            Assert.Equal(5, capturedAccount.MaximumNrTransactions);
            Assert.True(capturedAccount.Blocked);
        }

        [Fact]
        public async Task UpdateBankAccount_FailsButSendsCorrectData()
        {
            // Arrange
            var iban = "RO04SEUP0000000003";
            BankAccount capturedAccount = null;

            mockRepo.Setup(r => r.UpdateBankAccount(iban, It.IsAny<BankAccount>()))
                    .Callback<string, BankAccount>((_, acc) => capturedAccount = acc)
                    .ReturnsAsync(false);

            // Act
            var result = await service.UpdateBankAccount(iban, "Failing Update", 1000, 200, 3, false);

            // Assert
            Assert.False(result);
            Assert.NotNull(capturedAccount);
            Assert.Equal("Failing Update", capturedAccount.Name);
        }


    }
}