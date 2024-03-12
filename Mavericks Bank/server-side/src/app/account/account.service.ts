import { Injectable } from '@angular/core';
import { Account } from './accountmodel';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  readonly accountbyidApiurl='http://localhost:5126/api/Accounts/ByCustomer/';
  readonly accountApiurl='http://localhost:5126/api/Accounts';
  readonly branchurl='http://localhost:5126/api/BranchDetails';
  approvedlist: Account[];
  pendinglist:Account[];
  accountData: Account = new Account();
  branchDetails: any[] = [];

  constructor(private myHttp: HttpClient) { 
    this.fetchBranchDetails();
  }

  public getAccountById(id: number) {
    return this.myHttp.get<Account>(`${this.accountApiurl}/${id}`);
  }

  public accountList(customerId) {
    return this.myHttp.get<Account[]>(this.accountbyidApiurl+customerId);
  }

  public createAccount(status): any {
    this.accountData.status = status;
    console.log(this.accountData);
    return this.myHttp.post(this.accountApiurl, this.accountData);
  }

  public delAccount(id: number): any {
    return this.myHttp.delete(`${this.accountApiurl}/${id}`)
  }

  public updateAccountStatus(id: number, status: string): any {
    const updateUrl = `${this.accountApiurl}/${id}`;
    const updatedAccount = { ...this.accountData, status };
    var body= {
      "accountId": id,
      "status": status
    };
    this.myHttp.get<Account>("http://localhost:5126/api/Accounts/"+id).subscribe(
      res=>{
        alert("Account deactivation request raised");
        res.status = "tobedeleted";
        this.myHttp.put("http://localhost:5126/api/Accounts/"+id, res).subscribe(

        );
      }
    );
  } 

  public fetchBranchDetails(): void {
    this.myHttp.get<any[]>(this.branchurl).subscribe(
      branches => {
        this.branchDetails = branches;
      },
      error => {
        console.error('Error fetching branch details:', error);
      }
    );
  }

  public getIFSCCodeByBranch(branchName: string) {
    const branch = this.branchDetails.find(branch => branch.branchName === branchName);
    return branch ? branch.ifscCode : undefined;
  }
}
