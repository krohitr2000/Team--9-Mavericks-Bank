import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-loan-plans',
  templateUrl: './loan-plans.component.html',
  styleUrl: './loan-plans.component.css'
})
export class LoanPlansComponent {
  accountId:number;
  constructor(private router: Router, private route: ActivatedRoute){}

  ngOnInit():void
  {
    this.accountId = this.route.snapshot.params['accountId'];
  }
  applyForLoan(loanType: string) {
    console.log(`Applying for ${loanType} loan`);
    this.router.navigate(['/apply-loan', this.accountId, loanType]);
  }

}
