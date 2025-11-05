using FluentValidation;
using Application.Commons.Queries;
using Application.ErrorCatalog;


namespace Application.Commons.Validators
{
    public class GuidsQueryValidator : AbstractValidator<GuidsQuery>
    {
        public GuidsQueryValidator()
        {
            RuleFor(x => x.Ids)
                .NotNull()
                .WithErrorCode(ErrorConstants.GuidsFormat00001.ErrorCode)
                .WithMessage((ErrorConstants.GuidsFormat00001).ErrorMessage)
                .OverridePropertyName((ErrorConstants.GuidsFormat00001).PropertyName);

            RuleFor(x => x.Ids)
                .NotEmpty()
                .WithErrorCode(ErrorConstants.GuidsFormat00002.ErrorCode)
                .WithMessage(ErrorConstants.GuidsFormat00002.ErrorMessage)
                .OverridePropertyName(ErrorConstants.GuidsFormat00002.PropertyName)
                .When(x => x.Ids != null);

            RuleForEach(x => x.Ids)
                .Must(v => Guid.TryParse(v, out _))
                .WithErrorCode(ErrorConstants.GuidsFormat00003.ErrorCode)
                .WithMessage(ErrorConstants.GuidsFormat00003.ErrorMessage)
                .OverridePropertyName(ErrorConstants.GuidsFormat00003.PropertyName)
                .When(x => x.Ids != null && x.Ids.Count > 0);

            RuleFor(x => x.Ids)
                .Must(x => x.Count() == x.Distinct().Count())
                .WithErrorCode(ErrorConstants.GuidsFormat00004.ErrorCode)
                .WithMessage(ErrorConstants.GuidsFormat00004.ErrorMessage)
                .OverridePropertyName(ErrorConstants.GuidsFormat00004.PropertyName)
                .When(x => x.Ids != null && x.Ids.Count > 0);
        }
    }
}
