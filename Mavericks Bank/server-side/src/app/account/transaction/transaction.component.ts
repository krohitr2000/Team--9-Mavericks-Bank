import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TransactionService } from './transaction.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-transaction',
  templateUrl: './transaction.component.html',
  styleUrls: ['./transaction.component.css']
})
export class TransactionComponent {
  accountId:number;

  showDeposit = false;
  showWithdraw = false;
  showTransfer = false;

  depositAmount: number;
  withdrawAmount: number;
  transferAccountId: number;
  transferAmount: number;
  requestBody = {
    "senderAccountId": -1,
    "receiverAccountId": -1,
    "amount": 500
  };

  constructor(private http: HttpClient, private srv: TransactionService,private route:ActivatedRoute) {
    this.depositAmount = null;
    this.withdrawAmount = null;
    this.transferAccountId = null;
    this.transferAmount = null;
  }
  ngOnInit(): void {
    this.accountId = this.route.snapshot.params['accountId'];
    
    }


  showDepositForm() {
    this.showDeposit = true;
    this.showWithdraw = false;
    this.showTransfer = false;
  }

  showWithdrawForm() {
    this.showDeposit = false;
    this.showWithdraw = true;
    this.showTransfer = false;
  }

  showTransferForm() {
    this.showDeposit = false;
    this.showWithdraw = false;
    this.showTransfer = true;
  }

  transfer() {
    this.srv.transfer(this.accountId,this.transferAmount,this.transferAccountId).subscribe(
      (res)=>{
  
        alert("Transaction Successful");
        window.location.reload();
      },
      (err)=> {
        console.log(err);
      }
    )
  }

  withdraw() {
    this.srv.withdraw(this.accountId,this.withdrawAmount).subscribe(
      (res)=>
      {
        alert("Transaction Successful");
        window.location.reload();
      },
      (err)=> {
        console.log(err);
      }
    )
    
  }

  deposit() {
    this.srv.deposit(this.accountId,this.depositAmount).subscribe(
    (res)=>
      {
        alert("Transaction Successful");
        window.location.reload();
      },
      (err)=> {
        console.log(err);
      }
    )
  }

}
