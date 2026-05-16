# `is` vs `as` Operators in C#

Both `is` and `as` are used for working with types in C#, especially when dealing with:

- inheritance
- interfaces
- polymorphism
- nullable reference types

They are related, but they serve different purposes.

---

# `is` Operator

The `is` operator checks whether an object is compatible with a specific type.

It returns:

- `true`
- `false`

## Basic Syntax

```csharp
value is TargetType
```

---

# Simple Example

```csharp
object item = "Hello";

bool result = item is string;
```

`result` becomes:

```csharp
true
```

because the object actually contains a `string`.

---

# Example with `false`

```csharp
object item = 42;

bool result = item is string;
```

Result:

```csharp
false
```

---

# Type Pattern Matching

Modern C# commonly uses `is` with pattern matching.

## Example

```csharp
object data = "CSharp";

if (data is string text)
{
    Console.WriteLine(text.Length);
}
```

What happens:

1. checks whether `data` is a `string`
2. if true:
   - creates variable `text`
   - automatically casts it

Equivalent old-style logic:

```csharp
if (data is string)
{
    string text = (string)data;
}
```

Pattern matching is cleaner and safer.

---

# `is not`

You can also negate the check.

```csharp
if (value is not null)
{
    Console.WriteLine("Has value");
}
```

---

# `as` Operator

The `as` operator attempts a safe cast.

Instead of throwing an exception when casting fails, it returns:

```csharp
null
```

---

# Basic Syntax

```csharp
value as TargetType
```

---

# Example

```csharp
object input = "example";

string? text = input as string;
```

Since the cast succeeds:

```csharp
text == "example"
```

---

# Failed Cast Example

```csharp
object input = 900;

string? text = input as string;
```

The cast fails, so:

```csharp
text == null
```

No exception is thrown.

---

# Why `as` Exists

Normal casting can throw exceptions.

## Explicit Cast

```csharp
object value = 50;

string text = (string)value;
```

This throws:

```text
InvalidCastException
```

because `50` is not a string.

---

## Safer Version with `as`

```csharp
object value = 50;

string? text = value as string;

if (text != null)
{
    Console.WriteLine(text);
}
```

No exception occurs.

---

# Major Difference

| Operator | Purpose | Result on Failure |
|---|---|---|
| `is` | Checks type compatibility | `false` |
| `as` | Attempts safe cast | `null` |

---

# Real Mental Model

## `is`

> “Is this object compatible with this type?”

Returns:

```csharp
true / false
```

---

## `as`

> “Try converting this object to this type.”

Returns:

- converted object
- or `null`

---

# Example with Inheritance

## Classes

```csharp
class Vehicle
{
}

class Car : Vehicle
{
    public void Drive()
    {
        Console.WriteLine("Driving");
    }
}
```

---

# Using `is`

```csharp
Vehicle vehicle = new Car();

if (vehicle is Car)
{
    Console.WriteLine("This is a car");
}
```

---

# Using `as`

```csharp
Vehicle vehicle = new Car();

Car? car = vehicle as Car;

if (car != null)
{
    car.Drive();
}
```

---

# Pattern Matching vs `as`

## Older Style (`as`)

```csharp
Car? car = vehicle as Car;

if (car != null)
{
    car.Drive();
}
```

---

## Modern Style (`is` Pattern Matching)

```csharp
if (vehicle is Car car)
{
    car.Drive();
}
```

Modern C# usually prefers this version because:

- shorter
- cleaner
- combines checking + casting

---

# Important Limitation of `as`

`as` only works with:

- reference types
- nullable value types

It does NOT work with non-nullable value types.

---

# Invalid Example

```csharp
object number = 12;

int value = number as int;
```

❌ Compilation error

because `int` is a non-nullable value type.

---

# Valid Nullable Version

```csharp
object number = 12;

int? value = number as int?;
```

✅ Valid

---

# `is` Works Fine with Value Types

```csharp
object number = 12;

if (number is int value)
{
    Console.WriteLine(value);
}
```

This is one reason modern code often prefers `is`.

---

# Null Behavior

## `is`

```csharp
object? item = null;

bool result = item is string;
```

Result:

```csharp
false
```

---

## `as`

```csharp
object? item = null;

string? text = item as string;
```

Result:

```csharp
text == null
```

---

# Common Usage Patterns

# Using `is`

## Type Checking

```csharp
if (shape is Circle)
{
    ...
}
```

---

## Pattern Matching

```csharp
if (shape is Circle circle)
{
    Console.WriteLine(circle.Radius);
}
```

---

## Null Checks

```csharp
if (user is not null)
{
    ...
}
```

---

# Using `as`

## Safe Conversion

```csharp
Button? button = control as Button;
```

---

## Optional Usage

```csharp
var manager = employee as Manager;
```

---

# Side-by-Side Comparison

## Using `is`

```csharp
object entity = "sample";

if (entity is string text)
{
    Console.WriteLine(text.ToUpper());
}
```

---

## Using `as`

```csharp
object entity = "sample";

string? text = entity as string;

if (text != null)
{
    Console.WriteLine(text.ToUpper());
}
```

Both work.

The `is` pattern version is usually preferred today.

---

# Performance Notes

In modern C#, performance differences are generally tiny.

Choose based on:

- readability
- clarity
- intent

not micro-optimization.

---

# Best Practice

## Prefer Modern Pattern Matching

Usually prefer:

```csharp
if (obj is Customer customer)
{
    ...
}
```

instead of:

```csharp
Customer? customer = obj as Customer;

if (customer != null)
{
    ...
}
```

because it is:

- cleaner
- more expressive
- harder to misuse

---

# Quick Reference Table

| Feature | `is` | `as` |
|---|---|---|
| Purpose | Type check | Safe cast |
| Returns | `bool` | object or `null` |
| Failure result | `false` | `null` |
| Performs cast | With pattern matching | Yes |
| Throws exception on failure | No | No |
| Works with value types | Yes | Only nullable value types |
| Common modern usage | Very common | Less common today |

---

# Mini Examples

## `is`

```csharp
object value = 18;

if (value is int number)
{
    Console.WriteLine(number * 2);
}
```

---

## `as`

```csharp
object value = "admin";

string? role = value as string;

if (role != null)
{
    Console.WriteLine(role);
}
```

---

# Easy Rule to Remember

## Use `is` when:

- you want to check a type
- you want pattern matching
- you want clean modern syntax

---

## Use `as` when:

- you specifically want a nullable result instead of a boolean
- you want a safe cast without exceptions

