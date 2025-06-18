using FluentValidation;
using MediatR;

namespace GymManager.Application.Common.Behaviors;

public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse> where TRequest : notnull
{
	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		// sprawdzenie czy jest jakiś walidator
		if (validators.Any())
		{
			// pobranie kontekstu walidacji
			ValidationContext<TRequest> context = new(request);

			// walidacja kontekstu
			var validationResults = await Task.WhenAll(validators.Select(x => x.ValidateAsync(context, cancellationToken)));

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