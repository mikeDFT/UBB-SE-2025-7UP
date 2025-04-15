using LoanShark.Data;
using LoanShark.Domain;
using LoanShark.Repository;
using LoanShark.Service;
using Moq;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Xunit;

namespace LoanShark.Tests
{
    public class BankAccountRepoTests
    {
        private readonly Mock<IBankAccountRepository> _mockRepo;

        public BankAccountRepoTests()
        {
            _mockRepo = new Mock<IBankAccountRepository>();
        }

        [Fact]
        public void ConvertDataTableRowToBankAccount_ReturnsCorrectBankAccount()
        {
            // Arrange
            var dataTable = new DataTable();
            dataTable.Columns.Add("iban", typeof(string));
            dataTable.Columns.Add("currency", typeof(string));
            dataTable.Columns.Add("amount", typeof(decimal));
            dataTable.Columns.Add("blocked", typeof(bool));
            dataTable.Columns.Add("id_user", typeof(int));
            dataTable.Columns.Add("custom_name", typeof(string));
            dataTable.Columns.Add("daily_limit", typeof(decimal));
            dataTable.Columns.Add("max_per_transaction", typeof(decimal));
            dataTable.Columns.Add("max_nr_transactions_daily", typeof(int));

            var row = dataTable.NewRow();
            row["iban"] = "RO99TEST0000000001";
            row["currency"] = "EUR";
            row["amount"] = 1200.50m;
            row["blocked"] = false;
            row["id_user"] = 7;
            row["custom_name"] = "Teo";
            row["daily_limit"] = 500.00m;
            row["max_per_transaction"] = 200.00m;
            row["max_nr_transactions_daily"] = 3;

            dataTable.Rows.Add(row);

            var repo = new BankAccountRepository();

            // Act
            var result = repo.ConvertDataTableRowToBankAccount(dataTable.Rows[0]);

            // Assert
            Assert.Equal("RO99TEST0000000001", result.Iban);
            Assert.Equal("EUR", result.Currency);
            Assert.Equal(1200.50m, result.Balance);
            Assert.False(result.Blocked);
            Assert.Equal(7, result.UserID);
            Assert.Equal("Teo", result.Name);
            Assert.Equal(500.00m, result.DailyLimit);
            Assert.Equal(200.00m, result.MaximumPerTransaction);
            Assert.Equal(3, result.MaximumNrTransactions);
        }

        [Fact]
        public async Task ConvertDataTableToCurrencyList_ReturnsListOfCurrencies()
        {
            // Arrange
            var dataTable = new DataTable();
            dataTable.Columns.Add("currency_name", typeof(string));
            dataTable.Rows.Add("USD");
            dataTable.Rows.Add("EUR");

            var repo = new BankAccountRepository();

            // Act
            var result = await repo.ConvertDataTableToCurrencyList(dataTable);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains("USD", result);
            Assert.Contains("EUR", result);
        }

        [Fact]
        public void Add_ShouldCallRepositoryAdd()
        {
            var account = new BankAccount("RO49AAAA", "EUR", 1000m, false, 1, "Teo’s account", 1000, 500, 5);

            _mockRepo.Object.AddBankAccount(account);

            _mockRepo.Verify(r => r.AddBankAccount(It.Is<BankAccount>(a => a.Iban == "RO49AAAA")), Times.Once);
        }

        [Fact]
        public void Delete_ShouldCallRepositoryDelete()
        {
            string ibanToDelete = "RO22TEST";
            _mockRepo.Object.RemoveBankAccount(ibanToDelete);

            _mockRepo.Verify(r => r.RemoveBankAccount("RO22TEST"), Times.Once);
        }
    }

}