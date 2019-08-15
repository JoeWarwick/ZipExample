using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ZipExample.DBModel
{
    public class ZEModel : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }

        public static void Initialize(IServiceProvider serviceProvider)
        {
            ZEModel context = serviceProvider.GetRequiredService<ZEModel>();
            if (!context.Database.EnsureCreated())
                context.Database.Migrate();
        }

        public ZEModel(DbContextOptions<ZEModel> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.UserId)
                .IsUnique();
            modelBuilder.Entity<User>()
                .HasAlternateKey(c => c.EmailAddress)
                .HasName("AlternateKey_EmailAddress");
            modelBuilder.Entity<Account>()
                .HasIndex(a => a.AccountId)
                .IsUnique();
            modelBuilder.Entity<Account>()
                .Property(a => a.Created)
                .HasDefaultValueSql("getDate()");

            var u1 = Guid.NewGuid();
            var u2 = Guid.NewGuid();
            var u3 = Guid.NewGuid();

            modelBuilder.Entity<User>()
                .HasData(
                new User
                {
                    UserId = u1,
                    EmailAddress = "ted.bundy@outlook.com",
                    Name = "Ted Bundy",
                    MonthlyExpenses = 3000,
                    MonthlySalary = 6000,
                },
                new User { 
                    UserId = u2,
                    EmailAddress = "tina.arena@outlook.com",
                    Name = "Tina Arena",
                    MonthlyExpenses = 20000,
                    MonthlySalary = 60000,
                },
                new User
                {
                    UserId = u3,
                    EmailAddress = "matt.smith@outlook.com",
                    Name = "Matt Smith",
                    MonthlyExpenses = 6000,
                    MonthlySalary = 6000,
                });

            modelBuilder.Entity<Account>()
                .HasData(
                     new Account
                     {
                         AccountId = Guid.NewGuid(),
                         UserId = u1,
                         AccountName = "Spending",
                         TotalCredit = 2000,
                         CreditRemaining = 20
                     },
                    new Account
                    {
                        AccountId = Guid.NewGuid(),
                        UserId = u1,
                        AccountName = "Bills",
                        TotalCredit = 700,
                        CreditRemaining = 0
                    },
                    new Account
                    {
                        AccountId = Guid.NewGuid(),
                        UserId = u2,
                        AccountName = "Travel",
                        TotalCredit = 6000,
                        CreditRemaining = 3000
                    },
                    new Account
                    {
                        AccountId = Guid.NewGuid(),
                        UserId = u2,
                        AccountName = "Sundries",
                        TotalCredit = 60000,
                        CreditRemaining = 33000
                    },
                    new Account
                    {
                        AccountId = Guid.NewGuid(),
                        UserId = u3,
                        AccountName = "Expenses",
                        TotalCredit = 2000,
                        CreditRemaining = 1000
                    },
                    new Account
                    {
                        AccountId = Guid.NewGuid(),
                        UserId = u3,
                        AccountName = "Emergencies",
                        TotalCredit = 1000,
                        CreditRemaining = 1000
                    }
                );
        }


    }

    public class User
    {
        public Guid UserId { get; set; }

        [Required]
        public String Name { get; set; }

        [Required]
        public string EmailAddress { get; set; }

        [Range(0, int.MaxValue)]
        public int MonthlySalary { get; set; }

        [Range(0, int.MaxValue)]
        public int MonthlyExpenses { get; set; }

        public List<Account> Accounts { get; set; }
    }

    public class Account
    {
        public Guid AccountId { get; set; }

        public Guid UserId { get; set; }

        public User Owner { get; set; }
        
        public int TotalCredit { get; set; }

        public int CreditRemaining { get; set; }

        [Required]
        public string AccountName { get; set; }

        public DateTime Created { get; set; }
    }
}
