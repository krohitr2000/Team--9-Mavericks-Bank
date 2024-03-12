import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TokenvalidationService } from '../tokenvalidation.service';

@Injectable({
  providedIn: 'root'
})
export class SigninServiceService {
  readonly customerloginurl = 'http://localhost:5126/api/Customers/login';

  constructor(private http: HttpClient, private srv:TokenvalidationService) { }

  authenticate(username: string, password: string) {
    return this.http.post(this.customerloginurl, { username, password });
  }
}
