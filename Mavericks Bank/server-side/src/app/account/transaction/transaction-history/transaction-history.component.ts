import { Component, OnInit } from '@angular/core';
import { TransactionComponent } from '../transaction.component';
import { HttpClient } from '@angular/common/http';
import { Transaction } from '../transaction';
import { ActivatedRoute, Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-transaction-history',
  templateUrl: './transaction-history.component.html',
  styleUrls: ['./transaction-history.component.css']
})
export class TransactionHistoryComponent implements OnInit {
  transactionHistory: any[] = [];
  accountId = undefined;
  response: Transaction[] = [];

  constructor(private http:HttpClient,private route:ActivatedRoute, private jwtHelper:JwtHelperService, private router:Router) 
  {
    
  }

  processForAllRecords() {
    this.response.forEach(t=>{
      this.transactionHistory.push(this.processResponseForUi(t));
    });
  }

  processResponseForUi(transactionHistory: Transaction) {
    let otherAccountId = null as number | null;
    let type = "";
    if(transactionHistory.recieverAccountId != null && transactionHistory.senderAccountId != null ) {
      if(transactionHistory.senderAccountId == this.accountId) {
        type = "Debit";
        otherAccountId = transactionHistory.recieverAccountId;
      } else {
        type = "Credit";
        otherAccountId = transactionHistory.senderAccountId;
      }
    } else if(transactionHistory.senderAccountId == null) {
      type = "Credit";
    } else {
      type = "Debit";
    }
    return {
      transactionId: transactionHistory.transactionId,
      transactionAmount: transactionHistory.amount,
      transactionType: type,
      accountId: otherAccountId
    };

  }

  ngOnInit(): void {
    this.accountId = this.route.snapshot.params['accountId'];
    this.http.get<Transaction[]>("http://localhost:5126/api/Transactions/ByAccountId/"+this.accountId).subscribe(
      res=>{
        this.response = res;
        console.log(res);
        this.processForAllRecords();
      }

    )
  }

  authenticate() {
    const token = localStorage.getItem("jwt");
  
    if (token && !this.jwtHelper.isTokenExpired(token)){
      return true;
    }
    this.router.navigate(['/signin']);
    return false;
  }

  refreshPage() {
    window.location.reload();
}

}
