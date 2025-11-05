using System.ComponentModel;

namespace Commons.Enums
{
    public enum AccountTransactionStatus
    {
        [Description("Pending")]
        Pending = 0,

        [Description("Approved")]
        Approved = 1,

        [Description("Rejected")]
        Rejected = 2
    }
}
