import { Component } from '@angular/core';
import { AccountService } from '../account.service';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { Account } from '../accountmodel';

@Component({
  selector: 'app-display-account',
  templateUrl: './display-account.component.html',
  styleUrl: './display-account.component.css'
})
export class DisplayAccountComponent {
  customerId:number;
  approvedList:Account[];
  constructor(public srv:AccountService, private route:ActivatedRoute,private router:Router)
  {

  }
  ngOnInit():void
  {
    this.customerId = this.route.snapshot.params['customerId'];
    this.srv.accountList(this.customerId).subscribe(
      (res)=>{
        this.approvedList = res.filter(account => account.status === 'approved');
      }
    );
  }

  fillform(account)
  {
    this.srv.accountData=Object.assign({},account);
  }
  redirectToAccountDetails(accountId: number): void {
    console.log(accountId);
    this.router.navigate(['/account', accountId]);
}

    
  }

