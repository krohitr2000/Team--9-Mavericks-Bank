import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { EmployeeService } from '../employee.service';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';


@Component({
  selector: 'app-empsignin',
  templateUrl: './empsignin.component.html',
  styleUrl: './empsignin.component.css'
})
export class EmpsigninComponent {
  employeeId: number = undefined;

  signinForm: FormGroup;
  errorMessage: string;

  constructor(
    private formBuilder: FormBuilder,
    private empsigninService: EmployeeService,
    private router: Router
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
  
      this.empsigninService.authenticate(username, password).subscribe(
        (response: any) => {
          console.log(response);
          const employeeId = response.employee.employeeId;
          alert('Login successful');
          console.log('Navigating to customer page');
          console.log(this.employeeId);
          console.log(response);
          this.router.navigate(['/employee', employeeId]);
        },
        error => {
          this.errorMessage = 'Error occurred while signing in';
          console.error('Error occurred:', error);
        }
      );
    }
  }

}
