# ✨ Primary Constructors in C# 12

In **C# 12**, you can place constructor parameters **directly on the type declaration**.

This is called a **primary constructor**.

```csharp
public class ServiceClient(string endpoint, int timeoutSeconds)
{
}
```

Here:

- `ServiceClient` is a class
- `endpoint` and `timeoutSeconds` are constructor parameters
- creating the object requires those values

```csharp
var client = new ServiceClient("https://api.example.dev", 30);
```

---

# 🧠 What this means

A primary constructor lets you write constructor parameters **next to the class name**, instead of declaring a separate constructor block first.

## Traditional style

```csharp
public class ServiceClient
{
    public ServiceClient(string endpoint, int timeoutSeconds)
    {
    }
}
```

## C# 12 style

```csharp
public class ServiceClient(string endpoint, int timeoutSeconds)
{
}
```

Both define a constructor, but the C# 12 version is shorter and often clearer.

---

# 📌 Basic syntax

## Class

```csharp
public class ProductCard(string title, decimal price)
{
}
```

## Struct

```csharp
public struct Measurement(int width, int height)
{
}
```

Primary constructors work with:

- **classes**
- **structs**

---

# 🔍 Important idea

The parameters in a primary constructor are **in scope throughout the type body**.

That means you can use them inside:

- fields
- properties
- methods
- expressions in the class body

Example:

```csharp
public class ProductCard(string title, decimal price)
{
    public string Title { get; } = title;
    public decimal Price { get; } = price;
}
```

Usage:

```csharp
var card = new ProductCard("Wireless Mouse", 49.99m);
Console.WriteLine(card.Title); // Wireless Mouse
Console.WriteLine(card.Price); // 49.99
```

---

# 🏗️ Using primary constructor parameters to initialize members

A common pattern is to copy the parameters into properties or fields.

```csharp
public class Account(string username, string email)
{
    public string Username { get; } = username;
    public string Email { get; } = email;
}
```

This behaves similarly to writing:

```csharp
public class Account
{
    public string Username { get; }
    public string Email { get; }

    public Account(string username, string email)
    {
        Username = username;
        Email = email;
    }
}
```

---

# ⚠️ Important distinction: parameters are not automatically properties

Primary constructor parameters are **just parameters**.

They do **not** automatically become public properties.

## Example

```csharp
public class Vehicle(string brand, int year)
{
}
```

This does **not** mean the class now has:

- `Brand`
- `Year`

The following would fail:

```csharp
var car = new Vehicle("Arvand", 2024);
// Console.WriteLine(car.brand); // Not accessible
```

If you want stored members, define them explicitly:

```csharp
public class Vehicle(string brand, int year)
{
    public string Brand { get; } = brand;
    public int Year { get; } = year;
}
```

---

# 🧱 Fields vs properties

You can use primary constructor parameters to initialize either **fields** or **properties**.

## Backing fields

```csharp
public class FileCache(string rootPath)
{
    private readonly string _rootPath = rootPath;
}
```

## Public properties

```csharp
public class FileCache(string rootPath)
{
    public string RootPath { get; } = rootPath;
}
```

## When to use which?

- Use **fields** for internal implementation details
- Use **properties** for values that should be exposed publicly

---

# 🛠️ Using parameters inside methods

Since the parameters are in scope in the type body, methods can use them too.

```csharp
public class Banner(string message)
{
    public void Print() => Console.WriteLine(message);
}
```

```csharp
var banner = new Banner("Deployment completed");
banner.Print();
```

---

# ⚠️ Be careful: using parameters directly can be misleading

This works:

```csharp
public class Banner(string message)
{
    public void Print() => Console.WriteLine(message);
}
```

But many developers prefer storing the value in a field or property for clarity:

```csharp
public class Banner(string message)
{
    private readonly string _message = message;

    public void Print() => Console.WriteLine(_message);
}
```

Why?

- it makes the object’s stored state explicit
- it is easier to read later
- it avoids confusion about whether the value is actually stored

---

# 📦 Dependency injection example

Primary constructors are especially nice when a class needs dependencies.

```csharp
public interface ILogger
{
    void Log(string text);
}
```

```csharp
public class ReportGenerator(ILogger logger)
{
    private readonly ILogger _logger = logger;

    public void Generate()
    {
        _logger.Log("Generating report...");
    }
}
```

This is shorter than the traditional form:

```csharp
public class ReportGenerator
{
    private readonly ILogger _logger;

    public ReportGenerator(ILogger logger)
    {
        _logger = logger;
    }

    public void Generate()
    {
        _logger.Log("Generating report...");
    }
}
```

---

# 🧮 Validation example

You can validate constructor parameters while initializing members.

```csharp
public class TemperatureSensor(string id, int sampleRate)
{
    public string Id { get; } = string.IsNullOrWhiteSpace(id)
        ? throw new ArgumentException("Sensor id is required.", nameof(id))
        : id;

    public int SampleRate { get; } = sampleRate <= 0
        ? throw new ArgumentOutOfRangeException(nameof(sampleRate))
        : sampleRate;
}
```

Usage:

```csharp
var sensor = new TemperatureSensor("sensor-west-01", 5);
```

---

# 🧭 Example with computed members

Primary constructor parameters can help build calculated values.

```csharp
public class InvoiceItem(decimal unitPrice, int quantity)
{
    public decimal UnitPrice { get; } = unitPrice;
    public int Quantity { get; } = quantity;
    public decimal Total => UnitPrice * Quantity;
}
```

```csharp
var item = new InvoiceItem(24.50m, 4);
Console.WriteLine(item.Total); // 98.00
```

---

# 🪜 Multiple members using the same parameter

One parameter can be used in several places.

```csharp
public class UserProfile(string displayName)
{
    public string DisplayName { get; } = displayName;
    public string NormalizedName { get; } = displayName.Trim().ToUpperInvariant();
}
```

---

# 🧩 Example with inheritance

Primary constructors also work with inheritance.

## Base class

```csharp
public class Person(string fullName)
{
    public string FullName { get; } = fullName;
}
```

## Derived class

```csharp
public class Instructor(string fullName, string subject)
    : Person(fullName)
{
    public string Subject { get; } = subject;
}
```

Usage:

```csharp
var teacher = new Instructor("Niloofar Ramin", "Mathematics");
Console.WriteLine(teacher.FullName);
Console.WriteLine(teacher.Subject);
```

---

# ⚠️ Constructor parameters vs object members

This is one of the most important things to understand.

## These are constructor parameters

```csharp
public class Session(string token, DateTime expiresAt)
{
}
```

## These are members

```csharp
public class Session(string token, DateTime expiresAt)
{
    public string Token { get; } = token;
    public DateTime ExpiresAt { get; } = expiresAt;
}
```

> **Primary constructor parameters are inputs to construction.**  
> They only become part of the object’s stored state if you assign them to fields or properties.

---

# 🆚 Traditional constructor vs primary constructor

| Feature | Traditional Constructor | Primary Constructor |
|---|---|---|
| Constructor declared separately | Yes | No |
| Parameters written on type declaration | No | Yes |
| Less boilerplate | No | Yes |
| Parameters automatically become properties | No | No |
| Good for dependency injection | Yes | Yes |
| Good for concise data setup | Yes | Yes |

---

# 🧪 Simple side-by-side example

## Traditional

```csharp
public class NotificationService
{
    private readonly string _channel;

    public NotificationService(string channel)
    {
        _channel = channel;
    }

    public void Send(string message)
    {
        Console.WriteLine($"[{_channel}] {message}");
    }
}
```

## Primary constructor

```csharp
public class NotificationService(string channel)
{
    private readonly string _channel = channel;

    public void Send(string message)
    {
        Console.WriteLine($"[{_channel}] {message}");
    }
}
```

---

# 📍 Struct example

Primary constructors also work for structs.

```csharp
public struct ScreenSize(int width, int height)
{
    public int Width { get; } = width;
    public int Height { get; } = height;
    public int Area => Width * Height;
}
```

```csharp
var size = new ScreenSize(1920, 1080);
Console.WriteLine(size.Area);
```

---

# 🚫 Common misunderstanding

## Incorrect assumption

> “If I write parameters on the class declaration, C# automatically creates matching properties.”

That is **not true** for classes and structs with primary constructors.

Example:

```csharp
public class Order(string code, decimal amount)
{
}
```

This does **not** create:

- `Code`
- `Amount`

You must write them yourself:

```csharp
public class Order(string code, decimal amount)
{
    public string Code { get; } = code;
    public decimal Amount { get; } = amount;
}
```

---

# ✅ Good use cases

Primary constructors are especially useful when a type:

- needs a few required construction values
- mainly stores dependencies
- has simple initialization logic
- would otherwise require repetitive constructor boilerplate

Examples:

- services
- validators
- small domain types
- wrappers
- lightweight data holders

---

# ⚠️ When to be careful

Primary constructors can become harder to read if:

- the class body is large
- many parameters are used throughout the type
- the constructor logic is complex
- validation becomes extensive

In those cases, a traditional constructor may be easier to understand.

---

# 🧠 Mental model

> A **primary constructor** is just a constructor whose parameters are written **on the type declaration itself**.

It is mainly about:

- **less boilerplate**
- **cleaner initialization**
- **keeping constructor intent visible near the type name**

---

# 📌 Quick pattern reference

## Minimal form

```csharp
public class ApiClient(string baseUrl)
{
}
```

## Store in a property

```csharp
public class ApiClient(string baseUrl)
{
    public string BaseUrl { get; } = baseUrl;
}
```

## Store in a field

```csharp
public class ApiClient(string baseUrl)
{
    private readonly string _baseUrl = baseUrl;
}
```

## Use in inheritance

```csharp
public class Animal(string name)
{
    public string Name { get; } = name;
}

public class Dog(string name, string breed) : Animal(name)
{
    public string Breed { get; } = breed;
}
```