using Application.ErrorCatalog;
using Commons.Enums;
using FluentValidation;

namespace Application.UseCases.AccountTransactions.Commands.CreateAccountTransaction
{
    public class CreateAccountTransactionCommandValidator : AbstractValidator<CreateAccountTransactionCommand>
    {
        public CreateAccountTransactionCommandValidator()
        {
            // 'SourceAccountId' only with Guid format and not null
            RuleFor(x => x.SourceAccountId)
                .Must(v => Guid.TryParse(v, out _))
                .WithErrorCode(ErrorConstants.CreateAccountTransactionFormat00002.ErrorCode)
                .WithMessage(ErrorConstants.CreateAccountTransactionFormat00002.ErrorMessage)
                .OverridePropertyName(ErrorConstants.CreateAccountTransactionFormat00002.PropertyName)
                .When(x => x.SourceAccountId != null);

            // 'TargetAccountId' only with Guid format and not null
            RuleFor(x => x.TargetAccountId)
                .Must(v => Guid.TryParse(v, out _))
                .WithErrorCode(ErrorConstants.CreateAccountTransactionFormat00004.ErrorCode)
                .WithMessage(ErrorConstants.CreateAccountTransactionFormat00004.ErrorMessage)
                .OverridePropertyName(ErrorConstants.CreateAccountTransactionFormat00004.PropertyName)
                .When(x => x.TargetAccountId != null);

            // 'TranferTypeId' only int with TransferType format
            RuleFor(x => x.TransferTypeId)
                .NotNull()
                .WithErrorCode(ErrorConstants.CreateAccountTransactionFormat00005.ErrorCode)
                .WithMessage(ErrorConstants.CreateAccountTransactionFormat00005.ErrorMessage)
                .OverridePropertyName(ErrorConstants.CreateAccountTransactionFormat00005.PropertyName);
            RuleFor(x => x.TransferTypeId)
                .ChildRules(oc =>
                {
                    oc.RuleFor(c => c)
                        .GreaterThanOrEqualTo(0)
                        .WithErrorCode(ErrorConstants.CreateAccountTransactionFormat00006.ErrorCode)
                        .WithMessage(ErrorConstants.CreateAccountTransactionFormat00006.ErrorMessage)
                        .OverridePropertyName(ErrorConstants.CreateAccountTransactionFormat00006.PropertyName);
                    oc.RuleFor(c => c)
                        .Must(v => Enum.IsDefined(typeof(TransferType), v))
                        .WithErrorCode(ErrorConstants.CreateAccountTransactionFormat00007.ErrorCode)
                        .WithMessage(ErrorConstants.CreateAccountTransactionFormat00007.ErrorMessage)
                        .OverridePropertyName(ErrorConstants.CreateAccountTransactionFormat00007.PropertyName);
                })
                .When(x => x.TransferTypeId != null);

            // 'Value' not null and greater than zero
            RuleFor(x => x.Amount)
                .NotNull()
                .WithErrorCode(ErrorConstants.CreateAccountTransactionFormat00008.ErrorCode)
                .WithMessage(ErrorConstants.CreateAccountTransactionFormat00008.ErrorMessage)
                .OverridePropertyName(ErrorConstants.CreateAccountTransactionFormat00008.PropertyName);
            RuleFor(x => x.Amount)
                .ExclusiveBetween(0, 2501)
                .WithErrorCode(ErrorConstants.CreateAccountTransactionFormat00009.ErrorCode)
                .WithMessage(ErrorConstants.CreateAccountTransactionFormat00009.ErrorMessage)
                .OverridePropertyName(ErrorConstants.CreateAccountTransactionFormat00009.PropertyName)
                .When(x => x.Amount != null);

            // 'Value' not null and greater than zero
            RuleFor(x => x.TicketValidator)
                .NotNull()
                .WithErrorCode(ErrorConstants.CreateAccountTransactionFormat00010.ErrorCode)
                .WithMessage(ErrorConstants.CreateAccountTransactionFormat00010.ErrorMessage)
                .OverridePropertyName(ErrorConstants.CreateAccountTransactionFormat00010.PropertyName);

            // TransferTypeId must be B2C when source is null
            RuleFor(x => x)
                .Must(x => x.TransferTypeId == (int)TransferType.B2C)
                .WithErrorCode(ErrorConstants.CreateAccountTransactionFormat00011.ErrorCode)
                .WithMessage(ErrorConstants.CreateAccountTransactionFormat00011.ErrorMessage)
                .OverridePropertyName(ErrorConstants.CreateAccountTransactionFormat00011.PropertyName)
                .When(x => x.SourceAccountId == null && x.TargetAccountId != null);

            // TransferTypeId must be C2B when target is null
            RuleFor(x => x)
                .Must(x => x.TransferTypeId == (int)TransferType.C2B)
                .WithErrorCode(ErrorConstants.CreateAccountTransactionFormat00012.ErrorCode)
                .WithMessage(ErrorConstants.CreateAccountTransactionFormat00012.ErrorMessage)
                .OverridePropertyName(ErrorConstants.CreateAccountTransactionFormat00012.PropertyName)
                .When(x => x.SourceAccountId != null && x.TargetAccountId == null);

            // Accounts must be differents when both are not null and TransferTypeId must be C2C
            RuleFor(x => x)
                .Must(x =>
                    x.SourceAccountId != x.TargetAccountId &&
                    x.TransferTypeId == (int)TransferType.C2C)
                .WithErrorCode(ErrorConstants.CreateAccountTransactionFormat00013.ErrorCode)
                .WithMessage(ErrorConstants.CreateAccountTransactionFormat00013.ErrorMessage)
                .OverridePropertyName(ErrorConstants.CreateAccountTransactionFormat00013.PropertyName)
                .When(x => x.SourceAccountId != null && x.TargetAccountId != null);
        }
    }
}
