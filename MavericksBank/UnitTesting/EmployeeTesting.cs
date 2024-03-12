using MavericksBank.Controllers;
using MavericksBank.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace UnitTesting
{
    public class EmployeeTesting
    {
        private IConfigurationRoot config;

        [SetUp]
        public void Setup()
        {
            config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        }


        //Tests for Accounts
        [Test]
        public async Task GetEmpTest()
        {
            var options = new DbContextOptionsBuilder<MavericksBankDb2Context>().UseSqlServer(config.GetConnectionString("BankConStr")).Options;
            using var context = new MavericksBankDb2Context(options);
            context.Database.EnsureCreated();
            var mavCtr = new EmployeesController(context);
            var newEmp = new Employee() { Username = "rohitk" };

            await mavCtr.PostEmployee(newEmp);

            var actualEmp = await context.Employees.FirstOrDefaultAsync(e => e.Username == "rohitk");

            Assert.IsNotNull(actualEmp);
            Assert.AreEqual(newEmp.Username, actualEmp.Username);
        }

        [Test]
        public async Task AddEmpTest()
        {
            var options = new DbContextOptionsBuilder<MavericksBankDb2Context>().UseSqlServer(config.GetConnectionString("BankConStr")).Options;
            using var context = new MavericksBankDb2Context(options);
            context.Database.EnsureCreated();
            var mavCtr = new EmployeesController(context);
            var newEmp = new Employee() { Username="AjayRaj",Password= "AjayRaj", Email= "AjayRaj@12", Phone="9087665544" ,};
            await Task.Run(() => mavCtr.PostEmployee(newEmp));
            var actualEmp = context.Employees.FirstOrDefault(e => e.Username == "AjayRaj");
            Assert.IsNotNull(actualEmp);
            Assert.AreEqual(newEmp.Username, actualEmp.Username);
            Assert.AreEqual(newEmp.Password, actualEmp.Password);

        }

        [Test]
        public async Task DelEmpTest()
        {
            var options = new DbContextOptionsBuilder<MavericksBankDb2Context>().UseSqlServer(config.GetConnectionString("BankConStr")).Options;
            using var context = new MavericksBankDb2Context(options);
            context.Database.EnsureCreated();
            var mavCtr = new EmployeesController(context);
            var empToDelete = await context.Employees.SingleOrDefaultAsync(e => e.EmployeeId == 1);
            if (empToDelete != null)
            {
                await Task.Run(() => mavCtr.DeleteEmployee(empToDelete.EmployeeId));
                var deleteEmp = await context.Employees.FindAsync(empToDelete.EmployeeId);
                Assert.IsNull(deleteEmp);
            }
        }
    }
}
