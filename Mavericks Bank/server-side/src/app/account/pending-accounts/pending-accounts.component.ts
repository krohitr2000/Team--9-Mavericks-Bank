import { Component, OnInit } from '@angular/core';
import { AccountService } from '../account.service';
import { Account } from '../accountmodel';
import { ActivatedRoute, Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';


@Component({
  selector: 'app-pending-accounts',
  templateUrl: './pending-accounts.component.html',
  styleUrls: ['./pending-accounts.component.css']
})
export class PendingAccountsComponent implements OnInit {
  customerId:number;
  pendingList:Account[];
  constructor(public srv: AccountService, private route: ActivatedRoute, private jwtHelper:JwtHelperService, private router:Router) {
    
  }
  accounts: Account[] = [];
  ngOnInit():void
  {
    this.customerId = this.route.snapshot.params['customerId'];
    this.srv.accountList(this.customerId).subscribe(
      (res)=>
      this.pendingList = res.filter(account => account.status !== 'disapproved')
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
}
