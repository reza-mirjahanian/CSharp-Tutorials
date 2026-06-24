# Why a `DateTime` field can’t be `const`

## Rule for `const` in C#

A `const` field must be a value the compiler can fully determine at compile time.

That means the value must be a **compile-time constant**.

---

## What kinds of things can be `const`

C# allows `const` for a limited set of types, such as:

- integral numeric types
- `bool`
- `char`
- `string`
- `enum`
- `null` for reference types

Example:

```csharp
public const int MaxItems = 100;
public const string Title = "Report";
public const DayOfWeek FirstDay = DayOfWeek.Monday;
```

---

## Why `DateTime` does not qualify

`DateTime` is a **struct**, not a special built-in constant type.

Even though it is a value type, this is not enough.

This does **not** work:

```csharp
public const DateTime Start = new DateTime(2025, 1, 1);
```

Why?

Because:

- `new DateTime(...)` is a **constructor call**
- constructor calls are evaluated at runtime, not compile time
- `const` requires the compiler to embed the value directly

---

## Key reason

A `const` field cannot require:

- object creation
- constructor execution
- method calls
- runtime evaluation

But creating a `DateTime` value requires calling its constructor.

So it cannot be `const`.

---

## What to use instead

Use `static readonly`.

```csharp
public static readonly DateTime Start = new DateTime(2025, 1, 1);
```

This works because:

- the value is assigned once
- it is initialized at runtime
- it cannot be changed afterward

---

# `const` vs `static readonly`

| Feature | `const` | `static readonly` |
|---|---|---|
| Must be compile-time constant | Yes | No |
| Can use constructor call | No | Yes |
| Evaluated at | Compile time | Runtime |
| Good for `DateTime` | No | Yes |

---

## Example

```csharp
public class Schedule
{
    public static readonly DateTime LaunchDate = new DateTime(2025, 6, 1);
}
```

---

## Why `string` can be `const` but `DateTime` cannot

This often feels confusing because `string` is a reference type, yet it can be `const`.

Example:

```csharp
public const string Name = "Sample";
```

That works because string literals are treated specially by the compiler.

But `DateTime` has no literal syntax like this:

```csharp
public const DateTime X = ???;
```

There is no built-in `DateTime` literal in C#.

---

## Important distinction

This is allowed:

```csharp
public const int Days = 7;
```

This is not:

```csharp
public const DateTime Today = new DateTime(2025, 5, 1);
```

The difference is that `7` is a compile-time literal, while `new DateTime(...)` is a runtime construction.

---

## Short answer

A `DateTime` field cannot be `const` because `const` requires a compile-time constant, and a `DateTime` value must be created by calling a constructor, which is not allowed in a constant expression.