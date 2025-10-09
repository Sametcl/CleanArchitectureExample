using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace CleanArchitecture.Application.Behaviors
{
    public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : class, IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }

            var context = new ValidationContext<TRequest> (request);
            var errrorDictionary = _validators
                .Select(v => v.Validate(context))
                .SelectMany(result => result.Errors)
                .Where(failure => failure != null)
                .GroupBy(
                e => e.PropertyName,
                e => e.ErrorMessage, 
                (propertName, errorMessage) => new
                {
                    Key= propertName,
                    Values = errorMessage.Distinct().ToArray()
                }).ToDictionary(s=>s.Key , s => s.Values[0]);
            if (errrorDictionary.Any())
            {
                var errors = errrorDictionary.Select(s => new ValidationFailure
                {
                    PropertyName = s.Key,
                    ErrorMessage = s.Value
                });
                throw new ValidationException(errors);      
            }
            return await next();    
        }
    }
}
