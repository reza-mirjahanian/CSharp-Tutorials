C# 9 and later significantly improved pattern matching, especially for matching objects using type, property, positional, relational, and logical patterns.

Below are the main enhancements and examples.

---

## 1. Type patterns

You can check an object’s runtime type and assign it to a variable.

```csharp
object value = "Hello";

if (value is string text)
{
    Console.WriteLine(text.Length);
}
```

You can also use it in a `switch` expression:

```csharp
static string Describe(object obj) => obj switch
{
    string s => $"String with length {s.Length}",
    int i => $"Integer: {i}",
    null => "Null",
    _ => "Unknown"
};
```

---

## 2. Property patterns

Property patterns let you match object properties directly.

```csharp
public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}
```

```csharp
Person person = new Person { Name = "Alice", Age = 25 };

if (person is { Age: >= 18 })
{
    Console.WriteLine("Adult");
}
```

You can match multiple properties:

```csharp
if (person is { Name: "Alice", Age: >= 18 })
{
    Console.WriteLine("Adult Alice");
}
```

---

## 3. Nested property patterns

You can match nested objects.

```csharp
public class Address
{
    public string City { get; set; }
}

public class Customer
{
    public string Name { get; set; }
    public Address Address { get; set; }
}
```

```csharp
Customer customer = new Customer
{
    Name = "John",
    Address = new Address { City = "London" }
};

if (customer is { Address: { City: "London" } })
{
    Console.WriteLine("Customer is in London");
}
```

C# 10 introduced a shorter syntax for nested property patterns:

```csharp
if (customer is { Address.City: "London" })
{
    Console.WriteLine("Customer is in London");
}
```

---

## 4. Relational patterns

Introduced in C# 9, relational patterns let you compare values using operators such as:

```csharp
<, <=, >, >=
```

Example:

```csharp
static string GetAgeGroup(int age) => age switch
{
    < 13 => "Child",
    >= 13 and < 20 => "Teenager",
    >= 20 and < 65 => "Adult",
    >= 65 => "Senior"
};
```

With object properties:

```csharp
static string Describe(Person person) => person switch
{
    { Age: < 18 } => "Minor",
    { Age: >= 18 and < 65 } => "Adult",
    { Age: >= 65 } => "Senior"
};
```

---

## 5. Logical patterns: `and`, `or`, `not`

C# 9 added logical pattern combinators.

### `and`

```csharp
if (person is { Age: >= 18 and <= 30 })
{
    Console.WriteLine("Young adult");
}
```

### `or`

```csharp
static bool IsWeekend(DayOfWeek day) =>
    day is DayOfWeek.Saturday or DayOfWeek.Sunday;
```

### `not`

```csharp
if (person is not null)
{
    Console.WriteLine("Person exists");
}
```

This is often preferred over:

```csharp
if (person != null)
{
}
```

Because pattern matching avoids overloaded equality operators.

---

## 6. Constant patterns

You can match exact values.

```csharp
static string GetStatusCodeMessage(int code) => code switch
{
    200 => "OK",
    404 => "Not Found",
    500 => "Server Error",
    _ => "Unknown"
};
```

With object properties:

```csharp
if (person is { Name: "Alice" })
{
    Console.WriteLine("Hello Alice");
}
```

---

## 7. Positional patterns

Positional patterns work with types that support `Deconstruct`.

Records support this naturally.

```csharp
public record Point(int X, int Y);
```

```csharp
Point point = new Point(10, 20);

string result = point switch
{
    (0, 0) => "Origin",
    (0, _) => "On Y axis",
    (_, 0) => "On X axis",
    (> 0, > 0) => "First quadrant",
    _ => "Other"
};
```

You can also use positional patterns in `if` statements:

```csharp
if (point is (> 0, > 0))
{
    Console.WriteLine("First quadrant");
}
```

---

## 8. Pattern matching with records

C# 9 introduced records, which work very well with pattern matching.

```csharp
public record Person(string Name, int Age);
```

```csharp
Person person = new("Alice", 25);

string description = person switch
{
    ("Alice", >= 18) => "Adult Alice",
    (_, < 18) => "Minor",
    (_, >= 18) => "Adult"
};
```

You can also use property patterns:

```csharp
string description = person switch
{
    { Name: "Alice", Age: >= 18 } => "Adult Alice",
    { Age: < 18 } => "Minor",
    _ => "Other"
};
```

---

## 9. Pattern variables

You can capture matched values.

```csharp
if (person is { Name: var name, Age: >= 18 })
{
    Console.WriteLine($"{name} is an adult");
}
```

Another example:

```csharp
static string Describe(object obj) => obj switch
{
    string { Length: var length } => $"String length: {length}",
    int number => $"Integer: {number}",
    _ => "Unknown"
};
```

---

## 10. Discard pattern `_`

The discard pattern matches anything.

```csharp
static string Describe(object obj) => obj switch
{
    string s => $"String: {s}",
    int i => $"Integer: {i}",
    _ => "Something else"
};
```

---

## 11. Parenthesized patterns

C# 9 allows parentheses to clarify precedence.

```csharp
if (person is { Age: (> 18 and < 65) })
{
    Console.WriteLine("Working age adult");
}
```

Useful with `and` / `or` combinations:

```csharp
if (person is { Age: (< 18 or > 65) })
{
    Console.WriteLine("Dependent age group");
}
```

---

## 12. Improved `switch` expressions

Pattern matching becomes especially useful with switch expressions.

```csharp
static decimal CalculateDiscount(Customer customer) => customer switch
{
    { IsPremium: true, YearsRegistered: >= 5 } => 0.20m,
    { IsPremium: true } => 0.10m,
    { YearsRegistered: >= 5 } => 0.05m,
    _ => 0.00m
};
```

Example class:

```csharp
public class Customer
{
    public bool IsPremium { get; set; }
    public int YearsRegistered { get; set; }
}
```

---

## 13. Matching object type and properties together

You can combine type and property patterns.

```csharp
object obj = new Person("Alice", 25);

string result = obj switch
{
    Person { Name: "Alice", Age: >= 18 } => "Adult Alice",
    Person { Age: < 18 } => "Minor person",
    Person p => $"Some person named {p.Name}",
    _ => "Unknown object"
};
```

With class:

```csharp
public record Person(string Name, int Age);
```

---

## 14. Extended property patterns in C# 10

C# 10 made deeply nested property matching cleaner.

Before C# 10:

```csharp
if (customer is { Address: { City: "London" } })
{
}
```

C# 10 and later:

```csharp
if (customer is { Address.City: "London" })
{
}
```

Another example:

```csharp
if (order is { Customer.Address.Country: "UK" })
{
    Console.WriteLine("UK order");
}
```

---

## 15. List patterns in C# 11

C# 11 introduced list patterns for arrays and list-like structures.

```csharp
int[] numbers = { 1, 2, 3 };

string result = numbers switch
{
    [] => "Empty",
    [1, 2, 3] => "Exactly 1, 2, 3",
    [1, _, _] => "Starts with 1 and has 3 items",
    [1, ..] => "Starts with 1",
    [.., 3] => "Ends with 3",
    _ => "Other"
};
```

You can also capture ranges:

```csharp
int[] numbers = { 1, 2, 3, 4, 5 };

if (numbers is [1, .. var middle, 5])
{
    Console.WriteLine(string.Join(", ", middle));
}
```

Output:

```text
2, 3, 4
```

---

## 16. Pattern matching with nullable values

Pattern matching is useful for null checks.

```csharp
string? name = GetName();

if (name is not null)
{
    Console.WriteLine(name.Length);
}
```

You can also combine it with property patterns:

```csharp
if (name is { Length: > 0 })
{
    Console.WriteLine("Non-empty string");
}
```

This checks both:

1. `name` is not null
2. `Length > 0`

---

## 17. Practical example

```csharp
public abstract record Shape;

public record Circle(double Radius) : Shape;

public record Rectangle(double Width, double Height) : Shape;

public record Triangle(double Base, double Height) : Shape;
```

```csharp
static string DescribeShape(Shape shape) => shape switch
{
    Circle { Radius: <= 0 } => "Invalid circle",
    Circle { Radius: < 10 } => "Small circle",
    Circle => "Large circle",

    Rectangle { Width: <= 0 or Height: <= 0 } => "Invalid rectangle",
    Rectangle { Width: var w, Height: var h } when w == h => "Square",
    Rectangle { Width: > 100 or Height: > 100 } => "Large rectangle",
    Rectangle => "Rectangle",

    Triangle { Base: <= 0 or Height: <= 0 } => "Invalid triangle",
    Triangle => "Triangle",

    _ => "Unknown shape"
};
```

---

## Summary

Important C# 9+ pattern matching enhancements include:

| Feature | Version | Example |
|---|---:|---|
| Relational patterns | C# 9 | `age is >= 18` |
| Logical patterns | C# 9 | `age is >= 18 and < 65` |
| `not` pattern | C# 9 | `obj is not null` |
| Parenthesized patterns | C# 9 | `x is (> 0 and < 10)` |
| Records with positional patterns | C# 9 | `person is ("Alice", >= 18)` |
| Extended property patterns | C# 10 | `{ Address.City: "London" }` |
| List patterns | C# 11 | `[1, .., 5]` |

C# pattern matching is especially powerful when combined with:

```csharp
switch expressions
records
property patterns
relational patterns
logical patterns
nullable reference types
```