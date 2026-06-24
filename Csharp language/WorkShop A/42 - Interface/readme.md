# C# Interfaces

An **interface** in C# defines a *contract* that a class, struct, or record can implement.

An interface says:

> “Any type that implements me must provide these members.”

Interfaces are useful when you want different types to share the same behavior without forcing them to inherit from the same base class.

---

## 1. What Is an Interface?

An interface contains member declarations such as:

- Methods
- Properties
- Events
- Indexers
- Static members
- Default method implementations
- Generic constraints

A simple interface looks like this:

```csharp
public interface IPrintable
{
    void Print();
}
```

Any class that implements `IPrintable` must provide a `Print()` method.

```csharp
public class Invoice : IPrintable
{
    public void Print()
    {
        Console.WriteLine("Printing invoice...");
    }
}

Usage:

csharp
IPrintable document = new Invoice();
document.Print();

Output:

text
Printing invoice...
```

---

## 2. Interface Naming Convention

In C#, interface names usually start with the letter **`I`**.

| Interface Name | Meaning |
|---|---|
| `IRunnable` | Something that can run |
| `ISavable` | Something that can be saved |
| `ILogger` | Something that can log messages |
| `IRepository` | Something that manages data access |
| `INotificationSender` | Something that sends notifications |

Example:

```csharp
public interface ILogger
{
    void Log(string message);
}
```

---

## 3. Why Use Interfaces?

Interfaces are commonly used for:

1. **Abstraction**
2. **Loose coupling**
3. **Polymorphism**
4. **Dependency injection**
5. **Testing and mocking**
6. **Multiple behavior support**

---

## 4. Basic Interface Example

### Interface

```csharp
public interface IMessageSender
{
    void Send(string recipient, string message);
}
```

### Implementation 1

```csharp
public class EmailSender : IMessageSender
{
    public void Send(string recipient, string message)
    {
        Console.WriteLine($"Email sent to {recipient}: {message}");
    }
}
```

### Implementation 2

```csharp
public class SmsSender : IMessageSender
{
    public void Send(string recipient, string message)
    {
        Console.WriteLine($"SMS sent to {recipient}: {message}");
    }
}

### Usage

csharp
IMessageSender sender = new EmailSender();
sender.Send("maya@example.com", "Your order is ready.");

sender = new SmsSender();
sender.Send("+15551234567", "Your package has arrived.");

Output:

text
Email sent to maya@example.com: Your order is ready.
SMS sent to +15551234567: Your package has arrived.
```

---

## 5. Interface Members Are Public by Default

In an interface, members are normally public by default.

```csharp
public interface IWorker
{
    void Work();
}
```

This means the following is unnecessary:

```csharp
public interface IWorker
{
    public void Work();
}
```

Both are valid in modern C#, but the shorter version is more common.

---

## 6. Implementing an Interface

A class implements an interface using the `:` symbol.

```csharp
public interface IAnimal
{
    void MakeSound();
}

csharp
public class Cat : IAnimal
{
    public void MakeSound()
    {
        Console.WriteLine("Meow");
    }
}

csharp
public class Dog : IAnimal
{
    public void MakeSound()
    {
        Console.WriteLine("Woof");
    }
}

Usage:

csharp
IAnimal animal1 = new Cat();
IAnimal animal2 = new Dog();

animal1.MakeSound();
animal2.MakeSound();

Output:

text
Meow
Woof
```

---

## 7. Interfaces Support Polymorphism

Polymorphism allows different objects to be treated through the same interface.

```csharp
public interface IPaymentProcessor
{
    void ProcessPayment(decimal amount);
}

csharp
public class CardPaymentProcessor : IPaymentProcessor
{
    public void ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Processed card payment: ${amount}");
    }
}

csharp
public class WalletPaymentProcessor : IPaymentProcessor
{
    public void ProcessPayment(decimal amount)
    {
        Console.WriteLine($"Processed wallet payment: ${amount}");
    }
}

csharp
public class CheckoutService
{
    public void Checkout(IPaymentProcessor paymentProcessor, decimal total)
    {
        paymentProcessor.ProcessPayment(total);
    }
}

Usage:

csharp
var checkout = new CheckoutService();

checkout.Checkout(new CardPaymentProcessor(), 49.99m);
checkout.Checkout(new WalletPaymentProcessor(), 18.75m);

Output:

text
Processed card payment: $49.99
Processed wallet payment: $18.75
```

---

## 8. Interface with Properties

An interface can require properties.

```csharp
public interface IProduct
{
    string Name { get; set; }
    decimal Price { get; set; }
}

Implementation:

csharp
public class Book : IProduct
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}

Usage:

csharp
IProduct product = new Book
{
    Name = "Clean Architecture Notes",
    Price = 24.50m
};

Console.WriteLine($"{product.Name}: ${product.Price}");

Output:

text
Clean Architecture Notes: $24.50
```

---

## 9. Read-Only and Write-Only Interface Properties

### Read-Only Property

```csharp
public interface IUserProfile
{
    string Username { get; }
}

csharp
public class UserProfile : IUserProfile
{
    public string Username { get; }

    public UserProfile(string username)
    {
        Username = username;
    }
}

Usage:

csharp
IUserProfile profile = new UserProfile("lina_dev");

Console.WriteLine(profile.Username);
```

---

### Write-Only Property

Write-only properties are rare, but possible.

```csharp
public interface ISecretStore
{
    string Secret { set; }
}

csharp
public class MemorySecretStore : ISecretStore
{
    private string _secret = string.Empty;

    public string Secret
    {
        set
        {
            _secret = value;
            Console.WriteLine("Secret updated.");
        }
    }
}

Usage:

csharp
ISecretStore store = new MemorySecretStore();
store.Secret = "temporary-token-4821";
```

---

## 10. Interface with Methods and Properties

Interfaces often contain multiple members.

```csharp
public interface IAccount
{
    string OwnerName { get; }
    decimal Balance { get; }

    void Deposit(decimal amount);
    bool Withdraw(decimal amount);
}

Implementation:

csharp
public class SavingsAccount : IAccount
{
    public string OwnerName { get; }
    public decimal Balance { get; private set; }

    public SavingsAccount(string ownerName, decimal initialBalance)
    {
        OwnerName = ownerName;
        Balance = initialBalance;
    }

    public void Deposit(decimal amount)
    {
        Balance += amount;
    }

    public bool Withdraw(decimal amount)
    {
        if (amount > Balance)
        {
            return false;
        }

        Balance -= amount;
        return true;
    }
}

Usage:

csharp
IAccount account = new SavingsAccount("Nora", 300m);

account.Deposit(120m);

bool success = account.Withdraw(50m);

Console.WriteLine($"Owner: {account.OwnerName}");
Console.WriteLine($"Balance: {account.Balance}");
Console.WriteLine($"Withdrawal successful: {success}");

Output:

text
Owner: Nora
Balance: 370
Withdrawal successful: True
```

---

## 11. A Class Can Implement Multiple Interfaces

C# does **not** allow multiple class inheritance, but it does allow implementing multiple interfaces.

```csharp
public interface IReadable
{
    void Read();
}

public interface IWritable
{
    void Write(string content);
}

csharp
public class TextDocument : IReadable, IWritable
{
    private string _content = "Initial text";

    public void Read()
    {
        Console.WriteLine(_content);
    }

    public void Write(string content)
    {
        _content = content;
    }
}

Usage:

csharp
var document = new TextDocument();

document.Read();

document.Write("Updated meeting notes");
document.Read();

Output:

text
Initial text
Updated meeting notes
```

---

## 12. Interface Inheritance

An interface can inherit from another interface.

```csharp
public interface IEntity
{
    int Id { get; set; }
}

csharp
public interface IAuditableEntity : IEntity
{
    DateTime CreatedAt { get; set; }
    DateTime UpdatedAt { get; set; }
}
```

A class implementing `IAuditableEntity` must implement all members from both interfaces.

```csharp
public class Customer : IAuditableEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public string FullName { get; set; } = string.Empty;
}

Usage:

csharp
IAuditableEntity customer = new Customer
{
    Id = 101,
    CreatedAt = DateTime.Now,
    UpdatedAt = DateTime.Now
};

Console.WriteLine(customer.Id);
```

---

## 13. Interface vs Class

| Feature | Interface | Class |
|---|---|---|
| Can be instantiated directly | ❌ No | ✅ Yes |
| Can contain method declarations | ✅ Yes | ✅ Yes |
| Can contain method implementations | ✅ Yes, modern C# supports default methods | ✅ Yes |
| Can contain fields | ❌ No instance fields | ✅ Yes |
| Supports multiple inheritance | ✅ Yes, through multiple interfaces | ❌ No |
| Used for | Contracts and abstraction | State and behavior |
| Constructor allowed | ❌ No instance constructors | ✅ Yes |

---

## 14. Interface vs Abstract Class

| Feature | Interface | Abstract Class |
|---|---|---|
| Multiple inheritance support | ✅ A class can implement many interfaces | ❌ A class can inherit only one abstract class |
| Can define contract | ✅ Yes | ✅ Yes |
| Can contain fields | ❌ No instance fields | ✅ Yes |
| Can contain constructors | ❌ No instance constructors | ✅ Yes |
| Best for | Capabilities/roles | Shared base behavior |
| Example | `ILogger`, `IComparable`, `IDisposable` | `Stream`, `DbContext`, `AnimalBase` |

---

## 15. When to Use an Interface

Use an interface when:

1. You want to define a **capability**.

   ```csharp
   public interface IExportable
   {
       void Export(string filePath);
   }
   ```

2. Different classes should share behavior without sharing inheritance.

   ```csharp
   public class Report : IExportable
   {
       public void Export(string filePath)
       {
           Console.WriteLine($"Report exported to {filePath}");
       }
   }

   public class Chart : IExportable
   {
       public void Export(string filePath)
       {
           Console.WriteLine($"Chart exported to {filePath}");
       }
   }
   ```

3. You want easy testing and mocking.

   ```csharp
   public interface IClock
   {
       DateTime Now { get; }
   }
   ```

4. You want to reduce dependency on concrete classes.

   ```csharp
   public class ReminderService
   {
       private readonly IClock _clock;

       public ReminderService(IClock clock)
       {
           _clock = clock;
       }

       public bool IsDue(DateTime reminderTime)
       {
           return _clock.Now >= reminderTime;
       }
   }
   ```

---

## 16. Dependency Injection with Interfaces

Interfaces are heavily used in dependency injection.

### Interface

```csharp
public interface INotificationService
{
    void Notify(string userId, string message);
}

### Implementation

csharp
public class EmailNotificationService : INotificationService
{
    public void Notify(string userId, string message)
    {
        Console.WriteLine($"Email notification for {userId}: {message}");
    }
}

### Service Using the Interface

csharp
public class OrderService
{
    private readonly INotificationService _notificationService;

    public OrderService(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public void CompleteOrder(string orderId, string userId)
    {
        Console.WriteLine($"Order {orderId} completed.");

        _notificationService.Notify(userId, "Your order has been completed.");
    }
}

### Usage

csharp
INotificationService notificationService = new EmailNotificationService();

var orderService = new OrderService(notificationService);

orderService.CompleteOrder("ORD-2048", "user-72");

Output:

text
Order ORD-2048 completed.
Email notification for user-72: Your order has been completed.

---

## 17. Explicit Interface Implementation

Sometimes a class implements an interface member explicitly.

This means the member can only be accessed through the interface type.

csharp
public interface IScanner
{
    void Start();
}

csharp
public class BarcodeScanner : IScanner
{
    void IScanner.Start()
    {
        Console.WriteLine("Barcode scanner started.");
    }
}

Usage:

csharp
var scanner = new BarcodeScanner();

// scanner.Start(); 
// Error: Start is not directly accessible.

IScanner device = scanner;
device.Start();

Output:

text
Barcode scanner started.

---

## 18. Why Use Explicit Interface Implementation?

Explicit implementation is useful when:

1. Two interfaces have members with the same name.
2. You want to hide interface-specific methods from the public class API.
3. The implementation only makes sense when the object is used through the interface.

Example:

csharp
public interface IOnlinePlayer
{
    void Connect();
}

public interface ILocalPlayer
{
    void Connect();
}

csharp
public class GamePlayer : IOnlinePlayer, ILocalPlayer
{
    void IOnlinePlayer.Connect()
    {
        Console.WriteLine("Connected to online server.");
    }

    void ILocalPlayer.Connect()
    {
        Console.WriteLine("Connected to local session.");
    }
}

Usage:

csharp
var player = new GamePlayer();

IOnlinePlayer onlinePlayer = player;
onlinePlayer.Connect();

ILocalPlayer localPlayer = player;
localPlayer.Connect();

Output:

text
Connected to online server.
Connected to local session.

---

## 19. Default Interface Methods

Modern C# allows interfaces to include default method implementations.

csharp
public interface IGreeter
{
    void Greet(string name);

    void GreetMorning(string name)
    {
        Console.WriteLine($"Good morning, {name}!");
    }
}

Implementation:

csharp
public class FriendlyGreeter : IGreeter
{
    public void Greet(string name)
    {
        Console.WriteLine($"Hello, {name}!");
    }
}

Usage:

csharp
IGreeter greeter = new FriendlyGreeter();

greeter.Greet("Samira");
greeter.GreetMorning("Samira");

Output:

text
Hello, Samira!
Good morning, Samira!

> Default interface methods are useful when you want to add new behavior to an interface without forcing every existing implementation to change immediately.

---

## 20. Overriding a Default Interface Method

A class can provide its own implementation.

csharp
public interface IStatusReporter
{
    void Report()
    {
        Console.WriteLine("Status: Unknown");
    }
}

csharp
public class ServerStatusReporter : IStatusReporter
{
    public void Report()
    {
        Console.WriteLine("Status: Server is healthy");
    }
}

Usage:

csharp
IStatusReporter reporter = new ServerStatusReporter();
reporter.Report();

Output:

text
Status: Server is healthy

---

## 21. Interface with Events

Interfaces can declare events.

csharp
public interface IDownloadTask
{
    event EventHandler Completed;

    void Start();
}

Implementation:

csharp
public class FileDownloadTask : IDownloadTask
{
    public event EventHandler? Completed;

    public void Start()
    {
        Console.WriteLine("Downloading file...");

        Completed?.Invoke(this, EventArgs.Empty);
    }
}

Usage:

csharp
IDownloadTask task = new FileDownloadTask();

task.Completed += (sender, args) =>
{
    Console.WriteLine("Download completed.");
};

task.Start();

Output:

text
Downloading file...
Download completed.

---

## 22. Interface with Indexers

An interface can declare an indexer.

csharp
public interface IStringCollection
{
    string this[int index] { get; set; }
}

Implementation:

csharp
public class SimpleStringCollection : IStringCollection
{
    private readonly string[] _items = new string[3];

    public string this[int index]
    {
        get => _items[index];
        set => _items[index] = value;
    }
}

Usage:

csharp
IStringCollection names = new SimpleStringCollection();

names[0] = "Aria";
names[1] = "Mina";
names[2] = "Rayan";

Console.WriteLine(names[1]);

Output:

text
Mina

---

## 23. Generic Interfaces

A generic interface works with different data types.

csharp
public interface IRepository<T>
{
    void Add(T item);
    T? FindById(int id);
    IEnumerable<T> GetAll();
}

Example model:

csharp
public class Course
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
}

Implementation:

csharp
public class CourseRepository : IRepository<Course>
{
    private readonly List<Course> _courses = new();

    public void Add(Course item)
    {
        _courses.Add(item);
    }

    public Course? FindById(int id)
    {
        return _courses.FirstOrDefault(course => course.Id == id);
    }

    public IEnumerable<Course> GetAll()
    {
        return _courses;
    }
}

Usage:

csharp
IRepository<Course> repository = new CourseRepository();

repository.Add(new Course { Id = 1, Title = "C# Fundamentals" });
repository.Add(new Course { Id = 2, Title = "LINQ Essentials" });

Course? course = repository.FindById(2);

Console.WriteLine(course?.Title);

Output:

text
LINQ Essentials

---

## 24. Generic Interface Constraints

Interfaces can be used in generic constraints.

csharp
public interface IValidatable
{
    bool IsValid();
}

csharp
public class RegistrationForm : IValidatable
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public bool IsValid()
    {
        return Email.Contains("@") && Password.Length >= 8;
    }
}

Generic method:

csharp
public static class Validator
{
    public static bool Validate<T>(T item) where T : IValidatable
    {
        return item.IsValid();
    }
}

Usage:

csharp
var form = new RegistrationForm
{
    Email = "nina@example.com",
    Password = "forest882"
};

bool result = Validator.Validate(form);

Console.WriteLine(result);

Output:

text
True

---

## 25. Interfaces and Structs

Structs can implement interfaces too.

csharp
public interface IMovable
{
    void Move(int x, int y);
}

csharp
public struct Point2D : IMovable
{
    public int X { get; private set; }
    public int Y { get; private set; }

    public void Move(int x, int y)
    {
        X += x;
        Y += y;
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}

Usage:

csharp
Point2D point = new Point2D();

point.Move(4, 7);

Console.WriteLine(point);

Output:

text
(4, 7)

---

## 26. Interfaces and Records

Records can implement interfaces.

csharp
public interface IIdentifiable
{
    Guid Id { get; }
}

csharp
public record CustomerRecord(Guid Id, string Name) : IIdentifiable;

Usage:

csharp
IIdentifiable customer = new CustomerRecord(
    Guid.NewGuid(),
    "Elena"
);

Console.WriteLine(customer.Id);

---

## 27. Interface References

An interface variable can hold any object that implements that interface.

csharp
public interface IShape
{
    double GetArea();
}

csharp
public class Circle : IShape
{
    public double Radius { get; }

    public Circle(double radius)
    {
        Radius = radius;
    }

    public double GetArea()
    {
        return Math.PI * Radius * Radius;
    }
}

csharp
public class Rectangle : IShape
{
    public double Width { get; }
    public double Height { get; }

    public Rectangle(double width, double height)
    {
        Width = width;
        Height = height;
    }

    public double GetArea()
    {
        return Width * Height;
    }
}

Usage:

csharp
List<IShape> shapes = new()
{
    new Circle(3),
    new Rectangle(5, 8),
    new Circle(2)
};

foreach (IShape shape in shapes)
{
    Console.WriteLine(shape.GetArea());
}

Example output:

text
28.274333882308138
40
12.566370614359172

---

## 28. Casting to an Interface

You can cast an object to an interface if it implements that interface.

csharp
public interface IArchivable
{
    void Archive();
}

csharp
public class ReportFile : IArchivable
{
    public void Archive()
    {
        Console.WriteLine("Report file archived.");
    }
}

Usage:

csharp
object item = new ReportFile();

if (item is IArchivable archivable)
{
    archivable.Archive();
}

Output:

text
Report file archived.

---

## 29. Checking Whether an Object Implements an Interface

Use the `is` operator.

csharp
public interface ICacheable
{
    string CacheKey { get; }
}

csharp
public class ProductPage : ICacheable
{
    public string CacheKey => "product-page-884";
}

Usage:

csharp
object page = new ProductPage();

if (page is ICacheable cacheable)
{
    Console.WriteLine($"Cache key: {cacheable.CacheKey}");
}
else
{
    Console.WriteLine("This object cannot be cached.");
}

Output:

text
Cache key: product-page-884

---

## 30. Built-In Interfaces in C#

C# and .NET include many commonly used interfaces.

| Interface | Purpose |
|---|---|
| `IEnumerable<T>` | Allows iteration using `foreach` |
| `IEnumerator<T>` | Controls iteration |
| `IDisposable` | Releases unmanaged or expensive resources |
| `IComparable<T>` | Compares one object with another |
| `IComparer<T>` | Provides custom comparison logic |
| `IEquatable<T>` | Compares equality with another object |
| `ICollection<T>` | Represents a collection that can be modified |
| `IList<T>` | Represents an index-based list |
| `IDictionary<TKey, TValue>` | Represents a key-value collection |
| `IReadOnlyList<T>` | Represents a read-only index-based list |
| `IAsyncEnumerable<T>` | Allows asynchronous iteration |

---

## 31. Example: `IComparable<T>`

`IComparable<T>` allows objects to be sorted.

csharp
public class Student : IComparable<Student>
{
    public string Name { get; set; } = string.Empty;
    public int Score { get; set; }

    public int CompareTo(Student? other)
    {
        if (other is null)
        {
            return 1;
        }

        return Score.CompareTo(other.Score);
    }
}

Usage:

csharp
var students = new List<Student>
{
    new Student { Name = "Omid", Score = 91 },
    new Student { Name = "Sara", Score = 84 },
    new Student { Name = "Leo", Score = 96 }
};

students.Sort();

foreach (Student student in students)
{
    Console.WriteLine($"{student.Name}: {student.Score}");
}

Output:

text
Sara: 84
Omid: 91
Leo: 96

---

## 32. Example: `IDisposable`

`IDisposable` is used to clean up resources.

csharp
public class TemporaryFileWriter : IDisposable
{
    private readonly string _filePath;
    private bool _disposed;

    public TemporaryFileWriter(string filePath)
    {
        _filePath = filePath;
        Console.WriteLine($"Opened file: {_filePath}");
    }

    public void Write(string content)
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(TemporaryFileWriter));
        }

        Console.WriteLine($"Writing: {content}");
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            Console.WriteLine($"Closed file: {_filePath}");
            _disposed = true;
        }
    }
}

Usage with `using`:

csharp
using var writer = new TemporaryFileWriter("notes-temp.txt");

writer.Write("Remember to review interface examples.");

Output:

text
Opened file: notes-temp.txt
Writing: Remember to review interface examples.
Closed file: notes-temp.txt

---

## 33. Example: `IEnumerable<T>`

`IEnumerable<T>` allows an object to be used in a `foreach` loop.

csharp
public class Playlist : IEnumerable<string>
{
    private readonly List<string> _songs = new()
    {
        "Blue Horizon",
        "Night Lantern",
        "Silver Rain"
    };

    public IEnumerator<string> GetEnumerator()
    {
        return _songs.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

Because this example uses non-generic `IEnumerable`, include:

csharp
using System.Collections;
using System.Collections.Generic;

Usage:

csharp
var playlist = new Playlist();

foreach (string song in playlist)
{
    Console.WriteLine(song);
}

Output:

text
Blue Horizon
Night Lantern
Silver Rain

---

## 34. Interface Segregation Principle

A good interface should usually be small and focused.

Instead of one large interface:

csharp
public interface IMachine
{
    void Print();
    void Scan();
    void Fax();
}

This can be problematic because not every machine supports every feature.

Better:

csharp
public interface IPrinter
{
    void Print();
}

public interface IScanner
{
    void Scan();
}

public interface IFaxMachine
{
    void Fax();
}

Now classes can implement only what they need.

csharp
public class BasicPrinter : IPrinter
{
    public void Print()
    {
        Console.WriteLine("Printing page...");
    }
}

csharp
public class OfficePrinter : IPrinter, IScanner, IFaxMachine
{
    public void Print()
    {
        Console.WriteLine("Printing page...");
    }

    public void Scan()
    {
        Console.WriteLine("Scanning document...");
    }

    public void Fax()
    {
        Console.WriteLine("Sending fax...");
    }
}

---

## 35. Common Mistakes with Interfaces

### Mistake 1: Forgetting to Implement All Members

csharp
public interface IReportGenerator
{
    void Generate();
    void Save(string path);
}

Incorrect:

csharp
public class PdfReportGenerator : IReportGenerator
{
    public void Generate()
    {
        Console.WriteLine("PDF report generated.");
    }
}

This causes a compiler error because `Save` is missing.

Correct:

csharp
public class PdfReportGenerator : IReportGenerator
{
    public void Generate()
    {
        Console.WriteLine("PDF report generated.");
    }

    public void Save(string path)
    {
        Console.WriteLine($"PDF report saved to {path}");
    }
}

---

### Mistake 2: Trying to Create an Interface Object Directly

Incorrect:

csharp
IReportGenerator generator = new IReportGenerator();

Interfaces cannot be instantiated directly.

Correct:

csharp
IReportGenerator generator = new PdfReportGenerator();

---

### Mistake 3: Using Interfaces for Everything

Not every class needs an interface.

Avoid unnecessary interfaces like:

csharp
public interface IUserNameFormatter
{
    string Format(string firstName, string lastName);
}

If there is only one implementation and no need for abstraction, a simple class may be enough.

csharp
public class UserNameFormatter
{
    public string Format(string firstName, string lastName)
    {
        return $"{firstName} {lastName}";
    }
}

---

## 36. Practical Example: Logging

### Interface

csharp
public interface ILogger
{
    void Info(string message);
    void Error(string message);
}

### Console Logger

csharp
public class ConsoleLogger : ILogger
{
    public void Info(string message)
    {
        Console.WriteLine($"INFO: {message}");
    }

    public void Error(string message)
    {
        Console.WriteLine($"ERROR: {message}");
    }
}

### File Logger

csharp
public class FileLogger : ILogger
{
    public void Info(string message)
    {
        Console.WriteLine($"Writing info to file: {message}");
    }

    public void Error(string message)
    {
        Console.WriteLine($"Writing error to file: {message}");
    }
}

### Service Using Logger

csharp
public class InventoryService
{
    private readonly ILogger _logger;

    public InventoryService(ILogger logger)
    {
        _logger = logger;
    }

    public void AddItem(string sku, int quantity)
    {
        _logger.Info($"Adding {quantity} units of item {sku}.");

        Console.WriteLine("Inventory updated.");
    }
}

### Usage

csharp
ILogger logger = new ConsoleLogger();

var inventoryService = new InventoryService(logger);

inventoryService.AddItem("SKU-7421", 15);

Output:

text
INFO: Adding 15 units of item SKU-7421.
Inventory updated.

---

## 37. Practical Example: Payment System

### Interface

csharp
public interface IPaymentMethod
{
    bool Pay(decimal amount);
}

### Credit Card Payment

csharp
public class CreditCardPayment : IPaymentMethod
{
    public bool Pay(decimal amount)
    {
        Console.WriteLine($"Paid ${amount} using credit card.");
        return true;
    }
}

### Gift Card Payment

csharp
public class GiftCardPayment : IPaymentMethod
{
    public bool Pay(decimal amount)
    {
        Console.WriteLine($"Paid ${amount} using gift card.");
        return true;
    }
}

### Payment Service

csharp
public class PaymentService
{
    public void CompletePayment(IPaymentMethod paymentMethod, decimal amount)
    {
        bool success = paymentMethod.Pay(amount);

        if (success)
        {
            Console.WriteLine("Payment completed.");
        }
        else
        {
            Console.WriteLine("Payment failed.");
        }
    }
}

### Usage

csharp
var paymentService = new PaymentService();

paymentService.CompletePayment(new CreditCardPayment(), 75.30m);
paymentService.CompletePayment(new GiftCardPayment(), 22.10m);

Output:

text
Paid $75.30 using credit card.
Payment completed.
Paid $22.10 using gift card.
Payment completed.

---

## 38. Practical Example: Testing with Interfaces

Interfaces make testing easier because you can replace real implementations with fake ones.

### Interface

csharp
public interface IEmailClient
{
    void SendEmail(string to, string subject, string body);
}

### Real Implementation

csharp
public class SmtpEmailClient : IEmailClient
{
    public void SendEmail(string to, string subject, string body)
    {
        Console.WriteLine($"Sending real email to {to}");
    }
}

### Service

csharp
public class WelcomeService
{
    private readonly IEmailClient _emailClient;

    public WelcomeService(IEmailClient emailClient)
    {
        _emailClient = emailClient;
    }

    public void WelcomeUser(string email)
    {
        _emailClient.SendEmail(
            email,
            "Welcome!",
            "Thanks for creating your account."
        );
    }
}

### Fake Implementation for Testing

csharp
public class FakeEmailClient : IEmailClient
{
    public List<string> SentMessages { get; } = new();

    public void SendEmail(string to, string subject, string body)
    {
        SentMessages.Add($"{to}|{subject}|{body}");
    }
}

### Test-Style Usage

csharp
var fakeEmailClient = new FakeEmailClient();
var welcomeService = new WelcomeService(fakeEmailClient);

welcomeService.WelcomeUser("tala@example.com");

Console.WriteLine(fakeEmailClient.SentMessages.Count);
Console.WriteLine(fakeEmailClient.SentMessages[0]);

Output:

text
1
tala@example.com|Welcome!|Thanks for creating your account.

---

## 39. Access Modifiers in Interfaces

Modern C# allows access modifiers in interfaces for certain scenarios, especially with default implementations.

Example:

csharp
public interface ITokenGenerator
{
    string GenerateToken(string userId)
    {
        string prefix = CreatePrefix();

        return $"{prefix}-{userId}-{Guid.NewGuid()}";
    }

    private string CreatePrefix()
    {
        return "auth";
    }
}

Usage:

csharp
public class TokenGenerator : ITokenGenerator
{
}

csharp
ITokenGenerator generator = new TokenGenerator();

string token = generator.GenerateToken("user-301");

Console.WriteLine(token);

Example output:

text
auth-user-301-6c5bd298-0a2a-4e6a-b12d-20e8e7d24210

---

## 40. Static Abstract Members in Interfaces

Modern C# supports static abstract members in interfaces.

This is useful for generic math and similar patterns.

csharp
public interface IParsableValue<TSelf>
    where TSelf : IParsableValue<TSelf>
{
    static abstract TSelf Parse(string value);
}

Implementation:

csharp
public readonly struct Temperature : IParsableValue<Temperature>
{
    public double Celsius { get; }

    public Temperature(double celsius)
    {
        Celsius = celsius;
    }

    public static Temperature Parse(string value)
    {
        return new Temperature(double.Parse(value));
    }

    public override string ToString()
    {
        return $"{Celsius}°C";
    }
}

Generic method:

csharp
public static class ValueParser
{
    public static T ParseValue<T>(string text)
        where T : IParsableValue<T>
    {
        return T.Parse(text);
    }
}

Usage:

csharp
Temperature temperature = ValueParser.ParseValue<Temperature>("23.5");

Console.WriteLine(temperature);

Output:

text
23.5°C

---

## 41. Static Virtual Members in Interfaces

Interfaces can also provide static virtual members with default implementations.

csharp
public interface IEntityLabel<TSelf>
    where TSelf : IEntityLabel<TSelf>
{
    static virtual string Label => typeof(TSelf).Name;
}

Implementation:

csharp
public class SupportTicket : IEntityLabel<SupportTicket>
{
}

Usage:

csharp
Console.WriteLine(IEntityLabel<SupportTicket>.Label);

Output:

text
SupportTicket

---

## 42. Sealed Interface Members

An interface member with a default implementation can be marked as `sealed` to prevent derived interfaces from overriding it.

csharp
public interface ISystemMessage
{
    sealed string GetPrefix()
    {
        return "[SYSTEM]";
    }
}

Usage:

csharp
public class SystemMessage : ISystemMessage
{
}

csharp
ISystemMessage message = new SystemMessage();

Console.WriteLine(message.GetPrefix());

Output:

text
[SYSTEM]

---

## 43. Combining Class Inheritance and Interfaces

A class can inherit from one class and implement multiple interfaces.

The base class must come first.

csharp
public class Person
{
    public string Name { get; set; } = string.Empty;
}

csharp
public interface IEmployee
{
    string EmployeeId { get; set; }
}

csharp
public interface ITimeTrackable
{
    void TrackHours(int hours);
}

csharp
public class Developer : Person, IEmployee, ITimeTrackable
{
    public string EmployeeId { get; set; } = string.Empty;

    public void TrackHours(int hours)
    {
        Console.WriteLine($"{Name} tracked {hours} hours.");
    }
}

Usage:

csharp
var developer = new Developer
{
    Name = "Reza",
    EmployeeId = "DEV-512"
};

developer.TrackHours(6);

Output:

text
Reza tracked 6 hours.

---

## 44. Interface Design Guidelines

### ✅ Prefer Small Interfaces

Good:

csharp
public interface IImageResizer
{
    byte[] Resize(byte[] imageBytes, int width, int height);
}

Less ideal:

csharp
public interface IImageTool
{
    byte[] Resize(byte[] imageBytes, int width, int height);
    byte[] Crop(byte[] imageBytes, int x, int y, int width, int height);
    byte[] Rotate(byte[] imageBytes, int degrees);
    byte[] AddWatermark(byte[] imageBytes, string text);
}

---

### ✅ Name Interfaces by Capability

Good:

csharp
public interface ISearchable
{
    IEnumerable<string> Search(string query);
}

Good:

csharp
public interface IOrderRepository
{
    Order? FindById(int id);
    void Save(Order order);
}

Less clear:

csharp
public interface IOrderThings
{
    void DoStuff();
}

---

### ✅ Depend on Interfaces, Not Concrete Classes

Less flexible:

csharp
public class ReportService
{
    private readonly ConsoleLogger _logger;

    public ReportService(ConsoleLogger logger)
    {
        _logger = logger;
    }
}

More flexible:

csharp
public class ReportService
{
    private readonly ILogger _logger;

    public ReportService(ILogger logger)
    {
        _logger = logger;
    }
}

---

### ✅ Avoid Interfaces with Only One Unnecessary Implementation

Sometimes this is useful:

csharp
public interface IWeatherClient
{
    decimal GetTemperature(string city);
}

Especially if:

- You need unit testing
- The implementation calls an external API
- You may replace it later

But this may be unnecessary:

csharp
public interface IStringTrimmer
{
    string TrimText(string text);
}

If it only wraps:

csharp
text.Trim();

---

## 45. Full Example: Notification System

### Interfaces

csharp
public interface INotificationChannel
{
    void Send(string destination, string message);
}

csharp
public interface INotificationFormatter
{
    string Format(string title, string body);
}

### Channel Implementations

csharp
public class EmailChannel : INotificationChannel
{
    public void Send(string destination, string message)
    {
        Console.WriteLine($"Email to {destination}: {message}");
    }
}

csharp
public class PushChannel : INotificationChannel
{
    public void Send(string destination, string message)
    {
        Console.WriteLine($"Push notification to {destination}: {message}");
    }
}

### Formatter Implementation

csharp
public class SimpleNotificationFormatter : INotificationFormatter
{
    public string Format(string title, string body)
    {
        return $"{title} - {body}";
    }
}

### Service

csharp
public class NotificationService
{
    private readonly INotificationChannel _channel;
    private readonly INotificationFormatter _formatter;

    public NotificationService(
        INotificationChannel channel,
        INotificationFormatter formatter)
    {
        _channel = channel;
        _formatter = formatter;
    }

    public void Notify(string destination, string title, string body)
    {
        string message = _formatter.Format(title, body);

        _channel.Send(destination, message);
    }
}

### Usage

csharp
INotificationChannel channel = new EmailChannel();
INotificationFormatter formatter = new SimpleNotificationFormatter();

var service = new NotificationService(channel, formatter);

service.Notify(
    "lara@example.com",
    "Appointment Reminder",
    "Your appointment starts at 3:30 PM."
);

Output:

text
Email to lara@example.com: Appointment Reminder - Your appointment starts at 3:30 PM.

---

## 46. Quick Syntax Reference

### Basic Interface

csharp
public interface IExample
{
    void DoWork();
}

### Class Implementation

csharp
public class Example : IExample
{
    public void DoWork()
    {
        Console.WriteLine("Working...");
    }
}

### Multiple Interfaces

csharp
public class MultiTool : IReadable, IWritable
{
    public void Read()
    {
        Console.WriteLine("Reading...");
    }

    public void Write(string text)
    {
        Console.WriteLine($"Writing: {text}");
    }
}

### Interface Inheritance

csharp
public interface IAdvancedExample : IExample
{
    void DoAdvancedWork();
}

### Generic Interface

csharp
public interface IStorage<T>
{
    void Save(T item);
    T? Load(int id);
}

### Explicit Interface Implementation

csharp
public class HiddenRunner : IRunnable
{
    void IRunnable.Run()
    {
        Console.WriteLine("Running through interface.");
    }
}

### Default Interface Method

csharp
public interface INotifier
{
    void Notify(string message);

    void NotifyDefault()
    {
        Notify("Default notification");
    }
}
```