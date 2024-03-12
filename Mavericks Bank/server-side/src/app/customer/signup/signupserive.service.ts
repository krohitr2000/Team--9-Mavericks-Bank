import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CustomersModel } from '../CustomerModel';
import { TokenvalidationService } from '../signin/tokenvalidation.service';

@Injectable({
  providedIn: 'root'
})
export class SeriveService {
  readonly customerurl='http://localhost:5126/api/Customers';
  readonly customerVerifyurl='http://localhost:5126/api/Customers/verify';

  list:CustomersModel[];
  ppData:CustomersModel=new CustomersModel();
  latestCustomerId:number;
  constructor(private myHttp:HttpClient, private token: TokenvalidationService) { }
  public customertList()
  {
    this.myHttp.get(this.customerurl).toPromise().then(res=>{
      this.list=res as CustomersModel[];
      this.latestCustomerId = this.list.reduce((maxValue, currentItem)=>{
        return currentItem.customerId > maxValue ? currentItem.customerId : maxValue;
      }, -Infinity);
    })
  }
  public regcustomer()
  {
    return this.myHttp.post(this.customerurl,this.ppData);
  }
  public delcustomer(customerId:number)
  {
    return this.myHttp.delete(this.customerurl+'/'+customerId);
  }
  public updatecustomer(customerId, customer)
  {
    return this.myHttp.put(this.customerurl+'/'+customerId,customer);
  }

  public verify(customerData)
  {
    return this.myHttp.post(this.customerVerifyurl,customerData);
  }

  public getCustomer(customerId) {
    return this.myHttp.get(this.customerurl+"/"+customerId);
  }

  public getLatestCustomerId

}
