using FluentValidation;
using FluentValidation.Results;


namespace Application.Commons.Exceptions
{
    public class FormatValidationException : ValidationException
    {
        public FormatValidationException(IEnumerable<ValidationFailure> errors) : base(string.Empty, errors, true)
        {
        }
    }
}
