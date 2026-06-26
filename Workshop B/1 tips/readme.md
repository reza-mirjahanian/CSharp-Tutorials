# Part 1 (Tips 1–8)

---

# Tip 1 — Return Empty Collections Efficiently

## The Problem

Methods frequently return empty collections when no data exists.

Many developers write:

```csharp
return new List<Customer>();
```

or

```csharp
return new Customer[0];
```

Although these look harmless, **every call allocates a new object on the heap**.

In high-throughput applications this causes:

* Extra memory allocations
* More Garbage Collection (GC)
* Reduced performance
* Application pauses caused by GC

---

## Better Solutions

### Arrays

Use:

```csharp
return Array.Empty<Customer>();
```

instead of

```csharp
return new Customer[0];
```

---

### IEnumerable

Use:

```csharp
return Enumerable.Empty<Customer>();
```

instead of

```csharp
return new List<Customer>();
```

---

## Why It Is Faster

Both methods internally return **one cached singleton instance**.

Instead of:

```
Call 1 -> allocate
Call 2 -> allocate
Call 3 -> allocate
```

They do:

```
Application starts
    ↓
Allocate once
    ↓
Reuse forever
```

---

## Best Practice

✔ Use `Array.Empty<T>()` for arrays.

✔ Use `Enumerable.Empty<T>()` for enumerable return values.

Avoid manually caching your own empty collections unless absolutely necessary.

---

# Tip 2 — Rethrow Exceptions Correctly

## The Problem

A common pattern:

```csharp
try
{
    DoWork();
}
catch (Exception ex)
{
    Log(ex);
    throw ex;
}
```

This **destroys the original stack trace**.

---

## Why Stack Traces Matter

The stack trace tells you:

* where the exception originated
* call chain
* execution flow

Without it debugging becomes significantly harder.

---

## Correct Way

```csharp
try
{
    DoWork();
}
catch (Exception ex)
{
    Log(ex);

    throw;
}
```

Notice:

```
throw;
```

instead of

```
throw ex;
```

---

## Difference

### Wrong

```
throw ex;
```

Result:

```
Stack trace starts HERE
```

Original location is lost.

---

### Correct

```
throw;
```

Result:

```
Original exception
Original stack trace
Original call chain
```

Everything is preserved.

---

## Rule

Use

```csharp
throw;
```

unless you're intentionally wrapping the exception:

```csharp
throw new BusinessException("...", ex);
```

---

# Tip 3 — Async Locking with SemaphoreSlim

## Problem

The classic lock keyword works only with synchronous code.

This is illegal:

```csharp
lock (_lock)
{
    await SaveAsync();
}
```

Compiler error.

---

## Solution

Use

```csharp
SemaphoreSlim
```

---

## Example

```csharp
private readonly SemaphoreSlim _lock = new(1, 1);

public async Task SaveAsync()
{
    await _lock.WaitAsync();

    try
    {
        await WriteToDatabaseAsync();
    }
    finally
    {
        _lock.Release();
    }
}
```

---

## Why It Works

Constructor:

```csharp
new SemaphoreSlim(1, 1)
```

means:

Only **one execution** may enter simultaneously.

Everyone else waits asynchronously.

---

## Always Release

Never write:

```csharp
await _lock.WaitAsync();

DoSomething();

_lock.Release();
```

If an exception occurs:

```
Release()
```

never executes.

Eventually:

```
Deadlock
```

Always use:

```csharp
try
{
}
finally
{
    _lock.Release();
}
```

---

## When to Use

* async methods
* API request synchronization
* background workers
* file access
* shared resources

---

# Tip 4 — Beware of Multiple LINQ Enumeration

## The Hidden Trap

LINQ uses **deferred execution**.

This means:

```csharp
var numbers = Enumerable.Range(1,10)
                        .Select(x =>
                        {
                            Console.WriteLine(x);
                            return x;
                        });
```

Nothing executes yet.

---

Later:

```csharp
numbers.Count();

numbers.All(x => x > 0);
```

Output:

```
1
2
3
...

1
2
3
...
```

Entire query runs **twice**.

---

## Why This Is Bad

Imagine instead of numbers you have:

```csharp
db.Users
```

or

```csharp
ReadCsv()
```

or

```csharp
ReadBigFile()
```

Now you're doing:

* two database queries
* two API calls
* two file reads
* double CPU work

---

## Fix

Materialize once.

```csharp
var list = numbers.ToList();

list.Count();

list.All(x => x > 0);
```

Now:

```
Enumeration occurs once
```

---

## Materialization Options

```csharp
ToList()
```

```csharp
ToArray()
```

Choose whichever fits the scenario.

---

## Rule

If you're going to iterate multiple times, materialize once.

---

# Tip 5 — Use C# REPL Instead of Creating Test Projects

Sometimes you simply want to test:

* syntax
* LINQ
* Regex
* APIs
* language features

Creating a Console App every time is slow.

---

## Solution

Use the C# REPL.

Install globally:

```bash
dotnet tool install --global csharprepl
```

Then:

```bash
csharp
```

---

## Advantages

* Instant execution
* IntelliSense
* Autocomplete
* Experiment quickly
* Install NuGet packages
* Test ASP.NET APIs

Perfect for learning and experimentation.

---

# Tip 6 — Access List Internals Using CollectionsMarshal

## Background

`List<T>` is internally backed by an array.

Normally that array is hidden.

---

## New Option

```csharp
CollectionsMarshal.AsSpan(list)
```

returns a `Span<T>` over the internal array.

Example:

```csharp
Span<int> span =
    CollectionsMarshal.AsSpan(numbers);
```

---

## Why Use It

`Span<T>` avoids:

* iterator overhead
* extra allocations
* bounds checking optimizations

Useful for:

* high-performance code
* parsing
* numerical processing

---

## Danger

While using the span:

**Do NOT modify the List.**

Example:

```csharp
span = CollectionsMarshal.AsSpan(list);

list.Add(10);
```

Now the span may point to invalid memory after the list resizes.

Unlike normal enumeration:

```
No InvalidOperationException
```

Behavior becomes unsafe.

---

## Use Only When

* performance matters
* you understand Span<T>
* list won't change during processing

---

# Tip 7 — Use Structured Logging Instead of String Interpolation

## Wrong

```csharp
logger.LogInformation(
    $"User {id} logged in");
```

Problems:

* Creates unnecessary strings
* Allocates memory
* Harder to search logs
* Loses structured properties

---

## Correct

```csharp
logger.LogInformation(
    "User {UserId} logged in",
    id);
```

---

## Why Structured Logging Matters

Logging systems (e.g., Serilog, Seq, Elasticsearch) store:

```
Message:
User 15 logged in

Properties:
UserId = 15
```

Now you can query:

```
UserId = 15
```

instead of performing text searches.

---

## Benefits

* Less memory allocation
* Faster logging
* Better filtering
* Better analytics
* Better dashboards

---

## Rule

Treat the first parameter as a **message template**, not a formatted string.

---

# Tip 8 — Empty Types in C# 12

C# 12 introduced a small but useful syntax improvement.

Previously:

```csharp
public interface IMarker
{
}
```

or

```csharp
public class Marker
{
}
```

Even empty types required braces.

---

## New Syntax

Now you can write:

```csharp
public interface IMarker;
```

or

```csharp
public class Marker;
```

---

## Benefits

* Cleaner code
* Less visual noise
* Ideal for:

  * marker interfaces
  * marker classes
  * empty structs
  * placeholder types

---

## Typical Use Cases

Marker interface:

```csharp
public interface IAssemblyMarker;
```

Marker class:

```csharp
public sealed class InfrastructureAssembly;
```

These types often exist only to identify an assembly or participate in dependency injection, so the concise syntax makes the intent clearer.

---

# Part 2 (Tips 9–16)

---

# Tip 9 — `ToList()` vs `ToArray()`

## Overview

When materializing an `IEnumerable<T>`, the two most common options are:

```csharp
var list = items.ToList();
```

or

```csharp
var array = items.ToArray();
```

Choosing between them depends on **both functionality and performance**.

---

## Functional Differences

### Return a List When

The consumer needs to:

* Add items
* Remove items
* Change collection size

Example:

```csharp
List<Customer> customers = GetCustomers().ToList();

customers.Add(new Customer());
```

---

### Return an Array When

The collection size is fixed.

The consumer only needs to:

* Iterate
* Read
* Modify existing values

Example:

```csharp
Customer[] customers = GetCustomers().ToArray();

customers[0] = updatedCustomer;
```

---

## Performance Considerations

Since `List<T>` internally wraps an array, many assume `ToArray()` is always faster.

That isn't always true.

Historically:

* Small collections → negligible difference
* Large collections → performance varies depending on .NET version

Example benchmark:

```
10,000 items

ToList()
   slightly faster

ToArray()
   slightly slower
```

---

## .NET 9 Improvement

.NET 9 introduces significant optimizations.

Now:

```
ToArray()
```

became considerably faster and is often the preferred option for fixed-size collections.

---

## Recommendation

Choose based on semantics first.

| Need                        | Recommendation |
| --------------------------- | -------------- |
| Fixed-size collection       | `ToArray()`    |
| Collection will grow/shrink | `ToList()`     |

---

# Tip 10 — Assembly Marker Types

## The Problem

Libraries such as:

* MediatR
* AutoMapper
* FluentValidation

often require:

```csharp
services.AddMediatR(typeof(SomeType));
```

or

```csharp
services.AddAutoMapper(typeof(SomeType));
```

The type itself isn't important.

The library simply needs to locate the **assembly**.

---

## Common Approach

Many developers write:

```csharp
typeof(Program)
```

This works, but it's unclear why `Program` is being referenced.

---

## Better Approach

Create a dedicated marker type.

Example:

```csharp
public interface IApplicationAssembly;
```

or

```csharp
public sealed class ApplicationAssembly;
```

Registration becomes:

```csharp
services.AddMediatR(typeof(IApplicationAssembly));
```

---

## Benefits

* Self-documenting
* Clear intent
* Easy to locate
* Independent of `Program.cs`
* Easier to maintain

---

## Best Practice

Create one marker type per project:

```
Application
Infrastructure
Domain
API
```

Each project exposes one assembly marker.

---

# Tip 11 — `StringSyntaxAttribute`

## Problem

Many APIs accept strings that actually represent something else.

Examples:

* Regex
* JSON
* XML
* URLs
* Date formats

Without context, the IDE treats them as ordinary strings.

---

## Solution

Use:

```csharp
[StringSyntax(StringSyntaxAttribute.Regex)]
```

Example:

```csharp
public void Validate(
    [StringSyntax(StringSyntaxAttribute.Regex)]
    string pattern)
{
}
```

---

## Benefits

Your IDE now understands the parameter represents regex.

You receive:

* syntax highlighting
* validation
* IntelliSense improvements
* developer assistance

---

## Other Supported Syntaxes

Examples include:

* Regex
* Json
* Uri
* DateTime format strings

---

## When to Use

Whenever a string has a specific meaning.

---

# Tip 12 — Primary Constructors and Readonly Fields

## What Are Primary Constructors?

Instead of:

```csharp
public class UserService
{
    private readonly IRepository _repository;

    public UserService(IRepository repository)
    {
        _repository = repository;
    }
}
```

You can write:

```csharp
public class UserService(IRepository repository)
{
}
```

Much cleaner.

---

## The Drawback

Primary constructors currently don't provide a clean way to create explicit `readonly` backing fields automatically.

Traditional constructor:

```csharp
private readonly IRepository _repository;
```

Primary constructor:

```
repository
```

is merely a constructor parameter in scope.

---

## Why This Matters

Immutability is important.

Readonly fields:

* prevent accidental reassignment
* improve code safety
* express intent

Current primary constructors cannot fully replace this pattern.

---

## IDE Warning

Many IDEs suggest converting constructors into primary constructors.

Don't blindly accept the suggestion.

Verify that behavior remains identical.

---

## Recommendation

Use primary constructors when:

* classes are simple
* immutability isn't compromised

Continue using traditional constructors when explicit readonly fields improve clarity.

---

# Tip 13 — UUID Version 7

## Background

Traditional GUIDs in .NET are effectively UUID Version 4.

Characteristics:

* random
* unique
* not sortable

---

## Database Problem

Random GUIDs make terrible clustered keys.

Insertion order becomes random:

```
A
Z
B
Q
L
```

Result:

* page splits
* fragmentation
* slower indexes

---

## UUID Version 7

.NET 9 introduces:

```csharp
Guid.CreateVersion7();
```

UUID v7 embeds timestamp information into the identifier.

Result:

```
Earlier IDs
↓

Later IDs
```

They become naturally sortable.

---

## Benefits

* Better database performance
* Less fragmentation
* Sequential inserts
* No third-party package required

---

## Example

```csharp
Guid id = Guid.CreateVersion7();
```

---

## Recommended Use Cases

* Database primary keys
* Distributed systems
* Event sourcing
* Cloud applications

---

# Tip 14 — Smallest Valid C# Program

## Before C# 9

A console application required:

```csharp
public class Program
{
    public static void Main(string[] args)
    {
    }
}
```

Lots of ceremony.

---

## Top-Level Statements

C# 9 allows:

```csharp
Console.WriteLine("Hello");
```

The compiler generates `Program` and `Main()` automatically.

---

## Interesting Fact

The smallest valid C# program is simply:

```csharp
;
```

A single semicolon.

It compiles successfully because it's treated as an empty statement.

---

## Benefit

Less boilerplate for:

* console apps
* demos
* tutorials
* experiments

---

# Tip 15 — Cancellation Tokens in ASP.NET Core

## Common Mistake

Developers create their own token:

```csharp
var token = new CancellationToken();
```

This token isn't connected to the HTTP request.

---

## Correct Approach

Allow ASP.NET Core to inject it.

Controller example:

```csharp
public async Task<IActionResult> Get(
    CancellationToken cancellationToken)
{
}
```

Minimal API:

```csharp
app.MapGet("/users",
    async (CancellationToken cancellationToken) =>
{
});
```

---

## What Happens

If the client:

* closes the browser
* refreshes the page
* disconnects

ASP.NET automatically cancels the request token.

---

## Pass It Down

Don't stop at the controller.

Pass it through every async call.

Example:

```csharp
await repository.GetUsersAsync(cancellationToken);
```

then

```csharp
await context.Users
    .ToListAsync(cancellationToken);
```

---

## Benefits

* Saves CPU
* Saves database resources
* Stops unnecessary work
* Improves scalability

---

## Best Practice

Every async method that can be canceled should accept:

```csharp
CancellationToken cancellationToken
```

---

# Tip 16 — Collection Expressions (C# 12)

## Before C# 12

Array:

```csharp
int[] numbers = new[]
{
    1,
    2,
    3
};
```

List:

```csharp
List<int> numbers =
    new()
    {
        1,
        2,
        3
    };
```

Dictionary:

```csharp
var dict = new Dictionary<int,string>();
```

Different syntax everywhere.

---

## New Collection Expressions

Now you can simply write:

```csharp
int[] numbers = [1, 2, 3];
```

or

```csharp
List<int> numbers = [1, 2, 3];
```

The compiler chooses the appropriate collection type based on the target.

---

## Advantages

* Cleaner syntax
* Less boilerplate
* Easier to read
* Consistent across collection types

---

## Empty Collections

Instead of:

```csharp
Array.Empty<int>()
```

or

```csharp
new List<int>()
```

you can write:

```csharp
int[] empty = [];
```

or

```csharp
List<int> empty = [];
```

---

## Collection Composition

Collection expressions also support the spread operator.

Example:

```csharp
int[] first = [1, 2];
int[] second = [3, 4];

int[] all = [..first, ..second];
```

Result:

```text
1
2
3
4
```

---

## Best Practice

Prefer collection expressions for new code targeting **C# 12+**, as they reduce boilerplate while remaining expressive and type-safe.

---

# Part 3 (Tips 17–24)

---

# Tip 17 — Keep NuGet Packages Updated with `dotnet-outdated`

## The Problem

Over time, projects accumulate outdated NuGet packages.

Outdated dependencies can lead to:

* Missing bug fixes
* Security vulnerabilities
* Compatibility issues
* Missed performance improvements

Many developers only check updates through Visual Studio's NuGet Package Manager.

---

## Solution

Use the **`dotnet-outdated`** CLI tool.

Install globally:

```bash
dotnet tool install --global dotnet-outdated-tool
```

Run it:

```bash
dotnet outdated
```

---

## What It Shows

The tool reports:

* Current version
* Latest version
* Available upgrades
* Project/package relationships

Example:

```text
Package              Current   Latest

Serilog              3.0.1     4.0.0
FluentValidation     11.8      12.0
```

---

## Upgrade Packages

Upgrade automatically:

```bash
dotnet outdated --upgrade
```

---

## Version Locking

You can configure upgrade rules to avoid jumping to major versions unexpectedly.

Example:

```text
11.x → 11.y
```

instead of

```text
11.x → 12.x
```

This reduces the risk of breaking changes.

---

## Best Practice

Run `dotnet outdated` regularly in:

* CI pipelines
* Monthly maintenance
* Before releases

---

# Tip 18 — Generate Realistic Placeholder Text with Waffle Generator

## The Problem

Developers often use **Lorem Ipsum**.

Example:

```text
Lorem ipsum dolor sit amet...
```

The problem:

* Doesn't resemble real application content
* Poor for UI testing
* Doesn't expose layout issues
* Doesn't mimic realistic user input

---

## Better Approach

Use **Waffle Generator**.

It produces text that:

* Looks like real English
* Has realistic sentence lengths
* Can generate HTML or Markdown
* Is customizable

---

## Example

Instead of:

```text
Lorem ipsum...
```

You get something like:

```text
Our platform provides secure authentication
for distributed cloud services...
```

This is much closer to real-world content.

---

## Installation

```bash
dotnet add package WaffleGenerator
```

---

## Example Usage

```csharp
var html = WaffleEngine.Html();
```

or

```csharp
var markdown = WaffleEngine.Markdown();
```

---

## Integration with Bogus

Works nicely with **Bogus** to generate realistic fake object data.

Example:

```csharp
var faker = new Faker<User>()
    .RuleFor(x => x.Description,
        _ => WaffleEngine.Text());
```

Useful for:

* Seed data
* UI previews
* Demo applications
* Automated testing

---

# Tip 19 — Understanding `Run`, `Use`, and `Map` in ASP.NET Core

The ASP.NET Core request pipeline is built from middleware.

Three methods define most of that pipeline.

---

## `Use()`

Adds general-purpose middleware.

Example:

```csharp
app.UseAuthentication();

app.UseAuthorization();
```

Middleware can:

* Execute before the next middleware
* Execute after the next middleware
* Continue the pipeline

Flow:

```text
Request
    ↓
Middleware A
    ↓
Middleware B
    ↓
Middleware C
```

---

## `Run()`

Adds **terminal middleware**.

Example:

```csharp
app.Run(async context =>
{
    await context.Response.WriteAsync("Done");
});
```

After `Run()` executes:

```text
Pipeline stops
```

No later middleware runs.

---

## `Map()`

Creates a separate branch for specific request paths.

Example:

```csharp
app.Map("/admin", admin =>
{
    admin.Run(async ctx =>
    {
        await ctx.Response.WriteAsync("Admin");
    });
});
```

Only requests beginning with:

```text
/admin
```

reach that branch.

---

## Middleware Order Matters

Example:

```csharp
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
```

Changing the order may break authentication or routing.

---

## Rule

Think of the pipeline like this:

```text
Use  -> Continue

Run  -> Stop

Map  -> Branch
```

---

# Tip 20 — Test Against Naughty Strings

## What Are Naughty Strings?

A collection of strings designed to expose:

* Crashes
* Encoding bugs
* Injection vulnerabilities
* Validation mistakes
* Unicode issues

Examples:

```text
NULL

😀

<script>

../../

DROP TABLE

𐍈𠜎
```

---

## Why They Matter

Many applications validate only:

* Length
* Empty values
* Required fields

They forget about malformed or unusual inputs.

---

## Install

```bash
dotnet add package NaughtyStrings
```

---

## Example

```csharp
foreach (var input in NaughtyStrings.All)
{
    Validate(input);
}
```

---

## Use Cases

Perfect for:

* QA
* End-to-end testing
* Input validation
* API testing
* Security testing

---

## Best Practice

Include Naughty Strings in automated test suites to uncover edge cases before production.

---

# Tip 21 — Reverse String Interpolation with InterpolatedParser

## The Problem

Suppose you receive:

```text
User 15 created Order 92
```

You want:

```text
UserId = 15
OrderId = 92
```

Normally you'd write:

* Regex
* String.Split
* Manual parsing

---

## Solution

Use **InterpolatedParser**.

Instead of writing a regex:

```regex
User (\d+) created Order (\d+)
```

You write a template.

---

## Example

```csharp
InterpolatedParser.Parse(
    input,
    $"User {userId} created Order {orderId}");
```

Result:

```text
userId = 15
orderId = 92
```

---

## Why It's Interesting

Think of it as **reverse string interpolation**.

Instead of:

```csharp
$"User {id}"
```

creating text,

it extracts values from matching text.

---

## Best Use Cases

* Log parsing
* CLI parsing
* Configuration parsing
* Human-readable formats

---

# Tip 22 — Alias Any Type (C# 12)

## Before C# 12

Aliases were limited.

Example:

```csharp
using Json = System.Text.Json;
```

---

## New Capability

You can alias virtually any type.

Example:

```csharp
using UserMap =
    Dictionary<Guid, User>;
```

Now:

```csharp
UserMap users = [];
```

---

## Benefits

### 1. Simplify Complex Types

Instead of:

```csharp
Dictionary<Guid,
    List<Tuple<int,string>>>
```

use:

```csharp
UserDictionary
```

---

### 2. Resolve Name Conflicts

Example:

```text
MyCompany.User

ThirdParty.User
```

Aliases remove ambiguity.

---

### 3. Improve Readability

Bad:

```csharp
Dictionary<Guid,List<Order>>
```

Better:

```csharp
OrderLookup
```

Intent becomes immediately obvious.

---

## Best Practice

Use aliases for:

* Frequently repeated generic types
* Long nested types
* Domain-specific names

Avoid excessive aliasing for simple types, as it can reduce readability.

---

# Tip 23 — Prefer `DateTimeOffset` Over `DateTime`

## The Problem

`DateTime` stores:

```text
Date + Time
```

but often lacks timezone context.

Example:

```csharp
DateTime.Now
```

Could represent:

```text
08:00 London

08:00 Berlin

08:00 Tokyo
```

These are completely different moments.

---

## Better Type

```csharp
DateTimeOffset
```

stores:

```text
Date

Time

UTC Offset
```

Example:

```text
2026-06-26

08:00

+02:00
```

Now the exact instant is known.

---

## Why It Matters

Business applications often involve:

* Multiple countries
* Customers worldwide
* Scheduling
* Auditing
* Logging

Using plain `DateTime` can introduce subtle timezone bugs.

---

## Example

```csharp
DateTimeOffset created =
    DateTimeOffset.UtcNow;
```

---

## Recommendation

Use:

* `DateTimeOffset` for persisted business timestamps
* UTC where appropriate
* `DateTime` only when timezone is genuinely irrelevant (for example, a recurring daily alarm at 9:00 AM).

---

# Tip 24 — Enforce Architecture with NetArchTest

## The Problem

As projects grow, architectural boundaries become harder to enforce.

Examples of unwanted dependencies:

```text
API
    ↓
Infrastructure
    ↓
Domain
```

or

```text
Domain
    ↓
Infrastructure
```

These violations may go unnoticed in code reviews.

---

## Solution

Use **NetArchTest** to create architecture tests.

Install:

```bash
dotnet add package NetArchTest.Rules
```

---

## Example Rule

Ensure Domain does **not** depend on Infrastructure:

```csharp
Types.InAssembly(domainAssembly)
    .ShouldNot()
    .HaveDependencyOn("Infrastructure");
```

---

## Other Rules You Can Enforce

* Classes must be sealed
* Classes must implement interfaces
* Namespace restrictions
* Layer dependency rules
* Naming conventions
* Inheritance policies

---

## Benefits

Architecture becomes **automatically testable**.

Instead of relying solely on documentation or code reviews, CI can fail immediately when a developer introduces an architectural violation.

---

## Best Practice

Include architecture tests alongside unit tests so design rules remain enforceable as the codebase evolves.

---

