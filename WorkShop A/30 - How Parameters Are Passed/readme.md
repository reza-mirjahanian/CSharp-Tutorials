# C#: Controlling How Parameters Are Passed

When you call a method in C#, the values you send into it are called **parameters**.

C# gives you several ways to control *how* those values are passed:

- **by value** *(default)*
- **by reference** with `ref`
- **as output values** with `out`
- **as read-only references** with `in`
- **as a variable-length list** with `params`

Understanding these makes your methods more flexible and predictable.

---

# 1) Passing by Value

By default, C# passes arguments **by value**.

That means the method receives a *copy* of the variable’s value.

## Example

```csharp
static void Increase(int amount)
{
    amount += 5;
}

int score = 12;
Increase(score);

Console.WriteLine(score);
```

### Output

```csharp
12
```

## Why?

Because `Increase` works with a copy of `score`, not the original variable.

---

## Visual Idea

| Outside the method | Inside the method |
|---|---|
| `score = 12` | `amount = 12` |

When `amount` changes, `score` stays the same.

---

# 2) Passing by Reference with `ref`

If you want a method to change the *original* variable, use `ref`.

With `ref`:

- the method works with the actual variable
- changes inside the method affect the caller’s variable
- the variable must already be initialized before the method call

---

## Example

```csharp
static void Increase(ref int amount)
{
    amount += 5;
}

int score = 12;
Increase(ref score);

Console.WriteLine(score);
```

### Output

```csharp
17
```

Now the original `score` changes.

---

## Important Rule

You must write `ref` in **both** places:

1. in the method definition
2. in the method call

### Definition

```csharp
static void Increase(ref int amount)
```

### Call

```csharp
Increase(ref score);
```

If you leave out `ref` in either place, the code will not compile.

---

# 3) Returning Values Through Parameters with `out`

Use `out` when a method needs to send a value back through a parameter.

This is useful when a method needs to produce **multiple outputs**.

---

## Example

```csharp
static void SplitTime(int totalSeconds, out int minutes, out int seconds)
{
    minutes = totalSeconds / 60;
    seconds = totalSeconds % 60;
}

int mins;
int secs;

SplitTime(367, out mins, out secs);

Console.WriteLine($"{mins} min {secs} sec");
```

### Output

```csharp
6 min 7 sec
```

---

## Rules for `out`

### The caller:

- does **not** need to initialize the variable first

### The method:

- **must assign** a value before the method ends

---

## Example of Declaration Before Call

```csharp
int wholePart;
int remainderPart;

SplitTime(367, out wholePart, out remainderPart);
```

---

## Inline Declaration Style

You can also declare the variables directly in the method call:

```csharp
SplitTime(367, out int wholePart, out int remainderPart);
```

---

# 4) Read-Only Reference Passing with `in`

Use `in` when you want to pass by reference **without allowing modification**.

This can be helpful for:

- large value types
- avoiding unnecessary copying
- protecting the input from changes

---

## Example

```csharp
static void ShowMeasurement(in int reading)
{
    Console.WriteLine(reading);
}

int sensorValue = 88;
ShowMeasurement(in sensorValue);
```

---

## Key Idea

With `in`:

- the method receives a reference
- but it cannot modify the incoming value

### Invalid example inside the method

```csharp
static void ShowMeasurement(in int reading)
{
    reading = 90; // Error
}
```

That assignment is not allowed.

---

# 5) Comparing `ref`, `out`, and `in`

| Keyword | Passed by reference? | Must be initialized before call? | Must be assigned inside method? | Can method modify it? |
|---|---:|---:|---:|---:|
| *(none)* | No | Yes | No | No effect on caller |
| `ref` | Yes | Yes | No | Yes |
| `out` | Yes | No | Yes | Yes |
| `in` | Yes | Yes | No | No |

---

# 6) When to Use Each One

## Use normal parameters when:

- the method only needs input
- changes should not affect the caller

```csharp
static int Triple(int value)
{
    return value * 3;
}
```

---

## Use `ref` when:

- the method should update an existing variable
- the caller already has a meaningful value

```csharp
static void ApplyBonus(ref decimal salary)
{
    salary += 250.00m;
}
```

---

## Use `out` when:

- the method needs to return extra values
- the outputs are created inside the method

```csharp
static void GetDimensions(out int width, out int height)
{
    width = 1280;
    height = 720;
}
```

---

## Use `in` when:

- the input should not be changed
- you want reference-style passing for safety or efficiency

```csharp
static void PrintCode(in int code)
{
    Console.WriteLine(code);
}
```

---

# Passing a Variable Number of Parameters

Sometimes you do not know how many arguments a caller will provide.

For that, C# uses `params`.

> `params` lets a method accept **zero or more arguments** of the same type.

---

# 7) Basic `params` Example

```csharp
static int AddAll(params int[] values)
{
    int total = 0;

    foreach (int item in values)
    {
        total += item;
    }

    return total;
}
```

## Calls

```csharp
Console.WriteLine(AddAll());
Console.WriteLine(AddAll(4));
Console.WriteLine(AddAll(4, 7, 9));
Console.WriteLine(AddAll(2, 5, 8, 11));
```

### Output

```csharp
0
4
20
26
```

---

## What `params` Does

This method:

```csharp
static int AddAll(params int[] values)
```

allows calls like:

- `AddAll()`
- `AddAll(4)`
- `AddAll(4, 7, 9)`

Even though the method actually receives an array.

---

# 8) `params` with Strings

```csharp
static void ShowTags(params string[] tags)
{
    foreach (string tag in tags)
    {
        Console.WriteLine($"#{tag}");
    }
}
```

## Calls

```csharp
ShowTags("csharp");
ShowTags("code", "methods", "params");
```

### Output

```csharp
#csharp
#code
#methods
#params
```

---

# 9) Rules for Using `params`

## A method can have only **one** `params` parameter.

## It must be the **last** parameter.

### Valid

```csharp
static void LogMessage(string category, params string[] messages)
{
}
```

### Invalid

```csharp
static void LogMessage(params string[] messages, string category)
{
}
```

The second version is not allowed because `params` is not last.

---

# 10) Mixing Regular Parameters with `params`

You can combine normal parameters with `params`.

## Example

```csharp
static void PrintReport(string title, params int[] scores)
{
    Console.WriteLine(title);

    foreach (int score in scores)
    {
        Console.WriteLine(score);
    }
}
```

## Call

```csharp
PrintReport("Quarter Results", 73, 81, 95, 88);
```

---

# 11) Passing an Array to a `params` Method

A `params` method can also receive an existing array.

## Example

```csharp
static int MultiplyAll(params int[] values)
{
    int product = 1;

    foreach (int number in values)
    {
        product *= number;
    }

    return product;
}
```

## Using separate arguments

```csharp
Console.WriteLine(MultiplyAll(2, 3, 4));
```

## Using an array

```csharp
int[] items = { 2, 3, 4 };
Console.WriteLine(MultiplyAll(items));
```

Both calls work.

---

# 12) `params` and Zero Arguments

A `params` parameter can accept no values at all.

## Example

```csharp
static void GreetPeople(params string[] names)
{
    if (names.Length == 0)
    {
        Console.WriteLine("No guests provided.");
        return;
    }

    foreach (string name in names)
    {
        Console.WriteLine($"Welcome, {name}!");
    }
}
```

## Calls

```csharp
GreetPeople();
GreetPeople("Lena", "Mason");
```

### Output

```csharp
No guests provided.
Welcome, Lena!
Welcome, Mason!
```

---

# 13) `ref`/`out` vs `params`

These features solve different problems.

| Feature | Purpose |
|---|---|
| `ref` | Let a method modify an existing variable |
| `out` | Let a method produce output through parameters |
| `in` | Pass by reference without allowing modification |
| `params` | Let a method accept many arguments of the same type |

---

# 14) Practical Examples

## Updating a value with `ref`

```csharp
static void AddRewardPoints(ref int points)
{
    points += 15;
}

int userPoints = 40;
AddRewardPoints(ref userPoints);

Console.WriteLine(userPoints);
```

### Output

```csharp
55
```

---

## Producing two values with `out`

```csharp
static void GetQuotientAndRemainder(int dividend, int divisor, out int quotient, out int remainder)
{
    quotient = dividend / divisor;
    remainder = dividend % divisor;
}

GetQuotientAndRemainder(53, 8, out int quotient, out int remainder);

Console.WriteLine($"Quotient: {quotient}");
Console.WriteLine($"Remainder: {remainder}");
```

### Output

```csharp
Quotient: 6
Remainder: 5
```

---

## Accepting many values with `params`

```csharp
static decimal AveragePrice(params decimal[] prices)
{
    if (prices.Length == 0)
    {
        return 0m;
    }

    decimal total = 0m;

    foreach (decimal price in prices)
    {
        total += price;
    }

    return total / prices.Length;
}
```

### Calls

```csharp
Console.WriteLine(AveragePrice());
Console.WriteLine(AveragePrice(12.5m, 18.0m, 24.5m));
```

### Output

```csharp
0
18.333333333333333333333333333
```

---

# 15) Common Mistakes

## Forgetting `ref` at the call site

### Wrong

```csharp
static void Raise(ref int number)
{
    number++;
}

int count = 3;
Raise(count);
```

### Right

```csharp
Raise(ref count);
```

---

## Not assigning every `out` parameter

### Wrong

```csharp
static void BuildPair(out int x, out int y)
{
    x = 10;
}
```

This causes an error because `y` is never assigned.

### Right

```csharp
static void BuildPair(out int x, out int y)
{
    x = 10;
    y = 20;
}
```

---

## Putting `params` in the wrong place

### Wrong

```csharp
static void Sample(params int[] values, string label)
{
}
```

### Right

```csharp
static void Sample(string label, params int[] values)
{
}
```

---

# 16) Quick Reference

| Syntax | Meaning |
|---|---|
| `void DoWork(int x)` | Pass by value |
| `void DoWork(ref int x)` | Pass by reference and allow changes |
| `void DoWork(out int x)` | Output value must be assigned |
| `void DoWork(in int x)` | Pass by reference without allowing changes |
| `void DoWork(params int[] x)` | Accept any number of `int` values |

---

# 17) One Compact Demo

```csharp
static void Adjust(ref int total)
{
    total += 3;
}

static void DescribeNumber(int value, out bool isEven, out int squared)
{
    isEven = value % 2 == 0;
    squared = value * value;
}

static int SumSet(params int[] values)
{
    int sum = 0;

    foreach (int value in values)
    {
        sum += value;
    }

    return sum;
}

int amount = 14;
Adjust(ref amount);
Console.WriteLine(amount);

DescribeNumber(9, out bool evenFlag, out int squareValue);
Console.WriteLine($"Even: {evenFlag}, Square: {squareValue}");

Console.WriteLine(SumSet(1, 2, 3, 4, 5));
```

### Output

```csharp
17
Even: False, Square: 81
15
```