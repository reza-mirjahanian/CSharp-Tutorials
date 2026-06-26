### Part 1: Tips 1-33

**1. Returning Empty Collections**
Returning `new List<T>()` or `new T[0]` allocates memory on the heap every time, causing Garbage Collector (GC) pauses.
*Solution:* Use `Array.Empty<T>()` for arrays and `Enumerable.Empty<T>()` for IEnumerables to guarantee a single allocation across the application's lifetime.
```csharp
public IEnumerable<string> GetNames() => Enumerable.Empty<string>();
public int[] GetNumbers() => Array.Empty<int>();
```

**2. Rethrowing Exceptions Correctly**
Using `throw ex;` destroys the original stack trace, making debugging difficult.
*Solution:* Use the `throw;` keyword alone to preserve the full stack trace.
```csharp
try { /* code */ }
catch (Exception ex) 
{
    _logger.Error(ex);
    throw; // Correct: preserves stack trace
}
```

**3. Async Locking with SemaphoreSlim**
The `lock` keyword cannot be used with `await`. 
*Solution:* Use `SemaphoreSlim(1, 1)` to allow only one thread access. Always call `Release()` in a `finally` block to prevent deadlocks.
```csharp
private static readonly SemaphoreSlim _semaphore = new(1, 1);

public async Task DoWorkAsync()
{
    await _semaphore.WaitAsync();
    try { /* critical section */ }
    finally { _semaphore.Release(); }
}
```

**4. LINQ Multiple Enumeration Trap**
Calling multiple LINQ methods (like `Count()` and `All()`) on an `IEnumerable` evaluates the collection multiple times, duplicating work and IO calls.
*Solution:* Materialize the enumerable into a `List` or `Array` first.
```csharp
var items = enumerable.ToList(); // Enumerates once
var count = items.Count;
var allValid = items.All(x => x.IsValid);
```

**5. C# REPL (Read-Eval-Print Loop)**
Use `dotnet-repl` (or `csi`) to run C# directly in the terminal with full IntelliSense and NuGet support without creating a project.
```bash
dotnet tool install -g dotnet-repl
dotnet repl
```

**6. Accessing List's Internal Span**
`List<T>` is backed by an array, but it's inaccessible. 
*Solution:* Use `CollectionsMarshal.AsSpan()` to grab the internal array as a `Span<T>`. *Warning:* This is unsafe; mutating the list while iterating the span won't throw an exception.
```csharp
List<int> list = new() { 1, 2, 3 };
Span<int> span = CollectionsMarshal.AsSpan(list);
```

**7. Logging Message Templates**
Using string interpolation (`$""`) in loggers wastes memory and loses parameter filtering capabilities.
*Solution:* Use named templates in the message and pass parameters separately.
```csharp
// Wrong: _logger.LogInformation($"User {userId} logged in");
// Right:
_logger.LogInformation("User {UserId} logged in", userId);
```

**8. Empty Types in C# 12**
C# 12 allows omitting curly brackets for empty types (classes, structs, interfaces) by using a semicolon.
```csharp
public class MyEmptyClass;
public interface IMarkerInterface;
```

**9. ToList() vs ToArray() Performance**
*   **Functionality:** Return `List<T>` if the consumer mutates the length. Return `Array` if the consumer only enumerates or mutates existing values.
*   **Performance:** `ToList` is slightly faster for 10,000 items, but .NET 9 brings heavy optimizations making `ToArray()` much faster than `ToList()`.

**10. Assembly Marker Interfaces**
For DI registration in libraries (like MediatR), instead of using `Program.cs` as a marker, use an empty interface named after the assembly.
```csharp
public interface IMyProjectAssemblyMarker { }
// Usage: services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<IMyProjectAssemblyMarker>());
```

**11. StringSyntax Attribute (.NET 7)**
Decorate string parameters with `[StringSyntax]` to tell the IDE what the string represents (Regex, JSON, Date, etc.), enabling syntax highlighting.
```csharp
public void ProcessRegex([StringSyntax(StringSyntaxAttribute.Regex)] string pattern)
```

**12. Primary Constructors Limitations**
C# 12 Primary Constructors don't support `readonly` fields. If you inject a service that must be immutable, you cannot use primary constructors effectively without losing the `readonly` guarantee.

**13. Guid v7 in .NET 9**
Standard `Guid` (v4) is completely random, causing database index fragmentation. 
*Solution:* .NET 9 introduces `Guid.CreateVersion7()`, which embeds time-related data, making it sortable and preventing fragmentation.

**14. Smallest Valid C# Program**
Since C# 9 (Top-Level Statements), the smallest valid program is just a semicolon. No `Main` method or class required.
```csharp
;
```

**15. ASP.NET Core Cancellation Tokens**
Avoid creating your own `CancellationToken`. Add a `CancellationToken` parameter to your endpoint, and the framework will automatically tie it to the request lifecycle.
```csharp
app.MapGet("/api", async (CancellationToken ct) => await DoWork(ct));
```

**16. Collection Expressions (C# 12)**
Replace verbose collection initialization with `[]`.
```csharp
int[] arr = [1, 2, 3];
List<string> list = ["a", "b"];
```

**17. Checking Outdated NuGet Packages**
Use the `dotnet-outdated` global CLI tool to find and upgrade outdated packages. Supports version locking to avoid accidental upgrades to paid versions (e.g., FluentAssertions v8).
```bash
dotnet tool install -g dotnet-outdated
dotnet outdated --upgrade
```

**18. Waffle Generator for Fake Text**
Lorem Ipsum is unrealistic. Use the `WaffleGenerator` NuGet package to generate customizable, realistic-looking text in HTML or Markdown. Integrates with `Bogus`.

**19. WebApplication Pipeline Methods**
*   `Run`: Adds a terminating middleware (short-circuits the pipeline).
*   `Use`: Adds general middleware.
*   `Map`: Branches the pipeline based on a request path.
*Note:* Order of execution is strictly enforced by registration order.

**20. Naughty Strings Validation**
Use the `NaughtyStrings` NuGet package to test your app against a list of strings known to cause crashes or expose security vulnerabilities. Best used in QA and end-to-end tests.

**21. Interpolated Parser (Reverse String Interpolation)**
Avoid writing Regex for simple string parsing. Use the `InterpolatedParser` NuGet package to extract variables using string interpolation syntax.
```csharp
var input = "John is 30";
var result = InterpolatedParser.Parse(input, $"{name} is {age}");
// result.name = "John", result.age = "30"
```

**22. Alias Any Type (C# 12)**
Use the `using` directive to alias any type, resolving conflicts, simplifying long names, and defining shared value tuples.
```csharp
using Point = (int X, int Y);
Point p = (10, 20);
```

**23. DateTimeOffset over DateTime**
`DateTime` is ambiguous without `Utc` kind. `DateTimeOffset` includes the UTC offset, representing an exact moment in time, which is crucial for business applications across time zones.

**24. Architecture Tests**
Enforce namespace and class rules without making dozens of projects. Use the `NetArchTest.Rules` NuGet package to write fluent assertions verifying architectural policies (e.g., infrastructure cannot reference UI classes).

**25. FluentAssertions Alternatives**
To avoid the $130/dev/year fee for FluentAssertions v8+:
1. Stay on v7 (feature-complete).
2. Use `AwesomeAssertions` (drop-in fork).
3. Use `Shouldly` (similar functionality, different syntax).

**26. JSON Schema Exporter (.NET 9)**
`System.Text.Json` in .NET 9 can export JSON schemas natively using `JsonSerializerOptions.GetJsonSchemaNode()`. This powers the new OpenAPI functionality.

**27. Parallel.ForEachAsync (.NET 6)**
Using `async` lambdas inside `Parallel.ForEach` creates `async void` (dangerous). 
*Solution:* Use `Parallel.ForEachAsync` for safe, concurrent asynchronous loops.
```csharp
await Parallel.ForEachAsync(items, async (item, ct) => { await ProcessAsync(item); });
```

**28. Retrying Flaky Tests**
Use the `dotnet-retest` global CLI tool to automatically retry failing tests a set number of times. *(Note: Fixing the flaky tests is the actual recommended approach).*

**29. Params with Collections (C# 13)**
The `params` keyword is no longer limited to arrays. It now supports `List<T>`, `IEnumerable<T>`, and `Span<T>`, making it heavily optimized and memory-efficient.
```csharp
public void Process(params Span<int> numbers) { /* ... */ }
```

**30. Prank: Custom Monitor Class**
Creating a class named `Monitor` in the `System.Threading` namespace with `Enter` and `Exit` methods will cause the C# compiler to use your class instead of the native .NET `Monitor` class when using the `lock` keyword. *(Joke tip, do not use in production).*

**31. Private Field Underscore Convention**
The `_` prefix for private fields defines scope, instantly distinguishing class-level variables from method-level local variables without needing to scroll or use `this.`.

**32. HttpClient Best Practices**
*   `new HttpClient()`: Causes socket exhaustion.
*   Static `HttpClient`: Fails to respect DNS changes.
*   *Solution:* Use `IHttpClientFactory` or long-lived clients with `PooledConnectionLifetime` configured.

**33. Snapshot Testing with Verify**
Use the `Verify` testing framework to validate outcomes (JSON, UI, text) by comparing them against a verified baseline file. Great for introducing tests to legacy codebases non-invasively.

---

### Part 2: Tips 34-67

**34. Refit for API Calls**
Avoid writing boilerplate HTTP client code. Use `Refit` to define an interface, and the library generates the implementation.
```csharp
public interface IMyApi
{
    [Get("/users/{id}")]
    Task<User> GetUserAsync(int id);
}
```

**35. Async Locking Timeout Safety**
When using `SemaphoreSlim` for async locking, always add a timeout to `WaitAsync` to prevent accidental deadlocks.
```csharp
if (await _semaphore.WaitAsync(TimeSpan.FromSeconds(5)))
{
    try { /* code */ }
    finally { _semaphore.Release(); }
}
```

**36. ULIDs for Sortable IDs**
If you aren't on .NET 9 for Guid v7, use the `Ulid` NuGet package. ULIDs are sortable, randomly generated IDs ideal for distributed systems. Can be converted back to `Guid`.
```csharp
var id = Ulid.NewUlid();
```

**37. Task.WhenAll for Parallel Async Operations**
Do not `await` independent async methods serially. Execute them in parallel using `Task.WhenAll`.
```csharp
var t1 = GetDataAsync();
var t2 = GetOtherDataAsync();
await Task.WhenAll(t1, t2);
var data1 = t1.Result; // Safe to access .Result after WhenAll completes
```

**38. Custom Dependency Injection Scopes**
The scoped DI lifetime isn't just for HTTP requests. Inject `IServiceScopeFactory` to create custom scopes manually (e.g., for background message processing). Always dispose the scope.
```csharp
using var scope = _serviceScopeFactory.CreateScope();
var service = scope.ServiceProvider.GetRequiredService<IMyScopedService>();
```

**39. Primary Constructors Drawbacks**
Aside from lacking `readonly` support, mixing primary constructor parameters (which look like method variables) with the standard `_` naming convention for fields feels semantically wrong and can reduce readability.

**40. UnitsNet Library**
Working with unit conversions is tricky. Use the `UnitsNet` NuGet package for explicit/implicit conversions between meters, feet, RPM, torque, etc.
```csharp
Length distance = Length.FromMeters(10);
double feet = distance.Feet;
```

**41. Validate DI at Build Time**
DI errors usually occur at runtime. Set `ValidateScopes = true` and `ValidateOnBuild = true` in `ServiceProviderOptions` to fail fast during application startup/build.
```csharp
builder.Host.UseDefaultServiceProvider(o => 
{
    o.ValidateScopes = true;
    o.ValidateOnBuild = true;
});
```

**42. Null Conditional Assignment (C# 14 Preview)**
Currently, `?.` allows safe reading but not writing. C# 14 introduces null-conditional assignment.
```csharp
obj?.Property = value; // Assigns only if obj is not null
```

**43. Dictionary Initialization Semantics**
*   C# 3 `Add` syntax: `new Dictionary<int, string> { {1, "a"} }` throws an exception on duplicate keys.
*   C# 6 Index syntax: `new Dictionary<int, string> { [1] = "a" }` silently overwrites duplicate keys.

**44. Records vs Classes with Primary Constructors**
*   **Records:** Create public, immutable, init-only properties with value-based equality.
*   **Classes:** Parameters are just in scope for the class body. No public property is auto-created. The compiler creates a hidden field if used in a method.

**45. Ref Structs**
`ref struct` (like `Span<T>`) must stay on the stack. They offer blazing-fast, zero-allocation memory access.
*Rules:* Cannot be boxed, used in async methods, or stored in fields of regular classes.

**46. The `in` Keyword for Structs**
Passes structs by reference (avoiding expensive copies) but makes them read-only.
*Catch:* If the struct isn't `readonly`, accessing members creates hidden defensive copies.
*Best Practice:* Combine `in` with `readonly struct`.
```csharp
public void Process(in ReadOnlyStruct data) { /* ... */ }
```

**47. LINQ Deferred Execution**
LINQ methods (`Where`, `Select`) build a query blueprint, not the result. If source data changes before execution, the output changes. C# compiles it into a single optimized operation upon enumeration.

**48. Stackalloc for Stack Memory**
Use `stackalloc` to allocate arrays on the stack, avoiding GC pressure. Memory is discarded when the method ends.
*Catch:* Stack space is limited; large allocations cause `StackOverflowException`.
```csharp
Span<int> buffer = stackalloc int[5];
```

**49. Built-in Delegates**
*   `Func<T, TResult>`: Returns a value (last parameter is return type).
*   `Action<T>`: Returns void.
*   `Predicate<T>`: Returns bool (prefer `Func<T, bool>` nowadays).
*   *Multicast delegates:* Multiple delegates chained to invoke sequentially.

**50. Overriding Base Class Behavior**
*   `abstract`: Must override, no base logic.
*   `virtual`: Has base logic, can override. Overriding methods are virtual unless sealed.
*   `new`: Method hiding. Replaces method only when accessed via derived type; base reference calls original.
*   Interface methods: Implicitly virtual; C# 8+ supports default implementations.

**51. ArrayPool for Reusing Arrays**
Avoid allocations by renting and returning arrays using `ArrayPool<T>.Shared`. Note: rented arrays may be larger than requested.
```csharp
int[] buffer = ArrayPool<int>.Shared.Rent(1024);
try { /* use buffer */ }
finally { ArrayPool<int>.Shared.Return(buffer, clearArray: true); }
```

**52. Avoid Async Void**
`async void` can't be awaited, has no error handling, and crashes the app on exception.
*Exception:* Event handlers (since they don't return Task). Always handle exceptions inside `async void` methods.

**53. Null Forgiving Operator (`!`)**
The `!` operator tells the compiler to relax, as you guarantee a non-nullable property will be initialized by the framework/constructor later.
```csharp
public string Name { get; set; } = null!;
```

**54. Using Declarations**
Since C# 8, `using` doesn't need braces. It calls `Dispose()` at the end of the current scope. Use `await using` for `IAsyncDisposable`.
```csharp
using var stream = new FileStream("path");
await using var db = new DbContext();
```

**55. `with` Keyword for Cloning**
Clones an object and modifies specific properties without touching the original. Works with Records (C# 9) and Structs (C# 10).
```csharp
var updatedPerson = originalPerson with { Age = 30 };
```

**56. Extension Members (C# 14 Preview)**
Beyond extension methods, C# 14 allows adding static methods, instance methods, and properties to existing types without modifying the source code.

**57. Collection Expressions Spread Operator**
Combine collections inline using the spread operator `..`.
```csharp
int[] part1 = [1, 2];
int[] combined = [.. part1, 3, 4]; // [1, 2, 3, 4]
```

**58. Params Span Support (C# 13)**
Using `params Span<T>` completely eliminates heap allocations because `Span` is stack-allocated, making variable argument methods highly performant.

**59. Target-Typed New (C# 9)**
Omit the type name on the right side if the compiler can infer it from the left.
```csharp
Person p = new();
Dictionary<int, string> dict = new();
```

**60. Top-Level Statements (C# 9)**
No `Main` method or class boilerplate required. Just write code in `Program.cs`. The compiler wraps it automatically. Supports `await`. Only one top-level file is allowed per project.

**61. Pattern Matching (not, and, or)**
Write expressive, declarative conditions without nested ifs.
```csharp
if (status is not null and ("Active" or "Pending")) { /* ... */ }
```

**62. `nameof` Keyword**
Returns the string name of a symbol at compile time. Prevents bugs when refactoring variable names.
```csharp
if (user == null) throw new ArgumentNullException(nameof(user));
```

**63. Custom Deconstruct Methods**
Add a `Deconstruct` method to any class or struct to enable tuple deconstruction.
```csharp
public class Point { public int X; public int Y;
    public void Deconstruct(out int x, out int y) => (x, y) = (X, Y);
}
var (x, y) = new Point();
```

**64. Attributes on Lambda Expressions (C# 10)**
Apply attributes to lambdas for metadata, useful for source generators, middleware, and analyzers.
```csharp
var handler = [Obsolete] (int x) => x * 2;
```

**65. Relational Pattern Matching**
Check ranges cleanly without `x > 0 && x < 100`.
```csharp
if (age is > 0 and < 100) { /* ... */ }
```

**66. ArgumentNullException.ThrowIfNull (C# 10)**
One-line null check that automatically uses `nameof()`.
```csharp
ArgumentNullException.ThrowIfNull(input);
```

**67. Expression-Bodied Constructors**
Simplify constructors for small types that just assign values.
```csharp
public class MyClass(int x) => Value = x;
```

---

### Part 3: Tips 68-100

**68. ValueTuple over Tuple**
`(int A, string B)` is a `ValueTuple` (struct, stack-allocated, named fields). `Tuple<T1, T2>` is a class (heap-allocated). Always prefer `ValueTuple`.

**69. C# Keyword Puns**
You can legally name variables using contextual keywords like `in`, `out`, `short`, `try`, `catch`, `double`, `long`, `object`, `break`, `event`, `public`, and `protected`. *(Humorous observation).*

**70. Passing `ref` to `in` Parameters**
`in` means "readonly by ref". You can pass a `ref` variable to an `in` parameter (compiler treats it as readonly). However, you cannot pass an `in` parameter to a `ref` method (as `ref` implies write access).

**71. Tuple Equality**
Tuples are compared element-by-element in order. Names don't affect equality, only values.
```csharp
(int X, int Y) a = (1, 2);
(int A, int B) b = (1, 2);
a == b; // true
```

**72. Caller Information Attributes**
Inject metadata at compile time without reflection overhead.
*   `[CallerMemberName]`: Method/property name.
*   `[CallerFilePath]`: File path.
*   `[CallerLineNumber]`: Line number.
```csharp
void Log(string msg, [CallerMemberName] string caller = "") { /* ... */ }
```

**73. Nullability Flow Attributes**
Tell the compiler about null states to remove warnings.
*   `[NotNull]`: Param won't be null after method runs.
*   `[DoesNotReturn]`: Method never returns (e.g., throws).
*   `[MaybeNullWhen(bool)]`: Might return null based on a boolean.

**74. Proper Usage of `var`**
Use `var` when the type is obvious (`var name = "John"` or `var p = new Person()`). Avoid `var` when it hides meaning (`var data = GetResult()` makes debugging hard).

**75. Avoid `dynamic`**
`dynamic` bypasses compile-time checking, loses refactor safety, and uses the slow Dynamic Language Runtime (DLR). Only use it for COM interop, JSON glue code, or scripting engines. Use strong typing otherwise.

**76. Exceptions are for Exceptional Cases**
Don't use exceptions for normal logic (like checking if a user exists). Exceptions create stack traces, hit the GC, and slow the app. Return a result/boolean instead.

**77. Never `async void` in Unit Tests**
Test runners can't track `async void`. The test will complete before the code finishes, or miss exceptions. Always return `Task`.

**78. Avoid `#region`**
Regions hide messy code instead of fixing it. If a class needs regions to be readable, it violates the Single Responsibility Principle. Break the class into smaller components.

**79. Proper Use of `[Obsolete]`**
Don't use `[Obsolete]` if you aren't actually planning to remove the method. It just generates warning noise that developers learn to ignore. Use it only when there is a clear replacement and a removal schedule.

**80. Running Single C# Files**
Run a `.cs` file directly without a project file.
```bash
dotnet run app.cs
```
Add directives at the top of the file for SDKs and packages:
```csharp
#:sdk Microsoft.NET.Sdk.Web
#:package Newtonsoft.Json
```

**81. Global Usings (C# 10)**
Move common `using` statements into a single file (e.g., `GlobalUsings.cs`) using the `global` modifier.
```csharp
global using System.Linq;
global using System.Collections.Generic;
```

**82. `nameof` vs Reflection**
`nameof` is resolved at compile time with zero runtime cost. `GetType().Name` and reflection run at runtime, cost performance, and can break AOT (Ahead-of-Time) compilation.

**83. `[InternalsVisibleTo]`**
Expose `internal` classes to test projects without making them `public`.
```csharp
[assembly: InternalsVisibleTo("MyProject.Tests")]
```

**84. Exception Filters (`when`)**
Filter exceptions cleanly without filling the catch block with `if` statements.
```csharp
catch (HttpException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
{
    // Only catches 404s
}
```

**85. `where T : not null` Constraint**
Ensures a generic type cannot be nullable. Works for reference types and non-nullable value types, but rejects `string?` or `int?`. Great for Dictionary keys and IDs.

**86. `scoped` Keyword (C# 12)**
Enforces that a parameter or local variable does not outlive the caller. Prevents heap capture, essential for stack-only types like `Span<T>`.
```csharp
void Process(scoped Span<int> data) { /* ... */ }
```

**87. Destructors (Finalizers)**
Finalizers run on a background thread whenever the GC decides. You don't control when they run, and they slow down GC. Prefer `IDisposable`. Only use finalizers as a safety net for unmanaged resources.

**88. Property Patterns**
Match deep inside objects cleanly without nested null checks.
```csharp
if (order is { Customer: { IsVip: true } })
{
    // Apply discount
}
```

**89. List Patterns (C# 11)**
Match arrays and lists by shape.
```csharp
if (arr is [1, 2, 3]) { /* exact match */ }
if (arr is [1, .., 5]) { /* starts with 1, ends with 5 */ }
```

**90. `sealed override`**
Stops further overriding of a method in the inheritance chain without sealing the entire class. Useful for framework code to prevent unexpected behavior.

**91. TryParse Pattern**
Avoid throwing exceptions for parsing. Use `TryParse`. Implement this pattern in your own complex parsing methods.
```csharp
if (int.TryParse(input, out int result)) { /* ... */ }
```

**92. C# Preview Features**
Enable upcoming C# features by setting `<LangVersion>preview</LangVersion>` in your `.csproj` file. Great for testing features like extension members before release.

**93. Index From End (`^`)**
Cleanly access elements from the end of a collection.
```csharp
var last = arr[^1];
var secondToLast = arr[^2];
```

**94. Empty Array Literal (`[]`)**
`[]` is fully optimized to reuse the same static instance as `Array.Empty<T>()`. Clean and performant.
```csharp
int[] empty = [];
```

**95. `await foreach` (IAsyncEnumerable)**
Consume asynchronous streams (like paginated APIs or file streams) as they arrive. No buffering or blocking.
```csharp
await foreach (var item in GetStreamAsync())
{
    Console.WriteLine(item);
}
```

**96. `checked` Keyword for Overflows**
By default, integer overflows wrap around silently. Use `checked` to force a runtime `OverflowException`.
```csharp
checked
{
    int max = int.MaxValue;
    int overflow = max + 1; // Throws
}
```

**97. Anonymous Type Equality**
Two anonymous objects with the same properties/values have `Equals()` return `true` (value equality), but `==` returns `false` (reference equality).
```csharp
var a = new { Id = 1 };
var b = new { Id = 1 };
a.Equals(b); // true
a == b; // false
```

**98. `Task.Yield()`**
Tells the runtime to pause here and resume later, yielding control back to the caller. Not a delay; just a context switch. Useful for breaking up synchronous chunks or returning to a UI thread.

**99. `[MethodImpl(MethodImplOptions.AggressiveInlining)]`**
Nudges the JIT compiler to inline a method (replacing the call with the method body) to remove call overhead. Good for micro-optimizations in hot paths, but don't overuse on large methods.

**100. Source-Generated Regex (.NET 7)**
Standard `new Regex()` compiles at runtime, costing memory and startup time. Source generators compile it at build time.
```csharp
[GeneratedRegex(@"\b\d{3}-\d{2}-\d{4}\b")]
private static partial Regex SsnRegex();
```