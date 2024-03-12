import { Component } from '@angular/core';
import { FormBuilder,FormGroup,Validators } from '@angular/forms';
import { AdminService } from '../admin-signin/admin-signin.service';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrl: './admin.component.css'
})
export class AdminComponent {
  employeeForm:FormGroup;

  constructor(private fb:FormBuilder,private service:AdminService)
  {
    this.employeeForm = this.fb.group({
      username:['',Validators.required],
      name: ['', Validators.required],
      email: ['', Validators.required],
      password:['',Validators.required],
      phone:['',Validators.required],
    });
  }

  ngOnInIt():void
  {
    this.resetform();
  }


  onSubmit() {
    if (this.employeeForm.valid) {
      const employeeData = this.employeeForm.value;
      this.service.addEmployee(employeeData).subscribe(
        (response: any) => {
          console.log('Employee added successfully:', response);
          alert(
            'Employee registered successfully'
          )
          window.location.reload();
        },
        error => {
          alert("Invalid Details");
          window.location.reload();
        }
      );
    }
  }
  resetform()
  {
    this.employeeForm.reset;
  }
  
}
