namespace MavericksBank.Models
{
    public class TransferRequest
    {
        public int SenderAccountId { get; set; }
        public int ReceiverAccountId { get; set; }
        public decimal Amount { get; set; }
    }

}
