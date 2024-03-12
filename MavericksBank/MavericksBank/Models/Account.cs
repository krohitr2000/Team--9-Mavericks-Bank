using System;
using System.Collections.Generic;

namespace MavericksBank.Models
{
    public partial class Account
    {
        public Account()
        {
            Loans = new HashSet<Loan>();
            TransactionRecieverAccounts = new HashSet<Transaction>();
            TransactionSenderAccounts = new HashSet<Transaction>();
            Employees = new HashSet<Employee>();
        }

        public int AccountId { get; set; }
        public int? CustomerId { get; set; }
        public string? AccountType { get; set; }
        public decimal? Balance { get; set; }
        public string? Status { get; set; }
        public string? IFSCCode {  get; set; }

        public virtual Customer? Customer { get; set; }
        public virtual ICollection<Loan> Loans { get; set; }
        public virtual ICollection<Transaction> TransactionRecieverAccounts { get; set; }
        public virtual ICollection<Transaction> TransactionSenderAccounts { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
