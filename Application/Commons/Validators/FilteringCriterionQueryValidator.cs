using System.Text.Json;
using Commons.Enums;
using Application.Commons.Queries;


namespace Application.Commons.Validators
{
    public static class FilteringCriterionQueryValidator
    {
        public static bool IsValid(FilteringCriterionQuery filteringCriterion)
        {
            if (filteringCriterion == null)
                return true;

            // Operator must not be null or invalid
            if (!EnumHelper.IsValidDescription<FilterOperator>(filteringCriterion.Operator))
                return false;

            // Value can be null
            if (filteringCriterion.Operand == null)
                return true;


            // Try to get the real value if it is a JsonElement
            var val = filteringCriterion.Operand.Value;
            var valueType = val.ValueKind;
            var filterOperator = EnumHelper.FromDescription<FilterOperator>(filteringCriterion.Operator);
            // Is a boolean?
            if (valueType == JsonValueKind.True || valueType == JsonValueKind.False)
            {
                return filterOperator == FilterOperator.Equals ||
                       filterOperator == FilterOperator.NotEquals;
            }

            // Is a number?
            if (valueType == JsonValueKind.Number)
            {
                return filterOperator == FilterOperator.Equals ||
                       filterOperator == FilterOperator.NotEquals ||
                       filterOperator == FilterOperator.GreaterThan ||
                       filterOperator == FilterOperator.LessThan ||
                       filterOperator == FilterOperator.GreaterThanOrEqual ||
                       filterOperator == FilterOperator.LessThanOrEqual;
            }

            // Is a string?
            if (valueType == JsonValueKind.String)
            {
                string strVal = val.GetString();

                // Check if it is a valid date or datetime
                if (DateTime.TryParse(strVal, out _))
                {
                    return filterOperator == FilterOperator.Equals ||
                           filterOperator == FilterOperator.NotEquals ||
                           filterOperator == FilterOperator.GreaterThan ||
                           filterOperator == FilterOperator.LessThan ||
                           filterOperator == FilterOperator.GreaterThanOrEqual ||
                           filterOperator == FilterOperator.LessThanOrEqual;
                }
                else
                {
                    // Regular string
                    return filterOperator == FilterOperator.Equals ||
                           filterOperator == FilterOperator.NotEquals ||
                           filterOperator == FilterOperator.Contains ||
                           filterOperator == FilterOperator.StartsWith ||
                           filterOperator == FilterOperator.EndsWith;
                }
            }

            // Is an array?
            if (valueType == JsonValueKind.Array)
            {
                var list = val.EnumerateArray();
                if (list.Count() > 0)
                {
                    var firstElementType = list.First().ValueKind;

                    // All elements must be the same type
                    if (!list.All(x => x.ValueKind == firstElementType))
                        return false;

                    // Special case: all elements are strings and valid dates
                    if (firstElementType == JsonValueKind.String)
                    {
                        var allAreValidDates = list.All(x => DateTime.TryParse(x.GetString(), out _));
                        if (allAreValidDates)
                            return filterOperator == FilterOperator.In;
                    }
                }

                // Only "In" is valid for any array
                return filterOperator == FilterOperator.In;
            }

            // If none of the cases apply, the object is invalid
            return false;
        }
    }
}
