using FluentValidation;
using Application.Commons.Queries;
using Application.ErrorCatalog;


namespace Application.Commons.Validators
{
    public class CodeQueryValidator : AbstractValidator<CodeQuery>
    {
        public CodeQueryValidator()
        {
            RuleFor(x => x.Code)
                .Length(3, 25)
                .WithErrorCode(ErrorConstants.CodeFormat00001.ErrorCode)
                .WithMessage(ErrorConstants.CodeFormat00001.ErrorMessage)
                .OverridePropertyName(ErrorConstants.CodeFormat00001.PropertyName)
                .When(x => !string.IsNullOrEmpty(x.Code));
        }
    }
}
