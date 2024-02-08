using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MavericksBankApi.Models
{
    public partial class MavericksBankDbContext : DbContext
    {
        public MavericksBankDbContext()
        {
        }

        public MavericksBankDbContext(DbContextOptions<MavericksBankDbContext> options)
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
                optionsBuilder.UseSqlServer("Server=DESKTOP-8QHJLG6;database=MavericksBankDb;trusted_connection=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.AccountId)
                    .ValueGeneratedNever()
                    .HasColumnName("AccountID");

                entity.Property(e => e.AccountType).HasMaxLength(50);

                entity.Property(e => e.Balance).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Account__Custome__3A81B327");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.HasIndex(e => e.Username, "UQ__Customer__536C85E43408CEC8")
                    .IsUnique();

                entity.Property(e => e.CustomerId)
                    .ValueGeneratedNever()
                    .HasColumnName("CustomerID");

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

                entity.HasIndex(e => e.Username, "UQ__Employee__536C85E405DFEF53")
                    .IsUnique();

                entity.Property(e => e.EmployeeId)
                    .ValueGeneratedNever()
                    .HasColumnName("EmployeeID");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Password).HasMaxLength(50);

                entity.Property(e => e.Phone).HasMaxLength(20);

                entity.Property(e => e.Username).HasMaxLength(50);

                entity.HasMany(d => d.Accounts)
                    .WithMany(p => p.Employees)
                    .UsingEntity<Dictionary<string, object>>(
                        "EmployeeAccountAssociation",
                        l => l.HasOne<Account>().WithMany().HasForeignKey("AccountId").HasConstraintName("FK__EmployeeA__Accou__46E78A0C"),
                        r => r.HasOne<Employee>().WithMany().HasForeignKey("EmployeeId").HasConstraintName("FK__EmployeeA__Emplo__45F365D3"),
                        j =>
                        {
                            j.HasKey("EmployeeId", "AccountId").HasName("PK__Employee__099995A96E41A873");

                            j.ToTable("EmployeeAccountAssociation");

                            j.IndexerProperty<int>("EmployeeId").HasColumnName("EmployeeID");

                            j.IndexerProperty<int>("AccountId").HasColumnName("AccountID");
                        });

                entity.HasMany(d => d.Loans)
                    .WithMany(p => p.Employees)
                    .UsingEntity<Dictionary<string, object>>(
                        "EmployeeLoanAssociation",
                        l => l.HasOne<Loan>().WithMany().HasForeignKey("LoanId").HasConstraintName("FK__EmployeeL__LoanI__4AB81AF0"),
                        r => r.HasOne<Employee>().WithMany().HasForeignKey("EmployeeId").HasConstraintName("FK__EmployeeL__Emplo__49C3F6B7"),
                        j =>
                        {
                            j.HasKey("EmployeeId", "LoanId").HasName("PK__Employee__1E25E2B28B707DDA");

                            j.ToTable("EmployeeLoanAssociation");

                            j.IndexerProperty<int>("EmployeeId").HasColumnName("EmployeeID");

                            j.IndexerProperty<int>("LoanId").HasColumnName("LoanID");
                        });
            });

            modelBuilder.Entity<Loan>(entity =>
            {
                entity.ToTable("Loan");

                entity.Property(e => e.LoanId)
                    .ValueGeneratedNever()
                    .HasColumnName("LoanID");

                entity.Property(e => e.ApprovalDate).HasColumnType("datetime");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.DisbursementDate).HasColumnType("datetime");

                entity.Property(e => e.InterestRate).HasColumnType("decimal(5, 2)");

                entity.Property(e => e.LoanAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.LoanStatus).HasMaxLength(50);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Loans)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Loan__CustomerID__403A8C7D");
            });

            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(e => e.TransactionId)
                    .ValueGeneratedNever()
                    .HasColumnName("TransactionID");

                entity.Property(e => e.AccountId).HasColumnName("AccountID");

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TransactionDate).HasColumnType("datetime");

                entity.Property(e => e.TransactionType).HasMaxLength(50);

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Transactions)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__Transacti__Accou__3D5E1FD2");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
