using FluentValidation.Results;


namespace Application.Commons.Utils
{
    public static class Extensions
    {
        public static Dictionary<string, IEnumerable<Dictionary<string, string>>> GetValidationFormatFailures(this IEnumerable<ValidationFailure> validationFailures)
        {
            return validationFailures
                .GroupBy(f => f.PropertyName)
                .ToDictionary(
                    group => group.Key,
                    group => group.Select(failure => new Dictionary<string, string>
                    {
                        { "error_code", failure.ErrorCode },
                        { "error_message", failure.ErrorMessage }
                    }).AsEnumerable()
                );
        }
    }
}
