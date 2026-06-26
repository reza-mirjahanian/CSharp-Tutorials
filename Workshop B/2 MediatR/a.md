# Replacing Mediator in ASP.NET Core: Built-in DI Decoration

## The Core Problem

The Mediator NuGet package gives you two main things in C#/.NET APIs:

1. **A request/handler pattern** — dispatch a request to a handler that returns a response (the CQRS-style flow)
2. **Pipeline behaviors** — wrap handlers with cross-cutting concerns like logging and validation

Both can be done with **plain ASP.NET Core DI** using a pattern called **DI decoration**. No package required.

---

## Part 1: The "Mediator Way" (What You're Replacing)

### Request + Handler

```csharp
// A query
public record GetProductByIdQuery(Guid Id) : IRequest<Product>;

// Its handler
public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Product>
{
    public Task<Product> Handle(GetProductByIdQuery query, CancellationToken ct)
    {
        // fetch product...
        return Task.FromResult(product);
    }
}
```

### Pipeline Behavior (the part people love)

```csharp
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
    {
        // log "before"
        var response = await next();
        // log "after"
        return response;
    }
}

public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    // grab IValidator<TRequest>, run it, throw if invalid...
}
```

Registration:

```csharp
services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<Program>();
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});
```

**The pain point:** request goes through a hidden dispatcher. You can't step into it easily, and there's a runtime/indirection cost.

---

## Part 2: The Built-in Way — DI Decoration

The trick: **register the handler as transient, then re-register it wrapped in a decorator.** Every time DI resolves the handler, you get the decorated version.

### Step 1 — Keep your handler shape

```csharp
public interface IRequestHandler<TRequest, TResponse>
{
    Task<TResponse> Handle(TRequest request, CancellationToken ct);
}

public record GetProductByIdQuery(Guid Id);
public record Product(Guid Id, string Name);

public class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, Product>
{
    public Task<Product> Handle(GetProductByIdQuery query, CancellationToken ct)
        => Task.FromResult(new Product(query.Id, "Sample"));
}
```

The handler is identical to before. **It doesn't know logging or validation exist.**

### Step 2 — Write decorators that implement the same interface

```csharp
public class LoggingDecorator<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
{
    private readonly IRequestHandler<TRequest, TResponse> _inner;
    private readonly ILogger<LoggingDecorator<TRequest, TResponse>> _logger;

    public LoggingDecorator(
        IRequestHandler<TRequest, TResponse> inner,
        ILogger<LoggingDecorator<TRequest, TResponse>> logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken ct)
    {
        _logger.LogInformation("Handling {Request}", typeof(TRequest).Name);
        try
        {
            var result = await _inner.Handle(request, ct);
            _logger.LogInformation("Handled {Request}", typeof(TRequest).Name);
            return result;
        }
        catch
        {
            _logger.LogError("Failed {Request}", typeof(TRequest).Name);
            throw;
        }
    }
}
```

```csharp
public class ValidationDecorator<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
{
    private readonly IRequestHandler<TRequest, TResponse> _inner;
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationDecorator(
        IRequestHandler<TRequest, TResponse> inner,
        IEnumerable<IValidator<TRequest>> validators)
    {
        _inner = inner;
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken ct)
    {
        var failures = _validators
            .Select(v => v.Validate(request))
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (failures.Count > 0)
            throw new ValidationException(failures);

        return await _inner.Handle(request, ct);
    }
}
```

### Step 3 — Register with decoration

```csharp
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRequestHandler<TRequest, TResponse, THandler>(
        this IServiceCollection services)
        where THandler : class, IRequestHandler<TRequest, TResponse>
    {
        // 1. Register the concrete handler as transient
        services.AddTransient<THandler>();

        // 2. Re-register the interface as the decorator wrapping the inner handler
        services.AddTransient<IRequestHandler<TRequest, TResponse>>(sp =>
        {
            var inner = sp.GetRequiredService<THandler>();
            var logger = sp.GetRequiredService<ILogger<LoggingDecorator<TRequest, TResponse>>>();

            var withLogging = new LoggingDecorator<TRequest, TResponse>(inner, logger);
            return new ValidationDecorator<TRequest, TResponse>(withLogging, /* validators */ null!);
        });

        return services;
    }
}
```

In `Program.cs`:

```csharp
builder.Services.AddRequestHandler<GetProductByIdQuery, Product, GetProductByIdHandler>();
```

That's it. Every `IRequestHandler<GetProductByIdQuery, Product>` resolution returns: **Validation → Logging → Real handler**.

### Step 4 — Use it from an endpoint (no mediator reference)

```csharp
app.MapGet("/products/{id:guid}", async (
    Guid id,
    IRequestHandler<GetProductByIdQuery, Product> handler) =>
{
    return await handler.Handle(new GetProductByIdQuery(id), default);
});
```

You still get the same CQRS shape (request + handler), you still get logging + validation cross-cutting, but **no package and no hidden dispatcher.**

---

## Part 3: Mental Model

Think of decoration like Russian nesting dolls:

```
Incoming request
     ↓
ValidationDecorator     ← checks request first
     ↓
LoggingDecorator        ← logs timing, success/failure
     ↓
GetProductByIdHandler   ← actual business logic
```

Each layer **implements the same interface** and **takes the next layer as a constructor dependency**. DI chains them for you.

---

## Part 4: Why This Is Equivalent to Mediator's Pipeline

| Mediator pipeline | DI decoration |
|---|---|
| Open behavior over all `IRequestHandler<,>` | Open generic decorator over all `IRequestHandler<,>` |
| Ordered: behavior registered first runs first | Order = order of wrapping in the factory |
| Reflection/dispatcher at runtime | Direct constructor injection |
| Hidden call site | Visible call site — just resolve the handler |
| NuGet dependency | Zero |

The handlers themselves are **byte-for-byte identical.** Only registration changes.

---

## Part 5: Variants and Trade-offs

**Apply decoration selectively per request**

```csharp
services.AddRequestHandler<GetProductByIdQuery, Product, GetProductByIdHandler>();      // decorated
services.AddRequestHandler<HealthCheckQuery, string, HealthCheckHandler>();              // not decorated
```

You control which handlers get which cross-cutting concerns — no global "behavior applies to everything" trap.

**Skip a layer on a specific handler**

Wrap with only `LoggingDecorator`, omit `ValidationDecorator`. Just choose the chain when you build the factory.

**Use Scrutor for less boilerplate** (optional, not required)

```csharp
services.AddTransient<IRequestHandler<GetProductByIdQuery, Product>, GetProductByIdHandler>();
services.Decorate<IRequestHandler<GetProductByIdQuery, Product>, LoggingDecorator<GetProductByIdQuery, Product>>();
services.Decorate<IRequestHandler<GetProductByIdQuery, Product>, ValidationDecorator<GetProductByIdQuery, Product>>();
```

Scrutor's `Decorate` method automates the factory pattern shown above. Pure-ASP.NET-Core version does the same thing manually.

---

## Key Takeaways

- **The handler interface stays the same.** You keep CQRS, keep `IRequestHandler<TRequest, TResponse>`, keep your code.
- **Cross-cutting concerns become decorators.** Same interface, same goal, no mediator dispatch.
- **Registration is the only thing that changes.** Factory functions wrap the real handler inside one or more decorators.
- **You get full visibility.** Step into the chain in a debugger. No black-box dispatcher.
- **No package, no version risk, no extra abstraction.** Built-in DI does the work.

The whole Mediator package is a thin layer over what `IServiceCollection` already gives you for free.