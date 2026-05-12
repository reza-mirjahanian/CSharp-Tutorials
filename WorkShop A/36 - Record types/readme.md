# 🎯 C# Record Types

`record` types are special reference or value types designed to represent **data-focused objects**.

They are especially useful when you want:

- **value-based equality**
- concise syntax
- immutability-friendly models
- easy copying with small changes
- good support for pattern matching

---

# 1. What problem do records solve?

In traditional classes, two objects with the same data are still different objects unless you override equality.

## Regular class example

```csharp
public class Employee
{
    public string FullName { get; init; }
    public int YearsOfExperience { get; init; }
}
```

```csharp
var e1 = new Employee { FullName = "Mina Darzi", YearsOfExperience = 6 };
var e2 = new Employee { FullName = "Mina Darzi", YearsOfExperience = 6 };

Console.WriteLine(e1 == e2);        // False
Console.WriteLine(e1.Equals(e2));   // False
```

Even though the data is identical, the two objects are treated as different because `class` uses **reference equality** by default.

---

# 2. Record basics

A `record` compares objects by their **contents**, not just by their memory reference.

## Record example

```csharp
public record EmployeeRecord(string FullName, int YearsOfExperience);
```

```csharp
var e1 = new EmployeeRecord("Mina Darzi", 6);
var e2 = new EmployeeRecord("Mina Darzi", 6);

Console.WriteLine(e1 == e2);        // True
Console.WriteLine(e1.Equals(e2));   // True
```

## Why?

Because records automatically generate:

- value-based `Equals`
- value-based `GetHashCode`
- helpful `ToString`
- support for non-destructive mutation with `with`

---

# 3. Positional records

A **positional record** defines its main data in the type declaration itself.

```csharp
public record Product(string Title, decimal UnitPrice);
```

This is roughly equivalent to a type with:

- properties
- constructor
- equality members
- `Deconstruct`
- `ToString`

## Usage

```csharp
var item = new Product("Mechanical Keyboard", 129.50m);

Console.WriteLine(item.Title);
Console.WriteLine(item.UnitPrice);
```

---

# 4. Generated properties in positional records

In a positional `record`, the parameters become public properties.

```csharp
public record Book(string Name, int PageCount);
```

This creates properties similar to:

```csharp
public string Name { get; init; }
public int PageCount { get; init; }
```

> `init` means the property can be assigned during object creation, but not freely changed afterward.

Example:

```csharp
var book = new Book("CLR via C#", 900);
// book.PageCount = 950;   // Not allowed
```

---

# 5. Records and value equality

## Equality behavior

Two record instances are equal if:

- they are the same record type
- their corresponding values are equal

```csharp
public record Customer(string Email, int LoyaltyPoints);
```

```csharp
var c1 = new Customer("ava@sample.dev", 120);
var c2 = new Customer("ava@sample.dev", 120);
var c3 = new Customer("nima@sample.dev", 120);

Console.WriteLine(c1 == c2); // True
Console.WriteLine(c1 == c3); // False
```

## Important note

For records, equality is based on the values of their members.

That makes them great for:

- DTOs
- API models
- configuration objects
- domain value objects

---

# 6. `ToString()` in records

Records automatically generate a readable `ToString()`.

```csharp
public record ServerConfig(string Host, int Port);
```

```csharp
var config = new ServerConfig("cache.internal", 6380);
Console.WriteLine(config);
```

Output:

```text
ServerConfig { Host = cache.internal, Port = 6380 }
```

This is very useful for:

- debugging
- logging
- quick inspection

---

# 7. Non-destructive mutation with `with`

One of the best features of records is the `with` expression.

It creates a **new object** based on an existing one, changing only selected members.

```csharp
public record AccountProfile(string Username, bool IsVerified);
```

```csharp
var original = new AccountProfile("sahar88", false);
var updated = original with { IsVerified = true };

Console.WriteLine(original); // AccountProfile { Username = sahar88, IsVerified = False }
Console.WriteLine(updated);  // AccountProfile { Username = sahar88, IsVerified = True }
```

## Why “non-destructive”?

Because the original object is not modified.

---

# 8. Records are immutable-friendly, not automatically immutable

A common misunderstanding:

> A record is **not automatically immutable**.

It is *friendly to immutability*, especially when using `init` properties.

## Mutable record example

```csharp
public record SessionInfo
{
    public string Token { get; set; }
    public DateTime ExpiresAt { get; set; }
}
```

This record is mutable because it uses `set`.

```csharp
var session = new SessionInfo
{
    Token = "ZX-91",
    ExpiresAt = DateTime.UtcNow.AddHours(2)
};

session.Token = "ZX-92";
```

## Immutable-friendly version

```csharp
public record SessionInfo(string Token, DateTime ExpiresAt);
```

Or:

```csharp
public record SessionInfo
{
    public string Token { get; init; }
    public DateTime ExpiresAt { get; init; }
}
```

---

# 9. Positional record vs record with body

You can define records in two main styles.

## A. Positional syntax

```csharp
public record Location(double Latitude, double Longitude);
```

Best when the type is mostly just data.

## B. Full body syntax

```csharp
public record Location
{
    public double Latitude { get; init; }
    public double Longitude { get; init; }
}
```

Best when you want:

- custom logic
- validation
- extra members
- explicit control over properties

---

# 10. Adding custom members to records

Records can contain methods, calculated properties, and other members.

```csharp
public record Rectangle(decimal Width, decimal Height)
{
    public decimal Area => Width * Height;
}
```

```csharp
var shape = new Rectangle(7.5m, 4m);
Console.WriteLine(shape.Area); // 30.0
```

You can also add methods:

```csharp
public record TemperatureReading(decimal Celsius)
{
    public decimal ToFahrenheit() => (Celsius * 9 / 5) + 32;
}
```

---

# 11. Validation in records

You can validate values in constructors or property initializers.

## Example with custom constructor logic

```csharp
public record InvoiceLine
{
    public string Description { get; init; }
    public int Quantity { get; init; }

    public InvoiceLine(string description, int quantity)
    {
        if (string.IsNullOrWhiteSpace(description))
            throw new ArgumentException("Description is required.", nameof(description));

        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(quantity));

        Description = description;
        Quantity = quantity;
    }
}
```

---

# 12. Records with inheritance

Records support inheritance.

```csharp
public record Vehicle(string Brand);
public record Car(string Brand, int DoorCount) : Vehicle(Brand);
public record Motorcycle(string Brand, bool HasTopBox) : Vehicle(Brand);
```

```csharp
Vehicle v1 = new Car("Kian Motors", 4);
Vehicle v2 = new Motorcycle("Radan", true);

Console.WriteLine(v1);
Console.WriteLine(v2);
```

## Why this is useful

This works very well with:

- pattern matching
- discriminated-style models
- message/event hierarchies

---

# 13. Equality and inheritance

Equality in records also considers the **runtime type**.

```csharp
public record Person(string Name);
public record Manager(string Name, int TeamSize) : Person(Name);
```

```csharp
Person p1 = new Person("Navid");
Person p2 = new Manager("Navid", 8);

Console.WriteLine(p1 == p2); // False
```

Even though the `Name` is the same, they are different record types.

> Equal data does **not** mean equal records if the actual record types differ.

---

# 14. Deconstruction

Positional records automatically support deconstruction.

```csharp
public record Coordinates(int Row, int Column);
```

```csharp
var point = new Coordinates(12, 34);
var (row, column) = point;

Console.WriteLine(row);     // 12
Console.WriteLine(column);  // 34
```

This is useful when you want to unpack values cleanly.

---

# 15. Pattern matching with records

Records pair nicely with pattern matching.

```csharp
public record Payment(decimal Amount, string Currency);
```

```csharp
var payment = new Payment(250m, "EUR");

if (payment is Payment(> 200m, "EUR"))
{
    Console.WriteLine("Large euro payment");
}
```

You can also use property patterns:

```csharp
if (payment is { Amount: > 200m, Currency: "EUR" })
{
    Console.WriteLine("Large euro payment");
}
```

---

# 16. Record classes vs record structs

There are two main families of records:

| Type | Kind | Default equality style | Nullable? |
|---|---|---|---|
| `record` / `record class` | Reference type | Value-based | Yes |
| `record struct` | Value type | Value-based | No, unless nullable value type |

---

# 17. `record class`

This is the default when you write `record`.

```csharp
public record class ApiKey(string Key, string Owner);
```

This is the same as:

```csharp
public record ApiKey(string Key, string Owner);
```

It is a **reference type**.

```csharp
ApiKey a = new("K-001", "reporting-service");
ApiKey b = new("K-001", "reporting-service");

Console.WriteLine(a == b); // True
```

---

# 18. `record struct`

A `record struct` is a **value type** with record-style features.

```csharp
public record struct Pixel(int X, int Y);
```

```csharp
var p1 = new Pixel(8, 12);
var p2 = new Pixel(8, 12);

Console.WriteLine(p1 == p2); // True
```

Useful for small data values where value-type behavior is desired.

Examples:

- coordinates
- measurements
- ranges
- lightweight identifiers

---

# 19. Mutable vs readonly record structs

## Mutable record struct

```csharp
public record struct Counter(int Value);
```

Its properties can be mutable depending on how it is declared.

## Readonly record struct

```csharp
public readonly record struct Distance(double Kilometers);
```

Use `readonly` when the value should not change after creation and you want better value-type semantics.

---

# 20. Customizing properties in positional records

You can redeclare generated properties.

```csharp
public record UserAccount(string Username)
{
    public string Username { get; init; } = Username.Trim();
}
```

This can be useful, but it should be done carefully to avoid confusion.

A clearer pattern is often:

```csharp
public record UserAccount
{
    public string Username { get; init; }

    public UserAccount(string username)
    {
        Username = username.Trim();
    }
}
```

---

# 21. Records with computed members

Records are excellent when some values are stored and others are derived.

```csharp
public record OrderLine(decimal UnitCost, int Count)
{
    public decimal Total => UnitCost * Count;
}
```

```csharp
var line = new OrderLine(19.99m, 3);
Console.WriteLine(line.Total); // 59.97
```

---

# 22. When records are a good fit

## ✅ Good use cases

- **value objects**
- **request/response models**
- **configuration settings**
- **messages/events**
- **immutable data carriers**
- **pattern-matching-friendly models**

## ⚠️ Less ideal when

- identity matters more than data equality
- the object is heavily mutable
- behavior is much more important than stored data
- you need classic entity semantics

> If two objects should be considered the same only because they are the *same instance*, a normal `class` may be a better choice.

---

# 23. Class vs record

| Feature | `class` | `record` |
|---|---|---|
| Default equality | Reference-based | Value-based |
| Built-in concise syntax | No | Yes |
| Built-in `with` support | No | Yes |
| Good for immutable models | Possible | Excellent |
| Auto-friendly `ToString()` | Basic | Rich generated form |
| Best for identity-based entities | Yes | Usually no |

---

# 24. Example: class vs record side by side

## Using a class

```csharp
public class ThemeOptions
{
    public string Mode { get; init; }
    public int FontSize { get; init; }
}
```

```csharp
var t1 = new ThemeOptions { Mode = "Dark", FontSize = 15 };
var t2 = new ThemeOptions { Mode = "Dark", FontSize = 15 };

Console.WriteLine(t1 == t2); // False
```

## Using a record

```csharp
public record ThemeOptionsRecord(string Mode, int FontSize);
```

```csharp
var t1 = new ThemeOptionsRecord("Dark", 15);
var t2 = new ThemeOptionsRecord("Dark", 15);

Console.WriteLine(t1 == t2); // True
```

---

# 25. Example: `with` in real usage

```csharp
public record NotificationSettings(bool EmailEnabled, bool SmsEnabled, string Language);
```

```csharp
var current = new NotificationSettings(true, false, "en-US");
var localized = current with { Language = "fa-IR" };
var smsEnabled = current with { SmsEnabled = true };
```

Each `with` expression creates a **new instance**.

---

# 26. Important caveat: shallow immutability

Records do **not** automatically protect nested mutable objects.

```csharp
public record Team(string Name, List<string> Members);
```

```csharp
var team = new Team("Platform", new List<string> { "Arman", "Yas" });
var copy = team with { };

copy.Members.Add("Niloofar");

Console.WriteLine(team.Members.Count); // 3
```

## Why did the original change too?

Because `with` performs a **shallow copy**.

- the outer record is copied
- the `List<string>` reference is reused

## Better options

- use immutable collections
- use arrays carefully
- avoid exposing mutable internals

Example:

```csharp
using System.Collections.Immutable;

public record Team(string Name, ImmutableArray<string> Members);
```

---

# 27. Record equality with reference-type members

If a record contains reference-type members, equality depends on how those members define equality.

```csharp
public record Folder(string Title, string[] Tags);
```

```csharp
var f1 = new Folder("Docs", new[] { "work", "draft" });
var f2 = new Folder("Docs", new[] { "work", "draft" });

Console.WriteLine(f1 == f2); // Usually False
```

## Why?

Because arrays use reference equality by default.

Even though the contents look the same, the array instances are different.

> Record equality is only as “deep” as the equality behavior of its members.

---

# 28. Named records with object initializer syntax

You do not have to use positional syntax.

```csharp
public record BlogPost
{
    public string Title { get; init; }
    public string Author { get; init; }
    public int ReadMinutes { get; init; }
}
```

```csharp
var post = new BlogPost
{
    Title = "Understanding async/await",
    Author = "Darya",
    ReadMinutes = 12
};
```

This style is useful when:

- property names matter more than parameter order
- the type has many optional members
- readability at call sites is important

---

# 29. Records and init-only setters

Records often pair with `init` setters.

```csharp
public record BuildOptions
{
    public string Configuration { get; init; } = "Release";
    public bool IncludeSymbols { get; init; } = false;
}
```

```csharp
var options = new BuildOptions
{
    Configuration = "Debug",
    IncludeSymbols = true
};
```

After creation, these values cannot normally be reassigned.

---

# 30. Practical model example

```csharp
public record ShippingAddress(
    string Recipient,
    string Street,
    string City,
    string PostalCode,
    string Country);
```

```csharp
var home = new ShippingAddress(
    "Leila Karimi",
    "18 Cedar Avenue",
    "Shiraz",
    "71845",
    "Iran");

var office = home with
{
    Recipient = "Leila Karimi - Office",
    Street = "42 Innovation Blvd"
};
```

This is a very natural use case for records:

- mostly data
- rarely mutated directly
- often copied with small changes

---

# 31. Mental model

> Think of a `record` as a type that says:  
> **“My data defines who I am.”**

Whereas a typical `class` often says:

> **“My identity as an object defines who I am.”**

---

# 32. Syntax cheat sheet

## Positional record class

```csharp
public record Order(int Id, decimal Total);
```

## Record class with body

```csharp
public record Order
{
    public int Id { get; init; }
    public decimal Total { get; init; }
}
```

## Record struct

```csharp
public record struct Size(int Width, int Height);
```

## Readonly record struct

```csharp
public readonly record struct Size(int Width, int Height);
```

## Copy with one changed member

```csharp
var resized = oldSize with { Width = 640 };
```

## Deconstruct

```csharp
var (width, height) = resized;
```