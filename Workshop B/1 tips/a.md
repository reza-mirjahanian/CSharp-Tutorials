# 100 Essential C# Tips - Complete Technical Breakdown

## Performance & Memory Management

### 1. Empty Collection Allocation
- **Problem:** `return new int[0]` or `return new List<int>()` allocates memory each time
- **Solution:** Use cached empty collections
```csharp
// Bad - allocates each time
public int[] GetItems() => new int[0];

// Good - cached
public int[] GetItems() => Array.Empty<int>();
public IEnumerable<int> GetItems() => Enumerable.Empty<int>();
```

### 2. Correct Exception Rethrowing
- **Wrong:** `throw ex;` - resets stack trace
- **Correct:** `throw;` - preserves full stack trace
```csharp
try {
    // some code
}
catch (Exception ex) {
    Log(ex);
    throw; // ✅ Preserves stack trace
    // throw ex; // ❌ Resets stack trace
}
```

### 3. Async Locking with SemaphoreSlim
- **Problem:** `lock` keyword doesn't work with async/await
- **Solution:** Use `SemaphoreSlim`
```csharp
private static readonly SemaphoreSlim _semaphore = new(1, 1);

public async Task DoWorkAsync() {
    await _semaphore.WaitAsync();
    try {
        // Critical section - only one thread at a time
        await SomeAsyncOperation();
    }
    finally {
        _semaphore.Release();
    }
}
```

### 4. Multiple Enumeration Problem
- **Problem:** LINQ queries execute multiple times
```csharp
// Bad - enumerates twice
var numbers = Enumerable.Range(1, 10).Select(x => x * 2);
Console.WriteLine(numbers.Count()); // First enumeration
Console.WriteLine(numbers.All(x => x > 0)); // Second enumeration
```
- **Solution:** Materialize once
```csharp
var numbers = Enumerable.Range(1, 10).Select(x => x * 2).ToList();
Console.WriteLine(numbers.Count()); // Uses cached list
Console.WriteLine(numbers.All(x => x > 0)); // Uses cached list
```

### 5. C# REPL (CSharpRepl)
- **Command:** `dotnet tool install -g dotnet-csharprepl`
- **Usage:** `csharprepl` in terminal
- **Features:** Full IntelliSense, NuGet package installation, ASP.NET support

### 6. Accessing List's Internal Array
- **Method:** `CollectionsMarshal.AsSpan(list)`
- **Warning:** Unsafe - no exception if list mutates during iteration
```csharp
var list = new List<int> { 1, 2, 3 };
Span<int> span = CollectionsMarshal.AsSpan(list);
// Use span for high-performance operations
```

### 7. Logging Best Practices
- **Wrong:** String interpolation in logger
```csharp
_logger.LogInformation($"User {userId} logged in"); // ❌
```
- **Correct:** Message template with parameters
```csharp
_logger.LogInformation("User {UserId} logged in", userId); // ✅
```
- **Benefits:** Proper filtering, less memory allocation

### 8. Empty Types (C# 12)
- **New syntax:** Omit curly braces
```csharp
// Old
public interface IMarker { }
public class MyClass { }

// New (C# 12)
public interface IMarker;
public class MyClass;
```

### 9. List vs Array Performance
- **For returning mutable collections:** Use `List<T>`
- **For returning fixed-size collections:** Use `T[]`
- **Performance:** .NET 9 makes `ToArray()` faster than `ToList()` for many scenarios

### 10. Assembly Markers
- **Purpose:** Type used to identify assembly for DI registration
- **Best Practice:** Use empty interface
```csharp
// MyProject/AssemblyMarker.cs
public interface IMyProjectAssemblyMarker { }

// Registration
services.AddMediatR(typeof(IMyProjectAssemblyMarker));
```

## Language Features & Syntax

### 11. String Syntax Attribute (.NET 7)
- **Usage:** `[StringSyntax(StringSyntaxAttribute.Json)]`
```csharp
public void ProcessJson([StringSyntax(StringSyntaxAttribute.Json)] string json) {
    // IDE provides JSON syntax highlighting
}
```

### 12. Primary Constructor Limitations (C# 12)
- **Problem:** Cannot mark parameters as `readonly`
```csharp
// ❌ Not possible
public class Service(readonly ILogger logger) { }

// ✅ Workaround
public class Service(ILogger logger) {
    private readonly ILogger _logger = logger;
}
```

### 13. UUID v7 (.NET 9)
- **Method:** `Guid.CreateVersion7()`
- **Benefits:** Time-sorted, reduces database fragmentation
```csharp
var sortedGuid = Guid.CreateVersion7(); // Lexicographically sortable
```

### 14. Smallest Valid C# Program
```csharp
// Single line - valid program
Console.WriteLine("Hello");
```

### 15. CancellationToken in ASP.NET Core
```csharp
app.MapGet("/api/data", async (CancellationToken ct) => {
    // Token automatically from request
    await SomeOperationAsync(ct);
});
```

### 16. Collection Expressions (C# 12)
```csharp
// Old
List<int> list = new List<int> { 1, 2, 3 };
int[] array = new int[] { 1, 2, 3 };

// New
List<int> list = [1, 2, 3];
int[] array = [1, 2, 3];
Dictionary<string, int> dict = [["key", 1]];
```

### 17. NuGet Package Version Checking
```bash
dotnet tool install -g dotnet-outdated
dotnet outdated --upgrade
```

### 18. Realistic Test Data Generation
- **Library:** WaffleGenerator + Bogus
```csharp
var waffle = new WaffleEngine();
string text = waffle.GenerateText(5);
```

### 19. Middleware Methods (Run, Use, Map)
- **Run:** Terminating middleware
- **Use:** General middleware
- **Map:** Path-specific middleware
```csharp
app.Use(async (context, next) => { /* Runs for all requests */ });
app.Map("/api", app => { /* Only runs for /api paths */ });
app.Run(async context => { /* Terminating handler */ });
```

### 20. Naughty Strings Validation
- **Purpose:** Test for malicious input
- **Package:** `NaughtyStrings`
```csharp
var naughtyStrings = NaughtyStrings.GetAll(); // Test in QA
```

### 21. Reverse String Interpolation
- **Package:** `InterpolatedParser`
```csharp
var result = InterpolatedParser.Parse(
    "User: John, Age: 30",
    "User: {Name}, Age: {Age}"
);
```

### 22. Alias Any Type (C# 12)
```csharp
using IntList = System.Collections.Generic.List<int>;
using Point = (int X, int Y);
using MyAlias = Very.Long.Namespace.With.Complex<Generic, Type>;
```

### 23. DateTime vs DateTimeOffset
- **Use DateTimeOffset:** Always represents exact UTC moment
- **Avoid DateTime:** Ambiguous timezone handling
```csharp
// ✅ Good
DateTimeOffset now = DateTimeOffset.UtcNow;

// ❌ Bad
DateTime now = DateTime.Now; // Ambiguous
```

### 24. Architecture Tests
- **Package:** `NetArchTest.Rules`
```csharp
Types.InCurrentDomain()
    .That()
    .ResideInNamespace("MyApp.Domain")
    .Should()
    .BeSealed()
    .GetResult();
```

### 25. FluentValidation Alternatives
- **Option 1:** Pin version 7 (free forever)
- **Option 2:** Use AwesomeValidations (drop-in replacement)
- **Option 3:** Use Shouldly

### 26. JSON Schema Export (.NET 9)
```csharp
var schema = JsonSerializerOptions.Default
    .GetJsonSchemaNode(typeof(MyClass));
```

### 27. Parallel Async Loops (.NET 6)
```csharp
await Parallel.ForEachAsync(items, async (item, ct) => {
    await ProcessAsync(item);
});
```

### 28. Flaky Test Retry
```bash
dotnet tool install -g dotnet-retest
dotnet retest
# But actually fix your tests!
```

### 29. Params Keyword Enhancement (C# 13)
```csharp
// Can now use with:
public void Process(params Span<int> values) { }
public void Process(params List<string> values) { }
```

### 30. Threading Monitor Trick (Joke)
- **Do not use:** Creating custom Monitor class with same namespace

### 31. Private Field Naming Convention
- **Standard:** `_fieldName` with underscore
- **Why:** Distinguishes field from local variables
- **Alternative:** `this.fieldName`

### 32. HttpClient Correct Usage
```csharp
// ❌ Bad - socket exhaustion
using var client = new HttpClient();

// ❌ Bad - DNS issues
static readonly HttpClient client = new();

// ✅ Good - IHttpClientFactory
services.AddHttpClient();
var client = _httpClientFactory.CreateClient();
```

### 33. Snapshot Testing with Verify
```csharp
await Verify(result).UseDirectory("Snapshots");
// Compares against verified snapshot
```

### 34. Refit for API Clients
```csharp
[Get("/users/{id}")]
Task<User> GetUserAsync(int id);

var api = RestService.For<IUserApi>("https://api.example.com");
```

### 35. SemaphoreSlim with Timeout
```csharp
if (await _semaphore.WaitAsync(TimeSpan.FromSeconds(5))) {
    try { /* critical section */ }
    finally { _semaphore.Release(); }
}
```

### 36. ULID for Sortable IDs
```csharp
// Install Ulid package
var ulid = Ulid.NewUlid();
Guid guid = ulid.ToGuid();
```

### 37. Parallel Async Operations
```csharp
var task1 = Operation1Async();
var task2 = Operation2Async();
await Task.WhenAll(task1, task2);
var result1 = task1.Result;
var result2 = task2.Result;
```

### 38. Custom Service Scopes
```csharp
using (var scope = _serviceScopeFactory.CreateScope()) {
    var service = scope.ServiceProvider.GetService<MyService>();
    // Service lifetime matches scope
}
```

### 39. Primary Constructor Criticism
- **Issues:** No `readonly` support, conflicting naming conventions
- **Advice:** Use with caution, review code carefully

### 40. Units.NET for Unit Conversions
```csharp
// Install UnitsNet
Length meter = Length.FromMeters(10);
Length feet = meter.ToUnit(LengthUnit.Foot);
```

## Advanced Programming Concepts

### 41. DI Validation at Build Time
```csharp
var provider = services.BuildServiceProvider(new ServiceProviderOptions {
    ValidateScopes = true,
    ValidateOnBuild = true
});
```

### 42. Null Conditional Assignment (C# 14 Preview)
```csharp
// Future feature
person?.Name = "John"; // Assign only if person is not null
```

### 43. Dictionary Initialization Differences
```csharp
// Add syntax - throws on duplicate
var dict = new Dictionary<string, int> { ["key"] = 1 };

// Indexer syntax - overwrites duplicates silently
var dict = new Dictionary<string, int> { { "key", 1 } };
```

### 44. Class vs Record Primary Constructors
```csharp
// Record - creates public immutable properties
record Person(string Name, int Age);

// Class - parameters available in scope
class Person(string name, int age) {
    public string Name => name; // Manual property
}
```

### 45. Ref Structs
```csharp
// Must stay on stack
ref struct FastBuffer {
    private Span<byte> _buffer;
    // Cannot be used in async methods
    // Cannot be boxed
    // Cannot be stored on heap
}
```

### 46. In Keyword with Readonly Structs
```csharp
void Process(in ReadonlyStruct data) {
    // data is read-only reference
    // No defensive copy if struct is readonly
}
```

### 47. LINQ Deferred Execution
```csharp
var query = users.Where(u => u.Age > 18)
                 .OrderBy(u => u.Name);
// No execution yet - blueprint only

// Conditionally add filters
if (filterActive) {
    query = query.Where(u => u.IsActive);
}
// Executes only when enumerated
var result = query.ToList();
```

### 48. Stackalloc for Zero-Allocation Arrays
```csharp
Span<int> numbers = stackalloc int[5];
numbers[0] = 1; // Allocates on stack
// No garbage collection needed
// Limited stack space - use sparingly
```

### 49. Built-in Delegates
```csharp
Func<int, string> getString = i => i.ToString();
Action<string> log = s => Console.WriteLine(s);
Predicate<int> isPositive = i => i > 0;
// Delegates can be chained
Action logChain = Log1 + Log2 + Log3;
```

### 50. Method Override Options
```csharp
class Base {
    public abstract void MustImplement();
    public virtual void CanOverride() { }
    public void Hidden() { }
}
class Derived : Base {
    public override void MustImplement() { }
    public override void CanOverride() { }
    public new void Hidden() { } // Hides base method
}
```

### 51. Array Pool for Memory Reuse
```csharp
var pool = ArrayPool<int>.Shared;
int[] array = pool.Rent(100);
try {
    // Use array
    // Array may be larger than requested
}
finally {
    pool.Return(array, clearArray: true);
}
```

### 52. Async Void Dangers
```csharp
// ❌ Only for event handlers
async void Button_Click(object sender, EventArgs e) {
    await DoWorkAsync();
}

// ✅ Always return Task otherwise
async Task ProcessAsync() { }
```

### 53. Null Forgiving Operator
```csharp
// Tells compiler "this won't be null"
string? maybeNull = GetValue();
string definitelyNotNull = maybeNull!;

// Useful when property is initialized by framework
class MyClass {
    public string Name { get; set; } = null!;
}
```

### 54. Using Statement Internals
```csharp
// Using translates to try-finally
using (var file = File.OpenRead("data.txt")) {
    // Use file
} // Dispose called here

// C# 8 - using without braces
using var file = File.OpenRead("data.txt");
// Dispose at end of scope
```

### 55. With Expressions for Immutability
```csharp
record Person(string Name, int Age);
var original = new Person("John", 30);
var updated = original with { Age = 31 };
// Original unchanged
```

### 56. Extension Everything (C# 14 Preview)
```csharp
// Add instance methods, properties to any type
public extension MyExtensions for string {
    public bool IsValid() => !string.IsNullOrEmpty(this);
}
```

### 57. Collection Expressions
```csharp
// Old
int[] nums = new int[] { 1, 2, 3 };

// New (C# 12)
int[] nums = [1, 2, 3];
int[] combined = [.. nums, 4, 5, 6]; // Spread operator
```

### 58. Params with Span (C# 13)
```csharp
void Process(params Span<int> values) {
    // Stack allocation, no heap pressure
}
```

### 59. Target-Typed New (C# 9)
```csharp
// Old
Person person = new Person("John");

// New
Person person = new("John");
```

### 60. Top-Level Statements (C# 9)
```csharp
// Complete valid program
Console.WriteLine("Hello World!");
await Task.Delay(1000);
```

### 61. Pattern Matching with Not, And, Or
```csharp
if (value is not null and > 0) { }
switch (value) {
    case > 0 and < 100:
    case not string.Empty:
}
```

### 62. Nameof for Symbol Names
```csharp
// Instead of hardcoded strings
throw new ArgumentException(nameof(userId));

// Rename-safe - compiler updates automatically
```

### 63. Custom Deconstruct Methods
```csharp
class Person {
    public void Deconstruct(out string name, out int age) {
        name = Name;
        age = Age;
    }
}
var (name, age) = person;
```

### 64. Lambda Attributes (C# 10)
```csharp
var add = [Conditional("DEBUG")] (int a, int b) => a + b;
```

### 65. Range Pattern Matching
```csharp
if (value is > 0 and < 100) { }
switch (value) {
    case > 0 and <= 10: break;
    case < 0: break;
}
```

### 66. ArgumentNullException.ThrowIfNull (C# 10)
```csharp
// Old
if (input is null) throw new ArgumentNullException(nameof(input));

// New
ArgumentNullException.ThrowIfNull(input);
```

### 67. Expression-Bodied Constructors
```csharp
public Person(string name) => Name = name;
```

### 68. ValueTuples vs Tuple
```csharp
// ValueTuple - struct, lightweight
var tuple = (Name: "John", Age: 30);
var name = tuple.Name; // Named fields

// Tuple - class, heap allocation
Tuple<string, int> old = new("John", 30);
var item1 = old.Item1; // No named fields
```

### 69. Interesting C# Keywords
- **Keywords as identifiers:** `@in`, `@out`, `@short`, `@double`, `@long`
```csharp
var @class = "keyword used as variable";
```

### 70. Ref to In Parameter
```csharp
// Legal: passing ref to in parameter
void Method(in int x) { }
int y = 5;
Method(ref y); // ✅ Works

// Illegal: passing in to ref
void Method2(ref int x) { }
Method2(in y); // ❌ Compiler error
```

### 71. ValueTuple Comparison
```csharp
var a = (Name: "John", Age: 30);
var b = (Name: "John", Age: 30);
if (a == b) { } // True - compares values
```

### 72. CallerMemberName Attribute
```csharp
void Log(string message, 
         [CallerMemberName] string member = "",
         [CallerFilePath] string file = "",
         [CallerLineNumber] int line = 0) {
    Console.WriteLine($"{member}: {message}");
}
// Automatically fills caller info
```

### 73. Nullability Attributes
```csharp
[return: NotNull]
string GetValue() { }

[DoesNotReturn]
void ThrowError() { }

[MaybeNull]
string? GetMaybe() { }
```

### 74. Var Usage Guidelines
- **Use var:** When type is obvious
```csharp
var name = "John"; // Clearly string
var person = new Person(); // Clearly Person
```
- **Avoid var:** When type is hidden
```csharp
var data = GetData(); // What type is this?
```

### 75. Dynamic Keyword Considerations
```csharp
// ❌ Avoid in application code
dynamic obj = GetData();
var name = obj.Name; // Runtime resolution, no type safety

// ✅ Acceptable uses: COM interop, JSON (with proper serialization)
```

### 76. Exception Handling Best Practices
```csharp
// ❌ Bad - exceptions for control flow
try { user = GetUser(id); }
catch { /* user not found */ }

// ✅ Good - check first
if (UserExists(id)) { user = GetUser(id); }
```

### 77. Async Test Methods
```csharp
// ❌ Wrong
public async void Test() { }

// ✅ Correct
public async Task Test() { }
```

### 78. Regions Are Code Smells
- **Problem:** Hide poor structure
- **Solution:** Split classes, extract responsibilities
- **Avoid:** Using regions to organize

### 79. Obsolete Attribute Best Practices
```csharp
// Only use when:
// - Schedule for removal
// - Clear replacement exists
[Obsolete("Use NewMethod instead", true)] // With error
[Obsolete("Use NewMethod instead", false)] // With warning
```

### 80. Running Single C# Files
```bash
dotnet run app.cs
```
```csharp
// Can include configuration
// <Project Sdk="Microsoft.NET.Sdk">
// <PackageReference Include="Newtonsoft.Json" />
```

### 81. Global Usings (C# 10)
```csharp
// GlobalUsings.cs
global using System;
global using System.Collections.Generic;
global using static System.Math;
```

### 82. Nameof Performance
- **Zero runtime cost:** Resolved at compile time
- **Better than:** `typeof(T).Name`, reflection

### 83. InternalsVisibleTo for Testing
```csharp
[assembly: InternalsVisibleTo("MyProject.Tests")]
// Or in project file
// <InternalsVisibleTo Include="MyProject.Tests" />
```

### 84. Exception Filters
```csharp
try { }
catch (HttpException ex) when (ex.StatusCode == 404) {
    // Only catch 404 errors
}
```

### 85. Not Null Generic Constraint
```csharp
// T cannot be null
public class MyClass<T> where T : notnull { }
```

### 86. Scoped Keyword (C# 12)
```csharp
void Method(scoped Span<int> span) {
    // span cannot escape this method
}
```

### 87. Destructors/Finalizers
```csharp
// Rarely needed
~MyClass() {
    // Cleanup if Dispose not called
}
```

### 88. Property Patterns
```csharp
if (person is { Age: > 18, Name: { Length: > 0 } }) { }
```

### 89. List Patterns (C# 11)
```csharp
if (list is [1, 2, 3]) { }
if (list is [.., last]) { }
```

### 90. Sealed Override
```csharp
public sealed override void Method() { }
// Cannot be overridden further
```

### 91. Try Pattern for Parse Methods
```csharp
if (int.TryParse(input, out int result)) {
    // Use result
}
```

### 92. Preview Language Version
```xml
<LangVersion>preview</LangVersion>
```

### 93. Index from End (^) Operator
```csharp
var last = array[^1]; // Last element
var secondLast = array[^2]; // Second to last
```

### 94. Empty Array Literal
```csharp
// .NET 9
int[] empty = [];
// Same as Array.Empty<int>()
```

### 95. Async Enumerable (IAsyncEnumerable)
```csharp
await foreach (var item in GetDataStreamAsync()) {
    // Process as data arrives
}
```

### 96. Checked Keyword for Overflow
```csharp
checked {
    int max = int.MaxValue;
    int overflow = max + 1; // Throws OverflowException
}
```

### 97. Anonymous Type Equality
```csharp
var a = new { Name = "John" };
var b = new { Name = "John" };
a.Equals(b); // True (value equality)
```

### 98. Task.Yield for UI Responsiveness
```csharp
await Task.Yield(); // Return to caller, resume later
// Prevent UI freeze in long operations
```

### 99. Inlining Optimization
```csharp
[MethodImpl(MethodImplOptions.AggressiveInlining)]
int Add(int a, int b) => a + b;
```

### 100. Generated Regex (.NET 7+)
```csharp
[GeneratedRegex(@"\d+")]
partial Regex MyRegex();

// Usage: MyRegex().IsMatch(input)
// No runtime compilation, improved performance
```

---

## Summary Table: Quick Reference

| Category | Key Tips |
|----------|----------|
| **Performance** | Array.Empty, Enumeration caching, ArrayPool, stackalloc |
| **Async** | SemaphoreSlim, Task.WhenAll, IAsyncEnumerable |
| **Memory** | Span<T>, ref structs, using statements |
| **New Features** | Collection expressions, primary constructors, UUID v7 |
| **Best Practices** | Proper rethrowing, logging templates, HttpClient factory |