import { Component } from '@angular/core';
import { AccountService } from './account.service';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Route } from '@angular/router';
import { Loan } from './create-account/apply-loan/loan.model';
import { LoanService } from './create-account/apply-loan/loan.service';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent {
  accountId:number;
  loans:Loan[];
  accountBalance: number;
  showAccountBalance = false;
  selectedBranch: string;
  ifscCode: string; 
  constructor(public objservice: AccountService, private route:ActivatedRoute, private loanService: LoanService ) {}

  ngOnInit(): void {
    this.accountId = this.route.snapshot.params['accountId'];
    this.loanService.loanById(this.accountId).subscribe(
      (res)=>this.loans = res as Loan[]
    );
    this.objservice.getAccountById(this.accountId).subscribe(
      (res)=>this.accountBalance = res.balance
    )
    }

    deleteAccount(accountId: number): void {
      this.objservice.updateAccountStatus(accountId, 'tobedeleted').subscribe(
        (res) => {
          alert("Request for account deactivatin is raised")
          console.log('Account status updated successfully.');
        },
        error => {
          console.error('Error updating account status:', error);
        }
      );
    }

    viewAccountBalance() {
      this.showAccountBalance = true;
      return this.accountBalance;
    }

    onBranchChange(branchName: string): void {
      this.objservice.getIFSCCodeByBranch(branchName).subscribe(
        ifsc => {
          this.ifscCode = ifsc;
        },
        error => {
          console.error('Error fetching IFSC code:', error);
        }
      );
    }
}
