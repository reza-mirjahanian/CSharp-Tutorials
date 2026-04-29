# `TryParse` Method in C#

## What `TryParse` Means

`TryParse` is a safe way to convert **text** into another data type without throwing an exception when the conversion fails.

> It tries to parse a value.  
> If successful, it returns `true`.  
> If unsuccessful, it returns `false`.

---

# Why Use `TryParse`?

When user input or external data might be invalid, `TryParse` is usually better than `Parse`.

## `Parse`

```csharp
int amount = int.Parse("250");
```

This works if the string is valid.

But this fails:

```csharp
int amount = int.Parse("two hundred");
```

That throws an exception.

---

## `TryParse`

```csharp
bool success = int.TryParse("250", out int amount);
```

If conversion succeeds:

- `success` is `true`
- `amount` contains the parsed number

If conversion fails:

- `success` is `false`
- `amount` gets a default value

---

# Basic Syntax

```csharp
bool success = SomeType.TryParse(text, out result);
```

## Parts

- `SomeType` → the target type, such as `int`, `double`, `DateTime`, `bool`
- `text` → the string to convert
- `out result` → where the converted value will be stored
- return value → `true` or `false`

---

# First Example: `int.TryParse`

```csharp
string input = "125";

bool isValid = int.TryParse(input, out int number);
```

## Result

| Variable | Value |
|---|---|
| `isValid` | `true` |
| `number` | `125` |

---

## Failed Example

```csharp
string input = "hello";

bool isValid = int.TryParse(input, out int number);
```

## Result

| Variable | Value |
|---|---|
| `isValid` | `false` |
| `number` | `0` |

When parsing fails, the `out` variable gets the type’s default value.

For `int`, that is `0`.

---

# Common Pattern with `if`

```csharp
string input = "42";

if (int.TryParse(input, out int quantity))
{
    Console.WriteLine($"Valid number: {quantity}");
}
else
{
    Console.WriteLine("Invalid number");
}
```

---

# Why `TryParse` Is Better Than `Parse` for User Input

User input is unpredictable.

```csharp
Console.Write("Enter your age: ");
string text = Console.ReadLine();

if (int.TryParse(text, out int age))
{
    Console.WriteLine($"Your age is {age}");
}
else
{
    Console.WriteLine("Please enter a valid whole number.");
}
```

This avoids crashing when the user enters invalid text.

---

# `Parse` vs `TryParse`

| Method | On valid input | On invalid input |
|---|---|---|
| `Parse` | Returns converted value | Throws exception |
| `TryParse` | Returns `true` and sets result | Returns `false` and sets default value |

---

# Types That Commonly Use `TryParse`

Many built-in C# types support `TryParse`, including:

- `int`
- `long`
- `double`
- `decimal`
- `float`
- `bool`
- `DateTime`
- `TimeSpan`
- `Guid`
- `Enum` via `Enum.TryParse`

---

# Integer Example

```csharp
string text = "900";

if (int.TryParse(text, out int score))
{
    Console.WriteLine(score);
}
```

---

# Decimal Example

```csharp
string priceText = "49.95";

if (decimal.TryParse(priceText, out decimal price))
{
    Console.WriteLine($"Price: {price}");
}
else
{
    Console.WriteLine("Invalid price");
}
```

---

# Double Example

```csharp
string ratioText = "3.1415";

bool ok = double.TryParse(ratioText, out double ratio);
```

---

# Boolean Example

```csharp
string text = "true";

if (bool.TryParse(text, out bool isEnabled))
{
    Console.WriteLine($"Parsed: {isEnabled}");
}
```

## Valid boolean strings include:

- `"true"`
- `"false"`

Usually case-insensitive:

- `"True"`
- `"FALSE"`

---

# DateTime Example

```csharp
string dateText = "2026-08-15";

if (DateTime.TryParse(dateText, out DateTime dueDate))
{
    Console.WriteLine($"Due date: {dueDate}");
}
else
{
    Console.WriteLine("Invalid date");
}
```

---

# Guid Example

```csharp
string idText = "f47ac10b-58cc-4372-a567-0e02b2c3d479";

if (Guid.TryParse(idText, out Guid orderId))
{
    Console.WriteLine($"Order ID: {orderId}");
}
```

---

# Enum Example

```csharp
enum AccessLevel
{
    Guest,
    Member,
    Manager
}

string text = "Manager";

if (Enum.TryParse(text, out AccessLevel level))
{
    Console.WriteLine($"Parsed level: {level}");
}
```

---

## Case-insensitive enum parsing

```csharp
string text = "manager";

if (Enum.TryParse(text, ignoreCase: true, out AccessLevel level))
{
    Console.WriteLine($"Parsed level: {level}");
}
```

---

# The `out` Parameter

## What `out` Does

The `out` keyword lets the method assign a value to a variable and give it back to you.

```csharp
int.TryParse("77", out int value);
```

Here:

- the method returns a `bool`
- the parsed number is placed into `value`

So `TryParse` gives you **two results**:

1. **Did it work?**
2. **What is the converted value?**

---

## Older Style

```csharp
int value;
bool ok = int.TryParse("77", out value);
```

---

## Inline Declaration Style

```csharp
bool ok = int.TryParse("77", out int value);
```

This is more common in modern C#.

---

# Default Values When Parsing Fails

If parsing fails, the `out` variable receives the type’s default value.

| Type | Default value |
|---|---|
| `int` | `0` |
| `double` | `0` |
| `decimal` | `0` |
| `bool` | `false` |
| `DateTime` | `01/01/0001 00:00:00` |
| `Guid` | `Guid.Empty` |

Example:

```csharp
bool ok = bool.TryParse("not-bool", out bool flag);
```

Result:

- `ok` → `false`
- `flag` → `false`

---

# Important Rule

## Always Check the Boolean Result

Do not assume the parsed value is valid just because the `out` variable exists.

### Less safe

```csharp
int.TryParse("oops", out int count);
Console.WriteLine(count);
```

This prints `0`, but `0` might mean:

- actual parsed zero
- parsing failed

---

### Better

```csharp
if (int.TryParse("oops", out int count))
{
    Console.WriteLine($"Count: {count}");
}
else
{
    Console.WriteLine("Input was not a valid number.");
}
```

---

# Real Input Example

```csharp
Console.Write("Enter quantity: ");
string text = Console.ReadLine();

if (!int.TryParse(text, out int quantity))
{
    Console.WriteLine("Quantity must be a whole number.");
    return;
}

Console.WriteLine($"You entered {quantity}");
```

This is a good example of **guard clause** style with `TryParse`.

---

# Combining `TryParse` with Early Return

```csharp
void SaveAge(string input)
{
    if (!int.TryParse(input, out int age))
        return;

    Console.WriteLine($"Saved age: {age}");
}
```

Or with a message:

```csharp
void SaveAge(string input)
{
    if (!int.TryParse(input, out int age))
    {
        Console.WriteLine("Invalid age.");
        return;
    }

    Console.WriteLine($"Saved age: {age}");
}
```

---

# Multiple `TryParse` Calls

```csharp
string quantityText = "3";
string priceText = "14.50";

if (!int.TryParse(quantityText, out int quantity))
{
    Console.WriteLine("Invalid quantity");
    return;
}

if (!decimal.TryParse(priceText, out decimal price))
{
    Console.WriteLine("Invalid price");
    return;
}

decimal total = quantity * price;
Console.WriteLine($"Total: {total}");
```

---

# `TryParse` in Loops

A common pattern is repeating until the user enters valid input.

```csharp
int age;

while (true)
{
    Console.Write("Enter your age: ");
    string text = Console.ReadLine();

    if (int.TryParse(text, out age))
        break;

    Console.WriteLine("Please enter a valid number.");
}

Console.WriteLine($"Accepted age: {age}");
```

---

# `TryParse` with Nullable Logic

Sometimes you want `null` instead of a default value when parsing fails.

```csharp
string text = "88";

int? result = int.TryParse(text, out int number)
    ? number
    : null;
```

If parsing fails, `result` becomes `null`.

---

# Example: Convert String to `int?`

```csharp
int? ToIntOrNull(string text)
{
    if (int.TryParse(text, out int value))
        return value;

    return null;
}
```

---

# Culture and Formatting

Some types like `decimal`, `double`, and `DateTime` can behave differently depending on regional settings.

```csharp
decimal.TryParse("12.75", out decimal amount);
```

Whether `"12.75"` is valid may depend on culture settings in some cases.

For more control, overloads of `TryParse` let you specify formatting rules and culture information.

---

## Example with number styles and culture

```csharp
using System.Globalization;

string text = "12.75";

bool ok = decimal.TryParse(
    text,
    NumberStyles.Number,
    CultureInfo.InvariantCulture,
    out decimal amount);
```

This is useful when parsing data from files, APIs, or fixed-format input.

---

# `TryParseExact`

Some types also provide stricter methods like `TryParseExact`.

## Example with `DateTime`

```csharp
using System.Globalization;

string text = "2026/11/05";

bool ok = DateTime.TryParseExact(
    text,
    "yyyy/MM/dd",
    CultureInfo.InvariantCulture,
    DateTimeStyles.None,
    out DateTime date);
```

This only succeeds if the input matches the exact format.

---

# Common Mistakes

## 1. Using the Result Without Checking Success

```csharp
int.TryParse("abc", out int number);
Console.WriteLine(number);
```

Problem: `number` becomes `0`, which may be misleading.

---

## 2. Using `Parse` for Untrusted Input

```csharp
int number = int.Parse(userInput);
```

If `userInput` is invalid, the program may crash.

Use:

```csharp
if (int.TryParse(userInput, out int number))
{
    Console.WriteLine(number);
}
```

---

## 3. Assuming All Text Numbers Are Valid

```csharp
string text = "19.99";
int.TryParse(text, out int number);
```

This fails because `"19.99"` is not a valid integer.

Use `decimal.TryParse` or `double.TryParse` instead.

---

## 4. Ignoring Whitespace and Formatting Issues

```csharp
string text = " 42 ";
int.TryParse(text, out int number);
```

This often succeeds, but formatting rules depend on the type and overload used.

---

# Quick Reference Table

| Type | Example |
|---|---|
| `int` | `int.TryParse(text, out int value)` |
| `decimal` | `decimal.TryParse(text, out decimal value)` |
| `double` | `double.TryParse(text, out double value)` |
| `bool` | `bool.TryParse(text, out bool value)` |
| `DateTime` | `DateTime.TryParse(text, out DateTime value)` |
| `Guid` | `Guid.TryParse(text, out Guid value)` |
| `enum` | `Enum.TryParse(text, out MyEnum value)` |

---

# Short Examples

## Number

```csharp
if (int.TryParse("64", out int points))
{
    Console.WriteLine(points);
}
```

## Decimal

```csharp
if (decimal.TryParse("15.40", out decimal fee))
{
    Console.WriteLine(fee);
}
```

## Date

```csharp
if (DateTime.TryParse("2026-12-01", out DateTime startDate))
{
    Console.WriteLine(startDate);
}
```

## Boolean

```csharp
if (bool.TryParse("false", out bool isArchived))
{
    Console.WriteLine(isArchived);
}
```