using Domain.Entities.Bases;

namespace Domain.Entities
{
    public class Account : BaseEntityWithGuidAndCode
    {
        #region ====== RELATIONSHIPS: ONE TO MANY - HAS MANY ======
        public IEnumerable<AccountTransaction> TransactionsSource { get; set; }
        public IEnumerable<AccountTransaction> TransactionsTarget { get; set; }
        #endregion

        public string? Name { get; set; }
        public string? Email { get; set; }
        public DateTime? DisabledAt { get; set; }
    }
}
