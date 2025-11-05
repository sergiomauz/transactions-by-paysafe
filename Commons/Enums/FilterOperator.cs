using System.ComponentModel;


namespace Commons.Enums
{
    public enum FilterOperator
    {
        [Description("=")]
        Equals = 0,

        [Description("!=")]
        NotEquals = 1,

        [Description(">")]
        GreaterThan = 2,

        [Description("<")]
        LessThan = 3,

        [Description(">=")]
        GreaterThanOrEqual = 4,

        [Description("<=")]
        LessThanOrEqual = 5,

        [Description("contains")]
        Contains = 6,

        [Description("starts_with")]
        StartsWith = 7,

        [Description("ends_with")]
        EndsWith = 8,

        [Description("in")]
        In = 9
    }
}
