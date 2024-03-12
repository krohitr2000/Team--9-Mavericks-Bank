using MavericksBank.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MavericksBank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransferController : ControllerBase
    {
        private readonly MavericksBankDb2Context _context;

        public TransferController(MavericksBankDb2Context context)
        {
            _context = context;
        }
        // POST: api/Transactions/TransferFunds
        [HttpPost("TransferFunds")]
        public async Task<IActionResult> TransferFunds(TransferRequest transferRequest)
        {
            // Validate the transfer request
            if (transferRequest == null || transferRequest.Amount <= 0 || transferRequest.SenderAccountId == transferRequest.ReceiverAccountId)
            {
                return BadRequest("Invalid transfer request");
            }

            // Retrieve sender and receiver accounts
            var senderAccount = transferRequest.SenderAccountId == -1? null: await _context.Accounts.FindAsync(transferRequest.SenderAccountId);
            var receiverAccount = transferRequest.ReceiverAccountId == -1 ? null : await _context.Accounts.FindAsync(transferRequest.ReceiverAccountId);

            // Check if accounts exist
            if (senderAccount == null && receiverAccount == null)
            {
                return NotFound("Both accounts not found");
            }

            // Check if sender has sufficient balance
            if (senderAccount != null && senderAccount.Balance < transferRequest.Amount)
            {
                return BadRequest("Insufficient funds");
            }
            string transactionType = "Transfer";
            if(senderAccount==null)
            {
                transactionType = "Deposit";
            }
            else if(receiverAccount == null)
            {
                transactionType = "Withdraw";
            }

            // Create a new transaction
            var transaction = new Transaction
            {
                SenderAccountId = transferRequest.SenderAccountId == -1 ? null : transferRequest.SenderAccountId,
                RecieverAccountId = transferRequest.ReceiverAccountId == -1 ? null : transferRequest.ReceiverAccountId,
                Amount = transferRequest.Amount,
                TransactionType = transactionType,
                TransactionDate = DateTime.Now
            };

            // Update account balances
            if (senderAccount != null)
            {
                senderAccount.Balance -= transferRequest.Amount;
            }
            if(receiverAccount != null)
            {
                receiverAccount.Balance += transferRequest.Amount;
            }

            // Update database with transaction and account balance changes
            using (var transaction1 = _context.Database.BeginTransaction())
            {
                try
                {
                    _context.Transactions.Add(transaction);
                    await _context.SaveChangesAsync();

                    await transaction1.CommitAsync();
                }
                catch (Exception)
                {
                    // Rollback transaction if an error occurs
                    await transaction1.RollbackAsync();
                    return StatusCode(500, "An error occurred while processing the transaction");
                }
            }

            return Ok(new { message = "Transaction successful" }); ;
        }

    }
}
