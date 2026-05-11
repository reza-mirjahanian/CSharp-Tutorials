# C# Returned Values Using Tuples

Tuples let a method return **multiple values at once** without creating a separate class or struct.

## Why use tuples?

Use tuples when:

- a method naturally produces **more than one result**
- the returned values are **closely related**
- creating a full type would be **unnecessary overhead**

## Basic idea

Instead of returning only one value like this:

```csharp
public int GetScore()
{
    return 95;
}
```

you can return several values together:

```csharp
public (int points, int level) GetPlayerStats()
{
    return (1200, 7);
}
```

Here:

- `int points` is the first returned value
- `int level` is the second returned value

## Calling a method that returns a tuple

```csharp
var stats = GetPlayerStats();

Console.WriteLine(stats.points);
Console.WriteLine(stats.level);
```

### Output

```csharp
1200
7
```

## Tuple syntax

A tuple type is written like this:

```csharp
(type1 name1, type2 name2, type3 name3)
```

Example:

```csharp
(string product, decimal price, bool available)
```

A tuple value is written like this:

```csharp
("Notebook", 18.75m, true)
```

## Example: returning student information

```csharp
public (string fullName, double average, bool passed) GetStudentReport()
{
    return ("Sara Nouri", 17.45, true);
}
```

Using it:

```csharp
var report = GetStudentReport();

Console.WriteLine($"Name: {report.fullName}");
Console.WriteLine($"Average: {report.average}");
Console.WriteLine($"Passed: {report.passed}");
```

## Returning tuples without element names

You can also return unnamed tuple elements:

```csharp
public (string, int) GetBookData()
{
    return ("Clean Coding", 320);
}
```

Access them with default names:

- `Item1`
- `Item2`
- `Item3`
- ...

```csharp
var book = GetBookData();

Console.WriteLine(book.Item1);
Console.WriteLine(book.Item2);
```

> Named tuple elements are usually easier to read than `Item1`, `Item2`, and so on.

---

# Aliasing Tuples

Tuple types can become long and repetitive.  
A **tuple alias** gives a complex tuple type a shorter, clearer name.

## Why alias a tuple?

Without an alias:

```csharp
(string title, decimal cost, int quantity, bool inStock)
```

This is fine once, but if used many times, it becomes hard to maintain.

## Creating a tuple alias

You can define an alias with `using`.

```csharp
using InventoryItem = (string title, decimal cost, int quantity, bool inStock);
```

Now `InventoryItem` can be used like a type name.

## Example

```csharp
using EmployeeRecord = (int id, string name, string department);

public class Program
{
    public static EmployeeRecord GetEmployee()
    {
        return (501, "Mina Karimi", "Finance");
    }

    public static void Main()
    {
        EmployeeRecord employee = GetEmployee();

        Console.WriteLine(employee.id);
        Console.WriteLine(employee.name);
        Console.WriteLine(employee.department);
    }
}
```

## Another example

```csharp
using OrderInfo = (int orderId, string customer, decimal totalAmount);

public class Shop
{
    public OrderInfo GetLatestOrder()
    {
        return (8842, "Arman", 249.90m);
    }
}
```

## Important note about aliases

A tuple alias:

- **does not create a new type**
- only creates a **different name** for an existing tuple type

That means this works:

```csharp
using PairA = (int left, int right);
using PairB = (int first, int second);

PairA a = (10, 20);
PairB b = a;
```

Even though the element names differ, the underlying tuple structure is compatible.

> Tuple aliases improve readability, but they are not the same as defining a custom class or struct.

---

# Deconstructing Tuples

**Deconstruction** means unpacking tuple values into separate variables.

Instead of this:

```csharp
var data = GetPlayerStats();
int points = data.points;
int level = data.level;
```

you can write:

```csharp
var (points, level) = GetPlayerStats();
```

This is shorter and often easier to read.

## Basic example

```csharp
public (string city, int zipCode) GetLocation()
{
    return ("Shiraz", 71845);
}
```

Deconstructing the result:

```csharp
var (city, zipCode) = GetLocation();

Console.WriteLine(city);
Console.WriteLine(zipCode);
```

## Deconstruction with explicit types

You can also specify the variable types:

```csharp
(string cityName, int postalCode) = GetLocation();
```

## Deconstructing existing tuple values

```csharp
var coordinates = (x: 14, y: 28);

var (x, y) = coordinates;

Console.WriteLine(x);
Console.WriteLine(y);
```

## Discards in deconstruction

Sometimes you only need some values and want to ignore the rest.  
Use `_` as a **discard**.

```csharp
public (string username, string email, DateTime createdAt) GetUser()
{
    return ("nima77", "nima@example.com", new DateTime(2025, 3, 12));
}
```

If you only need the username:

```csharp
var (username, _, _) = GetUser();

Console.WriteLine(username);
```

## Mixed practical example

```csharp
using WeatherInfo = (string condition, int temperature, int humidity);

public class WeatherService
{
    public WeatherInfo GetToday()
    {
        return ("Sunny", 29, 40);
    }
}
```

Using normal access:

```csharp
var weather = new WeatherService().GetToday();

Console.WriteLine(weather.condition);
Console.WriteLine(weather.temperature);
Console.WriteLine(weather.humidity);
```

Using deconstruction:

```csharp
var (condition, temperature, humidity) = new WeatherService().GetToday();

Console.WriteLine($"{condition} - {temperature}C - {humidity}%");
```

---

# Comparing Access Styles

| Style | Example | Best when |
|---|---|---|
| Named access | `result.totalPrice` | You want self-documenting code |
| Default tuple access | `result.Item1` | Names are not available |
| Deconstruction | `var (price, tax) = GetInvoice()` | You want local variables immediately |

---

# Useful Patterns

## 1. Returning two related values

```csharp
public (int min, int max) FindRange()
{
    return (4, 92);
}
```

Usage:

```csharp
var (min, max) = FindRange();
```

## 2. Returning success + message

```csharp
public (bool ok, string message) SaveDocument()
{
    return (true, "Saved successfully");
}
```

Usage:

```csharp
var result = SaveDocument();

if (result.ok)
{
    Console.WriteLine(result.message);
}
```

## 3. Returning calculated values

```csharp
public (double area, double perimeter) MeasureRectangle(double width, double height)
{
    return (width * height, 2 * (width + height));
}
```

Usage:

```csharp
var (area, perimeter) = MeasureRectangle(6.5, 3.2);
```

---

# Key Rules to Remember

## Returned tuples

- A method can return **multiple values** in one tuple
- Tuple elements can be:
  - **named**
  - **unnamed**
- Named elements make code easier to understand

## Tuple aliases

- Created with `using`
- Help shorten long tuple declarations
- Do **not** define a brand-new type

## Deconstruction

- Splits a tuple into separate variables
- Can use:
  - `var`
  - explicit types
  - discards with `_`

---

# Mini Reference

## Return a tuple

```csharp
public (string item, decimal amount) GetReceipt()
{
    return ("Pen", 3.50m);
}
```

## Read tuple values

```csharp
var receipt = GetReceipt();
Console.WriteLine(receipt.item);
Console.WriteLine(receipt.amount);
```

## Alias a tuple

```csharp
using ReceiptInfo = (string item, decimal amount);
```

## Deconstruct a tuple

```csharp
var (item, amount) = GetReceipt();
```

## Ignore unwanted values

```csharp
var (item, _) = ("Marker", 8.25m);
```