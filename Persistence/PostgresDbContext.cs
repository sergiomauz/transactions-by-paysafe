using Microsoft.EntityFrameworkCore;
using Persistence.Mapping;
using Domain.Entities;

namespace Persistence
{
    public class PostgresDbContext : DbContext
    {
        public PostgresDbContext(DbContextOptions<PostgresDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _ = new AccountTransactionMap(modelBuilder.Entity<AccountTransaction>());
            _ = new AccountMap(modelBuilder.Entity<Account>());

            // Accounts by default
            modelBuilder.Entity<Account>().HasData(
                new Account
                {
                    Id = new Guid("0055ea39-0bf2-4556-8f58-4f17248c936d"),
                    Name = "Nieto Portales, Karla",
                    Email = "sergiouce16@gmail.com",
                    Code = "10447853660",
                    CreatedAt = DateTime.UtcNow
                },
                new Account
                {
                    Id = new Guid("3f615cbe-c226-4064-8bd0-00d6dc1933e6"),
                    Name = "Zambrano Jove, Sergio",
                    Email = "sergio.mauz88@gmail.com",
                    Code = "10447853669",
                    CreatedAt = DateTime.UtcNow
                }
            );
        }
    }
}
