import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { SeriveService } from '../customer/signup/signupserive.service';
import { CustomersModel } from '../customer/CustomerModel';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-confirm-password',
  templateUrl: './confirm-password.component.html',
  styleUrl: './confirm-password.component.css'
})
export class ConfirmPasswordComponent {
  newPassword: string = '';
  confirmPassword: string = '';
  customerId:number;

  constructor(private route: ActivatedRoute, private service: SeriveService, private router: Router, private jwtHelper:JwtHelperService) {

  }

  ngOnInit():void
  {
    this.customerId = this.route.snapshot.params['customerId'];
  }

  onSubmit(): void {
    if (this.newPassword !== this.confirmPassword) {
      alert("New password and confirm password do not match.");
      window.location.reload();
    } else {
      this.service.getCustomer(this.customerId).subscribe(
        (res)=> {
          let customer = res as CustomersModel;
          console.log(customer);
          customer.password = this.confirmPassword;
          this.service.updatecustomer(this.customerId, customer).subscribe();
          this.router.navigate(["/signin"])
        }
        );
      
    }
    
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
