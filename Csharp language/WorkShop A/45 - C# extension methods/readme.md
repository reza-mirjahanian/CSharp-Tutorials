# C# `sealed`

(Method chaining or fluent style)

The `sealed` keyword is used to **stop inheritance** or **stop overriding**.

You can use it with:

- **classes**
- **methods**
- **properties**
- **events**

---

## What `sealed` Means

> **`sealed` = “no more inheritance/overriding beyond this point.”**

It is useful when you want to:

- protect a design
- prevent unwanted customization
- lock down behavior
- improve clarity of intent

---

# 1. `sealed` Class

A `sealed class` cannot be inherited.

## Example

```csharp
public sealed class AuditLogger
{
    public void WriteEntry(string text)
    {
        Console.WriteLine($"Audit: {text}");
    }
}
```

## Valid usage

```csharp
var logger = new AuditLogger();
logger.WriteEntry("User signed in.");
```

## Invalid inheritance

```csharp
public class FileAuditLogger : AuditLogger
{
}
```

### Result

This causes a compile-time error because `AuditLogger` is sealed.

---

## Why Use a `sealed` Class?

### Common reasons

- ✅ You want to **prevent subclassing**
- ✅ The class design is **complete**
- ✅ Inheritance could break the intended behavior
- ✅ You want stricter control over usage

### Examples of good candidates

- utility/helper classes
- security-sensitive classes
- classes with fixed behavior
- final implementation types

---

# 2. `sealed` Method

A `sealed` method prevents further overriding in derived classes.

⚠️ A method cannot be sealed by itself.  
It must be:

- inherited from a base class
- overridden in a derived class
- then marked as `sealed override`

---

## Example

```csharp
public class NotificationService
{
    public virtual void Send()
    {
        Console.WriteLine("Sending generic notification...");
    }
}

public class EmailNotificationService : NotificationService
{
    public sealed override void Send()
    {
        Console.WriteLine("Sending email notification...");
    }
}
```

Now another class cannot override `Send()` again.

```csharp
public class CustomEmailService : EmailNotificationService
{
    public override void Send()
    {
        Console.WriteLine("Modified email sending...");
    }
}
```

This gives a compile-time error because `Send()` was sealed in `EmailNotificationService`.

---

## Why Seal an Overridden Method?

Use `sealed override` when:

- the override contains the final approved behavior
- further customization would be unsafe
- the method must stay consistent in all deeper subclasses

---

# 3. `sealed` Property

Properties can also be sealed when overridden.

## Example

```csharp
public class Vehicle
{
    public virtual string Category => "General";
}

public class Car : Vehicle
{
    public sealed override string Category => "Passenger Car";
}
```

Now a class inheriting from `Car` cannot override `Category` again.

---

# 4. `sealed` Event

Events can also be sealed in the same pattern.

## Example

```csharp
public class Sensor
{
    public virtual event EventHandler? Triggered;
}

public class MotionSensor : Sensor
{
    public sealed override event EventHandler? Triggered;
}
```

This prevents deeper derived classes from overriding that event again.

---

# 5. `sealed` vs `abstract`

These two keywords are opposites in purpose.

| Keyword | Meaning |
|---|---|
| `abstract` | Must be inherited / completed |
| `sealed` | Cannot be inherited further |

---

## `abstract` Example

```csharp
public abstract class Shape
{
    public abstract double GetArea();
}
```

This class is meant to be inherited.

---

## `sealed` Example

```csharp
public sealed class CircleCalculator
{
    public double GetArea(double radius)
    {
        return 3.14159 * radius * radius;
    }
}
```

This class is not meant to be inherited.

---

# 6. Important Rule

A class cannot be both:

- `abstract`
- `sealed`

at the same time.

Why?

- `abstract` says: *must be inherited*
- `sealed` says: *cannot be inherited*

Those meanings conflict.

---

# 7. `sealed` in Inheritance Chains

## Example

```csharp
public class Appliance
{
    public virtual void Start()
    {
        Console.WriteLine("Appliance starting...");
    }
}

public class Washer : Appliance
{
    public sealed override void Start()
    {
        Console.WriteLine("Washer starting with locked cycle.");
    }
}

public class SmartWasher : Washer
{
}
```

`SmartWasher` can inherit from `Washer`, but it cannot override `Start()`.

---

# 8. When to Use `sealed`

## Use `sealed` when:

- you want to stop inheritance completely
- a derived override should be the final version
- the class was not designed for extension
- extending behavior could cause bugs
- you want a clear, controlled API

---

## Avoid using `sealed` when:

- you expect users to customize behavior by inheritance
- the class is meant to be a base class
- extensibility is part of the design

---

# 9. Real-World Example

```csharp
public class Account
{
    public virtual void Close()
    {
        Console.WriteLine("Closing account...");
    }
}

public class VerifiedAccount : Account
{
    public sealed override void Close()
    {
        Console.WriteLine("Closing verified account with compliance checks...");
    }
}
```

This ensures compliance behavior cannot be replaced in deeper derived classes.

---

# 10. Quick Reference

| Usage | Example | Meaning |
|---|---|---|
| Sealed class | `public sealed class ReportBuilder` | No inheritance allowed |
| Sealed method | `public sealed override void Save()` | No further overriding |
| Sealed property | `public sealed override string Name => "X";` | Property override is final |
| Sealed event | `public sealed override event EventHandler Updated;` | Event override is final |

---

# C# Extension Methods

Extension methods let you **add methods to an existing type** without modifying the original type.

They make it look as if the type already had that method.

---

## What Extension Methods Do

> They allow you to call a static method using instance method syntax.

This means:

- the method is actually `static`
- but you call it like a normal object method

---

# 11. Basic Idea

Suppose you want to add a method to `string`.

Normally, you cannot edit the built-in `string` type.

But with an extension method, you can write:

```csharp
string code = "  zx-42  ";
Console.WriteLine(code.CleanCode());
```

Even though `CleanCode()` is not originally part of `string`.

---

# 12. Syntax Rules

An extension method must follow these rules:

1. It must be inside a **static class**
2. The method itself must be **static**
3. The first parameter must use the `this` keyword
4. The first parameter specifies the type being extended

---

## Example

```csharp
public static class StringTools
{
    public static string CleanCode(this string value)
    {
        return value.Trim().ToUpper();
    }
}
```

### Usage

```csharp
string code = "  dev-77 ";
string result = code.CleanCode();

Console.WriteLine(result);
```

---

# 13. How It Really Works

This:

```csharp
code.CleanCode()
```

is actually rewritten by the compiler like this:

```csharp
StringTools.CleanCode(code)
```

So extension methods are **static methods in disguise**.

---

# 14. Example with `int`

```csharp
public static class NumberExtensions
{
    public static bool IsAbove(this int value, int threshold)
    {
        return value > threshold;
    }
}
```

## Usage

```csharp
int temperature = 28;
bool hot = temperature.IsAbove(24);

Console.WriteLine(hot);
```

---

# 15. Example with Custom Classes

You can extend your own classes too.

## Original class

```csharp
public class Customer
{
    public string FullName { get; set; } = "";
}
```

## Extension method

```csharp
public static class CustomerExtensions
{
    public static string ToBadgeLabel(this Customer customer)
    {
        return $"Customer: {customer.FullName}";
    }
}
```

## Usage

```csharp
var customer = new Customer { FullName = "Aria D." };
Console.WriteLine(customer.ToBadgeLabel());
```

---

# 16. Example with Nullable Safety

Extension methods should often validate parameters.

```csharp
public static class TextExtensions
{
    public static int SafeLength(this string? text)
    {
        return text?.Length ?? 0;
    }
}
```

## Usage

```csharp
string? note = null;
Console.WriteLine(note.SafeLength());
```

This prints `0` instead of crashing.

---

# 17. Chaining Extension Methods

Extension methods are often useful in method chains.

```csharp
public static class TextFormattingExtensions
{
    public static string RemoveSpaces(this string text)
    {
        return text.Replace(" ", "");
    }

    public static string Mark(this string text)
    {
        return $"[{text}]";
    }
}
```

## Usage

```csharp
string value = "A 12";
string output = value.RemoveSpaces().Mark();

Console.WriteLine(output);
```

---

# 18. Why Extension Methods Are Useful

## Benefits

- ✅ Add helper methods to existing types
- ✅ Keep code readable
- ✅ Avoid changing original classes
- ✅ Support fluent-style APIs
- ✅ Organize reusable utilities

---

# 19. Common Use Cases

### Frequently used with:

- `string`
- `int`
- collections like `List<T>`
- custom models
- framework types you cannot modify

---

# 20. Extension Method on Collections

```csharp
public static class ListExtensions
{
    public static void PrintItems(this List<string> items)
    {
        foreach (var item in items)
        {
            Console.WriteLine(item);
        }
    }
}
```

## Usage

```csharp
var tags = new List<string> { "api", "csharp", "guide" };
tags.PrintItems();
```

---

# 21. Namespace Requirement

To use an extension method naturally, the namespace containing it must be imported.

## Example

```csharp
using MyProject.Helpers;
```

If the namespace is not imported, the method may not appear as available.

---

# 22. Extension Methods vs Regular Static Methods

| Feature | Extension Method | Regular Static Method |
|---|---|---|
| Declared in static class | Yes | Yes |
| Method must be static | Yes | Yes |
| First parameter uses `this` | Yes | No |
| Called like instance method | Yes | No |
| Can extend existing types | Yes | Not in instance-like syntax |

---

## Comparison Example

### Regular static method

```csharp
var cleaned = StringTools.CleanCode("  id-15 ");
```

### Extension method style

```csharp
var cleaned = "  id-15 ".CleanCode();
```

The second form is often easier to read.

---

# 23. Important Limitations

Extension methods **cannot**:

- access private members of the type
- truly modify the original type definition
- override existing instance methods
- replace inheritance or polymorphism

---

# 24. Instance Methods Take Priority

If a type already has a matching instance method, that method is used instead of the extension method.

## Example idea

If a class defines:

```csharp
public void Print()
```

and you also create an extension method named `Print()`, the real instance method wins.

---

# 25. Best Practices for Extension Methods

## ✅ Good practices

- keep them small and focused
- use meaningful names
- place them in clear static classes
- use them for helper behavior
- validate inputs when necessary

## ❌ Avoid

- putting too much business logic in them
- creating confusing method names
- overusing them just to make code look fancy
- using them when a normal instance method is better

---

# 26. Full Example

```csharp
public class Product
{
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
}

public static class ProductExtensions
{
    public static string ToDisplayCard(this Product product)
    {
        return $"{product.Name} | {product.Price:C2}";
    }

    public static bool IsPremium(this Product product)
    {
        return product.Price >= 500m;
    }
}
```

## Usage

```csharp
var product = new Product
{
    Name = "Mechanical Keyboard",
    Price = 620m
};

Console.WriteLine(product.ToDisplayCard());
Console.WriteLine(product.IsPremium());
```

---

# 27. `sealed` vs Extension Methods

These topics are unrelated, but both affect class design.

| Topic | Purpose |
|---|---|
| `sealed` | Prevent inheritance or overriding |
| Extension method | Add helper behavior to an existing type |

### Example mental difference

- `sealed` says: **“Do not extend this type through inheritance.”**
- Extension methods say: **“Add extra callable behavior without editing the type.”**

---

# 28. Mini Examples

## Sealed class

```csharp
public sealed class SessionToken
{
    public string Value { get; set; } = "";
}
```

## Sealed override

```csharp
public class BaseWriter
{
    public virtual void Write()
    {
        Console.WriteLine("Base write");
    }
}

public class SecureWriter : BaseWriter
{
    public sealed override void Write()
    {
        Console.WriteLine("Secure write");
    }
}
```

## Simple extension method

```csharp
public static class DateExtensions
{
    public static string ToShortLabel(this DateTime date)
    {
        return date.ToString("yyyy-MM-dd");
    }
}
```

## Usage

```csharp
Console.WriteLine(DateTime.Now.ToShortLabel());
```