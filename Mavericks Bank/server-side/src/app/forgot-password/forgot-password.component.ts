import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SeriveService } from '../customer/signup/signupserive.service';
import { Router } from '@angular/router';
import { CustomersModel } from '../customer/CustomerModel';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent implements OnInit {
  model: any = {}; // Object to hold form data
  constructor(private http: HttpClient, private service: SeriveService, private route: Router) {}

  ngOnInit(): void {}

  onSubmit(): void {
    // Form submission logic
    this.service.verify(this.model).subscribe(
      (res: CustomersModel) => {
        let customerId = res.customerId;
        this.route.navigate(["/confirm-password", customerId]);
      },
      (error) => {
        alert("Invalid Details");
        window.location.reload();
        // Handle error
      }
    );
  }
}
