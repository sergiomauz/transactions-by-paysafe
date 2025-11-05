using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Application.Commons.Exceptions;


namespace Application.Commons.Behaviours
{
    public class ValidationBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        private async Task<IEnumerable<ValidationFailure>> GetValidationResults(TRequest request, CancellationToken cancellationToken)
        {
            var validationResults = await Task.WhenAll(_validators
                                                        .Select(v => v.ValidateAsync(request, cancellationToken)));

            return validationResults.Where(r => r.Errors.Any()).SelectMany(r => r.Errors).ToList();
        }

        public ValidationBehaviour(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (_validators.Any())
            {
                var formatFailures = await GetValidationResults(request, cancellationToken);
                if (formatFailures.Any())
                {
                    throw new FormatValidationException(formatFailures);
                }
            }

            return await next();
        }
    }
}
