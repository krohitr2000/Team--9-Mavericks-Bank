export class LoanDetails {
    loanId: number;
    loanType: string;
    accountId: number;
    loanAmount: number;
    interestRate: number;
    loanStatus: string;
    approvalDate: Date;
    disbursementDate: Date;
    account: any;
    employees: any[];

    constructor(
        loanId: number,
        loanType: string,
        accountId: number,
        loanAmount: number,
        interestRate: number,
        loanStatus: string,
        approvalDate: Date,
        disbursementDate: Date,
        account: any,
        employees: any[]
    ) {
        this.loanId = loanId;
        this.loanType = loanType;
        this.accountId = accountId;
        this.loanAmount = loanAmount;
        this.interestRate = interestRate;
        this.loanStatus = loanStatus;
        this.approvalDate = approvalDate;
        this.disbursementDate = disbursementDate;
        this.account = account;
        this.employees = employees;
    }
}