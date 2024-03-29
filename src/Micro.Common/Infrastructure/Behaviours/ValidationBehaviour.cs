using FluentValidation;

namespace Micro.Common.Infrastructure.Behaviours;

public class ValidationBehaviour<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators, ILogger<TRequest> logs) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var name = typeof(TRequest).FullName;

        if (!validators.Any())
        {
            logs.LogWarning("{Name} has no validators", name);
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var results = await Task.WhenAll(validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = results
            .Where(r => r.Errors.Any())
            .SelectMany(r => r.Errors)
            .ToList();

        if (!failures.Any())
        {
            logs.LogDebug("{Name} is valid", name);
            return await next();
        }

        var errors = string.Join(",", failures.Select(x => x.ErrorMessage));
        logs.LogInformation("{Name} Failed validation {@Request} {errors}", name, request, errors);
        throw new Exceptions.ValidationException(errors);
    }
}