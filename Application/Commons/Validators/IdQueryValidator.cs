using FluentValidation;
using Application.Commons.Queries;
using Application.ErrorCatalog;


namespace Application.Commons.Validators
{
    public class IdQueryValidator : AbstractValidator<IdQuery>
    {
        public IdQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .WithErrorCode(ErrorConstants.IdFormat00001.ErrorCode)
                .WithMessage(ErrorConstants.IdFormat00001.ErrorMessage)
                .OverridePropertyName(ErrorConstants.IdFormat00001.PropertyName);

            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithErrorCode(ErrorConstants.IdFormat00002.ErrorCode)
                .WithMessage(ErrorConstants.IdFormat00002.ErrorMessage)
                .OverridePropertyName(ErrorConstants.IdFormat00002.PropertyName)
                .When(x => x.Id != null);
        }
    }
}
