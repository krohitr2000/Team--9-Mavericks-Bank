using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MavericksBank.Models
{
    public partial class Loan
    {
        public Loan()
        {
            Employees = new HashSet<Employee>();
        }

        public int LoanId { get; set; }
        public string? LoanType { get; set; }
        public int? AccountId { get; set; }
        public decimal? LoanAmount { get; set; }
        public decimal? InterestRate { get; set; }
        public string? LoanStatus { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? DisbursementDate { get; set; }
        public int  Tenure {  get; set; }

        public virtual Account? Account { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
