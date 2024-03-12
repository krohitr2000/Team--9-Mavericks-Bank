using System.ComponentModel.DataAnnotations;

namespace MavericksBank.Models
{
    public class BranchDetails
    {
        [Key]
        public string IFSCCode {  get; set; }
        public string BranchName { get; set;}
    }
}
