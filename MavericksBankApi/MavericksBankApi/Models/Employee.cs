using System;
using System.Collections.Generic;

namespace MavericksBankApi.Models
{
    public partial class Employee
    {
        public Employee()
        {
            Accounts = new HashSet<Account>();
            Loans = new HashSet<Loan>();
        }

        public int EmployeeId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Loan> Loans { get; set; }
    }
}
