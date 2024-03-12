export class CreatedAccountDetails {
    accountId: number;
    customerId: number;
    accountType: string;
    balance: number;
    status: string;
    branchName:string;
    ifscCode:string;

    constructor(accountId: number, customerId: number, accountType: string, balance: number, status: string,branchName:string,ifscCode:string) {
        this.accountId = accountId;
        this.customerId = customerId;
        this.accountType = accountType;
        this.balance = balance;
        this.status = status;
        this.branchName= branchName;
        this.ifscCode=ifscCode;
    }
}