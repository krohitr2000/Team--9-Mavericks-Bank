using System;
using System.Collections.Generic;

namespace MavericksBank.Models
{
    public partial class Transaction
    {
        public int TransactionId { get; set; }
        public int? SenderAccountId { get; set; }
        public int? RecieverAccountId { get; set; }
        public string? TransactionType { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? TransactionDate { get; set; }

        public virtual Account? RecieverAccount { get; set; }
        public virtual Account? SenderAccount { get; set; }
    }
}
