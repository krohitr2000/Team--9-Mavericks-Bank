import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateAccountComponent } from '../../account/create-account/create-account.component';
import { TransactionHistoryComponent } from '../../account/transaction/transaction-history/transaction-history.component';
import { ApplyLoanComponent } from '../../account/create-account/apply-loan/apply-loan.component';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private empUrl = "http://localhost:5126/api/";

  constructor(private http: HttpClient,private jwtHelper:JwtHelperService, private router:Router) { }

  getAccounts(): Observable<CreateAccountComponent[]> {
    return this.http.get<CreateAccountComponent[]>(`${this.empUrl}Accounts`);
  }

  getTransactions(): Observable<TransactionHistoryComponent[]> {
    return this.http.get<TransactionHistoryComponent[]>(`${this.empUrl}Transactions`);
  }

  getLoans(): Observable<ApplyLoanComponent[]> {
    return this.http.get<ApplyLoanComponent[]>(`${this.empUrl}Loans`);
  }

  approveNewCustomerAccount(accountId: number): Observable<any> {
    return this.http.put(`${this.empUrl}Accounts/${accountId}`, { status: "approved" });
  }

  disapproveNewCustomerAccount(accountId: number): Observable<any> {
    return this.http.put(`${this.empUrl}Accounts/${accountId}`, { status: "disapproved" });
  }

  disapproveCloseCustomerAccount(accountId: number): Observable<any> {
    return this.http.put(`${this.empUrl}Accounts/${accountId}`, { status: "approved" });
  }

  approveCloseCustomerAccount(accountId: number): Observable<any> {
    return this.http.delete(`${this.empUrl}Accounts/${accountId}`);
  }

  approveLoan(loanId: number, loanType: string): Observable<any> {
    return this.http.put(`${this.empUrl}Loans/${loanId}`, { loanStatus: "Approved" });
  }
  authenticate() {
    const token = localStorage.getItem("jwt");

    if (token && !this.jwtHelper.isTokenExpired(token)){
      return true;
    }
    this.router.navigate(['/empsignin']);
    return false;
  }
 
}
