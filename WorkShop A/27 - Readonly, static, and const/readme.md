# 🧱 `readonly`, `static`, and `const` Fields in C#

## 1. Fields in C#

A **field** is a variable declared directly inside a class or struct.

```csharp
class Product
{
    private string name;
    private decimal price;
}
```

Fields store data that belongs to an object or to the type itself.

---

# 🔹 Instance Fields

## What Is an Instance Field?

An **instance field** belongs to a specific object.

Each object gets its own copy of the field.

```csharp
class Customer
{
    public string Name;
    public int RewardPoints;
}
```

Usage:

```csharp
Customer customer1 = new Customer();
customer1.Name = "Lina";
customer1.RewardPoints = 120;

Customer customer2 = new Customer();
customer2.Name = "Omar";
customer2.RewardPoints = 45;

Console.WriteLine(customer1.Name);
Console.WriteLine(customer2.Name);
```

Output:

```text
Lina
Omar
```

> ✅ `customer1` and `customer2` each have their own `Name` and `RewardPoints`.

---

# 🧊 `readonly` Fields

## 1. What Is a `readonly` Field?

A **`readonly` field** can only be assigned:

1. **At declaration**
2. **Inside a constructor**

After the object is created, the field cannot be reassigned.

```csharp
class Invoice
{
    public readonly string InvoiceNumber;

    public Invoice(string invoiceNumber)
    {
        InvoiceNumber = invoiceNumber;
    }
}
```

Usage:

```csharp
Invoice invoice = new Invoice("INV-2048");

Console.WriteLine(invoice.InvoiceNumber);
```

Output:

```text
INV-2048
```

---

## 2. Reassigning a `readonly` Field Is Not Allowed

```csharp
class Invoice
{
    public readonly string InvoiceNumber;

    public Invoice(string invoiceNumber)
    {
        InvoiceNumber = invoiceNumber;
    }

    public void ChangeInvoiceNumber()
    {
        InvoiceNumber = "INV-9999"; // ❌ Compile-time error
    }
}
```

> ⚠️ A `readonly` field cannot be assigned outside its declaration or constructor.

---

## 3. Assigning `readonly` at Declaration

```csharp
class ServerSettings
{
    public readonly int MaxConnections = 250;
}
```

Usage:

```csharp
ServerSettings settings = new ServerSettings();

Console.WriteLine(settings.MaxConnections);
```

Output:

```text
250
```

---

## 4. Assigning `readonly` in the Constructor

```csharp
class UserSession
{
    public readonly string SessionId;
    public readonly DateTime StartedAt;

    public UserSession(string sessionId)
    {
        SessionId = sessionId;
        StartedAt = DateTime.UtcNow;
    }
}
```

Usage:

```csharp
UserSession session = new UserSession("S-7312");

Console.WriteLine(session.SessionId);
Console.WriteLine(session.StartedAt);
```

---

## 5. Why Use `readonly`?

Use `readonly` when a value should be set once and never reassigned afterward.

### ✅ Common uses

- Object identifiers
- Creation dates
- Configuration values passed into a constructor
- Dependency fields
- Values that should not change after initialization

```csharp
class EmailSender
{
    private readonly string smtpServer;

    public EmailSender(string server)
    {
        smtpServer = server;
    }

    public void Send(string to, string message)
    {
        Console.WriteLine($"Sending email through {smtpServer}");
    }
}
```

---

# ⚠️ `readonly` with Reference Types

## 1. `readonly` Prevents Reassignment, Not Mutation

If a `readonly` field stores a reference type, the reference cannot be changed, but the object itself may still be mutable.

```csharp
class ShoppingCart
{
    public readonly List<string> Items = new List<string>();
}
```

Usage:

```csharp
ShoppingCart cart = new ShoppingCart();

cart.Items.Add("Notebook");
cart.Items.Add("Pencil");

Console.WriteLine(cart.Items.Count);
```

Output:

```text
2
```

This is allowed because the list object is not being replaced.

---

## 2. Reassigning the Reference Is Not Allowed

```csharp
class ShoppingCart
{
    public readonly List<string> Items = new List<string>();

    public void Reset()
    {
        Items = new List<string>(); // ❌ Compile-time error
    }
}
```

> 🧠 `readonly` means the field cannot point to a different object after construction.

---

## 3. Safer Pattern with Read-Only Collections

```csharp
class ShoppingCart
{
    private readonly List<string> items = new List<string>();

    public IReadOnlyList<string> Items => items;

    public void AddItem(string item)
    {
        items.Add(item);
    }
}
```

Usage:

```csharp
ShoppingCart cart = new ShoppingCart();

cart.AddItem("Desk Lamp");

Console.WriteLine(cart.Items.Count);
```

Output:

```text
1
```

> ✅ The class controls how items are added, while outside code cannot directly modify the list.

---

# 🧮 `const` Fields

## 1. What Is a `const` Field?

A **`const` field** is a compile-time constant.

Its value must be known when the code is compiled.

```csharp
class MathSettings
{
    public const double PiApproximation = 3.14159;
}
```

Usage:

```csharp
Console.WriteLine(MathSettings.PiApproximation);
```

Output:

```text
3.14159
```

---

## 2. `const` Fields Are Implicitly Static

You access a `const` field using the class name.

```csharp
class AppLimits
{
    public const int MaxLoginAttempts = 4;
}
```

Usage:

```csharp
Console.WriteLine(AppLimits.MaxLoginAttempts);
```

Output:

```text
4
```

> ✅ You do not need to create an object to access a `const`.

---

## 3. You Cannot Use `static const`

This is invalid:

```csharp
class AppLimits
{
    public static const int MaxLoginAttempts = 4; // ❌ Compile-time error
}
```

Correct:

```csharp
class AppLimits
{
    public const int MaxLoginAttempts = 4;
}
```

> 🧠 `const` is already static by nature.

---

## 4. Allowed Types for `const`

A `const` field can only use certain simple types.

| Allowed Type Category | Examples |
|---|---|
| Numeric types | `int`, `double`, `decimal`, `byte`, `long` |
| Character | `char` |
| Boolean | `bool` |
| String | `string` |
| Enum | `DayOfWeek`, custom enums |
| Null reference | `null` |

Example:

```csharp
class Defaults
{
    public const int PageSize = 25;
    public const string CompanyName = "Northwind Tools";
    public const bool EnableGuestAccess = false;
    public const char Separator = '|';
}
```

---

## 5. `const` Must Be Assigned Immediately

A `const` field must be assigned at declaration.

```csharp
class ApiSettings
{
    public const int TimeoutSeconds = 30;
}
```

This is invalid:

```csharp
class ApiSettings
{
    public const int TimeoutSeconds; // ❌ Compile-time error
}
```

---

## 6. `const` Cannot Be Assigned in a Constructor

```csharp
class ApiSettings
{
    public const int TimeoutSeconds;

    public ApiSettings()
    {
        TimeoutSeconds = 30; // ❌ Compile-time error
    }
}
```

Correct:

```csharp
class ApiSettings
{
    public const int TimeoutSeconds = 30;
}
```

---

## 7. `const` Values Cannot Come from Runtime Logic

Invalid:

```csharp
class BuildInfo
{
    public const DateTime StartedAt = DateTime.Now; // ❌ Compile-time error
}
```

Invalid:

```csharp
class FileSettings
{
    public const string TempPath = Path.GetTempPath(); // ❌ Compile-time error
}
```

Valid:

```csharp
class FileSettings
{
    public const string Extension = ".data";
}
```

> ⚠️ `const` values must be known at compile time.

---

# ⚙️ `static` Fields

## 1. What Is a `static` Field?

A **`static` field** belongs to the type itself, not to a specific object.

There is only **one shared copy** of a static field.

```csharp
class VisitCounter
{
    public static int TotalVisits;
}
```

Usage:

```csharp
VisitCounter.TotalVisits++;
VisitCounter.TotalVisits++;

Console.WriteLine(VisitCounter.TotalVisits);
```

Output:

```text
2
```

---

## 2. Static Field Shared Across All Objects

```csharp
class Player
{
    public string Name;
    public static int PlayerCount;

    public Player(string name)
    {
        Name = name;
        PlayerCount++;
    }
}
```

Usage:

```csharp
Player p1 = new Player("Ava");
Player p2 = new Player("Noah");
Player p3 = new Player("Milo");

Console.WriteLine(Player.PlayerCount);
```

Output:

```text
3
```

> ✅ `PlayerCount` is shared by all `Player` objects.

---

## 3. Instance Field vs Static Field

```csharp
class BankAccount
{
    public string OwnerName;          // Instance field
    public decimal Balance;           // Instance field

    public static decimal InterestRate; // Static field
}
```

Usage:

```csharp
BankAccount.InterestRate = 0.035m;

BankAccount account1 = new BankAccount();
account1.OwnerName = "Sara";
account1.Balance = 800m;

BankAccount account2 = new BankAccount();
account2.OwnerName = "Leo";
account2.Balance = 1450m;

Console.WriteLine(account1.OwnerName);
Console.WriteLine(account2.OwnerName);
Console.WriteLine(BankAccount.InterestRate);
```

Output:

```text
Sara
Leo
0.035
```

---

## 4. Accessing Static Fields

Static fields should be accessed through the class name.

```csharp
class SystemMetrics
{
    public static int ActiveUsers;
}
```

Correct:

```csharp
SystemMetrics.ActiveUsers = 18;
```

Avoid:

```csharp
SystemMetrics metrics = new SystemMetrics();
metrics.ActiveUsers = 18; // ⚠️ Not recommended
```

> ✅ Use the type name for static members: `SystemMetrics.ActiveUsers`.

---

# 🏗️ Static Constructors

## 1. What Is a Static Constructor?

A **static constructor** initializes static data.

It runs automatically once before the class is used for the first time.

```csharp
class CurrencyRates
{
    public static decimal UsdToEur;

    static CurrencyRates()
    {
        UsdToEur = 0.92m;
    }
}
```

Usage:

```csharp
Console.WriteLine(CurrencyRates.UsdToEur);
```

Output:

```text
0.92
```

---

## 2. Static Constructor Rules

| Rule | Explanation |
|---|---|
| No access modifier | Cannot be `public`, `private`, etc. |
| No parameters | Static constructors cannot accept arguments |
| Runs automatically | You do not call it yourself |
| Runs once | Executes once per type |
| Initializes static members | Usually used for static fields |

---

## 3. Static Constructor Example

```csharp
class ApplicationCache
{
    public static readonly Dictionary<string, string> Settings;

    static ApplicationCache()
    {
        Settings = new Dictionary<string, string>
        {
            ["Theme"] = "Ocean",
            ["Language"] = "English",
            ["Region"] = "EU"
        };
    }
}
```

Usage:

```csharp
Console.WriteLine(ApplicationCache.Settings["Theme"]);
```

Output:

```text
Ocean
```

---

# 🧊 `static readonly` Fields

## 1. What Is `static readonly`?

A **`static readonly` field** belongs to the type and can be assigned only:

1. At declaration
2. Inside a static constructor

```csharp
class AppConfiguration
{
    public static readonly string ApplicationName = "Inventory Desk";
}
```

Usage:

```csharp
Console.WriteLine(AppConfiguration.ApplicationName);
```

Output:

```text
Inventory Desk
```

---

## 2. Assigning `static readonly` in a Static Constructor

```csharp
class RuntimeSettings
{
    public static readonly DateTime StartedAt;

    static RuntimeSettings()
    {
        StartedAt = DateTime.UtcNow;
    }
}
```

Usage:

```csharp
Console.WriteLine(RuntimeSettings.StartedAt);
```

> ✅ This works because `static readonly` values can be assigned at runtime.

---

## 3. `static readonly` Can Use Runtime Values

```csharp
class MachineInfo
{
    public static readonly string MachineName = Environment.MachineName;
    public static readonly int ProcessorCount = Environment.ProcessorCount;
}
```

Usage:

```csharp
Console.WriteLine(MachineInfo.MachineName);
Console.WriteLine(MachineInfo.ProcessorCount);
```

---

## 4. `const` Cannot Do This

Invalid:

```csharp
class MachineInfo
{
    public const string MachineName = Environment.MachineName; // ❌ Compile-time error
}
```

Correct:

```csharp
class MachineInfo
{
    public static readonly string MachineName = Environment.MachineName;
}
```

---

# 🆚 `const` vs `readonly` vs `static readonly`

## 1. Main Differences

| Feature | `const` | `readonly` | `static readonly` |
|---|---|---|---|
| Belongs to | Type | Object instance | Type |
| Value assigned | At declaration only | Declaration or instance constructor | Declaration or static constructor |
| Runtime values allowed? | ❌ No | ✅ Yes | ✅ Yes |
| Compile-time constant? | ✅ Yes | ❌ No | ❌ No |
| Access through class name? | ✅ Yes | ❌ Usually object | ✅ Yes |
| Can differ per object? | ❌ No | ✅ Yes | ❌ No |
| Implicitly static? | ✅ Yes | ❌ No | Already static |

---

## 2. Example Comparison

```csharp
class StoreSettings
{
    public const decimal TaxRate = 0.0825m;

    public readonly string StoreId;

    public static readonly DateTime ApplicationStartedAt = DateTime.UtcNow;

    public StoreSettings(string storeId)
    {
        StoreId = storeId;
    }
}
```

Usage:

```csharp
Console.WriteLine(StoreSettings.TaxRate);
Console.WriteLine(StoreSettings.ApplicationStartedAt);

StoreSettings store = new StoreSettings("STORE-17");
Console.WriteLine(store.StoreId);
```

Output:

```text
0.0825
2026-05-01 10:15:42
STORE-17
```

---

# 🧠 When to Use Each One

## Use `const` When...

Use `const` when the value is:

- Known at compile time
- Never expected to change
- Simple and primitive-like
- Truly constant

Examples:

```csharp
class Geometry
{
    public const int DegreesInCircle = 360;
    public const double GoldenRatioApprox = 1.618;
}
```

Good candidates:

- Mathematical constants
- Fixed string keys
- Format characters
- Simple numeric limits

```csharp
class ValidationRules
{
    public const int MinimumUsernameLength = 3;
    public const int MaximumUsernameLength = 32;
    public const string DefaultCountryCode = "CA";
}
```

---

## Use `readonly` When...

Use `readonly` when the value:

- Belongs to one object
- Is assigned when the object is created
- Should not be reassigned afterward
- May vary between different objects

Example:

```csharp
class Employee
{
    public readonly int EmployeeId;
    public readonly DateTime HireDate;

    public Employee(int employeeId)
    {
        EmployeeId = employeeId;
        HireDate = DateTime.Today;
    }
}
```

Usage:

```csharp
Employee employee1 = new Employee(501);
Employee employee2 = new Employee(502);

Console.WriteLine(employee1.EmployeeId);
Console.WriteLine(employee2.EmployeeId);
```

Output:

```text
501
502
```

---

## Use `static readonly` When...

Use `static readonly` when the value:

- Belongs to the whole type
- Is shared by all objects
- Is assigned once
- May require runtime calculation

Example:

```csharp
class SecuritySettings
{
    public static readonly TimeSpan TokenLifetime = TimeSpan.FromMinutes(45);
    public static readonly string IssuerName = Environment.UserDomainName;
}
```

Usage:

```csharp
Console.WriteLine(SecuritySettings.TokenLifetime);
Console.WriteLine(SecuritySettings.IssuerName);
```

---

# 🔥 Important Versioning Difference: `const` vs `static readonly`

## 1. `const` Values Are Inlined

When another project uses a `public const`, the value may be copied directly into the compiled code.

Library project:

```csharp
public class ShippingRules
{
    public const decimal FreeShippingThreshold = 75m;
}
```

Application project:

```csharp
if (orderTotal >= ShippingRules.FreeShippingThreshold)
{
    Console.WriteLine("Free shipping applied.");
}
```

The compiled application may treat it like this:

```csharp
if (orderTotal >= 75m)
{
    Console.WriteLine("Free shipping applied.");
}
```

---

## 2. Problem When `const` Changes

If the library changes:

```csharp
public class ShippingRules
{
    public const decimal FreeShippingThreshold = 90m;
}
```

Other projects may still use the old value until they are recompiled.

> ⚠️ Public `const` values can create versioning problems across assemblies.

---

## 3. `static readonly` Avoids Inlining

```csharp
public class ShippingRules
{
    public static readonly decimal FreeShippingThreshold = 90m;
}
```

Other projects read the value at runtime instead of copying it at compile time.

> ✅ For public values that might change in future versions, prefer `static readonly`.

---

# 🧪 Practical Example: Product Pricing

## 1. Define Pricing Rules

```csharp
class PricingRules
{
    public const decimal StandardTaxRate = 0.075m;

    public static readonly DateTime RulesLoadedAt = DateTime.UtcNow;

    public readonly decimal DiscountRate;

    public PricingRules(decimal discountRate)
    {
        DiscountRate = discountRate;
    }
}
```

---

## 2. Use the Rules

```csharp
PricingRules rules = new PricingRules(0.12m);

decimal price = 240m;

decimal tax = price * PricingRules.StandardTaxRate;
decimal discount = price * rules.DiscountRate;

Console.WriteLine($"Tax: {tax}");
Console.WriteLine($"Discount: {discount}");
Console.WriteLine($"Rules loaded at: {PricingRules.RulesLoadedAt}");
```

Possible output:

```text
Tax: 18.000
Discount: 28.80
Rules loaded at: 2026-05-01 14:25:10
```

---

# 🧰 Static Methods

## 1. What Is a Static Method?

A **static method** belongs to the class itself.

You call it without creating an object.

```csharp
class TemperatureConverter
{
    public static double CelsiusToFahrenheit(double celsius)
    {
        return celsius * 9 / 5 + 32;
    }
}
```

Usage:

```csharp
double result = TemperatureConverter.CelsiusToFahrenheit(22);

Console.WriteLine(result);
```

Output:

```text
71.6
```

---

## 2. Static Methods Cannot Directly Access Instance Fields

```csharp
class Counter
{
    private int value;

    public static void Reset()
    {
        value = 0; // ❌ Compile-time error
    }
}
```

Why?

> ⚠️ A static method belongs to the class, but `value` belongs to a specific object.

---

## 3. Static Methods Can Access Static Fields

```csharp
class Counter
{
    private static int total;

    public static void Increment()
    {
        total++;
    }

    public static int GetTotal()
    {
        return total;
    }
}
```

Usage:

```csharp
Counter.Increment();
Counter.Increment();

Console.WriteLine(Counter.GetTotal());
```

Output:

```text
2
```

---

# 🧩 Static Properties

## 1. What Is a Static Property?

A **static property** belongs to the type instead of an object.

```csharp
class AppState
{
    public static string CurrentEnvironment { get; set; } = "Development";
}
```

Usage:

```csharp
Console.WriteLine(AppState.CurrentEnvironment);

AppState.CurrentEnvironment = "Staging";

Console.WriteLine(AppState.CurrentEnvironment);
```

Output:

```text
Development
Staging
```

---

## 2. Static Read-Only Property

```csharp
class BuildMetadata
{
    public static DateTime StartedAt { get; } = DateTime.UtcNow;
}
```

Usage:

```csharp
Console.WriteLine(BuildMetadata.StartedAt);
```

---

## 3. Static Property with Private Setter

```csharp
class RequestTracker
{
    public static int TotalRequests { get; private set; }

    public static void RecordRequest()
    {
        TotalRequests++;
    }
}
```

Usage:

```csharp
RequestTracker.RecordRequest();
RequestTracker.RecordRequest();

Console.WriteLine(RequestTracker.TotalRequests);
```

Output:

```text
2
```

---

# 🏛️ Static Classes

## 1. What Is a Static Class?

A **static class** can contain only static members.

You cannot create an object from a static class.

```csharp
static class StringTools
{
    public static bool IsShortCode(string value)
    {
        return value.Length == 6;
    }
}
```

Usage:

```csharp
bool result = StringTools.IsShortCode("AB42XZ");

Console.WriteLine(result);
```

Output:

```text
True
```

---

## 2. Cannot Instantiate a Static Class

```csharp
StringTools tools = new StringTools(); // ❌ Compile-time error
```

---

## 3. Common Uses for Static Classes

Static classes are often used for:

- Utility methods
- Extension methods
- Constants
- Helper functions
- Shared stateless behavior

Example:

```csharp
static class PriceFormatter
{
    public const string CurrencySymbol = "$";

    public static string Format(decimal amount)
    {
        return $"{CurrencySymbol}{amount:0.00}";
    }
}
```

Usage:

```csharp
Console.WriteLine(PriceFormatter.Format(18.5m));
```

Output:

```text
$18.50
```

---

# 🧬 Static Members and Object Instances

## 1. Instance Members Need an Object

```csharp
class Notebook
{
    public string Title;
}
```

Usage:

```csharp
Notebook notebook = new Notebook();
notebook.Title = "Design Notes";
```

---

## 2. Static Members Need the Type Name

```csharp
class Notebook
{
    public static int TotalCreated;
}
```

Usage:

```csharp
Notebook.TotalCreated = 10;
```

---

## 3. Mixed Example

```csharp
class Notebook
{
    public string Title;
    public static int TotalCreated;

    public Notebook(string title)
    {
        Title = title;
        TotalCreated++;
    }
}
```

Usage:

```csharp
Notebook first = new Notebook("Meeting Notes");
Notebook second = new Notebook("Recipe Ideas");

Console.WriteLine(first.Title);
Console.WriteLine(second.Title);
Console.WriteLine(Notebook.TotalCreated);
```

Output:

```text
Meeting Notes
Recipe Ideas
2
```

---

# 🛡️ Access Modifiers with Fields

Fields can use access modifiers such as:

| Modifier | Meaning |
|---|---|
| `public` | Accessible from anywhere |
| `private` | Accessible only inside the class |
| `protected` | Accessible in the class and derived classes |
| `internal` | Accessible within the same assembly |
| `private protected` | Accessible in derived classes within same assembly |
| `protected internal` | Accessible in derived classes or same assembly |

---

## Prefer Private Fields

Instead of exposing fields directly:

```csharp
class Account
{
    public decimal Balance; // ⚠️ Usually not recommended
}
```

Prefer properties or methods:

```csharp
class Account
{
    private decimal balance;

    public decimal Balance => balance;

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Amount must be positive.");
        }

        balance += amount;
    }
}
```

---

# 🧾 Naming Conventions

## 1. Constants

Constants usually use **PascalCase** in C#.

```csharp
class Limits
{
    public const int MaxRetryCount = 5;
    public const string DefaultLanguage = "en-US";
}
```

---

## 2. Private Fields

Common private field styles include camelCase or underscore camelCase.

```csharp
class UserService
{
    private readonly string connectionString;
}
```

Or:

```csharp
class UserService
{
    private readonly string _connectionString;
}
```

---

## 3. Static Fields

Private static fields often use camelCase or underscore style.

```csharp
class CacheStore
{
    private static int itemCount;
}
```

Or:

```csharp
class CacheStore
{
    private static int _itemCount;
}
```

---

## 4. Static Readonly Fields

Public static readonly fields often use PascalCase.

```csharp
class FileDefaults
{
    public static readonly TimeSpan CleanupInterval = TimeSpan.FromHours(6);
}
```

---

# 🧯 Common Mistakes

## 1. Trying to Change a `const`

```csharp
class Limits
{
    public const int MaxItems = 100;

    public void Change()
    {
        MaxItems = 200; // ❌ Compile-time error
    }
}
```

---

## 2. Trying to Assign `readonly` Outside the Constructor

```csharp
class Device
{
    public readonly string SerialNumber;

    public Device(string serialNumber)
    {
        SerialNumber = serialNumber;
    }

    public void UpdateSerial()
    {
        SerialNumber = "SN-9000"; // ❌ Compile-time error
    }
}
```

---

## 3. Expecting `readonly` to Make Objects Immutable

```csharp
class Team
{
    public readonly List<string> Members = new List<string>();
}
```

This is allowed:

```csharp
Team team = new Team();

team.Members.Add("Ravi");
```

> ⚠️ The field is readonly, but the list object can still change.

---

## 4. Using `const` for Values That Might Change

Risky:

```csharp
public class ApiRoutes
{
    public const string BaseUrl = "https://api.example-app.local/v1";
}
```

Better if the value might change:

```csharp
public class ApiRoutes
{
    public static readonly string BaseUrl = "https://api.example-app.local/v2";
}
```

---

## 5. Using Public Mutable Static Fields

Avoid:

```csharp
class GlobalSettings
{
    public static string CurrentTheme = "Light";
}
```

Prefer:

```csharp
class GlobalSettings
{
    public static string CurrentTheme { get; private set; } = "Light";

    public static void ChangeTheme(string theme)
    {
        CurrentTheme = theme;
    }
}
```

---

# 🧪 Practice Example: Application Settings

## 1. Define the Class

```csharp
class ApplicationSettings
{
    public const int MaxUploadSizeMb = 20;

    public static readonly DateTime StartedAt = DateTime.UtcNow;

    public readonly string EnvironmentName;

    public ApplicationSettings(string environmentName)
    {
        EnvironmentName = environmentName;
    }
}
```

---

## 2. Use the Class

```csharp
ApplicationSettings settings = new ApplicationSettings("Testing");

Console.WriteLine(ApplicationSettings.MaxUploadSizeMb);
Console.WriteLine(ApplicationSettings.StartedAt);
Console.WriteLine(settings.EnvironmentName);
```

Possible output:

```text
20
2026-05-01 09:42:18
Testing
```

---

# 🧪 Practice Example: Order Number Generator

```csharp
class OrderNumberGenerator
{
    public const string Prefix = "ORD";

    private static int nextNumber = 1200;

    public static string CreateNext()
    {
        nextNumber++;
        return $"{Prefix}-{nextNumber}";
    }
}
```

Usage:

```csharp
Console.WriteLine(OrderNumberGenerator.CreateNext());
Console.WriteLine(OrderNumberGenerator.CreateNext());
Console.WriteLine(OrderNumberGenerator.CreateNext());
```

Output:

```text
ORD-1201
ORD-1202
ORD-1203
```

---

# 🧪 Practice Example: Readonly Dependency

```csharp
class PaymentProcessor
{
    private readonly string paymentGatewayName;

    public PaymentProcessor(string gatewayName)
    {
        paymentGatewayName = gatewayName;
    }

    public void Process(decimal amount)
    {
        Console.WriteLine($"Processing {amount:C} through {paymentGatewayName}.");
    }
}
```

Usage:

```csharp
PaymentProcessor processor = new PaymentProcessor("BlueRiver Pay");

processor.Process(64.75m);
```

Output:

```text
Processing $64.75 through BlueRiver Pay.
```

---

# 📌 Quick Reference

## `const`

```csharp
public const int MaxUsers = 300;
```

- Compile-time constant
- Must be assigned immediately
- Implicitly static
- Cannot use runtime values
- Accessed with class name

---

## `readonly`

```csharp
public readonly string Id;

public Customer(string id)
{
    Id = id;
}
```

- Instance-level by default
- Assigned at declaration or constructor
- Can use runtime values
- Can vary per object
- Cannot be reassigned after construction

---

## `static`

```csharp
public static int TotalCount;
```

- Belongs to the type
- Shared by all instances
- Accessed with class name
- Useful for shared state or utility behavior

---

## `static readonly`

```csharp
public static readonly DateTime CreatedAt = DateTime.UtcNow;
```

- Type-level read-only value
- Assigned at declaration or static constructor
- Can use runtime values
- Good alternative to public `const` when value may change

---

# 🧭 Choosing the Right Keyword

| Situation | Best Choice |
|---|---|
| Fixed mathematical value | `const` |
| Fixed string key | `const` |
| Value set per object in constructor | `readonly` |
| Runtime value shared by all instances | `static readonly` |
| Shared counter | `static` |
| Utility method | `static` method |
| Helper-only class | `static class` |
| Public value that may change in future | `static readonly` |
| Dependency injected through constructor | `readonly` field |