using FluentValidation;
using Application.Commons.Queries;
using Application.ErrorCatalog;


namespace Application.Commons.Validators
{
    public class BasicSearchQueryValidator : AbstractValidator<BasicSearchQuery>
    {
        public BasicSearchQueryValidator()
        {
            RuleFor(x => x)
                .SetValidator(new PaginatedQueryValidator());

            RuleFor(x => x.TextFilter)
                .Length(3, 100)
                .WithErrorCode(ErrorConstants.BasicSearchFormat00001.ErrorCode)
                .WithMessage(ErrorConstants.BasicSearchFormat00001.ErrorMessage)
                .OverridePropertyName(ErrorConstants.BasicSearchFormat00001.PropertyName)
                .When(x => !string.IsNullOrEmpty(x.TextFilter));
        }
    }
}
