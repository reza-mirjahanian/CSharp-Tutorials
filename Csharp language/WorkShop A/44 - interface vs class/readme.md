# C# `interface` vs `class`

In C#, both **`interface`** and **`class`** help you define structure and behavior, but they serve **different purposes**.

- **`class`** = describes an actual object with **data + behavior**
- **`interface`** = describes a **contract** that a type must follow

---

# The Big Idea

## `class`

A `class` is a blueprint for creating objects.

It can contain:

- fields
- properties
- methods
- constructors
- events
- full implementation logic

### Example

```csharp
public class Invoice
{
    public int InvoiceNumber { get; set; }
    public decimal Total { get; set; }

    public void Print()
    {
        Console.WriteLine($"Invoice #{InvoiceNumber} - Total: {Total}");
    }
}
```

This class defines a real type with data and behavior.

---

## `interface`

An `interface` defines **what** a type can do, not **how** it does it.

It usually contains:

- method signatures
- property definitions
- event definitions
- indexers

### Example

```csharp
public interface IPrintable
{
    void Print();
}
```

Any type that implements `IPrintable` must provide a `Print()` method.

---

# Core Difference

| Feature | `class` | `interface` |
|---|---|---|
| Purpose | Defines an object | Defines a contract |
| Can contain implementation | Yes | Usually no implementation requirement for the consumer’s contract |
| Can contain fields | Yes | No instance fields |
| Can be instantiated | Yes | No |
| Supports inheritance | Can inherit from one base class | Can inherit from multiple interfaces |
| Constructor allowed | Yes | No normal constructor usage like classes |
| Stores state | Yes | No object state storage |

---

# Think of It Like This

> A **class** is the actual machine.  
> An **interface** is the control standard the machine promises to support.

For example:

- `class SmartLamp` = real device
- `interface ISwitchable` = promises `TurnOn()` and `TurnOff()`

---

# 1. What a `class` Does

A class creates objects and can hold **state**.

## Example

```csharp
public class SmartLamp
{
    public string ModelName { get; set; }
    public bool IsOn { get; private set; }

    public SmartLamp(string modelName)
    {
        ModelName = modelName;
    }

    public void TurnOn()
    {
        IsOn = true;
        Console.WriteLine($"{ModelName} is now on.");
    }

    public void TurnOff()
    {
        IsOn = false;
        Console.WriteLine($"{ModelName} is now off.");
    }
}
```

## Usage

```csharp
var lamp = new SmartLamp("Luma X2");
lamp.TurnOn();
```

### Why this is a class

- It has **data**: `ModelName`, `IsOn`
- It has **behavior**: `TurnOn()`, `TurnOff()`
- It can be created with `new`

---

# 2. What an `interface` Does

An interface defines a rule that classes or structs agree to follow.

## Example

```csharp
public interface ISwitchable
{
    void TurnOn();
    void TurnOff();
}
```

This says:

- any implementing type must have:
  - `TurnOn()`
  - `TurnOff()`

But it does **not** say how they work.

---

# 3. A Class Implementing an Interface

A class can implement one or more interfaces.

## Example

```csharp
public interface IConnectable
{
    void Connect();
}

public class Speaker : IConnectable
{
    public void Connect()
    {
        Console.WriteLine("Speaker connected to audio source.");
    }
}
```

## Usage

```csharp
Speaker speaker = new Speaker();
speaker.Connect();
```

Here:

- `IConnectable` defines the contract
- `Speaker` provides the implementation

---

# 4. Why Interfaces Are Useful

Interfaces are useful when different types should share the **same capability** without sharing the same base class.

## Example

```csharp
public interface IExportable
{
    void Export(string path);
}
```

Now many different classes can implement it:

```csharp
public class Report : IExportable
{
    public void Export(string path)
    {
        Console.WriteLine($"Report exported to {path}");
    }
}

public class ImageFile : IExportable
{
    public void Export(string path)
    {
        Console.WriteLine($"Image saved to {path}");
    }
}
```

Even though `Report` and `ImageFile` are very different, both can be treated as `IExportable`.

---

# 5. Instantiation Difference

## A class can be instantiated

```csharp
var report = new Report();
```

## An interface cannot be instantiated

```csharp
IExportable item = new IExportable(); // invalid
```

Why?

Because an interface is only a contract, not a real object.

---

# 6. Implementation vs Contract

## Class = implementation

A class contains the actual code:

```csharp
public class Clock
{
    public void ShowTime()
    {
        Console.WriteLine(DateTime.Now.ToShortTimeString());
    }
}
```

## Interface = contract

```csharp
public interface ITimeDisplay
{
    void ShowTime();
}
```

The interface says a type must have `ShowTime()`, but not how it displays time.

---

# 7. Inheritance Rules

## Class inheritance

A class can inherit from **one** base class only.

```csharp
public class Device
{
    public string SerialCode { get; set; } = "";
}

public class Printer : Device
{
    public void PrintPage()
    {
        Console.WriteLine("Printing page...");
    }
}
```

`Printer` inherits from `Device`.

---

## Interface inheritance

An interface can inherit from **multiple interfaces**.

```csharp
public interface IStartable
{
    void Start();
}

public interface IStoppable
{
    void Stop();
}

public interface IControllable : IStartable, IStoppable
{
}
```

Now `IControllable` includes both `Start()` and `Stop()`.

---

## A class can implement multiple interfaces

```csharp
public class Robot : IStartable, IStoppable
{
    public void Start()
    {
        Console.WriteLine("Robot started.");
    }

    public void Stop()
    {
        Console.WriteLine("Robot stopped.");
    }
}
```

This is one of the biggest advantages of interfaces.

---

# 8. Fields and State

## Class can store state

```csharp
public class Account
{
    public decimal Balance { get; private set; }

    public void Deposit(decimal amount)
    {
        Balance += amount;
    }
}
```

This class stores changing data in `Balance`.

---

## Interface does not store object state

An interface can define properties, but it does not hold instance data itself.

```csharp
public interface IUserProfile
{
    string Username { get; set; }
}
```

The interface requires a `Username`, but the implementing type stores the actual value.

### Example implementation

```csharp
public class MemberProfile : IUserProfile
{
    public string Username { get; set; } = "";
}
```

---

# 9. Constructors

## Class can have constructors

```csharp
public class Session
{
    public string Token { get; }

    public Session(string token)
    {
        Token = token;
    }
}
```

## Interface does not work like that

Interfaces are not used to create objects directly, so they do not have normal constructors for object creation.

---

# 10. Access Modifiers

## In classes

Members can use access modifiers such as:

- `public`
- `private`
- `protected`
- `internal`

### Example

```csharp
public class CacheService
{
    private int _hits;

    public void AddHit()
    {
        _hits++;
    }
}
```

---

## In interfaces

Interface members are typically part of the public contract.

```csharp
public interface ILogger
{
    void Write(string message);
}
```

---

# 11. Real-World Comparison

## Using only classes

```csharp
public class EmailNotifier
{
    public void Send(string message)
    {
        Console.WriteLine($"Email sent: {message}");
    }
}

public class SmsNotifier
{
    public void Send(string message)
    {
        Console.WriteLine($"SMS sent: {message}");
    }
}
```

These classes have similar behavior, but there is no shared contract.

---

## Using an interface

```csharp
public interface INotifier
{
    void Send(string message);
}
```

```csharp
public class EmailNotifier : INotifier
{
    public void Send(string message)
    {
        Console.WriteLine($"Email dispatched: {message}");
    }
}

public class SmsNotifier : INotifier
{
    public void Send(string message)
    {
        Console.WriteLine($"Text message dispatched: {message}");
    }
}
```

Now both types can be handled in a common way.

---

# 12. Polymorphism with Interfaces

Interfaces enable polymorphism.

## Example

```csharp
public interface IPaymentHandler
{
    void Pay(decimal amount);
}

public class CardPayment : IPaymentHandler
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Paid {amount} using card.");
    }
}

public class WalletPayment : IPaymentHandler
{
    public void Pay(decimal amount)
    {
        Console.WriteLine($"Paid {amount} using digital wallet.");
    }
}
```

## Usage

```csharp
void ProcessPayment(IPaymentHandler handler)
{
    handler.Pay(245.75m);
}
```

```csharp
ProcessPayment(new CardPayment());
ProcessPayment(new WalletPayment());
```

### Benefit

The method works with **any** payment type that follows the contract.

---

# 13. When to Use a `class`

Use a `class` when you need:

- a real object
- stored data/state
- implementation details
- constructors
- inheritance from a base type
- object creation with `new`

### Typical examples

- `Customer`
- `Order`
- `DatabaseConnection`
- `Product`
- `FileManager`

---

# 14. When to Use an `interface`

Use an `interface` when you need:

- a shared contract
- multiple unrelated types to support the same behavior
- loose coupling
- easier testing and mocking
- polymorphism
- cleaner architecture

### Typical examples

- `ILogger`
- `IRepository`
- `IFormatter`
- `IEmailSender`
- `ICacheProvider`

---

# 15. Class and Interface Together

In practice, they are often used together.

## Example

```csharp
public interface IDataStore
{
    void Save(string content);
}
```

```csharp
public class FileDataStore : IDataStore
{
    public void Save(string content)
    {
        Console.WriteLine($"Saved to file: {content}");
    }
}
```

```csharp
public class DatabaseDataStore : IDataStore
{
    public void Save(string content)
    {
        Console.WriteLine($"Saved to database: {content}");
    }
}
```

Now code can depend on `IDataStore` instead of a specific class.

---

# 16. A Practical Dependency Example

```csharp
public interface IMessageWriter
{
    void Write(string text);
}

public class ConsoleMessageWriter : IMessageWriter
{
    public void Write(string text)
    {
        Console.WriteLine(text);
    }
}
```

```csharp
public class AlertService
{
    private readonly IMessageWriter _writer;

    public AlertService(IMessageWriter writer)
    {
        _writer = writer;
    }

    public void SendAlert()
    {
        _writer.Write("System alert triggered.");
    }
}
```

## Usage

```csharp
var writer = new ConsoleMessageWriter();
var service = new AlertService(writer);
service.SendAlert();
```

### Why this is good

`AlertService` depends on the **interface**, not a specific implementation.

That means you can later replace `ConsoleMessageWriter` with:

- a file writer
- a network writer
- a mock writer for tests

---

# 17. Side-by-Side Example

## Interface

```csharp
public interface IPlayable
{
    void Play();
}
```

## Class implementing the interface

```csharp
public class MusicTrack : IPlayable
{
    public string Title { get; set; } = "";

    public void Play()
    {
        Console.WriteLine($"Playing track: {Title}");
    }
}
```

---

# 18. Quick Comparison Table

| Topic | `class` | `interface` |
|---|---|---|
| Represents | Object/type | Capability/contract |
| Has method bodies | Yes | Usually contract-focused |
| Has fields | Yes | No |
| Has constructors | Yes | No |
| Can create instance | Yes | No |
| Stores state | Yes | No |
| Multiple inheritance | No, only one base class | Yes, multiple interfaces |
| Best for | Real implementations | Abstraction and loose coupling |

---

# 19. Common Interview-Style Difference

## `class`

> “Here is a complete object with data and working behavior.”

## `interface`

> “Any type that agrees to this must provide these members.”

---

# 20. Short Example Showing Both

```csharp
public interface IRunner
{
    void Run();
}

public class Athlete : IRunner
{
    public string Name { get; set; } = "Runner-17";

    public void Run()
    {
        Console.WriteLine($"{Name} is running on the track.");
    }
}
```

### In this code

- `IRunner` defines the contract
- `Athlete` is the real implementation
- `Athlete` can be instantiated
- `IRunner` cannot be instantiated

---

# 21. Rules to Remember

## About `class`

- Can contain full implementation
- Can store data
- Can inherit from one class
- Can implement multiple interfaces
- Can be instantiated

## About `interface`

- Defines required members
- Cannot be instantiated directly
- Does not hold object state
- Supports multiple inheritance between interfaces
- Is excellent for abstraction

---

# 22. Simple Mental Model

- **Use `class`** for the **thing**
- **Use `interface`** for the **ability**

### Example

- `Car` → the thing → `class`
- `IDriveable` → the ability to drive → `interface`

---

# 23. Mini Example Set

## Class only

```csharp
public class Book
{
    public string Name { get; set; } = "Clean Code Notes";
}
```

## Interface only

```csharp
public interface IReadable
{
    void Read();
}
```

## Class implementing interface

```csharp
public class DigitalBook : IReadable
{
    public void Read()
    {
        Console.WriteLine("Opening digital book...");
    }
}
```