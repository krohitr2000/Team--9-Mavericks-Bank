import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { CustomerComponent } from './customer/customer.component';
import { LoanPlansComponent } from './loan-plans/loan-plans.component';
import { AboutusComponent } from './aboutus/aboutus.component';
import { SigninComponent } from './customer/signin/signin.component';
import { SignupComponent } from './customer/signup/signup.component';
import { EmpsigninComponent } from './employee/empsignin/empsignin.component';
import { CreateAccountComponent } from './account/create-account/create-account.component';
import { DisplayAccountComponent } from './account/display-account/display-account.component';
import { PendingAccountsComponent } from './account/pending-accounts/pending-accounts.component';
import { RegisterComponent } from './customer/register/register.component';
import { AccountComponent } from './account/account.component';
import { ApplyLoanComponent } from './account/create-account/apply-loan/apply-loan.component';
import { TransactionComponent } from './account/transaction/transaction.component';
import { TransactionHistoryComponent } from './account/transaction/transaction-history/transaction-history.component';
import { EmployeeComponent } from './employee/employee.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { ConfirmPasswordComponent } from './confirm-password/confirm-password.component';
import { AdminSigninComponent } from './Admin/admin-signin/admin-signin.component';
import { AdminComponent } from './Admin/admin/admin.component';

const routes: Routes = [
  {path:'',component:HomeComponent},
  {path:'employee/:employeeId',component:EmployeeComponent},
  {path:'loan-plans/:accountId',component:LoanPlansComponent},
  {path:'aboutus',component:AboutusComponent},
  {path:'signin',component:SigninComponent},
  {path:'signup',component:SignupComponent},
  {path:'empsignin',component:EmpsigninComponent},
  {path:'create-account/:customerId',component:CreateAccountComponent},
  {path:'display-account/:customerId',component:DisplayAccountComponent},
  {path:'pending-accounts/:customerId',component:PendingAccountsComponent},
  {path:'customer/:customerId',component:CustomerComponent},
  {path:'customer',component:CustomerComponent},
  { path: 'account/:accountId', component: AccountComponent },
  {path:'apply-loan/:accountId/:loanType',component:ApplyLoanComponent},
  {path:'transaction/:accountId',component:TransactionComponent},
  {path:'transaction-history/:accountId',component:TransactionHistoryComponent},
  {path:'forgot-password',component:ForgotPasswordComponent},
  {path:'confirm-password/:customerId',component:ConfirmPasswordComponent},
  {path:'admin-signin',component:AdminSigninComponent},
  {path:'admin/:adminId',component:AdminComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
