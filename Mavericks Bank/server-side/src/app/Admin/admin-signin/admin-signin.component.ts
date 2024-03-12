import { Component } from '@angular/core';
import { FormBuilder,Validators,FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { AdminService } from './admin-signin.service';
import { TokenvalidationService } from '../../customer/signin/tokenvalidation.service';
import { MatSnackBar } from '@angular/material/snack-bar';


@Component({
  selector: 'app-admin-signin',
  templateUrl: './admin-signin.component.html',
  styleUrl: './admin-signin.component.css'
})
export class AdminSigninComponent {
  adminId:number= undefined;

  signinForm: FormGroup;
  errorMessage: string;
 
  constructor(
    private formbuilder:FormBuilder,
    private adminservice:AdminService,
    private router:Router,
    private tokenService:TokenvalidationService,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    this.signinForm = this.formbuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  onSubmit(): void {
    if (this.signinForm.valid) {
      const username = this.signinForm.value.username;
      const password = this.signinForm.value.password;
  
      this.adminservice.authenticate(username, password).subscribe(
        (response: any) => {
          console.log(response);
          const adminId = response.adminId;
          alert('Login successful');
          const token = response.token;
          this.tokenService.setToken(token);
          console.log('Navigating to customer page');
          console.log(this.adminId);
          console.log(response);
          this.router.navigate(['/admin', adminId]);
        },
        error => {
          alert("Login Failed")
          this.errorMessage = 'Please enter valid details';
          console.error('Error occurred:', error);
        }
      );
    }
  }

}
