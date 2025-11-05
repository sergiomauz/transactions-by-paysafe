using FluentValidation;
using Application.Commons.Queries;
using Application.ErrorCatalog;


namespace Application.Commons.Validators
{
    public class IdsQueryValidator : AbstractValidator<IdsQuery>
    {
        public IdsQueryValidator()
        {
            RuleFor(x => x.Ids)
                .NotNull()
                .WithErrorCode(ErrorConstants.IdsFormat00001.ErrorCode)
                .WithMessage(ErrorConstants.IdsFormat00001.ErrorMessage)
                .OverridePropertyName(ErrorConstants.IdsFormat00001.PropertyName);

            RuleFor(x => x.Ids)
                .NotEmpty()
                .WithErrorCode(ErrorConstants.IdsFormat00002.ErrorCode)
                .WithMessage(ErrorConstants.IdsFormat00002.ErrorMessage)
                .OverridePropertyName(ErrorConstants.IdsFormat00002.PropertyName)
                .When(x => x.Ids != null);

            RuleForEach(x => x.Ids)
                .GreaterThan(0)
                .WithErrorCode(ErrorConstants.IdsFormat00003.ErrorCode)
                .WithMessage(ErrorConstants.IdsFormat00003.ErrorMessage)
                .OverridePropertyName(ErrorConstants.IdsFormat00003.PropertyName)
                .When(x => x.Ids != null && x.Ids.Count > 0);

            RuleFor(x => x.Ids)
                .Must(x => x.Count() == x.Distinct().Count())
                .WithErrorCode(ErrorConstants.IdsFormat00004.ErrorCode)
                .WithMessage(ErrorConstants.IdsFormat00004.ErrorMessage)
                .OverridePropertyName(ErrorConstants.IdsFormat00004.PropertyName)
                .When(x => x.Ids != null && x.Ids.Count > 0);
        }
    }
}
