
---

# 2. Key Points

### Performance & Memory Management

* **Avoid allocating empty collections repeatedly**

  * Use `Array.Empty<T>()` and `Enumerable.Empty<T>()` instead of creating new empty arrays/lists every time.

* **Prevent multiple LINQ enumerations**

  * LINQ queries are often lazily evaluated. Enumerating them multiple times can repeat expensive computations or database calls.

* **Use Span<T> for high-performance memory access**

  * `Span<T>` provides allocation-free access to contiguous memory and can dramatically improve performance.

* **Access List<T> internals carefully**

  * `CollectionsMarshal.AsSpan()` allows fast access to a list's backing array but bypasses safety checks.

* **Use ArrayPool<T> for reusable buffers**

  * Renting and returning arrays reduces garbage collection pressure in high-throughput applications.

* **Use stackalloc for short-lived buffers**

  * Allocates memory on the stack instead of the heap, eliminating GC overhead.

* **Use ref structs appropriately**

  * Types like `Span<T>` remain on the stack and are optimized for high-performance scenarios.

* **Use `in` parameters for large structs**

  * Pass structs by readonly reference to avoid expensive copies.

* **Use compile-time generated Regex**

  * Source-generated regexes eliminate runtime compilation overhead and improve startup performance.

* **Understand JIT inlining**

  * Small frequently-called methods may benefit from aggressive inlining.

---

### Async & Concurrency

* **Never use lock with async code**

  * `lock` cannot contain `await`.

* **Use SemaphoreSlim for async locking**

  * Supports asynchronous waiting while preserving thread safety.

* **Always release SemaphoreSlim in finally blocks**

  * Prevents deadlocks when exceptions occur.

* **Run independent async operations concurrently**

  * Use `Task.WhenAll()` instead of awaiting tasks sequentially.

* **Avoid async void**

  * Use `Task` except for event handlers.

* **Use Parallel.ForEachAsync**

  * Modern replacement for async work inside traditional parallel loops.

* **Use await foreach**

  * Efficiently process asynchronous streams via `IAsyncEnumerable<T>`.

* **Use request cancellation tokens**

  * ASP.NET Core automatically provides request-specific cancellation support.

---

### Exception Handling

* **Rethrow exceptions correctly**

  * Use `throw;` instead of `throw ex;` to preserve stack traces.

* **Use exception filters**

  * `catch (...) when (...)` creates cleaner exception handling logic.

* **Don't use exceptions for expected behavior**

  * Prefer `TryXxx` patterns over exceptions for normal control flow.

* **Use checked arithmetic when overflow matters**

  * Prevent silent numeric overflow.

---

### Modern C# Language Features

* **Use collection expressions**

  * Modern collection initialization with `[]` reduces boilerplate.

* **Use target-typed new**

  * Avoid repeating type names unnecessarily.

* **Leverage top-level statements**

  * Simplify console applications.

* **Use pattern matching extensively**

  * `and`, `or`, `not`, relational patterns, property patterns, and list patterns improve readability.

* **Use nameof instead of strings**

  * Provides compile-time safety during refactoring.

* **Use deconstruction**

  * Make custom types easier to consume.

* **Use caller information attributes**

  * Access method names, file paths, and line numbers without reflection.

* **Use nullability attributes**

  * Help the compiler understand intent and improve static analysis.

* **Prefer records for immutable data**

  * Records support value equality and work naturally with `with` expressions.

---

### Dependency Injection & Architecture

* **Validate DI configuration at startup**

  * Enable `ValidateScopes` and `ValidateOnBuild`.

* **Create custom scopes when necessary**

  * Useful for background jobs and message processing.

* **Use assembly marker types**

  * Clearer than using random types for assembly scanning.

* **Enforce architecture rules with tests**

  * Use architecture testing libraries to prevent layer violations.

---

### Logging & Diagnostics

* **Use structured logging**

  * Avoid string interpolation inside logger calls.

* **Use message templates**

  * Preserve searchable structured data.

* **Use caller attributes**

  * Useful for logging and diagnostics without reflection.

---

### HTTP & Networking

* **Use IHttpClientFactory**

  * Solves both socket exhaustion and DNS refresh problems.

* **Avoid creating HttpClient per request**

  * Leads to resource exhaustion.

---

### Data & Serialization

* **Prefer DateTimeOffset over DateTime**

  * Represents an exact point in time and avoids timezone ambiguity.

* **Use JSON Schema Exporter**

  * .NET 9 can generate JSON Schema directly from models.

---

### Testing

* **Snapshot testing is highly effective**

  * Verify outputs by comparing against approved snapshots.

* **Never write async void tests**

  * Testing frameworks cannot properly track them.

* **Use InternalsVisibleTo for testing**

  * Avoid making internal code public.

* **Flaky tests should be fixed, not ignored**

  * Retrying tests is only a temporary workaround.

---

### Dependency & Package Management

* **Use net-outdated**

  * Easily identify and upgrade outdated NuGet packages.

* **Be aware of library licensing changes**

  * Understand implications of dependency upgrades.

---

### API Development

* **Refit eliminates API boilerplate**

  * Generate REST clients from interfaces.

* **Understand Run, Use, and Map**

  * Middleware ordering and behavior are critical in ASP.NET Core.

---

### Code Quality & Maintainability

* **Avoid excessive #region usage**

  * Large regions often indicate classes with too many responsibilities.

* **Use obsolete only when removal is planned**

  * Avoid warning fatigue.

* **Use var thoughtfully**

  * Use it when types are obvious; avoid it when clarity suffers.

* **Avoid dynamic unless absolutely necessary**

  * Sacrifices compile-time safety and performance.

* **Use explicit unit libraries**

  * Libraries like UnitsNet prevent unit conversion errors.

---

### Modern .NET Features

* **Use UUIDv7 / GUID v7**

  * Time-sortable identifiers reduce database fragmentation.

* **ULIDs are still a strong alternative**

  * Fast, sortable, distributed-system-friendly IDs.

* **Use global using directives**

  * Reduce repetitive imports.

* **Single-file execution is now possible**

  * Run `.cs` files directly without full projects.

---

# 3. Actionable Roadmap

### Roadmap to Write Better C# and .NET Applications

1. **Master the fundamentals**

   * Learn value types vs reference types.
   * Understand GC, stack vs heap, and memory allocation.

2. **Adopt modern C# features**

   * Use collection expressions.
   * Use pattern matching.
   * Use records where appropriate.
   * Use target-typed `new`.
   * Use top-level statements when useful.

3. **Write allocation-conscious code**

   * Use `Array.Empty<T>()`.
   * Use `ArrayPool<T>()`.
   * Learn `Span<T>`.
   * Avoid unnecessary LINQ enumerations.

4. **Become proficient with async programming**

   * Avoid `async void`.
   * Use `Task.WhenAll()`.
   * Use `SemaphoreSlim` instead of `lock` in async code.
   * Pass cancellation tokens everywhere.

5. **Improve exception handling**

   * Use `throw;` correctly.
   * Avoid exceptions for normal logic.
   * Implement `TryXxx` patterns.

6. **Build robust APIs**

   * Use `IHttpClientFactory`.
   * Implement structured logging.
   * Propagate cancellation tokens.
   * Validate inputs properly.

7. **Strengthen architecture**

   * Use dependency injection correctly.
   * Validate DI at startup.
   * Add architecture tests.

8. **Invest in testing**

   * Write async tests correctly.
   * Introduce snapshot testing.
   * Test internal components via `InternalsVisibleTo`.

9. **Optimize only after measuring**

   * Use profiling tools.
   * Apply Span, ArrayPool, stackalloc, and inlining only where justified.

10. **Stay current**

    * Follow new C# releases.
    * Experiment with preview features.
    * Regularly update dependencies.

---

# 4. Important Quotes

> "If you're rethrowing it by using the throw keyword followed by the exception variable name, then you are shooting yourself in the foot."

> "Dynamic is a parachute, not a pattern."

> "Exceptions are for errors and exceptional cases, not expected conditions."
