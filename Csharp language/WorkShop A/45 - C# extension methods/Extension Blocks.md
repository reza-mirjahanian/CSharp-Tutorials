# C# Extension Blocks

**Extension blocks** are a modern way to add extra members to an existing type **without editing that type’s original source code**.

They let you group extension members in a cleaner, more natural style than classic extension methods.

---

# Why Extension Blocks Matter

Sometimes you want to add helpful functionality to a type you do not own, such as:

- `string`
- `DateTime`
- `List<T>`
- a library class from another package

Instead of creating a wrapper class or changing the original type, you can define **extension members** externally.

With extension blocks, those members are organized in a way that feels more like they belong to the type.

---

# Basic Idea

An extension block is declared inside a **static class**, and it targets a specific type.

Inside the block, you can define members that act as if they are part of that target type.

## Example

```csharp
public static class TextHelpers
{
    extension(string text)
    {
        public int WordCount()
        {
            return text.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length;
        }

        public string SurroundWith(string prefix, string suffix)
        {
            return $"{prefix}{text}{suffix}";
        }
    }
}
```

## Usage

```csharp
string title = "C# extension blocks are useful";

int totalWords = title.WordCount();
string wrapped = title.SurroundWith("[", "]");
```

In this example:

- `text` is the receiver parameter
- `WordCount()` and `SurroundWith(...)` behave like instance members of `string`
- `string` itself is not modified

---

# Receiver Parameter

The value inside `extension(...)` is the **receiver**.

```csharp
extension(string message)
```

Here:

- `string` is the target type
- `message` is the name used inside the block to refer to the current instance

Think of it like `this` in instance methods, except you choose the name.

---

# Comparing with Classic Extension Methods

## Classic Style

```csharp
public static class TextHelpers
{
    public static int CountSentences(this string text)
    {
        return text.Split('.', StringSplitOptions.RemoveEmptyEntries).Length;
    }
}
```

## Extension Block Style

```csharp
public static class TextHelpers
{
    extension(string text)
    {
        public int CountSentences()
        {
            return text.Split('.', StringSplitOptions.RemoveEmptyEntries).Length;
        }
    }
}
```

## Main Difference

With classic extension methods:

- each method needs a `this` parameter
- related extension members can feel scattered

With extension blocks:

- the receiver is declared once
- related members are grouped together more clearly

---

# Important Rules

## 1. Must Be Inside a Static Class

Extension blocks are declared within a `static` class.

```csharp
public static class NumberExtensions
{
    extension(int value)
    {
        public bool IsPositive() => value > 0;
    }
}
```

---

## 2. They Extend an Existing Type

You can target:

- primitive types like `int`
- reference types like `string`
- generic types like `List<T>`
- custom classes and structs

---

## 3. They Do Not Actually Modify the Original Type

Even though usage looks like an instance member call:

```csharp
int score = 15;
bool ok = score.IsPositive();
```

the original `int` type remains unchanged.

This is just compiler-supported syntax.

---

## 4. Namespace Must Be In Scope

To use extension members, the namespace containing the extension block must be imported.

```csharp
using MyApp.Extensions;
```

If the namespace is missing, the member will not appear.

---

# Simple Example with `int`

```csharp
namespace UtilityPack;

public static class NumberExtensions
{
    extension(int number)
    {
        public bool IsEven()
        {
            return number % 2 == 0;
        }

        public int Cube()
        {
            return number * number * number;
        }
    }
}
```

## Usage

```csharp
using UtilityPack;

int amount = 6;

bool even = amount.IsEven();
int cube = amount.Cube();
```

---

# Example with `string`

```csharp
namespace UtilityPack;

public static class StringExtensions
{
    extension(string value)
    {
        public string ReverseText()
        {
            char[] buffer = value.ToCharArray();
            Array.Reverse(buffer);
            return new string(buffer);
        }

        public bool IsBlank()
        {
            return string.IsNullOrWhiteSpace(value);
        }
    }
}
```

## Usage

```csharp
using UtilityPack;

string label = "Hello";

string reversed = label.ReverseText();
bool blank = label.IsBlank();
```

---

# Example with Generic Types

You can also extend generic collections.

```csharp
namespace UtilityPack;

public static class CollectionExtensions
{
    extension<T>(List<T> items)
    {
        public bool HasElements()
        {
            return items.Count > 0;
        }

        public T? SecondOrFallback()
        {
            return items.Count > 1 ? items[1] : default;
        }
    }
}
```

## Usage

```csharp
using UtilityPack;

var colors = new List<string> { "red", "green", "blue" };

bool hasAny = colors.HasElements();
string? second = colors.SecondOrFallback();
```

---

# Instance Extensions vs Static Extensions

Extension blocks can support both **instance-like** and **static-like** members, depending on how they are declared.

## Instance Extension Members

These are called on an object instance:

```csharp
extension(decimal price)
{
    public decimal AddTax(decimal rate)
    {
        return price + (price * rate);
    }
}
```

Usage:

```csharp
decimal subtotal = 200m;
decimal total = subtotal.AddTax(0.15m);
```

---

## Static Extension Members

These are associated with the type itself rather than an instance.

```csharp
extension(DateOnly)
{
    public static DateOnly TodayUtc()
    {
        return DateOnly.FromDateTime(DateTime.UtcNow);
    }
}
```

Usage:

```csharp
DateOnly today = DateOnly.TodayUtc();
```

> Static extension members make a type appear to have extra static functionality.

---

# Extension Properties

Extension blocks can contain more than just methods.

They can also define **properties**.

## Example

```csharp
public static class TextMetrics
{
    extension(string text)
    {
        public int LengthWithoutSpaces
        {
            get
            {
                return text.Count(ch => ch != ' ');
            }
        }
    }
}
```

## Usage

```csharp
string phrase = "extension blocks";

int size = phrase.LengthWithoutSpaces;
```

---

# Extension Operators

Extension blocks may also support operators in appropriate scenarios.

This allows even more natural integration with a type.

```csharp
public static class VectorExtras
{
    extension(Vector2D value)
    {
        public double Magnitude
        {
            get { return Math.Sqrt(value.X * value.X + value.Y * value.Y); }
        }
    }
}
```

> Operator-related support depends on the language version and exact feature set available.

---

# A More Realistic Example

Imagine a custom type:

```csharp
public class Invoice
{
    public decimal Subtotal { get; set; }
    public decimal Discount { get; set; }
}
```

Now add useful behavior from outside the class:

```csharp
public static class InvoiceExtensions
{
    extension(Invoice invoice)
    {
        public decimal FinalAmount()
        {
            return invoice.Subtotal - invoice.Discount;
        }

        public bool IsFree()
        {
            return invoice.FinalAmount() <= 0;
        }
    }
}
```

## Usage

```csharp
Invoice bill = new Invoice
{
    Subtotal = 240m,
    Discount = 40m
};

decimal due = bill.FinalAmount();
bool free = bill.IsFree();
```

This is helpful when:

- the type comes from another assembly
- you want to avoid cluttering the original class
- you want related helper logic in one place

---

# How the Call Works

When you write:

```csharp
string note = "hello world";
int count = note.WordCount();
```

the compiler treats it like an extension member invocation.

It **looks like**:

- `note` owns `WordCount()`

but in reality:

- the member is defined elsewhere
- the compiler resolves it based on namespaces and available extension declarations

---

# Name Resolution and Priority

If a real instance member exists on the type, it usually takes priority over an extension member with the same signature.

## Example

If a type already defines:

```csharp
item.Format()
```

and an extension block also provides `Format()`, then the actual type member wins.

### Practical Advice

- avoid naming collisions
- use extension members for genuinely missing behavior
- keep names intuitive and specific

---

# Organizing Extension Blocks

A good pattern is to group extension blocks by domain or target type.

## Example Structure

- `TextExtensions`
- `DateExtensions`
- `CollectionExtensions`
- `InvoiceExtensions`

This keeps code readable and discoverable.

---

# When to Use Extension Blocks

Use them when you want to:

- add helper behavior to an existing type
- keep utility logic close together
- improve readability of chained calls
- avoid changing a class you do not own

## Good Fit

- formatting helpers
- validation helpers
- collection utilities
- domain-specific convenience members

## Less Ideal

- behavior that should truly belong inside the original class
- very large business logic
- members with surprising side effects

---

# Best Practices

## Keep Extensions Focused

Prefer small, clear members:

```csharp
public bool IsWeekend()
```

instead of overloaded “do everything” helpers.

---

## Use Meaningful Receiver Names

This improves readability inside the block.

```csharp
extension(Order order)
extension(string text)
extension(List<int> numbers)
```

Better than vague names like:

```csharp
extension(string x)
```

---

## Avoid Hidden Complexity

Extension members should feel lightweight and unsurprising.

> If an extension method sends emails, deletes files, or updates a database, it is probably too heavy.

---

## Be Careful with Null

For nullable-aware code, think carefully about whether the receiver could be `null`.

```csharp
extension(string? text)
{
    public bool IsMissing()
    {
        return string.IsNullOrWhiteSpace(text);
    }
}
```

Usage:

```csharp
string? input = null;
bool missing = input.IsMissing();
```

---

# Chaining Example

Extension blocks are especially nice for fluent-style code.

```csharp
public static class TextFlowExtensions
{
    extension(string text)
    {
        public string TrimAndLower()
        {
            return text.Trim().ToLowerInvariant();
        }

        public string Quote()
        {
            return $"\"{text}\"";
        }
    }
}
```

## Usage

```csharp
string raw = "  Welcome To CSharp  ";

string result = raw.TrimAndLower().Quote();
```

This reads smoothly because each member appears to belong to `string`.

---

# Syntax Pattern

## General Form

```csharp
public static class ExtensionContainer
{
    extension(TargetType receiverName)
    {
        public ReturnType MemberName(...)
        {
            ...
        }
    }
}
```

## Generic Form

```csharp
public static class ExtensionContainer
{
    extension<T>(IEnumerable<T> sequence)
    {
        public bool IsNotEmpty()
        {
            return sequence.Any();
        }
    }
}
```

---

# Mental Model

A useful way to think about extension blocks:

> “They let you attach extra members to an existing type from the outside, while keeping those members grouped under one receiver declaration.”

That is the core idea.

---

# Common Pitfalls

## Forgetting the Namespace

```csharp
using UtilityPack;
```

Without the correct `using`, extension members may seem to “disappear”.

---

## Expecting Real Type Modification

This does **not** alter the original type definition.

You are not adding members permanently to `string`, `int`, or any other type.

---

## Overusing Extensions

Too many extensions can make APIs confusing, especially if many namespaces add similar member names.

---

## Creating Ambiguous Names

If two imported namespaces define the same extension member for the same type, resolution may become unclear.

---

# Quick Comparison Table

| Feature | Classic Extension Methods | Extension Blocks |
|---|---|---|
| Receiver declaration | Repeated on each method with `this` | Declared once in the block |
| Organization | Per method | Grouped by target type |
| Readability | Good | Often better |
| Supports multiple related members | Yes | Yes, more naturally |
| Feels like part of the target type | Somewhat | More strongly |

---

# Mini Example Set

## Extending `double`

```csharp
public static class MeasurementExtensions
{
    extension(double distance)
    {
        public double ToMiles()
        {
            return distance * 0.621371;
        }

        public double ToRounded(int digits)
        {
            return Math.Round(distance, digits);
        }
    }
}
```

Usage:

```csharp
double km = 12.8;

double miles = km.ToMiles();
double rounded = km.ToRounded(1);
```

## Extending a Custom `UserProfile`

```csharp
public class UserProfile
{
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
}
```

```csharp
public static class UserProfileExtensions
{
    extension(UserProfile profile)
    {
        public string FullName()
        {
            return $"{profile.FirstName} {profile.LastName}".Trim();
        }
    }
}
```

Usage:

```csharp
var profile = new UserProfile
{
    FirstName = "Ava",
    LastName = "Stone"
};

string fullName = profile.FullName();
```

---

# Key Terms

- **Extension member**: a member added externally to an existing type
- **Receiver**: the variable declared in `extension(...)`
- **Static class**: the container where extension blocks are defined
- **Target type**: the type being extended
- **Namespace import**: required so the extension is visible

---

# Remember This Pattern

```csharp
public static class HelperExtensions
{
    extension(TargetType target)
    {
        public ReturnType ExtraBehavior(...)
        {
            ...
        }
    }
}
```

If you understand:

1. **static class**
2. **target type**
3. **receiver variable**
4. **member usage like normal instance calls**

then you understand the main concept of **C# extension blocks**.