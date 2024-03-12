using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using NuGet.Common;
using MavericksBank.Models;

namespace MavericksBank.Models
{
    public partial class MavericksBankDb2Context : DbContext
    {
        public MavericksBankDb2Context()
        {
        }

        public MavericksBankDb2Context(DbContextOptions<MavericksBankDb2Context> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Employee> Employees { get; set; } = null!;
        public virtual DbSet<Loan> Loans { get; set; } = null!;
        public virtual DbSet<Transaction> Transactions { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-8QHJLG6;database=MavericksBankDb1;trusted_connection=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.AccountType).HasMaxLength(50);

                entity.Property(e => e.Balance).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.Status).HasMaxLength(20);
                entity.Property(e => e.IFSCCode).HasColumnName("IFSCCode");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Account__Custome__3A81B327");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.HasIndex(e => e.Username, "UQ__Customer__536C85E420292607")
                    .IsUnique();

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(20);

                entity.Property(e => e.Username).HasMaxLength(50);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee");

                entity.HasIndex(e => e.Username, "UQ__Employee__536C85E4A9EDE3A7")
                    .IsUnique();

                entity.Property(e => e.EmployeeId).HasColumnName("EmployeeID");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(20);

                entity.Property(e => e.Username).HasMaxLength(50);

                entity.HasMany(d => d.Accounts)
                    .WithMany(p => p.Employees)
                    .UsingEntity<Dictionary<string, object>>(
                        "EmployeeAccountAssociation",
                        l => l.HasOne<Account>().WithMany().HasForeignKey("AccountId").HasConstraintName("FK__EmployeeA__Accou__4D94879B"),
                        r => r.HasOne<Employee>().WithMany().HasForeignKey("EmployeeId").HasConstraintName("FK__EmployeeA__Emplo__4CA06362"),
                        j =>
                        {
                            j.HasKey("EmployeeId", "AccountId").HasName("PK__Employee__099995A99B46B9EC");

                            j.ToTable("EmployeeAccountAssociation");

                            j.IndexerProperty<int>("EmployeeId").HasColumnName("EmployeeID");

                            j.IndexerProperty<int>("AccountId").HasColumnName("AccountID");
                        });

                entity.HasMany(d => d.Loans)
                    .WithMany(p => p.Employees)
                    .UsingEntity<Dictionary<string, object>>(
                        "EmployeeLoanAssociation",
                        l => l.HasOne<Loan>().WithMany().HasForeignKey("LoanId").HasConstraintName("FK__EmployeeL__LoanI__5165187F"),
                        r => r.HasOne<Employee>().WithMany().HasForeignKey("EmployeeId").HasConstraintName("FK__EmployeeL__Emplo__5070F446"),
                        j =>
                        {
                            j.HasKey("EmployeeId", "LoanId").HasName("PK__Employee__1E25E2B2270169FA");

                            j.ToTable("EmployeeLoanAssociation");

                            j.IndexerProperty<int>("EmployeeId").HasColumnName("EmployeeID");

                            j.IndexerProperty<int>("LoanId").HasColumnName("LoanID");
                        });
            });

            modelBuilder.Entity<Loan>(entity =>
            {
                entity.ToTable("Loan");

                entity.Property(e => e.LoanId).HasColumnName("LoanID");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.ApprovalDate).HasColumnType("datetime");

                entity.Property(e => e.DisbursementDate).HasColumnType("datetime");

                entity.Property(e => e.InterestRate).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.LoanAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.LoanStatus).HasMaxLength(50);

                entity.Property(e => e.LoanType).HasMaxLength(20);
                entity.Property(e => e.Tenure).HasColumnType("int");

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Loans)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Loan__AccountID__46E78A0C");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.TransactionId).HasColumnName("TransactionID");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.RecieverAccountId).HasColumnName("RecieverAccountID");

                entity.Property(e => e.SenderAccountId).HasColumnName("SenderAccountID");

                entity.Property(e => e.TransactionDate).HasColumnType("datetime");

                entity.Property(e => e.TransactionType).HasMaxLength(50);

                entity.HasOne(d => d.RecieverAccount)
                    .WithMany(p => p.TransactionRecieverAccounts)
                    .HasForeignKey(d => d.RecieverAccountId)
                    .HasConstraintName("FK__Transacti__Recie__440B1D61");

                entity.HasOne(d => d.SenderAccount)
                    .WithMany(p => p.TransactionSenderAccounts)
                    .HasForeignKey(d => d.SenderAccountId)
                    .HasConstraintName("FK__Transacti__Sende__4316F928");
            });

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.Property(e => e.AdminId).HasColumnName("AdminId");
                entity.Property(e => e.Username).HasColumnName("Username");
                entity.Property(e => e.Password).HasColumnName("Password");
                entity.Property(e => e.Name).HasColumnName("Name");
                entity.Property(e => e.Email).HasColumnName("Email");
            });

            modelBuilder.Entity<BranchDetails>(entity =>
            {
                entity.Property(e => e.IFSCCode).HasColumnName("IFSCCode");
                entity.Property(e => e.BranchName).HasColumnName("BranchName");

            });


            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        public DbSet<MavericksBank.Models.Admin>? Admin { get; set; }

        public DbSet<MavericksBank.Models.BranchDetails>? BranchDetails { get; set; }

        //public DbSet<MavericksBank.Models.BankBranch>? BankBranch { get; set; }
    }
}
