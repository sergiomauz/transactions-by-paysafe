using FluentValidation;
using Application.Commons.Queries;
using Application.ErrorCatalog;


namespace Application.Commons.Validators
{
    public class GuidQueryValidator : AbstractValidator<GuidQuery>
    {
        public GuidQueryValidator()
        {
            RuleFor(x => x.Id)
                .NotNull()
                .WithErrorCode(ErrorConstants.GuidFormat00001.ErrorCode)
                .WithMessage(ErrorConstants.GuidFormat00001.ErrorMessage)
                .OverridePropertyName(ErrorConstants.GuidFormat00001.PropertyName);

            RuleFor(x => x.Id)
                .Must(v => Guid.TryParse(v, out _))
                .WithErrorCode(ErrorConstants.GuidFormat00002.ErrorCode)
                .WithMessage(ErrorConstants.GuidFormat00002.ErrorMessage)
                .OverridePropertyName(ErrorConstants.GuidFormat00002.PropertyName)
                .When(x => x.Id != null);
        }
    }
}
