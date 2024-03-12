import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { CreatedAccountDetails } from '../account/create-account/createdAccountDetails';
import { TransactionHistoryDetails } from '../account/transaction/transaction-history/transaction';
import { LoanDetails } from '../account/create-account/apply-loan/loanDetails';
import { ActivatedRoute } from '@angular/router';
import { Route } from '@angular/router';

@Component({
  selector: 'app-employee',
  templateUrl: './employee.component.html',
  styleUrls: ['./employee.component.css']
})


export class EmployeeComponent {
  accounts:CreatedAccountDetails[] = [];
  createdAccounts:CreatedAccountDetails[] = [];
  deletedAccounts:CreatedAccountDetails[] = [];
  transactions:TransactionHistoryDetails[] =[];
  employeeId:number;
  loans:LoanDetails[]=[];

  constructor(private http:HttpClient, private route: ActivatedRoute) { 
    this.http.get<CreatedAccountDetails[]>("http://localhost:5126/api/Accounts").subscribe(
      res=>{
        this.accounts = res;
        this.setCreatedAccounts();
        this.setDeletedAccounts();
        console.log(this.accounts);
        console.log(this.createdAccounts);
        console.log(this.deletedAccounts);
      }
    );
    this.http.get<TransactionHistoryDetails[]>("http://localhost:5126/api/Transactions").subscribe(
      res=>this.transactions=res
    )
    this.http.get<LoanDetails[]>("http://localhost:5126/api/Loans").subscribe(
      res=>{
        this.loans=res;
        this.loans = this.loans.filter(loan=>loan.loanStatus === 'PENDING');
      }
    )
  }

  ngOnInit():void
  {
    this.employeeId = this.route.snapshot.params['employeeId'];
  }

  setCreatedAccounts() {
    this.createdAccounts = this.accounts.filter(a=>a.status=='PENDING');
  }
  setDeletedAccounts() {
    this.deletedAccounts = this.accounts.filter(a=>a.status=='tobedeleted');
  }

  approveNewCustomerAccount(accountId:number) {
    console.log(accountId);
    this.http.get<CreatedAccountDetails>("http://localhost:5126/api/Accounts/"+accountId).subscribe(
      res=>{
        res.status = "approved";
        this.http.put("http://localhost:5126/api/Accounts/"+accountId, res).subscribe(
          res=>this.refreshPage()
        );
      }
    );
    
  }

  disapproveNewCustomerAccount(accountId:number) {
    this.http.get<CreatedAccountDetails>("http://localhost:5126/api/Accounts/"+accountId).subscribe(
      res=>{
        res.status = "disapproved";
        this.http.put("http://localhost:5126/api/Accounts/"+accountId, res).subscribe(
          res=>this.refreshPage()
        );
      }
    );
    
  }

  disapproveCloseCustomerAccount(accountId:number) {
    this.http.get<CreatedAccountDetails>("http://localhost:5126/api/Accounts/"+accountId).subscribe(
      res=>{
        res.status = "approved";
        this.http.put("http://localhost:5126/api/Accounts/"+accountId, res).subscribe(
          res=>this.refreshPage()
        );
      }
    );
    
    
    
  }
  approveCloseCustomerAccount(accountId:number)
  {
    this.http.delete("http://localhost:5126/api/Accounts/"+accountId).subscribe(
      res=>this.refreshPage()
    );
  }


    approveLoan(loanId:number)
    {
      this.http.get<LoanDetails>("http://localhost:5126/api/Loans/"+loanId).subscribe(
        res=>{
          res.loanStatus = "Approved";
          this.http.put("http://localhost:5126/api/Loans/"+loanId, res).subscribe(
            res=>this.refreshPage()
          );
        }
      )
    }
    disapproveLoan(loanId:number)
    {
      this.http.get<LoanDetails>("http://localhost:5126/api/Loans/"+loanId).subscribe(
        res=>{
          res.loanStatus = "disapproved";
          this.http.put("http://localhost:5126/api/Loans/"+loanId, res).subscribe(
            res=>this.refreshPage()
          );
        }
      )
    }

  refreshPage() {
    window.location.reload();
}


}
