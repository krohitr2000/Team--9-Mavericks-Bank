using System;
using System.Collections.Generic;

namespace MavericksBankApi.Models
{
    public partial class Account
    {
        public Account()
        {
            Transactions = new HashSet<Transaction>();
            Employees = new HashSet<Employee>();
        }

        public int AccountId { get; set; }
        public int? CustomerId { get; set; }
        public string? AccountType { get; set; }
        public decimal? Balance { get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
