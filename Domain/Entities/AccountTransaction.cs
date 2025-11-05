using Commons.Enums;
using Domain.Entities.Bases;

namespace Domain.Entities
{
    public class AccountTransaction : BaseEntityWithGuid
    {
        #region ====== RELATIONSHIPS: BELONGS TO ======
        public Guid? SourceAccountId { get; set; }
        public Account? SourceAccount { get; set; }

        public Guid? TargetAccountId { get; set; }
        public Account? TargetAccount { get; set; }
        #endregion

        public long? TicketValidator { get; set; }
        public TransferType? TransferType { get; set; }
        public decimal? Amount { get; set; }
        public AccountTransactionStatus? AccountTransactionStatus { get; set; }
    }
}
