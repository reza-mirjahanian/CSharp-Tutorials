# 100 .NET / C# Tips — Structured Breakdown

## Part 1: Tips 1–50

### **1. Use `Array.Empty<T>()` / `Enumerable.Empty<T>()` instead of `new T[0]` / `new List<T>()`**
- Returning `new T[0]` or `new List<T>()` allocates a new empty array/list on the heap every call.
- Even with caching on a field/property, a better solution exists.
- For arrays: `Array.Empty<T>()`.
- For other enumerables: `Enumerable.Empty<T>()`.
- These guarantee a single allocation for the application's lifetime, avoiding GC pressure and app pauses.

### **2. Rethrow exceptions correctly with bare `throw`**
- Common pattern: catch → log/metric → rethrow for a centralized handler.
- **Wrong:** `throw ex;` — this **resets the stack trace**, losing debugging info.
- **Right:** `throw;` — rethrows the original exception, preserving the full stack trace.

### **3. Use `SemaphoreSlim` instead of `lock` for async code**
- `lock` cannot be used with `async`/`await`, making it impractical in modern C#.
- Alternative: `SemaphoreSlim` with `new SemaphoreSlim(1, 1)`.
- Pattern:
  - `await semaphore.WaitAsync()` at the start
  - `try { ...locked code... } finally { semaphore.Release(); }`
- Wrap `Release()` in `finally` to prevent deadlocks.
- The `1, 1` parameters mean only one thread enters at a time.

### **4. Beware of multiple enumeration with `IEnumerable` (deferred execution)**
- Calling `.Count()` and `.All(...)` on an `IEnumerable` from a `Select` triggers **two enumerations** of the source.
- This re-runs the entire pipeline, doubling allocations and possibly causing **multiple I/O calls** (e.g., DB hits).
- Fix: materialize once — `.ToList()` or `.ToArray()` — then operate on the concrete collection.

### **5. Try `dotnet-repl` for a C# REPL in your terminal**
- `dotnet-repl` is a cross-platform CLI tool to run C# interactively.
- Install globally: `dotnet tool install -g dotnet-repl`.
- Provides IntelliSense, autocomplete, NuGet package support, and can even run ASP.NET Core APIs in the console.

### **6. Get a `Span<T>` from a `List<T>` via `CollectionsMarshal.AsSpan`**
- Lists are backed by internal arrays, but the array isn't directly accessible.
- `CollectionsMarshal.AsSpan(list)` exposes that internal array as a `Span<T>`.
- **Warning:** unsafe — concurrent mutation won't throw; you're working directly on the internal buffer.

### **7. The logger's "message" is actually a message template**
- `ILogger.Log("...")` treats the string as a **template**, not a pre-formatted message.
- Bad: string interpolation/concatenation in the log call → loses structured properties, wastes memory on strings that GC must collect.
- Good: use named placeholders in the template, pass values as a separate argument (e.g., `logger.LogInformation("User {UserId} logged in", userId)`).
- Enables filtering by properties and avoids premature string allocation.

### **8. C# 12 empty types — no curly braces required**
- You can now declare empty types as `class Foo;` / `struct Bar;` / `interface Baz;` — no `{}` needed.
- Replaces the awkward "leave the body empty" patterns.

### **9. `ToList()` vs `ToArray()` — pick by use case, not just performance**
- **Functional guidance:**
  - Returning a `List` → caller may add/remove items.
  - Returning an `Array` → caller only enumerates or mutates values without changing length.
- **Performance note:** results vary by size. For ~10,000 items, `ToList()` is slightly faster than `ToArray()` (counter-intuitive).
- .NET 9 brings improvements that make `ToArray()` faster overall.

### **10. Don't use `Program.cs` as your assembly marker**
- Libraries like MediatR / AutoMapper accept a generic marker type for DI registration.
- Common mistake: pointing it at `Program` (lives in the startup project).
- Better: create an empty `IMyProjectAssembly` interface in the library itself, named after the assembly.
- Removes ambiguity, makes intent explicit, aids readability.

### **11. `[StringSyntax]` attribute highlights strings meaningfully**
- Net 7+: apply `[StringSyntax(StringSyntaxAttribute.Regex)]` (or `.Date`, `.Uri`, `.Json`) to string parameters.
- IDEs then highlight the string with the correct syntax (regex, URL, etc.) for a better developer experience.

### **12. C# 12 primary constructors have a `readonly` limitation**
- Primary constructors tidy classes by moving ctor parameters up to the type declaration.
- **Problem:** no way to mark them `readonly` (i.e., make them fields). They become mutable private fields.
- IDEs may suggest this refactoring, but accepting it produces fundamentally worse code. Be cautious.

### **13. `Guid.CreateVersion7()` — sortable, time-ordered GUIDs**
- Old `Guid.NewGuid()` is v4 (random) → causes DB index fragmentation when used as keys.
- .NET 9 adds `Guid.CreateVersion7()` — embeds timestamp in the first bytes.
- Result: lexicographically sortable GUIDs, no fragmentation, no need for 3rd-party libs.

### **14. The smallest valid C# program**
- Since C# 9 top-level statements, `Program.cs` no longer needs the `Main` method.
- An empty file isn't valid, but the **smallest valid C# program** is: a single semicolon `;`.

### **15. Use framework-injected `CancellationToken` in ASP.NET Core**
- Don't `new CancellationToken()` for API endpoints.
- Add `CancellationToken` as a parameter to your controller action or minimal API handler.
- ASP.NET Core auto-binds the request's token, so client disconnects cascade through your pipeline.
- **Always pass the token down** to all awaited operations.

### **16. C# 12 collection expressions — `[]` literal**
- New shorthand: `T[] x = [];` / `List<T> y = [];` / `Dictionary<K,V> z = [];`
- Compiler picks the correct collection initializer.
- Cleaner code, less boilerplate.

### **17. `dotnet-outdated` CLI for package upgrades**
- Install globally: `dotnet tool install -g dotnet-outdated`.
- Run `dotnet outdated` to see packages with newer versions.
- Use `--upgrade` to update them.
- Supports version locking to prevent accidental upgrades to paid editions.

### **18. Use Bogus + Waffle Generator instead of Lorem Ipsum**
- Lorem Ipsum is unrealistic; nobody speaks Latin in your domain.
- Waffle Generator (by Andrew Clark) creates realistic, domain-aware fake text.
- NuGet: `WaffleGenerator.Bogus` — generate fake text in HTML or Markdown.
- Integrates with **Bogus** for property-level fake data.

### **19. ASP.NET Core pipeline methods: `Run`, `Use`, `Map`**
- **`Run`**: terminal middleware — ends the pipeline.
- **`Use`**: general middleware — chains to the next.
- **`Map`**: branch the pipeline based on a request path.
- **Order matters** — they execute in registration order.

### **20. Validate against naughty strings**
- Naughty strings (github.com/minimaxir/big-list-of-naughty-strings) can crash servers or expose security holes.
- NuGet: `NaughtyStrings` package.
- Use in QA / E2E tests to see how your software reacts to hostile input.

### **21. `InterpolatedParser` — reverse string interpolation**
- NuGet: `InterpolatedParser`.
- Extract values from a string by using string interpolation as a **template**:
  - `InterpolatedParser.Parse<int>("{0} items", input)` — but cleaner: declare a template literal.
- Brilliant, cursed, and removes regex from common parsing tasks.

### **22. C# 12 `using` alias for any type**
- `using FriendlyName = Some.Long.Namespace.ComplicatedType;`
- Solves four problems:
  1. Simplifies long type names.
  2. Disambiguates naming conflicts.
  3. Defines value-tuple-like shared types.
  4. Improves clarity with descriptive names.

### **23. Prefer `DateTimeOffset` over `DateTime`**
- `DateTime` is ambiguous unless `Kind == Utc` — otherwise you don't know the timezone.
- `DateTimeOffset` always carries the UTC offset → exact moment in time.
- Critical for client-timezone-aware business logic.

### **24. Use architecture tests to enforce namespace boundaries**
- NuGet: `NetArchTest.Rules`.
- Fluent API: `Types().That().ResideInNamespace("X").ShouldNot().DependOnAny("Y")`.
- Enforce sealed-by-default, restrict which classes can be used where, prevent policy violations.

### **25. FluentAssertions alternatives (avoid the v8 license fee)**
- v8+ charges ~$130/dev/year. Options:
  1. **Pin to v7** — feature-complete, license change only applies from v8.
  2. **AwesomeAssertions** — fork of the free version, drop-in replacement, actively maintained.
  3. **Shouldly** — long-standing alternative, similar API, different wording.

### **26. .NET 9 `JsonSchemaExporter` for `System.Text.Json`**
- No more 3rd-party schema libs.
- `JsonSerializerOptions.GetJsonSchema(nodePath)` exports a JSON schema.
- Customizable via `JsonSchemaExporterOptions`.
- This is what the new OpenAPI functionality in .NET is built on.

### **27. `Parallel.ForEachAsync` / `Parallel.ForAsync` — proper async parallelism**
- Old `Parallel.ForEach` + `async` lambda = **async void** disaster (unhandled exceptions, no awaiting).
- .NET 6+: `Parallel.ForEachAsync` and `Parallel.ForAsync` properly return tasks.

### **28. `dotnet retest` — auto-retry flaky tests (or fix them)**
- NuGet CLI: `dotnet retest`.
- Automatically retries failing tests a few times.
- **Real advice:** fix the flaky tests. Stop ignoring them.

### **29. C# 13 — `params` works with `Span<T>`, `List<T>`, `IEnumerable<T>`, etc.**
- Previously `params` was array-only and had allocation overhead.
- C# 13 expands it to most collection types, with significant perf optimizations.
- **`params ReadOnlySpan<T>`** is stack-allocated, zero heap.

### **30. Prank tip: shadow `System.Threading.Monitor`**
- Create a class named `Monitor` with `Enter(object)` and `Exit(object)` methods.
- Place it in namespace `System.Threading` in a file the compiler picks up.
- The compiler resolves to **your** `Monitor` instead of the BCL one.
- **Disclaimer:** joke tip, not real advice. Don't do it.

### **31. Why C# private fields use `_camelCase`**
- The underscore signals **scope** — distinguishing class fields from local variables when scrolling.
- Alternatives exist (e.g., `this.fieldName`), but `_` is the recommended convention.

### **32. `HttpClient` best practices**
- **Wrong:** `new HttpClient()` per request → socket exhaustion.
- **Static client:** fixes exhaustion, but DNS is only resolved once → stale if DNS changes.
- **Best:** use `IHttpClientFactory` (handles handler pooling, refreshes DNS, no socket leaks), or set `PooledConnectionLifetime` manually.

### **33. Snapshot testing with `Verify`**
- Snapshot tests compare an output (object, JSON, text, image, UI) to a previously approved version.
- `Verify` works with xUnit, NUnit, MSTest, Entity Framework, etc.
- Great entry point for projects without unit tests — non-invasive but effective.

### **34. `Refit` — declarative API clients**
- Define an interface, decorate methods with `[Get]`, `[Post]`, etc.
- Refit generates the implementation.
- Instantiate via `RestService.For<T>()` or `HttpClientFactory` extensions.
- No more boilerplate HTTP code.

### **35. `SemaphoreSlim` async lock pattern (detailed)**
- `static SemaphoreSlim _semaphore = new(1, 1);`
- `await _semaphore.WaitAsync();`
- `try { ... } finally { _semaphore.Release(); }`
- **Add a timeout** to `WaitAsync()` to prevent deadlocks.
- Only one thread enters the block at a time.

### **36. ULIDs — alternative sortable IDs**
- `ULID` (Universally Unique Lexicographically Sortable Identifier) — sortable, fast, random.
- Useful for distributed systems and cloud workloads.
- NuGet: `Ulid` — `Ulid.NewUlid()`, convertible to/from `Guid`.

### **37. Parallelize independent async work with `Task.WhenAll`**
- Don't `await` serially when tasks are independent.
- Kick off all tasks, then `await Task.WhenAll(task1, task2, ...)`.
- Results can be read from `task.Result` afterwards (safe, since `WhenAll` completed).

### **38. Custom DI scopes beyond HTTP requests**
- Lifetimes: Transient / Scoped / Singleton. `Scoped` is usually tied to HTTP, but you can create your own.
- Inject `IServiceScopeFactory`, call `CreateScope()` to get a `IServiceScope` + `ServiceProvider`.
- Use it for message processing, background jobs, etc.
- Always `Dispose()` the scope when done.

### **39. Primary constructors: still not there yet (opinionated)**
- Two real issues:
  1. **No `readonly`** — parameters become fields that any method can change.
  2. **`_underscore` vs `camelCase`** — primary ctor params look like local variables, mixing styles.
- IDE suggestions to migrate `readonly` fields into primary constructors are misleading — semantics change.

### **40. `UnitsNet` — sane unit conversions**
- NuGet: `UnitsNet`.
- Hundreds of units and conversion methods (meters↔feet, RPM↔torque↔horsepower, etc.).
- Explicit and implicit conversions to make code natural.

### **41. Validate DI container at build time**
- DI errors normally surface at runtime → deployment-time failures.
- Set `ValidateScopes = true` and `ValidateOnBuild = true` on `ServiceProviderOptions`.
- Misconfigurations now fail the build, not the production app.

### **42. C# 14 — null-conditional assignment**
- Current: `obj?.Property` reads only.
- C# 14 adds: `obj?.Property = value;` — assigns only if `obj` is not null.

### **43. `Dictionary<K,V>` initialization semantics differ**
- Two ways:
  - **Add syntax (`dict.Add(...)`)** — throws on duplicate key.
  - **Index initializer (`dict[k] = v`)** — silently overwrites.
- The new `[]` empty dictionary literal compiles to whichever the compiler chooses — be aware.

### **44. Records vs classes with primary constructors**
- **Records:** primary ctor params auto-create public, immutable (read-only) properties → used for value-based equality.
- **Classes:** primary ctor params are in-scope throughout the class body, no auto-property created.
  - You can use them in methods.
  - If used in a method, the compiler generates a hidden field.
  - `this.Name` doesn't exist unless you explicitly declare it.

### **45. `ref struct` — stack-only, allocation-free**
- A `ref struct` (e.g., `Span<T>`) must remain on the stack.
- Benefits: blazing-fast, no heap allocations, no GC.
- Rules:
  - Cannot be boxed.
  - Cannot be used in `async` methods.
  - Cannot be stored on the heap (even in known-class fields).
- Use for parsing, slicing, large-data processing, high-perf code.

### **46. The `in` parameter modifier — read-only by ref**
- `void Foo(in BigStruct x)` — pass by reference, but the method can't modify `x`.
- Avoids the cost of copying large structs.
- **Catch:** if the struct isn't `readonly`, the compiler may insert defensive copies → hurts perf.
- Combine with `readonly struct` / `readonly` members for the full benefit.

### **47. LINQ deferred execution**
- LINQ methods (`Where`, `Select`, `Take`, …) don't execute immediately — they build a blueprint.
- Query can be extended even after declaration (conditional LINQ).
- When finally enumerated, LINQ compiles the whole pipeline into a single optimized operation.
- **Risk:** if the underlying data changes before enumeration, results reflect the new data.

### **48. `stackalloc` for zero-allocation arrays**
- Allocates the array on the stack, not the heap.
- Memory is freed automatically when the method returns (stack frame discarded).
- No GC pressure — huge for gamedev, parsers, hot paths.
- **Risk:** limited stack space — too large → `StackOverflowException`. Keep it small.

### **49. Built-in delegate types**
- **`Func<T, TResult>`** — returns a value (last type param is return).
- **`Predicate<T>`** — `Func<T, bool>`, used for filtering; predates `Func`. Prefer `Func<T,bool>` today.
- **`Action<T>`** — returns void.
- Delegates chain via multicast: one call invokes multiple methods in sequence.
- C# designers famously hate `Action`/`Func` overloads — yet they exist and are widely used.

### **50. Ways a base class member can be overridden**
- **`abstract`** — base has no logic, derived **must** implement.
- **`virtual`** — base has default, derived **may** override; overrides are themselves `virtual` unless `sealed`.
- **Interface members** — implicitly virtual; C# 8+ allows default interface implementations.
- **`new` (method hiding)** — replaces base member when accessed through derived type; calling via base reference still hits the base method.

---

## Part 2: Tips 51–100

### **51. `ArrayPool<T>.Shared` — rent & return arrays**
- Skip GC by reusing arrays from a pool.
- `var arr = ArrayPool<int>.Shared.Rent(100);` (may return a larger array).
- Use it like a normal array, then `ArrayPool<int>.Shared.Return(arr);` to give it back.
- **Sensitive data:** pass `clearArray: true` on return.
- Great for JSON parsing, file I/O, high-throughput APIs.

### **52. Avoid `async void` (except event handlers)**
- `async void` can't be awaited, can't be tested, exceptions in the body **crash the process**.
- Only legitimate use: **event handlers** (they don't return `Task`).
- If used there, catch all exceptions internally.

### **53. The null-forgiving operator `!`**
- `Foo!` tells the compiler "I know this isn't null at runtime" — silences nullable warnings.
- Useful when nullable analysis can't follow the initialization (e.g., framework sets it later, deserializer, etc.).
- Doesn't change the runtime type — purely a compile-time hint.

### **54. `using` is a `Dispose` contract, not just a shortcut**
- `using (var x = new Foo())` → compiler emits a `try { ... } finally { x.Dispose(); }`.
- Fires even on exception.
- Works with any `IDisposable` (files, streams, DB connections, timers).
- C# 8+ allows braceless `using` declarations; C# 8+ also supports `await using` for `IAsyncDisposable`.

### **55. `with` expression — non-destructive mutation**
- `var p2 = p1 with { Age = 30 };` — clones and overrides specified properties.
- Original is unchanged.
- Designed for **records** (and `struct` from C# 10).
- Auto-generated `Clone()` method handles the copy.
- **Note:** cloning a class reallocates (it's a reference type).

### **56. C# 14 — extension members (extensions on steroids)**
- No longer limited to static methods in a static class.
- Can add: static methods, instance methods, instance properties, static properties — to **any** type, even ones you don't own.
- Migrate existing extension methods so you don't maintain two patterns.

### **57. Collection expressions + spread operator**
- C# 12: `int[] x = [1, 2, 3];` — no `new List<int> { 1, 2, 3 }` ceremony.
- Spread: `int[] combined = [..a, ..b, 4, 5];` — merge collections inline.

### **58. C# 13 — `params Span<T>` = zero-alloc**
- `void Foo(params ReadOnlySpan<int> values)` — caller passes values without a heap-allocated array.
- Combines convenience of `params` with stack-only performance.

### **59. C# 9 target-typed `new`**
- `List<int> list = new();` — type inferred from the left-hand side.
- Works for constructors, collections, generics.
- Reduces noise in complex declarations.

### **60. C# 9 top-level statements**
- `Console.WriteLine("Hello");` in `Program.cs` is a complete program.
- No `class`, no `Main`. Compiler wraps it in a class/method behind the scenes.
- Works with a single `await`.
- Only **one** top-level file per project (the entry point).

### **61. Pattern matching combinators: `not`, `and`, `or`**
- `if (x is not null and > 0 and < 100) { ... }`
- Cleaner than nested `if`/guard clauses.
- Works in `is` expressions and `switch` arms.

### **62. `nameof` — compile-time-safe symbol names**
- `nameof(userId)` → `"userId"`.
- Compiler resolves it; **zero runtime cost, no reflection, AOT-safe**.
- Use in logging, validation, exceptions, attributes — survives renames automatically.

### **63. Custom `Deconstruct` on any type**
- Records get `Deconstruct` for free; you can add it to any class/struct.
- Define `public void Deconstruct(out T1 a, out T2 b) { a = ...; b = ...; }`.
- Allows `var (x, y) = obj;` syntax.
- Add overloads for more elements.

### **64. Attributes on lambda expressions (C# 10)**
- `[SomeAttr] (x) => x * 2` — applies attributes to the compiler-generated method.
- Use cases: source generators, middleware, custom analyzers, AOT/interop scenarios.
- Works with static lambdas too.

### **65. Range pattern matching**
- `if (x is > 0 and < 100) { ... }`
- Works in `is` expressions and `switch` statements/arms.
- Supports `>=`, `<=`, etc. — combined with `and`/`or`/`not`.

### **66. `ArgumentNullException.ThrowIfNull` (C# 10)**
- Replaces `if (x is null) throw new ArgumentNullException(nameof(x));`.
- One line: `ArgumentNullException.ThrowIfNull(x);`.
- Uses `nameof` internally — no risk of passing the wrong name.

### **67. Expression-bodied constructors**
- `public Person(string name) => Name = name;`
- No braces, no body — for trivial constructors.
- May produce long lines; format accordingly.

### **68. `ValueTuple` vs `Tuple`**
- `Tuple<T1, T2>`: heap-allocated, class, no named fields, pre-C# 7.
- `(int X, int Y)`: `ValueTuple` — `struct`, no allocation, named fields, pattern matching friendly.
- Always prefer `ValueTuple` syntax.

### **69. C# keyword pun (lighthearted)**
- A `string` can be `in` and `out` (parameters) — but its `Length` is `int`, can't be `null`/`void`/`float`/`double`.
- Treat it as `object`, ensure the `event` isn't `public`, and make sure it's `protected`.

### **70. `ref` → `in` is legal; `in` → `ref` is not**
- `void Foo(in T x)` accepts a `ref T` (compiler treats it as read-only).
- `void Bar(ref T x)` does **not** accept an `in T` (because `ref` allows writes).
- Asymmetric: `ref` is a superset of `in` in call-site flexibility.

### **71. Tuple equality**
- Tuples implement `==`, `!=`, and `.Equals()` element-by-element, in order.
- **Names don't affect equality** — only positions and values.

### **72. Caller-info attributes — `[CallerMemberName]`, `[CallerFilePath]`, `[CallerLineNumber]`**
- Default-parameter attributes that the compiler fills in at the call site.
- `void Log(string msg, [CallerMemberName] string caller = "")` — auto-fills with the calling method's name.
- **Zero runtime cost, no reflection**, AOT-safe.
- Use for logging, `INotifyPropertyChanged`, debugging.

### **73. Null-state analysis attributes**
- `[NotNull]` — parameter won't be null after the method returns.
- `[DoesNotReturn]` — method never returns (helps flow analysis after `throw`).
- `[MaybeNull]` — return may be null.
- `[NotNullWhen(true)]` — value is not null when the method returns true.
- Help the compiler reason about nullable flow → fewer false positives.

### **74. `var` — readability over dogma**
- `var` is fine when the type is obvious from the right-hand side.
- Avoid `var` when the type isn't obvious — debuggers still show the type, but readers don't.
- Rule of thumb: `var person = new Person();` ✓; `var data = GetThing();` ✗.

### **75. The `dynamic` keyword — use sparingly**
- `dynamic` bypasses compile-time checks → typos cause **runtime** exceptions.
- Internally uses the **DLR** (Dynamic Language Runtime) → significantly slower.
- **Legitimate uses:** COM interop, working with `ExpandoObject`/dynamic JSON, scripting engines (Python/JS), ad-hoc glue code.
- In app logic: use strong types, interfaces, records.

### **76. Don't use exceptions for normal control flow**
- Exceptions are **expensive** (stack trace capture, GC pressure).
- "User not found" is a **valid outcome**, not an error.
- Reserve `throw` for exceptional cases (DB down, IO failure, contract violation).
- Expected conditions → return a bool/`Result`/optional.

### **77. `async void` in tests = silent failure**
- xUnit/NUnit/MSTest can't track `async void` test methods → test "passes" before the code finishes, exceptions may be swallowed.
- **Always return `Task` (or `ValueTask`) in test methods.**
- `async void` is for event handlers only.

### **78. Stop hiding code with `#region`**
- `#region` is a visual crutch — it doesn't improve structure, it **hides** the mess.
- Reviewers on GitHub see the unfolded mess.
- **Fix:** split large classes, extract methods, refactor. If you need `#region`, the class has too many responsibilities.

### **79. `[Obsolete]` only when removal is planned**
- The attribute is a deprecation contract.
- If you add `[Obsolete]` without a removal plan, devs ignore the warning → noise.
- Use only when:
  - There's a tested replacement.
  - A removal date is scheduled.
  - The deprecation is real.
- Otherwise: leave it alone, or use a comment.

### **80. `dotnet run app.cs` — single-file programs**
- .NET 10: run a `.cs` file directly with `dotnet run app.cs` — no project, no boilerplate.
- Add `#:sdk` and `#:package` directives at the top of the file to configure SDK and NuGet packages.
- Shebang support (`#!/usr/bin/dotnet run`) → make it executable on Unix.

### **81. Global usings (C# 10)**
- `global using` directives in a `GlobalUsings.cs` (or any file) apply across the entire project.
- Works for namespaces and `using static`.
- Saves repetitive `using` lines in every file.

### **82. `nameof` is not reflection — it's compile-time**
- `nameof(x)` → string, resolved at compile time.
- No allocations, no runtime cost, AOT-safe.
- Contrast with `x.GetType().Name` or reflection — both run at runtime and cost perf.

### **83. Test internals with `[InternalsVisibleTo]`**
- Don't make everything `public` just to test it.
- `[assembly: InternalsVisibleTo("MyTests")]` in `AssemblyInfo.cs` (or via the new `InternalsVisibleTo` parameter) → test project can access `internal` members.
- `private` members should be tested through `public`/`internal` API.

### **84. `catch` `when` filters**
- `catch (HttpRequestException ex) when (ex.StatusCode == 404) { ... }` — filter the catch.
- No inner `if`, no rethrow tricks — clean and clear.

### **85. Generic constraint: `where T : notnull`**
- `void Foo<T>(T value) where T : notnull` — `T` can be a reference type or non-nullable value type, but not nullable.
- Works for `string`, `int`, `DateTime` — but not `string?`, `int?`.
- Perfect for dictionary keys, IDs, value objects — no null-key footguns.

### **86. C# 12 `scoped` keyword — lifetime safety**
- `void Foo(scoped Span<byte> buffer)` — buffer cannot escape the method (no heap capture, no async).
- Pair with `ref struct` types to enforce stack-only usage.
- Compiler-enforced lifetime safety — catches bugs at build time.
- Also works on `ref` locals.

### **87. C# destructors (finalizers) — almost never**
- `~Foo() { ... }` runs when GC decides the object is collected.
- **You don't control when**, doesn't run on app shutdown, runs on a background thread with no guarantees.
- Adding a finalizer **slows GC** (object goes to a finalization queue).
- Use `IDisposable` for predictable cleanup. Use finalizers only as a safety net in case `Dispose` was never called.

### **88. Property patterns — match deep without nesting**
- `if (obj is { Address: { City: "London" } }) { ... }`
- Combine values: `if (obj is { Age: > 18 and < 65, Name.Length: > 0 }) { ... }`
- No null checks, no temporary variables.

### **89. List patterns (C# 11)**
- Match arrays/lists by **shape**:
  - `if (list is [1, 2, 3])` — exactly `[1, 2, 3]`.
  - `if (list is [_, _, _])` — exactly 3 items.
  - `if (list is [var first, .., var last])` — first and last, ignore middle.
  - `if (list is [.., var last])` — just the last.

### **90. `sealed override` — lock down a virtual method**
- `public sealed override void Foo() { ... }` — no further derived class can override.
- Useful for library/framework code to prevent unexpected behavior in future subclasses.
- Doesn't require sealing the entire class.

### **91. `TryParse` / `Try*` methods — no exceptions on expected failures**
- `int.TryParse(s, out var n)` → `bool`, no throw on bad input.
- Pattern for your own code: `bool TryParseX(string s, out X result)` — `Try` suffix, returns `bool`, has an `out` parameter.
- Fast, predictable, no exception cost.

### **92. Try preview C# features with `<LangVersion>preview</LangVersion>`**
- Set `<LangVersion>preview</LangVersion>` in the `.csproj` to unlock the next-version features (e.g., extension members, discriminated unions).
- Great for evaluation and feedback, but APIs may change before release.

### **93. Index-from-end operator `^n`**
- `arr[^1]` → last item, `arr[^2]` → second-to-last, etc.
- Replaces `arr[arr.Length - 1]`.
- Works on arrays, spans, strings — anything indexable/sliceable.

### **94. Empty array literal `[]` is optimized**
- `T[] x = [];` — the compiler reuses the **same static instance** as `Array.Empty<T>()`.
- Same performance, cleaner syntax.

### **95. `await foreach` for async streams**
- `await foreach (var item in GetStreamAsync()) { ... }` — works on `IAsyncEnumerable<T>`.
- Source can be a paginated API, file stream, message queue, etc.
- Supports `CancellationToken` via `WithCancellation`.

### **96. `checked` keyword — catch silent integer overflow**
- Default: `int.MaxValue + 1` → wraps to negative (no exception).
- `checked { int x = int.MaxValue + 1; }` → throws `OverflowException`.
- Wrap entire blocks. Use for financial math, physics sims, anywhere overflow = real bug.

### **97. Anonymous types have value-based equality**
- Two anonymous objects with the same properties and values are `.Equals()` (even though they have different references).
- C# auto-overrides `Equals` and `GetHashCode` for anonymous types based on property values.
- Great for ad-hoc groupings in LINQ without writing boilerplate.

### **98. `Task.Yield()` — async context switch without delay**
- `await Task.Yield();` returns control to the caller immediately.
- **Not a delay** — no timer, no scheduling.
- Uses: yield back to the UI thread, prevent long-running sync work from blocking, break sync work into async chunks.

### **99. `[MethodImpl(MethodImplOptions.AggressiveInlining)]` — hint the JIT**
- Small methods are often auto-inlined. For hot paths, force it.
- Use for: micro-optimized libraries, math-heavy loops, hot inner loops.
- **Don't sprinkle it everywhere** — inlining large methods bloats code and may hurt performance.

### **100. Source-generated regex (.NET 7+)**
- Old: `new Regex("...")` — runtime compile, allocates, JIT delay.
- New: `[GeneratedRegex(@"...")] partial void Foo();` (source generator) — regex is **baked into the assembly at compile time**.
- Use: `Regex.Foo(input)`.
- Zero allocation, instant startup, fully static, AOT-friendly.