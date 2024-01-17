using EventBus.Extensions;
using FluentValidation;
using MediatR;
using OrderService.Domain.Exceptions;

namespace OrderService.Behaviors;

public class ValidatorBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<ValidatorBehavior<TRequest, TResponse>> _logger;
    private readonly IEnumerable<IValidator<TRequest>> _validattors;

    public ValidatorBehavior(ILogger<ValidatorBehavior<TRequest, TResponse>> logger, IEnumerable<IValidator<TRequest>> validattors)
    {
        _logger = logger;
        _validattors = validattors;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var typeName = request.GetGenericTypeName();

        _logger.LogInformation($"Validating command {typeName}");

        var failures = _validattors
            .Select(v => v.Validate(request))
            .SelectMany(result => result.Errors)
            .Where(error => error != null)
            .ToList();

        if (failures.Any())
        {
            _logger.LogWarning($"Validation errors - {typeName} - Command: {request} - Errors: {failures}");

            throw new OrderDomainException(
                $"Command Validation Errors for type {typeof(TRequest).Name}", 
                new ValidationException("Validation exception", failures));
        }

        return await next();
    }
}
