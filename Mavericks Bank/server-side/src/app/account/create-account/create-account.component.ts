import { Component } from '@angular/core';
import { AccountService } from '../account.service';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-create-account',
  templateUrl: './create-account.component.html',
  styleUrl: './create-account.component.css'
})
export class CreateAccountComponent {
  customerId:number;
  selectedBranch: string;
  branchDetails:[];
  constructor(public objservice: AccountService, private route: ActivatedRoute, private jwtHelper:JwtHelperService, private router:Router) {}
  ngOnInit(): void {
    this.customerId = this.route.snapshot.params['customerId'];
    this.resetForm();
    this.objservice.fetchBranchDetails();
  }

  resetForm(form?: NgForm): void {
    if (form != null) {
      form.form.reset();
    } else {
      this.objservice.accountData = { accountId:0,customerId: this.customerId, accountType: '', balance: 0, status: '', ifscCode: ''};
    }
    
  }

  onSubmit(form:NgForm): void {
    
    this.insertRecord(form);
}


insertRecord(form:NgForm): void {
  this.objservice.createAccount("PENDING").subscribe(
    res => {
      this.resetForm(form);
      this.objservice.accountList(this.customerId);
      alert('Account registration successful');
    },
    err => {
      alert('Error' + err);
    }
  );
}
onBranchChange(branchName: string): void {
  this.objservice.accountData.ifscCode = this.objservice.getIFSCCodeByBranch(branchName);

}
authenticate() {
  const token = localStorage.getItem("jwt");

  if (token && !this.jwtHelper.isTokenExpired(token)){
    return true;
  }
  this.router.navigate(['/signin']);
  return false;
}

}
