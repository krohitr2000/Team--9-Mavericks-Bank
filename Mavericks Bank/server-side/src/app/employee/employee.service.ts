import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  readonly employeeloginurl = 'http://localhost:5126/api/Employees/login';

  constructor(private http: HttpClient) { }

  authenticate(username: string, password: string) {
    return this.http.post(this.employeeloginurl, { username, password });
  }

 
}
