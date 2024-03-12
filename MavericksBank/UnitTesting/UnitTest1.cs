using MavericksBank.Controllers;
using MavericksBank.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace UnitTesting
{
    public class Tests
    {
        private IConfigurationRoot config;

        [SetUp]
        public void Setup()
        {
            config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }

        
        [Test]
        public async Task GetCustomerTest()
        {
            var options = new DbContextOptionsBuilder<MavericksBankDb2Context>().UseSqlServer(config.GetConnectionString("BankConStr")).Options;
            using var context = new MavericksBankDb2Context(options);
            context.Database.EnsureCreated();
            var mavCtr = new CustomersController(context);
            var newCust = new Customer() { Username = "Sai" };

            await mavCtr.PostCustomer(newCust);

            var actualCust = await context.Customers.FirstOrDefaultAsync(c => c.Username == "Sai");

            Assert.IsNotNull(actualCust);
            Assert.AreEqual(newCust.Username, actualCust.Username);
        }

        [Test]
        public async Task AddCustTest()
        {
            var options = new DbContextOptionsBuilder<MavericksBankDb2Context>().UseSqlServer(config.GetConnectionString("BankConStr")).Options;
            using var context = new MavericksBankDb2Context(options);
            context.Database.EnsureCreated();
            var mavCtr = new CustomersController(context);
            var newCust = new Customer() { Username = "Rajesh khanna", Password = "rajes", Name = "raj", Email = "raj@gmail.com", Phone = "9087654432", Address = "Palus" };
            await Task.Run(() => mavCtr.PostCustomer(newCust));
            var actualList = context.Customers.FirstOrDefault(c => c.Username == "Rajesh khanna");
            Assert.IsNotNull(actualList);
            Assert.AreEqual(newCust.Username, actualList.Username);
        }

        [Test]
        public async Task DelCustTest()
        {
            var options = new DbContextOptionsBuilder<MavericksBankDb2Context>().UseSqlServer(config.GetConnectionString("BankConStr")).Options;
            using var context = new MavericksBankDb2Context(options);
            context.Database.EnsureCreated();
            var mavCtr = new CustomersController(context);
            var custToDelete = context.Customers.SingleOrDefaultAsync(c => c.CustomerId == 5);
            await Task.Run(() => mavCtr.DeleteCustomer(custToDelete.Id));
            var deleteCust = context.Customers.Find(custToDelete.Id);
            Assert.IsNull(deleteCust);
        }
        

    }
}