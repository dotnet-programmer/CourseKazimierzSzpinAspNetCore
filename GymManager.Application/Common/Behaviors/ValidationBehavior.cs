using FluentValidation;
using MediatR;

namespace GymManager.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
	private readonly IEnumerable<IValidator<TRequest>> _validators;

	public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators) => _validators = validators;

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		if (_validators.Any())
		{
			var context = new ValidationContext<TRequest>(request);

			// walidacja kontekstu
			var validationResults = await Task.WhenAll(_validators.Select(x => x.ValidateAsync(context, cancellationToken)));

			// sprawdzanie błędów walidacji
			var failures = validationResults
				.Where(x => x.Errors.Any())
				.SelectMany(x => x.Errors)
				.ToList();

			// jeśli są jakieś błędy, to rzuć wyjątek
			if (failures.Any())
			{
				throw new Exceptions.ValidationException(failures);
			}
		}

		return await next();
	}
}