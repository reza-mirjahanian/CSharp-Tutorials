## Why MediatR is Unnecessary in Modern .NET

MediatR is frequently used in .NET applications to implement the Command Query Responsibility Segregation (CQRS) pattern and handle cross-cutting concerns (like logging and validation) via its pipeline behaviors. However, ASP.NET Core contains built-in dependency injection (DI) capabilities that can fully replicate MediatR's features without adding an external package dependency.

Eliminating MediatR removes an opaque runtime abstraction, allowing developers to step directly into handler code during debugging without navigating hidden source-generated or reflective plumbing.

---

## Replicating Pipeline Behaviors via DI Decoration

The primary argument for using MediatR is its pipeline behavior pattern, which intercepts requests to execute cross-cutting concerns. This exact behavior can be achieved using native **Dependency Injection-based decoration**.

By wrapping a core handler inside decorator classes, you create a compile-time safe pipeline where cross-cutting concerns execute sequentially before or after the core logic.

### 1. Defining the Core Abstractions

To implement a native mediator pattern, define a generic interface for your request handlers:

```csharp
public interface IRequestHandler<in TRequest, TResponse>
{
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken);
}

```

### 2. Creating Decorators for Cross-Cutting Concerns

Decorators implement the same `IRequestHandler<TRequest, TResponse>` interface but accept an inner instance of the handler via their constructor.

#### Logging Decorator Example

This decorator wraps the inner handler, logging details before and after execution:

```csharp
public class LoggingRequestHandlerDecorator<TRequest, TResponse> 
    : IRequestHandler<TRequest, TResponse>
{
    private readonly IRequestHandler<TRequest, TResponse> _inner;
    private readonly ILogger<LoggingRequestHandlerDecorator<TRequest, TResponse>> _logger;

    public LoggingRequestHandlerDecorator(
        IRequestHandler<TRequest, TResponse> inner, 
        ILogger<LoggingRequestHandlerDecorator<TRequest, TResponse>> logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public async Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling request {RequestName}", typeof(TRequest).Name);
        try
        {
            var response = await _inner.HandleAsync(request, cancellationToken);
            _logger.LogInformation("Successfully handled {RequestName}", typeof(TRequest).Name);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed handling {RequestName}", typeof(TRequest).Name);
            throw;
        }
    }
}

```

#### Validation Decorator Example

This decorator intercepts the request to run fluent validation rules before allowing the core handler to execute:

```csharp
public class ValidationRequestHandlerDecorator<TRequest, TResponse> 
    : IRequestHandler<TRequest, TResponse>
{
    private readonly IRequestHandler<TRequest, TResponse> _inner;
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationRequestHandlerDecorator(
        IRequestHandler<TRequest, TResponse> inner, 
        IEnumerable<IValidator<TRequest>> validators)
    {
        _inner = inner;
        _validators = validators;
    }

    public async Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
        var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();

        if (failures.Any())
        {
            throw new ValidationException(failures);
        }

        return await _inner.HandleAsync(request, cancellationToken);
    }
}

```

---

## Native Dependency Injection Registration

To construct the execution pipeline, you register the core handler and manually wrap it within the desired decorators inside your `Program.cs` file. This eliminates MediatR's magic resolution and gives you explicit control over which handlers get specific behaviors.

### Manual Registration Method

You can leverage the built-in `ActivatorUtilities` class to cleanly resolve dependencies for nested decorators without losing transient or scoped lifetime settings:

```csharp
public static class RequestHandlerRegistrationExtensions
{
    public static IServiceCollection AddDecoratedRequestHandler<TRequest, TResponse, THandler>(
        this IServiceCollection services)
        where THandler : class, IRequestHandler<TRequest, TResponse>
    {
        // 1. Register the core concrete handler as transient
        services.AddTransient<THandler>();

        // 2. Register the interface to resolve the decorated chain
        services.AddTransient<IRequestHandler<TRequest, TResponse>>(provider =>
        {
            // Instantiate the base concrete handler
            var coreHandler = provider.GetRequiredService<THandler>();

            // Wrap with the Validation Decorator first
            var validationDecorator = ActivatorUtilities.CreateInstance<ValidationRequestHandlerDecorator<TRequest, TResponse>>(
                provider, coreHandler);

            // Wrap the Validation Decorator inside the Logging Decorator
            var loggingDecorator = ActivatorUtilities.CreateInstance<LoggingRequestHandlerDecorator<TRequest, TResponse>>(
                provider, validationDecorator);

            return loggingDecorator; // Returns the fully constructed pipeline
        });

        return services;
    }
}

```

### Usage in Program.cs

When registering your application's commands and queries, call the extension method explicitly:

```csharp
builder.Services.AddDecoratedRequestHandler<CreateProductCommand, ProductResponse, CreateProductHandler>();
builder.Services.AddDecoratedRequestHandler<GetProductByIdQuery, ProductResponse, GetProductByIdHandler>();

```

> 
> **Alternative Approach:** If manually registering every individual handler becomes tedious, you can utilize reflection to automatically scan assemblies and register these open generic decorators, or use a lightweight utility library like **Scrutor** to simplify the native registration process (`services.Decorate(typeof(IRequestHandler<,>), typeof(LoggingRequestHandlerDecorator<,>))`).
> 
> 

---

## Consumption in Minimal APIs / Controllers

The consumption layer remains clean and decoupled. Instead of injecting an opaque `IMediator` instance, your Minimal API endpoints or Controllers inject the specific interface directly.

```csharp
app.MapPost("/products", async (
    CreateProductCommand command, 
    IRequestHandler<CreateProductCommand, ProductResponse> handler,
    CancellationToken ct) =>
{
    // Execution flows through: Logging -> Validation -> CreateProductHandler
    var result = await handler.HandleAsync(command, ct);
    return Results.Created($"/products/{result.Id}", result);
});

```

This pattern keeps core handlers entirely isolated from cross-cutting concerns, adheres perfectly to the Single Responsibility Principle, and retains clean CQRS design boundaries without MediatR overhead.

