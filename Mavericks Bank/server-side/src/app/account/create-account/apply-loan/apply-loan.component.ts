import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { LoanService } from './loan.service';
import { Loan } from './loan.model';
import { ActivatedRoute, Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-apply-loan',
  templateUrl: './apply-loan.component.html',
  styleUrls: ['./apply-loan.component.css']
})
export class ApplyLoanComponent implements OnInit {
  fixedInterestRate: string;
  accountId: number;
  loanType: string;

  constructor(public objservice: LoanService, private route: ActivatedRoute, private jwtHelper:JwtHelperService, private router:Router) {}

  ngOnInit(): void {
    this.accountId = this.route.snapshot.params['accountId'];
    this.loanType = this.route.snapshot.params['loanType'];
    this.objservice.loanData.loanType = 'home';
    this.fixedInterestRate = this.getInterestRate(this.objservice.loanData.loanType);
    this.objservice.loanData.loanStatus = 'PENDING';
    console.log("Loan Status:", this.objservice.loanData.loanStatus);
    this.resetForm();
  }

  resetForm(form?: NgForm): void {
    if (form != null) {
      form.form.reset();
    } else {
      this.objservice.loanData = new Loan();
    }
    this.objservice.loanData.interestRate = this.fixedInterestRate;
    this.objservice.loanData.loanType = this.loanType;
    this.objservice.loanData.accountId = this.accountId;
    this.objservice.loanData.loanStatus = 'PENDING';
    this.objservice.loanData.approvalDate = null;
    this.objservice.loanData.disbursementDate = null;

  }

  onSubmit(form: NgForm): void {
    this.insertRecord(form);
  }

  updateRecord(form: NgForm): void {
    if (!this.objservice.loanData || !this.objservice.loanData.loanAmount || !this.objservice.loanData.tenure) {
      alert('Loan data is null. Please fill in the required information.');
      return;
    }
    this.objservice.updateLoan().subscribe(
      res => {
        this.resetForm(form);
        this.objservice.loanList();
        alert('Loan request raised successfully');
      },
      err => {
        alert('Error while applying for loan: ' + err);
      }
    );
  }

  insertRecord(form: NgForm): void {
    if (!this.objservice.loanData || !this.objservice.loanData.loanAmount || !this.objservice.loanData.tenure) {
      alert('Loan data is null. Please fill in the required information.');
      return;
    }
    this.objservice.regLoan(this.objservice.loanData).subscribe(
      res => {
        this.resetForm(form);
        this.objservice.loanList();
        alert('Loan record inserted successfully');
      },
      err => {
        alert('Error inserting loan record: ' + err);
      }
    );
  }

  authenticate() {
    const token = localStorage.getItem("jwt");

    if (token && !this.jwtHelper.isTokenExpired(token)){
      return true;
    }
    this.router.navigate(['/signin']);
    return false;
  }
  
  getInterestRate(loanType: string): string {
    switch (loanType) {
      case 'home':
        return '6';
      case 'education':
        return '7';
      case 'car':
        return '10';
      case 'agricultural':
        return '5';
      case 'personal':
        return '9';
      default:
        return '';
    }
  }
}
