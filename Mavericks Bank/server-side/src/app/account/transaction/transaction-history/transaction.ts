export class TransactionHistoryDetails {
    transactionId: number;
    senderAccountId: number | null;
    recieverAccountId: number | null;
    transactionType: string | null;
    amount: number;
    transactionDate: Date;
    recieverAccount: any | null;
    senderAccount: any | null;
  
    constructor(
      transactionId: number,
      senderAccountId: number | null,
      recieverAccountId: number | null,
      transactionType: string | null,
      amount: number,
      transactionDate: Date,
      recieverAccount: any | null,
      senderAccount: any | null
    ) {
      this.transactionId = transactionId;
      this.senderAccountId = senderAccountId;
      this.recieverAccountId = recieverAccountId;
      this.transactionType = transactionType;
      this.amount = amount;
      this.transactionDate = transactionDate;
      this.recieverAccount = recieverAccount;
      this.senderAccount = senderAccount;
    }
    refreshPage() {
        window.location.reload();
    }
    
  }
  