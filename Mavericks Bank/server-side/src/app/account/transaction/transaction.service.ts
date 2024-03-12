import { Injectable } from '@angular/core';
import { Transaction } from './transaction';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute } from '@angular/router';


@Injectable({
  providedIn: 'root'
})
export class TransactionService {

  readonly transferApiurl='http://localhost:5126/api/Transfer/TransferFunds';
  list:Transaction[];
  transferData:Transaction=new Transaction();
  accountId:number;

  depositAmount: number;
  withdrawAmount: number;
  transferAccountId: number;
  transferAmount: number;
  requestBody = {
    "senderAccountId": -1,
    "receiverAccountId": -1,
    "amount": 500
  };

  
  constructor(private myhhtp:HttpClient,private route:ActivatedRoute) { }


  deposit(accountId,depositAmount) {
    console.log(`Depositing ${depositAmount} into account ${accountId}`);
    const requestBody = {
        "receiverAccountId": accountId,
        "senderAccountId":-1,
        "amount": depositAmount
    };
    console.log(requestBody);
    return this.myhhtp.post(this.transferApiurl, requestBody);
}

withdraw(accountId,withdrawAmount) {
    console.log(`Withdrawing ${withdrawAmount} from account ${accountId}`);
    const requestBody = {
        "senderAccountId": accountId,
        "receiverAccountId": -1,
        "amount": withdrawAmount
    };
    return this.myhhtp.post(this.transferApiurl, requestBody);
}

transfer(accountId,transferAmount,transferAccountId) {
    console.log(`Transferring ${transferAmount} to account ${transferAccountId} from account ${accountId}`);
    const requestBody = {
        "senderAccountId": accountId,
        "receiverAccountId": transferAccountId,
        "amount":transferAmount
    };
    return this.myhhtp.post(this.transferApiurl, requestBody);
}

}
