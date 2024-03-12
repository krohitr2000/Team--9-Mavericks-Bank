using MavericksBank.Controllers;
using MavericksBank.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace UnitTesting
{
    public class AccTests
    {
        private IConfigurationRoot config;

        [SetUp]
        public void Setup()
        {
            config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }


        //Tests for Accounts
        [Test]
        public async Task GetAccountTest()
        {
            var options = new DbContextOptionsBuilder<MavericksBankDb2Context>().UseSqlServer(config.GetConnectionString("BankConStr")).Options;
            using var context = new MavericksBankDb2Context(options);
            context.Database.EnsureCreated();
            var mavCtr = new AccountsController(context);
            var newAcc = new Account() { AccountType = "savings" };

            await mavCtr.PostAccount(newAcc);

            var actualCust = await context.Accounts.FirstOrDefaultAsync(a => a.AccountType == "savings");

            Assert.IsNotNull(actualCust);
            Assert.AreEqual(newAcc.AccountType, actualCust.AccountType);
        }

        [Test]
        public async Task AddAccTest()
        {
            var options = new DbContextOptionsBuilder<MavericksBankDb2Context>().UseSqlServer(config.GetConnectionString("BankConStr")).Options;
            using var context = new MavericksBankDb2Context(options);
            context.Database.EnsureCreated();
            var mavCtr = new AccountsController(context);
            var newAcc = new Account() { AccountType="savings",Balance=5000,Status="pending" };
            await Task.Run(() => mavCtr.PostAccount(newAcc));
            var actualList = context.Accounts.FirstOrDefault(a => a.AccountType == "savings");
            Assert.IsNotNull(actualList);
            Assert.AreEqual(newAcc.AccountType, actualList.AccountType);
        }

        [Test]
        public async Task DelAccTest()
        {
            var options = new DbContextOptionsBuilder<MavericksBankDb2Context>().UseSqlServer(config.GetConnectionString("BankConStr")).Options;
            using var context = new MavericksBankDb2Context(options);
            context.Database.EnsureCreated();
            var mavCtr = new AccountsController(context);
            var accToDelete = await context.Accounts.SingleOrDefaultAsync(a => a.AccountId == 5);
            if (accToDelete != null)
            {
                await Task.Run(() => mavCtr.DeleteAccount(accToDelete.AccountId));
                var deleteAcc = await context.Accounts.FindAsync(accToDelete.AccountId);
                Assert.IsNull(deleteAcc);
            }
        }
    }
   }
