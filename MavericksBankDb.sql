CREATE DATABASE MavericksBankDb;
USE MavericksBankDb;



CREATE TABLE Customer (
    CustomerID INT PRIMARY KEY,
    Username NVARCHAR(50) UNIQUE,
    Password NVARCHAR(50),
    Name NVARCHAR(100),
    Email NVARCHAR(100),
    Phone NVARCHAR(20),
    Address NVARCHAR(255)
);


CREATE TABLE Account (
    AccountID INT PRIMARY KEY,
    CustomerID INT,
    AccountType NVARCHAR(50),
    Balance DECIMAL(18, 2),
    FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID) ON DELETE CASCADE
);

CREATE TABLE Transactions (
    TransactionID INT PRIMARY KEY,
    AccountID INT,
    TransactionType NVARCHAR(50),
    Amount DECIMAL(18, 2),
    TransactionDate DATETIME,
    FOREIGN KEY (AccountID) REFERENCES Account(AccountID) ON DELETE CASCADE
);


CREATE TABLE Loan (
    LoanID INT PRIMARY KEY,
    CustomerID INT,
    LoanAmount DECIMAL(18, 2),
    InterestRate DECIMAL(5, 2),
    LoanStatus NVARCHAR(50),
    ApprovalDate DATETIME,
    DisbursementDate DATETIME,
    FOREIGN KEY (CustomerID) REFERENCES Customer(CustomerID) ON DELETE CASCADE
);

CREATE TABLE Employee (
    EmployeeID INT PRIMARY KEY,
    Username NVARCHAR(50) UNIQUE,
    Password NVARCHAR(50),
    Name NVARCHAR(100),
    Email NVARCHAR(100),
    Phone NVARCHAR(20)
);

CREATE TABLE EmployeeAccountAssociation (
    EmployeeID INT,
    AccountID INT,
    PRIMARY KEY (EmployeeID, AccountID),
    FOREIGN KEY (EmployeeID) REFERENCES Employee(EmployeeID) ON DELETE CASCADE,
    FOREIGN KEY (AccountID) REFERENCES Account(AccountID) ON DELETE CASCADE
);

CREATE TABLE EmployeeLoanAssociation (
    EmployeeID INT,
    LoanID INT,
    PRIMARY KEY (EmployeeID, LoanID),
    FOREIGN KEY (EmployeeID) REFERENCES Employee(EmployeeID) ON DELETE CASCADE,
    FOREIGN KEY (LoanID) REFERENCES Loan(LoanID) ON DELETE CASCADE
);

SELECT * FROM Customer;
SELECT * FROM Account;
SELECT * FROM Transactions;
SELECT * FROM Loan;
SELECT * FROM Employee;
SELECT * FROM EmployeeAccountAssociation;
SELECT * FROM EmployeeLoanAssociation;

