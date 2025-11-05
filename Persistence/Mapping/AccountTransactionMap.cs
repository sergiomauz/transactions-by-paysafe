using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Entities;

namespace Persistence.Mapping
{
    public class AccountTransactionMap
    {
        public AccountTransactionMap(EntityTypeBuilder<AccountTransaction> entityBuilder)
        {
            entityBuilder.ToTable(name: "AccountTransactions");

            #region ======== PRIMARY KEYS ========
            entityBuilder.HasKey(t => t.Id);
            entityBuilder
                .Property(t => t.Id)
                .ValueGeneratedOnAdd();
            #endregion

            entityBuilder
                .Property(t => t.TransferType)
                .IsRequired();

            entityBuilder
                .Property(t => t.Amount)
                .IsRequired();

            entityBuilder
                .Property(t => t.AccountTransactionStatus)
                .IsRequired();

            #region ======== AUDIT COLUMNS ========
            entityBuilder
                .Property(t => t.CreatedAt)
                .IsRequired();

            entityBuilder
                .Property(t => t.ModifiedAt);
            #endregion
        }
    }
}
