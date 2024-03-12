import { Component } from '@angular/core';
import { SeriveService } from '../signup/signupserive.service';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { TokenvalidationService } from '../signin/tokenvalidation.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'] 
})
export class RegisterComponent {
  customerId: number = undefined;
  emailMessage: string = ''; 

  constructor(public objservice: SeriveService, private router: Router, private tokenService: TokenvalidationService) {}

  ngOnInit(): void {
    this.resetForm();
  }

  resetForm(form?: NgForm): void {
    if (form != null) {
      form.form.reset();
    } else {
      this.objservice.ppData = { customerId: 0, username: '', password: '', name: '', email: '', phone: 0, address: '' };
    }
  }

  onSubmit(form: NgForm): void {
    this.insertRecord(form);
    this.router.navigate([`/customer/${this.customerId}`]);
  }

  insertRecord(form: NgForm): void {
    this.objservice.regcustomer().subscribe(
      (response: any) => {
        console.log(response);
        const customerId = response.customerId;
        alert('Customer registration successful');
        console.log('Navigating to customer page');
        const token = response.token;
        this.tokenService.setToken(token);
        console.log(customerId);
        console.log(response);
        this.router.navigate(['/customer', customerId]);
      },
      error => {
        alert('Error' + error);
        console.error('Error occurred:', error);
      }
    );
  }

  updateEmailMessage(email: any): void {
    this.emailMessage = (email.invalid && email.touched) ? 'Invalid email format' : '';
  }
}
