using System;
using System.Collections.Generic;

namespace MavericksBank.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Accounts = new HashSet<Account>();
        }

        public int CustomerId { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? Token { get; set; }

        public virtual ICollection<Account> Accounts { get; set; }
    }
}
