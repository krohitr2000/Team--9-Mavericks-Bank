import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Loan } from './loan.model';
import { NgForm } from '@angular/forms';

@Injectable({
  providedIn: 'root'
})
export class LoanService {
  readonly ppApiurl='http://localhost:5126/api/Loans';
  readonly loanByIdurl='http://localhost:5126/api/Loans/account'
  list:Loan[];
  loanData:Loan=new Loan();

  constructor(private myHttp:HttpClient) { }
  public loanList()
  {
    this.myHttp.get(this.ppApiurl).toPromise().then(res=>this.list=res as Loan[])
  }
  public regLoan(loanData)
  {
    loanData.accountId = Number(loanData.accountId);
    return this.myHttp.post(this.ppApiurl,loanData);
  }
  public updateLoan()
  {
    return this.myHttp.put(this.ppApiurl+'/'+this.loanData.loanId,this.loanData);
  }
  public loanById(accountId)
  {
    return this.myHttp.get(this.loanByIdurl+"/"+accountId);
  }
  

}
