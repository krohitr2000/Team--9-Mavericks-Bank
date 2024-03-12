using MavericksBank.Controllers;
using MavericksBank.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace UnitTesting
{
    public class LoansTesting
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
            var mavCtr = new LoansController(context);
            var newLoan = new Loan() { LoanType = "car" };

            await mavCtr.PostLoan(newLoan);

            var actualLoan = await context.Loans.FirstOrDefaultAsync(l => l.LoanType == "car");

            Assert.IsNotNull(actualLoan);
            Assert.AreEqual(newLoan.LoanType, actualLoan.LoanType);
        }

        [Test]
        public async Task AddAccTest()
        {
            var options = new DbContextOptionsBuilder<MavericksBankDb2Context>().UseSqlServer(config.GetConnectionString("BankConStr")).Options;
            using var context = new MavericksBankDb2Context(options);
            context.Database.EnsureCreated();
            var mavCtr = new LoansController(context);
            var newLoan = new Loan() { LoanType="car",LoanAmount=12000,InterestRate=12};
            await Task.Run(() => mavCtr.PostLoan(newLoan));
            var actualList = context.Loans.FirstOrDefault(a => a.LoanType == "car");
            Assert.IsNotNull(actualList);
            Assert.AreEqual(newLoan.LoanType, actualList.LoanType);
        }

        [Test]
        public async Task DelAccTest()
        {
            var options = new DbContextOptionsBuilder<MavericksBankDb2Context>().UseSqlServer(config.GetConnectionString("BankConStr")).Options;
            using var context = new MavericksBankDb2Context(options);
            context.Database.EnsureCreated();
            var mavCtr = new LoansController(context);
            var LoanToDelete = await context.Loans.SingleOrDefaultAsync(l => l.LoanId == 3);
            if (LoanToDelete != null)
            {
                await Task.Run(() => mavCtr.DeleteLoan(LoanToDelete.LoanId));
                var deleteAcc = await context.Loans.FindAsync(LoanToDelete.LoanId);
                Assert.IsNull(deleteAcc);
            }
        }
    }
}
