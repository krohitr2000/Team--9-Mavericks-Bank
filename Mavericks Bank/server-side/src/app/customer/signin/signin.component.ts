import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SigninServiceService } from './signin/signin-service.service';
import { Router } from '@angular/router';
import { TokenvalidationService } from './tokenvalidation.service';

@Component({
  selector: 'app-signin',
  templateUrl: './signin.component.html',
  styleUrls: ['./signin.component.css']
})
export class SigninComponent implements OnInit {
  customerId: number = undefined;

  signinForm: FormGroup;
  errorMessage: string;

  constructor(
    private formBuilder: FormBuilder,
    private signinService: SigninServiceService,
    private router: Router,
    private tokenService: TokenvalidationService
  ) {}

  ngOnInit(): void {
    this.signinForm = this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.signinForm.valid) {
      const username = this.signinForm.value.username;
      const password = this.signinForm.value.password;
  
      this.signinService.authenticate(username, password).subscribe(
        (response: any) => {
          console.log(response);
          const customerId = response.customerId;
          alert('Login successful');
          console.log('Navigating to customer page');
          const token=response.token;
          this.tokenService.setToken(token);
          console.log(customerId);
          console.log(response);
          this.router.navigate(['/customer', customerId]);
        },
        error => {
          this.errorMessage = 'Error occurred while signing in';
          console.error('Error occurred:', error);
        }
      );
    }
  }
}
