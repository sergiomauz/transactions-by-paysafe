using FluentValidation;
using Application.Commons.Queries;
using Application.ErrorCatalog;


namespace Application.Commons.Validators
{
    public class PaginatedQueryValidator : AbstractValidator<PaginatedQuery>
    {
        public PaginatedQueryValidator()
        {
            RuleFor(x => x.CurrentPage)
                .GreaterThan(0)
                .WithErrorCode(ErrorConstants.PaginatedFormat00001.ErrorCode)
                .WithMessage(ErrorConstants.PaginatedFormat00001.ErrorMessage)
                .OverridePropertyName(ErrorConstants.PaginatedFormat00001.PropertyName)
                .When(x => x.CurrentPage != null);

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithErrorCode(ErrorConstants.PaginatedFormat00002.ErrorCode)
                .WithMessage(ErrorConstants.PaginatedFormat00002.ErrorMessage)
                .OverridePropertyName(ErrorConstants.PaginatedFormat00002.PropertyName)
                .When(x => x.PageSize != null);
        }
    }
}
