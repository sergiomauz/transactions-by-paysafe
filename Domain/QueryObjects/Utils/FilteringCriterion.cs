using Commons.Enums;


namespace Domain.QueryObjects.Utils
{
    public class FilteringCriterion
    {
        public FilterOperator Operator { get; set; }
        public object? Operand { get; set; }
    }
}
