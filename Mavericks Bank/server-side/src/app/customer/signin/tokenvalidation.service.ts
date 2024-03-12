import { Injectable, ɵprovideZonelessChangeDetection } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { HttpHeaders } from '@angular/common/http';


@Injectable({
  providedIn: 'root'
})
export class TokenvalidationService {
  constructor(private router:Router){}

  getToken() {
    return localStorage.getItem("jwt");
  }
  setToken(token) {
    console.log(token);
    localStorage.setItem("jwt", token);
  }
  removeToken(){
    localStorage.removeItem("jwt");
  }

  getHttpOptions() {
    const token = this.getToken();
    if (token) {
      return {
        headers: new HttpHeaders({
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        })
      };
    } else {
      return {
        headers: new HttpHeaders({
          'Content-Type': 'application/json'
        })
      };
    }
  }

}
