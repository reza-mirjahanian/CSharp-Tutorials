### Part 1: Tips 1–25

**1. Returning Empty Collections**
*   **Problem:** Returning `new List<T>()` or `new T[0]` allocates memory on the heap every time, causing Garbage Collector (GC) pressure and app pauses.
*   **Solution:** Use `Array.Empty<T>()` or `Enumerable.Empty<T>()`. These guarantee the empty collection is allocated only once during the application's lifetime.

**2. Rethrowing Exceptions Correctly**
*   **Wrong:** `throw ex;` (Completely ignores and destroys the original exception stack trace).
*   **Right:** `throw;` (Rethrows the exact same exception, preserving the full, useful stack trace).

**3. Locking with Async/Await**
*   **Problem:** The `lock` keyword cannot be used with `async`/`await`.
*   **Solution:** Use `SemaphoreSlim(1, 1)`. 
*   **Implementation:** `await _semaphore.WaitAsync(); try { /* locked code */ } finally { _semaphore.Release(); }`. The `finally` block prevents deadlocks.

**4. LINQ Multiple Enumerations Trap**
*   **Problem:** Calling methods like `.Count()` and `.All()` on an `IEnumerable` forces the collection to be enumerated multiple times. This duplicates heavy workloads and can trigger multiple database I/O calls.
*   **Solution:** Materialize the collection first using `.ToList()` or `.ToArray()`, then perform operations on the materialized structure.

**5. .NET REPL (Read-Eval-Print Loop)**
*   **Tool:** Use cross-platform CLI tools like `dotnet-repl`.
*   **Benefit:** Allows you to run C# directly in the terminal with full IntelliSense, NuGet package support, and the ability to test ASP.NET Core APIs without creating a full project.

**6. Accessing the Span of a List**
*   **Problem:** Lists are backed by arrays, but the internal array is inaccessible for `Span<T>` operations.
*   **Solution:** Use `CollectionsMarshal.AsSpan(list)`.
*   **Warning:** This is unsafe. If the list mutates while you are iterating over the Span, no exception will be thrown.

**7. Structured Logging Templates**
*   **Problem:** Using string interpolation (`$"{user}"`) or concatenation in loggers wastes memory on string allocations and loses parameter metadata, making log filtering impossible.
*   **Solution:** Use named message templates: `logger.LogInformation("User {UserId} logged in", userId);`.

**8. Empty Types (C# 12)**
*   **Feature:** You no longer need empty curly brackets `{}` for empty structs, classes, or interfaces.
*   **Syntax:** Simply use a semicolon: `public class EmptyClass;`

**9. ToList() vs. ToArray()**
*   **Functionality:** Return `List` if the consumer will add/remove items. Return `Array` if they will only enumerate or mutate existing values.
*   **Performance:** Historically varied by size, but **.NET 9** introduces updates making `ToArray()` significantly faster than `ToList()`.

**10. Assembly Markers for Dependency Injection**
*   **Problem:** Libraries like MediatR use generic types to scan assemblies, often leading developers to use `Program.cs` as a marker.
*   **Solution:** Create an empty interface named after the assembly (e.g., `IAssemblyMarker`) to make DI registration explicit, readable, and unambiguous.

**11. StringSyntax Attribute (.NET 7)**
*   **Feature:** Apply `[StringSyntax(StringSyntaxAttribute.Regex)]` (or Json, Uri, etc.) to string parameters.
*   **Benefit:** The IDE will recognize the string's intent, providing appropriate syntax highlighting, validation, and IntelliSense.

**12. Primary Constructors Limitation (C# 12)**
*   **Problem:** Primary constructors currently lack support for defining `readonly` fields. 
*   **Warning:** Accepting IDE refactoring suggestions to move injected services to primary constructors may strip away immutability guarantees.

**13. Guid v7 (.NET 9)**
*   **Problem:** Standard Guids (v4) are completely random, causing index fragmentation in databases.
*   **Solution:** Use `Guid.CreateVersion7()`. It replaces the first bytes with time-related data, creating a sortable UUIDv7 natively without third-party libraries.

**14. Smallest Valid C# Program**
*   **Feature:** With C# 9 Top-Level Statements, the `public static void Main` boilerplate is gone. The smallest valid program can essentially be an empty file or a single expression/statement.

**15. Cancellation Tokens in ASP.NET Core**
*   **Best Practice:** Do not manually create cancellation tokens for HTTP requests. 
*   **Solution:** Add a `CancellationToken` parameter to your Controller or Minimal API action. The framework automatically binds it to the HTTP request lifecycle, cancelling cascading processes if the user aborts the request.

**16. Collection Expressions (C# 12)**
*   **Syntax:** Use `[1, 2, 3]` instead of `new List<int> { 1, 2, 3 }` or `new int[] { ... }`.
*   **Benefit:** The compiler automatically infers and applies the appropriate collection initializer behind the scenes for cleaner code.

**17. dotnet-outdated CLI Tool**
*   **Tool:** Install globally to run `dotnet outdated`.
*   **Benefit:** Instantly view and upgrade outdated NuGet packages from the terminal, with support for version locking to avoid accidental paid-tier upgrades.

**18. Waffle Generator vs. Lorem Ipsum**
*   **Problem:** Lorem Ipsum is unrealistic for UI testing.
*   **Solution:** Use the `WaffleGenerator` NuGet package to generate customizable, realistic-looking text (HTML, Markdown) that mimics real-world usage. Integrates well with the `Bogus` library.

**19. WebApplication Pipeline Methods**
*   **`Run`:** Adds a terminating middleware (ends the pipeline).
*   **`Use`:** Adds general middleware (can pass to the next delegate).
*   **`Map`:** Maps middleware to a specific request path.
*   *Note: The order in which these are called is critical.*

**20. Naughty Strings Validation**
*   **Concept:** "Naughty strings" are specific text inputs known to crash servers or expose security vulnerabilities.
*   **Solution:** Use the `NaughtyStrings` NuGet package in QA and End-to-End tests to ensure your application handles malicious or edge-case inputs safely.

**21. Reverse String Interpolation**
*   **Tool:** `InterpolatedParser` NuGet package.
*   **Benefit:** Allows you to extract variables out of a string by providing a template string using standard C# string interpolation syntax.

**22. Alias Any Type (C# 12)**
*   **Feature:** The `using` directive can now alias *any* type, not just namespaces or generics.
*   **Use Cases:** Simplifies long type names, resolves naming conflicts, defines shared ValueTuple types, and adds descriptive clarity to code.

**23. DateTimeOffset vs. DateTime**
*   **Problem:** `DateTime` lacks timezone context (unless explicitly UTC), leading to business logic errors.
*   **Solution:** Use `DateTimeOffset`. It includes the UTC offset, representing an exact, unambiguous moment in time regardless of the client's timezone.

**24. Architecture Tests (NetArchTest.Rules)**
*   **Concept:** Enforce architectural boundaries via tests rather than splitting code into dozens of isolated projects.
*   **Benefit:** Use fluent builders to restrict which namespaces can use certain classes, or enforce that specific types are `sealed` by default.

**25. FluentAssertions Alternatives**
*   **Context:** FluentAssertions v8+ introduced licensing fees for commercial use.
*   **Alternatives:** 
    1. Pin to v7 (feature-complete).
    2. Use **AwesomeAssertions** (a free, drop-in fork).
    3. Use **Shouldly** (a similar, long-standing assertion library).

---

### Part 2: Tips 26–50

**26. JSON Schema Exporter (.NET 9)**
*   **Feature:** `System.Text.Json` now includes `JsonSchemaExporter`.
*   **Usage:** Use `GetJsonSchemaNode` on `JsonSerializerOptions` to export JSON schemas natively (this powers .NET 9's new OpenAPI features).

**27. Parallel.ForEachAsync (.NET 6)**
*   **Problem:** Using `async` inside `Parallel.ForEach` creates `async void` methods, which are dangerous and unawaitable.
*   **Solution:** Use `Parallel.ForEachAsync` for proper, safe async/await parallel loops.

**28. dotnet-retest**
*   **Tool:** A CLI tool that automatically retries flaky tests.
*   **Caveat:** While it masks flakiness, the real solution is to fix the underlying test reliability issues.

**29. params with Span (C# 13)**
*   **Upgrade:** The `params` keyword now supports `IEnumerable`, `List`, and importantly, `Span<T>`.
*   **Benefit:** Using `params Span<T>` avoids heap allocations, making variable-argument methods highly performant.

**30. The "Monitor" Joke (Do Not Do This)**
*   **Concept:** Creating a custom class named `Monitor` inside the `System.Threading` namespace will trick the compiler into using your class instead of the native .NET locking mechanism. *(Strictly a joke; do not implement this).*

**31. Underscore Naming for Private Fields**
*   **Convention:** Use `_fieldName` for private fields.
*   **Reason:** It instantly defines scope, allowing you to distinguish class-level fields from local method variables without relying on `this.`.

**32. HttpClient Management**
*   **Problem:** `new HttpClient()` causes socket exhaustion. Static `HttpClient` fails to respect DNS changes.
*   **Solution:** Use `IHttpClientFactory` (which pools message handlers) or configure `PooledConnectionLifetime` on a long-lived client.

**33. Snapshot Testing (Verify)**
*   **Tool:** The `Verify` library.
*   **Concept:** Captures the output of an action (JSON, UI, text) and compares it against a "verified" snapshot on subsequent runs. Excellent for adding robust tests to legacy codebases.

**34. Refit**
*   **Tool:** An automatic type-safe REST library.
*   **Usage:** Define an interface with HTTP attributes (e.g., `[Get("/users")]`). Refit automatically generates the `HttpClient` implementation at runtime.

**35. Async Locking Timeouts**
*   **Tip:** When using `SemaphoreSlim` for async locking (see Tip 3), always add a timeout to `WaitAsync()` to prevent accidental deadlocks in your application.

**36. ULIDs**
*   **Concept:** Universally Unique Lexicographically Sortable Identifiers.
*   **Benefit:** An alternative to Guid v7. They are randomly generated but time-sorted, extremely fast to create, and ideal for distributed systems. Available via the `Ulid` NuGet package.

**37. Task.WhenAll**
*   **Problem:** Awaiting independent async tasks serially (`await A(); await B();`) wastes time.
*   **Solution:** Start the tasks, then await them together: `await Task.WhenAll(taskA, taskB);` for true parallel execution.

**38. Custom DI Scopes**
*   **Concept:** "Scoped" lifetimes aren't limited to HTTP requests.
*   **Usage:** Inject `IServiceScopeFactory` and call `CreateScope()` to process background messages or queue jobs, allowing you to resolve scoped services (like DB contexts) safely. Remember to dispose the scope.

**39. Primary Constructors Drawbacks**
*   **Issues:** Parameters cannot be marked `readonly` (meaning methods can alter their backing fields). Furthermore, `camelCase` parameter naming clashes with standard `_camelCase` private field conventions.

**40. UnitsNet**
*   **Tool:** A NuGet library that adds explicit and implicit conversions for physical units (e.g., meters to feet, RPM to torque, horsepower), eliminating manual math errors.

**41. DI Validation at Build Time**
*   **Configuration:** Set `ValidateScopes = true` and `ValidateOnBuild = true` on the `ServiceProvider`.
*   **Benefit:** Catches Dependency Injection misconfigurations immediately at startup/build time rather than crashing at runtime.

**42. Null Conditional Assignment (C# 14 Preview)**
*   **Feature:** Allows assigning a value only if the object is not null.
*   **Syntax:** `obj?.Property = value;`

**43. Dictionary Initialization Differences**
*   **`.Add(key, value)`:** Throws an exception if the key already exists.
*   **`[key] = value` (Index Initializer):** Silently overwrites the existing key. Choose based on whether duplicates should be fatal or ignored.

**44. Primary Constructors: Records vs. Classes**
*   **Records:** Auto-generate public, immutable properties used for value-based equality.
*   **Classes:** Parameters remain in scope for methods/initializers. The compiler creates hidden backing fields if used in methods, but *does not* auto-generate public properties.

**45. ref struct**
*   **Concept:** Structs that must strictly stay on the stack (e.g., `Span<T>`).
*   **Benefit:** Blazing fast, zero heap allocation.
*   **Restrictions:** Cannot be boxed, used in `async` methods, or stored in class fields (heap).

**46. The `in` Keyword**
*   **Usage:** Passes structs by reference but makes them read-only inside the method.
*   **Benefit:** Avoids the performance cost of copying large structs.
*   **Warning:** If the struct isn't marked `readonly`, the compiler may create hidden "defensive copies" when accessing members, negating the performance boost.

**47. LINQ Deferred Execution**
*   **Concept:** Methods like `Where` and `Select` only build a query blueprint. Execution is delayed until enumeration.
*   **Warning:** If the underlying data changes before execution, the new data will be included in the results.

**48. stackalloc**
*   **Concept:** Allocates memory directly on the stack instead of the heap.
*   **Benefit:** Zero GC overhead, incredibly fast.
*   **Warning:** Stack space is severely limited; allocating too much will cause a `StackOverflowException`.

**49. Built-in Delegates**
*   **`Func<T, TResult>`:** Returns a value.
*   **`Predicate<T>`:** Returns a `bool` (Legacy; prefer `Func<T, bool>`).
*   **`Action<T>`:** Returns `void`.
*   **Multicast Delegates:** Allow chaining multiple methods to be invoked in sequence with a single call.

**50. Overriding Base Class Behavior**
*   **`abstract`:** Must be overridden; no base logic.
*   **`virtual`:** Provides default logic but can be overridden.
*   **Interface Methods:** Implicitly virtual (C# 8+ supports default implementations).
*   **`new` Keyword:** Hides the base method (method hiding) when accessed via the derived type, but base references still call the original.

---

### Part 3: Tips 51–75

**51. ArrayPool<T>**
*   **Concept:** A memory recycling bin for arrays.
*   **Usage:** `ArrayPool<T>.Shared.Rent(size)` and `.Return(array, clearArray: true)`.
*   **Benefit:** Avoids heap allocations in high-throughput scenarios (JSON parsing, File I/O). Always return rented arrays.

**52. async void**
*   **Rule:** Avoid `async void`. It cannot be awaited, hides exceptions (crashing the app), and breaks control flow.
*   **Exception:** Only acceptable for **Event Handlers**, which inherently do not return Tasks.

**53. Null Forgiving Operator (`!`)**
*   **Usage:** `value!` tells the compiler "I guarantee this is not null at runtime."
*   **Benefit:** Silences nullable reference type warnings without altering the underlying type.

**54. `using` Statement / Declaration**
*   **Concept:** A contract that guarantees `Dispose()` is called, even if exceptions occur (translates to `try/finally`).
*   **C# 8+:** `using var x = ...;` implies the scope lasts until the end of the enclosing block.
*   **Async:** Supports `await using` for `IAsyncDisposable` types.

**55. `with` Expression**
*   **Concept:** Clones an immutable record or struct and updates specific properties.
*   **Warning:** Using `with` on a `class` (reference type) will reallocate memory on the heap.

**56. Extension Members (C# 14 Preview)**
*   **Feature:** Allows adding static/instance methods and properties to existing types without modifying their source code.
*   **Impact:** Expected to eventually replace traditional Extension Methods.

**57. Collection Expressions & Spread Operator**
*   **Syntax:** Use `[..list1, ..list2, 3]` to combine and initialize collections cleanly, similar to JavaScript/Python.

**58. params Span<T> (C# 13)**
*   **Benefit:** Passing variable arguments via `params Span<T>` ensures the data stays on the stack, completely avoiding heap allocations.

**59. Target-Typed `new` (C# 9)**
*   **Syntax:** `List<int> list = new();`
*   **Benefit:** The compiler infers the type from the left side of the assignment, reducing boilerplate.

**60. Top-Level Statements (C# 9)**
*   **Concept:** Eliminates the need for the `Program` class and `Main` method.
*   **Rule:** Only one file per project can contain top-level statements (acts as the entry point).

**61. Pattern Matching (`not`, `and`, `or`)**
*   **Syntax:** `if (x is not null and > 10 or < -10)`
*   **Benefit:** Replaces nested, confusing boolean `if` statements with declarative, readable logic.

**62. `nameof` Operator**
*   **Concept:** Returns the string name of a variable, type, or member.
*   **Benefit:** Refactoring-safe (updates automatically if renamed) and resolved at compile-time (zero runtime reflection cost).

**63. Custom `Deconstruct` Method**
*   **Feature:** You can add a `Deconstruct` method to *any* class or struct (not just records).
*   **Benefit:** Enables tuple deconstruction syntax: `var (a, b) = myCustomObject;`

**64. Attributes on Lambdas (C# 10)**
*   **Syntax:** `[MyAttribute] (x) => x + 1`
*   **Use Case:** Adds metadata to inline code, highly useful for source generators, custom analyzers, and middleware.

**65. Relational Patterns**
*   **Syntax:** `if (x is > 0 and < 100)`
*   **Benefit:** Cleanly checks ranges without writing `x > 0 && x < 100`. Works in `switch` statements as well.

**66. ArgumentNullException.ThrowIfNull (.NET 6)**
*   **Syntax:** `ArgumentNullException.ThrowIfNull(input);`
*   **Benefit:** Replaces boilerplate `if (x == null)` checks. Automatically uses `nameof` to pass the correct parameter name.

**67. Expression-Bodied Constructors**
*   **Syntax:** `public Person(string name) => Name = name;`
*   **Use Case:** Excellent for simple types where the constructor only assigns values.

**68. ValueTuples vs. Tuples**
*   **ValueTuple `(int a, string b)`:** A `struct` (stack-allocated, lightweight, supports named fields).
*   **Tuple `Tuple<T1, T2>`:** A `class` (heap-allocated, legacy, no named fields). Always prefer ValueTuples.

**69. C# Keywords Humor**
*   *(Transcript joke segment)*: Highlighting how C# keywords like `in`, `out`, `short`, `long`, `catch`, `double`, `object`, and `break` can be strung together to form grammatically correct but nonsensical English sentences.

**70. `ref` to `in` Parameter Passing**
*   **Rule:** It is legal to pass a `ref` variable into an `in` (read-only reference) parameter.
*   **Restriction:** You *cannot* pass an `in` variable to a `ref` parameter, because `ref` implies the method might write to it.

**71. Tuple Equality**
*   **Concept:** `(1, "a") == (1, "a")` evaluates to `true`.
*   **Rule:** Tuples compare element-by-element. Field *names* do not affect equality, only the *values*.

**72. Caller Attributes**
*   **Attributes:** `[CallerMemberName]`, `[CallerFilePath]`, `[CallerLineNumber]`.
*   **Benefit:** Injects caller metadata at compile-time. Perfect for logging and `INotifyPropertyChanged` without the performance hit of Reflection.

**73. Nullability Attributes**
*   **Attributes:** `[NotNull]`, `[DoesNotReturn]`, `[MaybeNull]`, `[NotNullWhen(true)]`.
*   **Benefit:** Helps the compiler understand control flow and null states, eliminating false positive warnings.

**74. `var` Keyword Usage**
*   **Good:** When the type is obvious (`var person = new Person();`).
*   **Bad:** When it hides meaning (`var data = GetData();`). Use `var` to reduce noise, not to obscure intent.

**75. `dynamic` Keyword**
*   **Problem:** Bypasses compile-time checking, uses the slow Dynamic Language Runtime (DLR), and breaks refactoring.
*   **Valid Uses:** COM interop, `ExpandoObject` / JSON parsing, or scripting glue code. Avoid in standard application logic.

---

### Part 4: Tips 76–100

**76. Exceptions for Control Flow**
*   **Rule:** Never use exceptions for expected conditions (e.g., checking if a user exists).
*   **Reason:** Exceptions are computationally expensive (stack traces, GC hits). Use booleans or specific return types for expected logic branches.

**77. `async void` in Unit Tests**
*   **Problem:** Test runners (xUnit, NUnit) cannot track `async void`. The test will pass before the code finishes executing, and exceptions will be swallowed.
*   **Rule:** Always return a `Task` in unit tests.

**78. `#region` Directive**
*   **Opinion:** `#region` hides messy code instead of fixing it.
*   **Solution:** If a file needs regions to be readable, the class has too many responsibilities. Extract logic into smaller, focused methods or classes.

**79. `[Obsolete]` Attribute**
*   **Rule:** Only use `[Obsolete]` if you have a scheduled removal date and a tested replacement.
*   **Warning:** Otherwise, it just generates warning noise that developers will permanently ignore.

**80. Single File C# Execution**
*   **Feature:** You can run a single `.cs` file directly via CLI (`dotnet run app.cs`) by adding SDK configurations at the top of the file.
*   **Bonus:** Add a shebang (`#!/usr/bin/env dotnet run`) to make the C# file executable like a bash script.

**81. Global Usings (C# 10)**
*   **Syntax:** `global using System;`
*   **Benefit:** Place this in a single file (e.g., `GlobalUsings.cs`) to apply the namespace import to every file in the project, eliminating repetitive headers.

**82. `nameof` Performance**
*   **Fact:** `nameof` is resolved entirely at compile-time. It generates a simple string literal in IL, meaning zero runtime allocation, zero reflection cost, and full AOT compatibility.

**83. `[InternalsVisibleTo]`**
*   **Concept:** Allows a test project to access `internal` members of your main assembly.
*   **Implementation:** Apply via an Assembly attribute or directly in the `.csproj` file. Keeps production code encapsulated while allowing thorough testing.

**84. Exception Filters (`when`)**
*   **Syntax:** `catch (HttpException ex) when (ex.StatusCode == 404)`
*   **Benefit:** Cleaner than `if` statements inside a `catch` block. Crucially, if the filter evaluates to false, the exception is *not* caught, preserving the original stack trace perfectly.

**85. Generic Constraint: `where T : notnull`**
*   **Concept:** Ensures the generic type cannot be null.
*   **Benefit:** Works for both reference types and non-nullable value types. Excellent for building safe APIs for Dictionary keys or IDs.

**86. `scoped` Keyword (C# 11)**
*   **Concept:** Acts as a lifetime fence. Ensures a value (like `Span<T>` or `ref` local) cannot escape the caller's scope or be captured by the heap.
*   **Benefit:** Prevents runtime crashes related to stack-only types leaking into async state machines or class fields.

**87. Destructors / Finalizers (`~Class()`)**
*   **Problem:** Run on a background GC thread with unpredictable timing. Adding a finalizer forces the GC to track the object in a special queue, severely slowing down garbage collection.
*   **Rule:** Almost never use them. Prefer `IDisposable`. Finalizers should only be a fallback for unmanaged resources.

**88. Property Patterns**
*   **Syntax:** `if (obj is { Property: value })`
*   **Benefit:** Matches deep inside an object's properties cleanly, inherently handling null checks without nested `if` statements.

**89. List Patterns (C# 11)**
*   **Syntax:** `if (list is [1, 2, .., 5])`
*   **Concept:** Matches arrays or lists by shape and elements. The `..` acts as a wildcard for any number of ignored elements.

**90. `sealed override`**
*   **Concept:** Overrides a virtual method from a base class, but locks it down so further derived classes *cannot* override it again. Useful for framework/library boundaries.

**91. The `Try` Parse Pattern**
*   **Concept:** Methods like `int.TryParse` return a boolean and use an `out` parameter instead of throwing exceptions on failure.
*   **Application:** Implement `TryX(out T result)` in your own code for predictable, high-performance control flow without stack trace overhead.

**92. `<LangVersion>preview</LangVersion>`**
*   **Configuration:** Add this to your `.csproj` to test upcoming C# features (like extension members) before the official .NET release.

**93. Index from End (`^`)**
*   **Syntax:** `array[^1]` gets the last element. `^2` gets the second to last.
*   **Benefit:** Eliminates the need for `array[array.Length - 1]`. Works on arrays, spans, and strings.

**94. Empty Array Literal `[]`**
*   **Syntax:** `int[] arr = [];`
*   **Optimization:** The compiler optimizes this to reuse a static, cached instance, providing the exact same performance as `Array.Empty<int>()` but with cleaner syntax.

**95. `await foreach`**
*   **Concept:** Iterates over `IAsyncEnumerable<T>`.
*   **Use Case:** Perfect for consuming paginated APIs, file streams, or message queues asynchronously without buffering the entire dataset into memory.

**96. `checked` Keyword**
*   **Syntax:** `checked { int.MaxValue + 1; }`
*   **Benefit:** Forces the runtime to validate math operations and throw an `OverflowException` instead of silently wrapping around to negative numbers. Vital for financial or physics calculations.

**97. Anonymous Type Equality**
*   **Concept:** Anonymous types automatically override `Equals` and `GetHashCode` based on their *property values*, not their memory references.
*   **Benefit:** Two distinct anonymous objects with the same values will evaluate as equal, making them perfect for temporary LINQ grouping.

**98. `Task.Yield()`**
*   **Concept:** Forces the method to asynchronously yield control back to the caller/context immediately.
*   **Use Case:** Keeps UI threads responsive or breaks up heavy synchronous work into async chunks. It does *not* act as a timer/delay like `Task.Delay`.

**99. `[MethodImpl(MethodImplOptions.AggressiveInlining)]`**
*   **Concept:** Hints to the JIT compiler to replace the method call with the actual method body (inlining).
*   **Use Case:** Excellent for micro-optimized math or hot paths. Do not use on large methods, as it increases code size and can hurt performance.

**100. `[GeneratedRegex]` (.NET 7)**
*   **Concept:** Generates Regex logic at compile-time via Source Generators.
*   **Syntax:** Define a `static partial Regex MyRegex();` method and decorate it with `[GeneratedRegex("pattern")]`.
*   **Benefit:** Zero runtime allocations, no JIT compilation delay, and vastly improved performance over standard `new Regex()`.