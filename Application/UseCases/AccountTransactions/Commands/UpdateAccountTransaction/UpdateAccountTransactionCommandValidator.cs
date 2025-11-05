using Application.ErrorCatalog;
using Commons.Enums;
using FluentValidation;

namespace Application.UseCases.AccountTransactions.Commands.UpdateAccountTransaction
{
    public class UpdateAccountTransactionCommandValidator : AbstractValidator<UpdateAccountTransactionCommand>
    {
        public UpdateAccountTransactionCommandValidator()
        {
            // Id is mandatory
            RuleFor(x => x.Id)
                .NotNull()
                .WithErrorCode(ErrorConstants.UpdateAccountTransactionFormat00001.ErrorCode)
                .WithMessage(ErrorConstants.UpdateAccountTransactionFormat00001.ErrorMessage)
                .OverridePropertyName(ErrorConstants.UpdateAccountTransactionFormat00001.PropertyName);
            RuleFor(x => x.Id)
                .Must(v => Guid.TryParse(v, out _))
                .WithErrorCode(ErrorConstants.UpdateAccountTransactionFormat00002.ErrorCode)
                .WithMessage(ErrorConstants.UpdateAccountTransactionFormat00002.ErrorMessage)
                .OverridePropertyName(ErrorConstants.UpdateAccountTransactionFormat00002.PropertyName)
                .When(x => x.Id != null);

            // Change status
            RuleFor(x => x.AccountTransactionStatus)
                .NotNull()
                .WithErrorCode(ErrorConstants.UpdateAccountTransactionFormat00003.ErrorCode)
                .WithMessage(ErrorConstants.UpdateAccountTransactionFormat00003.ErrorMessage)
                .OverridePropertyName(ErrorConstants.UpdateAccountTransactionFormat00003.PropertyName);
            RuleFor(x => x.AccountTransactionStatus)
                .ChildRules(oc =>
                {
                    oc.RuleFor(c => c)
                        .GreaterThan(0)
                        .WithErrorCode(ErrorConstants.UpdateAccountTransactionFormat00004.ErrorCode)
                        .WithMessage(ErrorConstants.UpdateAccountTransactionFormat00004.ErrorMessage)
                        .OverridePropertyName(ErrorConstants.UpdateAccountTransactionFormat00004.PropertyName);
                    oc.RuleFor(c => c)
                        .Must(v => Enum.IsDefined(typeof(AccountTransactionStatus), v))
                        .WithErrorCode(ErrorConstants.UpdateAccountTransactionFormat00005.ErrorCode)
                        .WithMessage(ErrorConstants.UpdateAccountTransactionFormat00005.ErrorMessage)
                        .OverridePropertyName(ErrorConstants.UpdateAccountTransactionFormat00005.PropertyName);
                })
                .When(x => x.AccountTransactionStatus != null);
        }
    }
}
