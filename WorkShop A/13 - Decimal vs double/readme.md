# `decimal` vs `double` in C#

Both `decimal` and `double` store numbers with fractional parts, but they are designed for **different purposes**.

---

# Quick Comparison

| Feature | `double` | `decimal` |
|---|---|---|
| Size | 8 bytes | 16 bytes |
| Precision style | Binary floating-point | Decimal floating-point |
| Approximate precision | 15–17 digits | 28–29 digits |
| Speed | Faster | Slower |
| Best for | Scientific/math calculations | Money and financial values |
| Can represent `0.1` exactly? | ❌ No | ✅ Yes |
| Literal suffix | none or `d` | `m` or `M` |

---

# Main Difference

## `double`

`double` stores values in **binary floating-point** format.

That makes it:

- very fast
- good for a wide range of values
- not always exact for decimal fractions like `0.1`, `0.2`, or `0.3`

## `decimal`

`decimal` stores values in **base-10 decimal** form.

That makes it:

- much better for values humans write in decimal
- more precise for currency and financial calculations
- slower and larger than `double`

---

# Why `double` Can Be Inexact

Some decimal numbers cannot be represented exactly in binary.

## Example

```csharp
double a = 0.1;
double b = 0.2;
double c = a + b;

Console.WriteLine(c);   // May display 0.30000000000000004
```

This happens because `0.1` and `0.2` are approximations in binary.

---

# Why `decimal` Is Better for Money

`decimal` can represent many decimal fractions exactly.

## Example

```csharp
decimal a = 0.1m;
decimal b = 0.2m;
decimal c = a + b;

Console.WriteLine(c);   // 0.3
```

This is why `decimal` is usually the right choice for:

- prices
- balances
- taxes
- invoices
- payroll calculations

---

# Literal Syntax

## `double` literals

```csharp
double x = 3.14;
double y = 3.14d;
```

- If you write a floating-point number like `3.14`, C# treats it as `double` by default.

## `decimal` literals

```csharp
decimal price = 3.14m;
```

- You must use the `m` or `M` suffix.

> Without `m`, the number is treated as a `double`, and assignment to `decimal` will fail.

## Example

```csharp
decimal price = 3.14;   // Error
decimal price2 = 3.14m; // Correct
```

---

# Precision

## `double`

- about **15–17 significant digits**
- good for measurements, physics, graphics, statistics

## `decimal`

- about **28–29 significant digits**
- better when exact decimal precision matters

---

# Range

`double` can represent a much **larger range** of values than `decimal`.

| Type | Approximate Range |
|---|---|
| `double` | ±5.0 × 10⁻³²⁴ to ±1.7 × 10³⁰⁸ |
| `decimal` | ±1.0 × 10⁻²⁸ to ±7.9 × 10²⁸ |

## Meaning

- use `double` when you need **very large or very small** numbers
- use `decimal` when you need **precise decimal arithmetic**

---

# Performance

## `double`

- faster
- usually preferred in:
  - scientific computing
  - simulations
  - 3D/game math
  - graphics
  - machine learning calculations

## `decimal`

- slower
- better when correctness in decimal digits matters more than speed

---

# Common Use Cases

## Use `double` for

- geometry
- physics formulas
- trigonometry
- sensor values
- scientific data
- percentages where tiny rounding differences are acceptable

## Use `decimal` for

- currency
- banking
- accounting
- billing
- tax calculations
- inventory prices

---

# Example: Money Calculation

## Using `double`

```csharp
double price = 19.99;
double quantity = 3;
double total = price * quantity;

Console.WriteLine(total);
```

This may work most of the time, but small rounding errors can appear in larger calculations.

## Using `decimal`

```csharp
decimal price = 19.99m;
decimal quantity = 3m;
decimal total = price * quantity;

Console.WriteLine(total);
```

This is safer for financial systems.

---

# Equality Comparison

Comparing `double` values directly can be risky.

## Problem

```csharp
double x = 0.1 + 0.2;
Console.WriteLine(x == 0.3); // Often false
```

## Better approach for `double`

Use a tolerance:

```csharp
double x = 0.1 + 0.2;
bool equal = Math.Abs(x - 0.3) < 0.000001;
Console.WriteLine(equal);
```

## `decimal` comparison

```csharp
decimal x = 0.1m + 0.2m;
Console.WriteLine(x == 0.3m); // True
```

---

# Conversions

You cannot mix them freely without conversion.

## Example

```csharp
double d = 1.5;
decimal m = 2.5m;

// decimal result = d + m; // Error
```

You must convert one type to the other:

```csharp
decimal result = (decimal)d + m;
```

or

```csharp
double result = d + (double)m;
```

> Be careful: converting from `double` to `decimal` can still carry over an already inexact value.

---

# Rule of Thumb

## Choose `double` when you want:

- speed
- huge range
- scientific/math calculations

## Choose `decimal` when you want:

- exact decimal behavior
- financial correctness
- fewer rounding surprises with base-10 numbers

---

# Simple Memory Trick

> **`double`** = **fast approximate math**  
> **`decimal`** = **precise human-style decimal math**

---

# Side-by-Side Example

```csharp
double d1 = 0.1;
double d2 = 0.2;
Console.WriteLine(d1 + d2);   // 0.30000000000000004

decimal m1 = 0.1m;
decimal m2 = 0.2m;
Console.WriteLine(m1 + m2);   // 0.3
```

---

# Practical Recommendation

## If the value represents:

- **money** → use `decimal`
- **measurement/science/math** → use `double`

## In most business applications:

```csharp
decimal totalAmount = 1250.75m;
```

## In most engineering or graphics applications:

```csharp
double distance = 1250.75;
```