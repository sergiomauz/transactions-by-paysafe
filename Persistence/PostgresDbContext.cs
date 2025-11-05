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
                    Id = Guid.NewGuid(),
                    Name = "Nieto Portales, Karla",
                    Email = "sergiouce16@gmail.com",
                    Code = "10447853660",
                    CreatedAt = DateTime.UtcNow
                },
                new Account 
                { 
                    Id = Guid.NewGuid(), 
                    Name = "Zambrano Jove, Sergio", 
                    Email = "sergio.mauz88@gmail.com", 
                    Code = "10447853669", 
                    CreatedAt = DateTime.UtcNow
                }
            );
        }
    }
}
