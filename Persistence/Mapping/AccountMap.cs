using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Persistence.Mapping
{
    public class AccountMap
    {
        public AccountMap(EntityTypeBuilder<Account> entityBuilder)
        {
            entityBuilder.ToTable(name: "Accounts");

            #region ======== PRIMARY KEYS ========
            entityBuilder.HasKey(t => t.Id);
            entityBuilder
                .Property(t => t.Id)
                .ValueGeneratedOnAdd();
            #endregion

            #region ======== RELATIONSHIPS: HAS MANY ========
            entityBuilder
                .HasMany(d => d.TransactionsSource)
                .WithOne(o => o.SourceAccount)
                .HasForeignKey(to => to.SourceAccountId)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder
                .HasMany(d => d.TransactionsTarget)
                .WithOne(o => o.TargetAccount)
                .HasForeignKey(to => to.TargetAccountId)
                .OnDelete(DeleteBehavior.Restrict);
            #endregion

            entityBuilder
                .Property(t => t.Code)
                .IsRequired()
                .HasMaxLength(11);
            entityBuilder
                .HasIndex(t => t.Code)
                .IsUnique();

            entityBuilder
                .Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(200);

            entityBuilder
                .Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(100);

            #region ======== AUDIT COLUMNS ========
            entityBuilder
                .Property(t => t.CreatedAt)
                .IsRequired();

            entityBuilder
                .Property(t => t.ModifiedAt);

            entityBuilder
                .Property(t => t.DisabledAt);
            #endregion
        }
    }
}
