using System.ComponentModel;

namespace Commons.Enums
{
    public enum TransferType
    {
        [Description("Customer to customer")]
        C2C = 0,

        [Description("Bank to customer")]
        B2C = 1,

        [Description("Customer to bank")]
        C2B = 2
    }
}
