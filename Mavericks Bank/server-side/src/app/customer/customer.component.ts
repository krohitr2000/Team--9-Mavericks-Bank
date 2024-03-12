import { Component } from '@angular/core';
import { SeriveService } from './signup/signupserive.service';
import { ActivatedRoute, Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-customer',
  templateUrl: './customer.component.html',
  styleUrl: './customer.component.css'
})
export class CustomerComponent {
  customerId:number;
  constructor(public custsrv:SeriveService, private route: ActivatedRoute, private jwtHelper: JwtHelperService,private router:Router)
  {

  }
  ngOnInit():void
  {
    this.customerId = this.route.snapshot.params['customerId'];
    this.custsrv.customertList();
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
