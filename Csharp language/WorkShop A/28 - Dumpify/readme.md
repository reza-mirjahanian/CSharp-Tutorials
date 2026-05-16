
# 📦 Dumpify Package in C#

## 1. What Is Dumpify?

**Dumpify** is a small .NET package that helps you quickly inspect objects while debugging.

It gives you a convenient `.Dump()` extension method that prints objects in a readable, structured way.

Instead of writing this:

```csharp
Console.WriteLine(customer.Name);
Console.WriteLine(customer.Email);
Console.WriteLine(customer.Address.City);
```

You can write this:

```csharp
customer.Dump();
```

Dumpify then displays the object with its properties, nested objects, lists, and values in a clearer format.

---

# 🎯 Why Use Dumpify?

Dumpify is useful when you want to quickly see what an object contains.

## Common use cases

- Inspecting objects during development
- Debugging method results
- Viewing collections
- Checking nested object values
- Understanding API responses
- Examining LINQ query results
- Exploring complex data structures

> 🧠 Dumpify is mainly a **developer debugging tool**, not a replacement for proper logging.

---

# 🧰 Installing Dumpify

## Using the .NET CLI

```bash
dotnet add package Dumpify
```

## Using NuGet Package Manager Console

```powershell
Install-Package Dumpify
```

## Using Visual Studio

1. Right-click the project
2. Select **Manage NuGet Packages**
3. Search for **Dumpify**
4. Install the package

---

# 🧾 Basic Setup

After installing the package, import the namespace:

```csharp
using Dumpify;
```

Then you can call `.Dump()` on objects.

```csharp
using Dumpify;

string message = "Hello from Dumpify";

message.Dump();
```

---

# 🧪 Basic Example

```csharp
using Dumpify;

var product = new
{
    Id = 18,
    Name = "Wireless Keyboard",
    Price = 49.95m,
    InStock = true
};

product.Dump();
```

Dumpify displays the anonymous object in a readable structure.

Instead of a plain output like:

```text
{ Id = 18, Name = Wireless Keyboard, Price = 49.95, InStock = True }
```

Dumpify formats it in a more useful way for inspection.

---

# 🧱 Dumping Simple Values

You can dump basic values such as strings, numbers, booleans, and dates.

```csharp
using Dumpify;

int orderCount = 42;
decimal totalAmount = 875.30m;
bool isCompleted = false;
DateTime createdAt = new DateTime(2026, 3, 15, 10, 45, 0);

orderCount.Dump();
totalAmount.Dump();
isCompleted.Dump();
createdAt.Dump();
```

---

# 🧍 Dumping an Object

## Example Class

```csharp
public class Customer
{
    public int Id { get; set; }

    public string FullName { get; set; } = "";

    public string Email { get; set; } = "";

    public bool IsPremiumMember { get; set; }
}
```

## Dumping the Object

```csharp
using Dumpify;

Customer customer = new Customer
{
    Id = 101,
    FullName = "Maya Collins",
    Email = "maya.collins@example.com",
    IsPremiumMember = true
};

customer.Dump();
```

Dumpify shows the object and its property values in a structured way.

---

# 🏷️ Adding Labels to Dumps

A label helps identify what you are dumping.

```csharp
using Dumpify;

var invoice = new
{
    InvoiceNumber = "INV-2045",
    Amount = 320.75m,
    Paid = false
};

invoice.Dump("Current Invoice");
```

This is useful when your code has multiple dumps.

```csharp
customer.Dump("Customer Before Update");

customer.Email = "maya.new@example.com";

customer.Dump("Customer After Update");
```

---

# 📚 Dumping Collections

Dumpify is especially helpful for lists and arrays.

```csharp
using Dumpify;

List<string> cities = new List<string>
{
    "Oslo",
    "Madrid",
    "Tokyo",
    "Nairobi"
};

cities.Dump();
```

---

## Dumping a List of Objects

```csharp
using Dumpify;

var products = new List<Product>
{
    new Product { Id = 1, Name = "Desk Lamp", Price = 34.99m },
    new Product { Id = 2, Name = "Office Chair", Price = 149.50m },
    new Product { Id = 3, Name = "Monitor Stand", Price = 27.25m }
};

products.Dump("Product List");
```

Example class:

```csharp
public class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public decimal Price { get; set; }
}
```

---

# 🪆 Dumping Nested Objects

Dumpify can display objects that contain other objects.

```csharp
using Dumpify;

var order = new Order
{
    Id = 5001,
    Customer = new Customer
    {
        Id = 25,
        FullName = "Ethan Parker",
        Email = "ethan.parker@example.com",
        IsPremiumMember = false
    },
    ShippingAddress = new Address
    {
        Street = "42 River Road",
        City = "Bristol",
        Country = "UK",
        PostalCode = "BS1 8QH"
    },
    Total = 189.40m
};

order.Dump("Order Details");
```

Supporting classes:

```csharp
public class Order
{
    public int Id { get; set; }

    public Customer Customer { get; set; } = new();

    public Address ShippingAddress { get; set; } = new();

    public decimal Total { get; set; }
}

public class Address
{
    public string Street { get; set; } = "";

    public string City { get; set; } = "";

    public string Country { get; set; } = "";

    public string PostalCode { get; set; } = "";
}
```

---

# 🧾 Dumping Records

Dumpify also works well with C# records.

```csharp
using Dumpify;

var employee = new EmployeeProfile(
    Id: 72,
    Name: "Nora Ahmed",
    Department: "Finance",
    Salary: 68000m
);

employee.Dump("Employee Profile");
```

Record:

```csharp
public record EmployeeProfile(
    int Id,
    string Name,
    string Department,
    decimal Salary
);
```

---

# 🧮 Dumping Anonymous Objects

Anonymous objects are often used for quick projections.

```csharp
using Dumpify;

var report = new
{
    Month = "April",
    SalesCount = 138,
    Revenue = 24580.90m,
    TopCategory = "Electronics"
};

report.Dump("Monthly Sales Report");
```

---

# 🔍 Dumpify with LINQ

Dumpify is helpful when inspecting LINQ results.

```csharp
using Dumpify;

var products = new List<Product>
{
    new Product { Id = 1, Name = "Tablet Case", Price = 18.99m },
    new Product { Id = 2, Name = "USB-C Hub", Price = 45.50m },
    new Product { Id = 3, Name = "Laptop Sleeve", Price = 29.95m },
    new Product { Id = 4, Name = "Wireless Mouse", Price = 24.75m }
};

var expensiveProducts = products
    .Where(product => product.Price > 25m)
    .OrderBy(product => product.Price)
    .Select(product => new
    {
        product.Name,
        product.Price
    });

expensiveProducts.Dump("Products Over 25");
```

---

# 🧪 Dumping Method Results

You can dump the result of a method directly.

```csharp
using Dumpify;

GetActiveUsers().Dump("Active Users");
```

Example method:

```csharp
static List<UserAccount> GetActiveUsers()
{
    return new List<UserAccount>
    {
        new UserAccount { Id = 1, Username = "mila", IsActive = true },
        new UserAccount { Id = 2, Username = "arjun", IsActive = true },
        new UserAccount { Id = 3, Username = "sofia", IsActive = true }
    };
}
```

Class:

```csharp
public class UserAccount
{
    public int Id { get; set; }

    public string Username { get; set; } = "";

    public bool IsActive { get; set; }
}
```

---

# 🧵 Dumping Strings

Dumpify can dump string values too.

```csharp
using Dumpify;

string connectionStatus = "Connected to payment gateway";

connectionStatus.Dump("Connection Status");
```

For simple strings, `Console.WriteLine()` may be enough.

```csharp
Console.WriteLine(connectionStatus);
```

But `.Dump()` becomes more useful when the value is part of a larger object or when you want labeled debug output.

---

# 🧰 Dumpify vs `Console.WriteLine`

| Feature | `Console.WriteLine` | Dumpify |
|---|---:|---:|
| Prints simple text | ✅ | ✅ |
| Shows object structure | ❌ Limited | ✅ |
| Good for nested objects | ❌ | ✅ |
| Good for collections | ❌ Limited | ✅ |
| Requires custom formatting | ✅ Often | ❌ Usually not |
| Useful during debugging | ✅ | ✅✅ |

---

# 🧰 Dumpify vs JSON Serialization

Sometimes developers use JSON serialization to inspect objects.

```csharp
using System.Text.Json;

Console.WriteLine(JsonSerializer.Serialize(customer));
```

Dumpify can be more convenient for quick debugging:

```csharp
customer.Dump();
```

## Comparison

| Feature | JSON Serialization | Dumpify |
|---|---:|---:|
| Produces JSON | ✅ | ❌ |
| Great for APIs/files | ✅ | ❌ |
| Good for quick object inspection | ✅ | ✅✅ |
| Requires serializer setup for advanced cases | Sometimes | Usually no |
| Designed mainly for debugging | ❌ | ✅ |

---

# 🧠 Dumpify Is Not the Same as Logging

Dumpify is useful while developing, but it should not replace structured logging.

## Dumpify example

```csharp
order.Dump("Order Before Payment");
```

## Logging example

```csharp
logger.LogInformation(
    "Processing order {OrderId} for customer {CustomerId}",
    order.Id,
    order.Customer.Id
);
```

## Main difference

| Tool | Purpose |
|---|---|
| Dumpify | Quick developer inspection |
| Logging | Application monitoring, diagnostics, audit trails |

> ⚠️ Avoid leaving many `.Dump()` calls in production code.

---

# 🏗️ Example: Using Dumpify in a Console App

## 1. Create a console app

```bash
dotnet new console -n DumpifyDemoApp
cd DumpifyDemoApp
```

## 2. Install Dumpify

```bash
dotnet add package Dumpify
```

## 3. Update `Program.cs`

```csharp
using Dumpify;

var customer = new Customer
{
    Id = 12,
    Name = "Layla Stone",
    Email = "layla.stone@example.com",
    LoyaltyPoints = 840
};

customer.Dump("Customer Data");

public class Customer
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string Email { get; set; } = "";

    public int LoyaltyPoints { get; set; }
}
```

## 4. Run the app

```bash
dotnet run
```

---

# 🧩 Example: Debugging a Calculation

Suppose you have this method:

```csharp
public static decimal CalculateDiscount(decimal orderTotal, bool isMember)
{
    if (isMember && orderTotal >= 200m)
    {
        return orderTotal * 0.15m;
    }

    if (isMember)
    {
        return orderTotal * 0.08m;
    }

    return 0m;
}
```

You can inspect the input and result:

```csharp
using Dumpify;

decimal orderTotal = 260m;
bool isMember = true;

var debugInfo = new
{
    OrderTotal = orderTotal,
    IsMember = isMember,
    Discount = CalculateDiscount(orderTotal, isMember),
    FinalTotal = orderTotal - CalculateDiscount(orderTotal, isMember)
};

debugInfo.Dump("Discount Calculation");
```

---

# 🧱 Example: Dumping API-Like Data

```csharp
using Dumpify;

var response = new WeatherResponse
{
    City = "Valencia",
    TemperatureCelsius = 24.6,
    Conditions = "Sunny",
    Forecast = new List<ForecastDay>
    {
        new ForecastDay { Day = "Monday", High = 26, Low = 18 },
        new ForecastDay { Day = "Tuesday", High = 25, Low = 17 },
        new ForecastDay { Day = "Wednesday", High = 28, Low = 19 }
    }
};

response.Dump("Weather API Response");
```

Classes:

```csharp
public class WeatherResponse
{
    public string City { get; set; } = "";

    public double TemperatureCelsius { get; set; }

    public string Conditions { get; set; } = "";

    public List<ForecastDay> Forecast { get; set; } = new();
}

public class ForecastDay
{
    public string Day { get; set; } = "";

    public int High { get; set; }

    public int Low { get; set; }
}
```

---

# 🔐 Dumping Sensitive Data

Be careful when dumping objects that contain sensitive information.

## Avoid dumping objects like this

```csharp
var loginRequest = new
{
    Username = "admin.user",
    Password = "P@ssword123!",
    Token = "secret-token-value"
};

loginRequest.Dump();
```

This can expose sensitive information in console output or logs.

---

## Safer approach

Create a safe debug object:

```csharp
var safeLoginDebug = new
{
    Username = "admin.user",
    Password = "***",
    Token = "***"
};

safeLoginDebug.Dump("Login Request Debug Info");
```

---

# 🚫 Avoid Dumping Sensitive Fields

Sensitive fields include:

| Field Type | Examples |
|---|---|
| Passwords | `Password`, `ConfirmPassword` |
| Tokens | `AccessToken`, `RefreshToken`, `JwtToken` |
| API keys | `ApiKey`, `ClientSecret` |
| Payment data | `CardNumber`, `Cvv` |
| Personal data | National IDs, private addresses, medical data |

> 🔒 Dumpify output is meant for developers, but it can still reveal data if used carelessly.

---

# ⚙️ Dumpify with Nullable Values

Dumpify can help you see whether values are present or `null`.

```csharp
using Dumpify;

var profile = new UserProfile
{
    Id = 45,
    DisplayName = "River Fox",
    PhoneNumber = null,
    Bio = null
};

profile.Dump("User Profile");
```

Class:

```csharp
public class UserProfile
{
    public int Id { get; set; }

    public string DisplayName { get; set; } = "";

    public string? PhoneNumber { get; set; }

    public string? Bio { get; set; }
}
```

---

# 🧭 Dumpify with Conditional Debugging

You may want Dumpify calls only in debug builds.

```csharp
using Dumpify;

#if DEBUG
customer.Dump("Debug Customer");
#endif
```

This code only runs when the project is built in **Debug** mode.

---

## Example

```csharp
using Dumpify;

Order order = CreateSampleOrder();

#if DEBUG
order.Dump("Order Debug View");
#endif

ProcessOrder(order);
```

This keeps debugging output out of release builds.

---

# 🧪 Dumpify in Unit Tests

Dumpify can be useful while writing or troubleshooting tests.

```csharp
using Dumpify;
using Xunit;

public class PriceCalculatorTests
{
    [Fact]
    public void CalculateTotal_ShouldApplyTax()
    {
        var calculator = new PriceCalculator();

        var result = calculator.CalculateTotal(100m, 0.12m);

        result.Dump("Calculated Total");

        Assert.Equal(112m, result);
    }
}
```

Class being tested:

```csharp
public class PriceCalculator
{
    public decimal CalculateTotal(decimal subtotal, decimal taxRate)
    {
        return subtotal + subtotal * taxRate;
    }
}
```

> 🧪 Dumpify can help while developing tests, but finalized tests should usually rely on assertions rather than dumped output.

---

# 🧱 Dumping Dictionaries

Dumpify can inspect key-value data.

```csharp
using Dumpify;

var featureFlags = new Dictionary<string, bool>
{
    ["EnableNewCheckout"] = true,
    ["UseExperimentalSearch"] = false,
    ["ShowBetaBanner"] = true
};

featureFlags.Dump("Feature Flags");
```

---

# 🧮 Dumping Grouped LINQ Results

```csharp
using Dumpify;

var orders = new List<SalesOrder>
{
    new SalesOrder { Id = 1, Region = "North", Total = 120m },
    new SalesOrder { Id = 2, Region = "South", Total = 340m },
    new SalesOrder { Id = 3, Region = "North", Total = 220m },
    new SalesOrder { Id = 4, Region = "West", Total = 180m }
};

var groupedOrders = orders
    .GroupBy(order => order.Region)
    .Select(group => new
    {
        Region = group.Key,
        Count = group.Count(),
        TotalSales = group.Sum(order => order.Total)
    });

groupedOrders.Dump("Sales by Region");
```

Class:

```csharp
public class SalesOrder
{
    public int Id { get; set; }

    public string Region { get; set; } = "";

    public decimal Total { get; set; }
}
```

---

# 🔁 Dumping Before and After Changes

Dumpify is useful for comparing state before and after an operation.

```csharp
using Dumpify;

var account = new BankAccount
{
    AccountNumber = "AC-1048",
    Balance = 500m
};

account.Dump("Before Deposit");

account.Balance += 150m;

account.Dump("After Deposit");
```

Class:

```csharp
public class BankAccount
{
    public string AccountNumber { get; set; } = "";

    public decimal Balance { get; set; }
}
```

---

# 🧰 Dumpify with Extension Methods

Because Dumpify uses extension methods, you can call `.Dump()` directly on values.

```csharp
using Dumpify;

"Payment completed".Dump();

12345.Dump();

DateTime.UtcNow.Dump();

new[] { 4, 8, 15, 16, 23, 42 }.Dump();
```

The important part is this import:

```csharp
using Dumpify;
```

Without it, C# may not recognize `.Dump()`.

---

# 🧯 Common Errors

## Error: `.Dump()` is not recognized

### Example

```csharp
customer.Dump(); // ❌ Compile-time error
```

### Possible causes

| Cause | Fix |
|---|---|
| Dumpify package is not installed | Run `dotnet add package Dumpify` |
| Missing namespace | Add `using Dumpify;` |
| Project not restored | Run `dotnet restore` |
| IDE cache issue | Restart the IDE or rebuild the project |

---

## Error: Package not found

Try restoring packages:

```bash
dotnet restore
```

Then rebuild:

```bash
dotnet build
```

---

## Error: Dump output does not appear

Possible reasons:

| Reason | Explanation |
|---|---|
| App exits too quickly | Add a breakpoint or keep console open |
| Output is redirected | Check terminal or test output window |
| Running in Release mode | Conditional `#if DEBUG` code may be skipped |
| Test runner captures output | Check the test output panel |

---

# 🧠 Best Practices

## 1. Use Dumpify During Development

```csharp
order.Dump("Order Debug Info");
```

Good for temporary inspection.

---

## 2. Remove or Guard Dumps Before Production

Use conditional compilation:

```csharp
#if DEBUG
order.Dump("Order Debug Info");
#endif
```

---

## 3. Add Labels for Clarity

Instead of this:

```csharp
customer.Dump();
order.Dump();
payment.Dump();
```

Prefer this:

```csharp
customer.Dump("Customer");
order.Dump("Order");
payment.Dump("Payment");
```

---

## 4. Avoid Dumping Sensitive Data

Avoid:

```csharp
userCredentials.Dump();
```

Prefer:

```csharp
var safeCredentials = new
{
    userCredentials.Username,
    Password = "***"
};

safeCredentials.Dump("Credentials Debug View");
```

---

## 5. Use It to Inspect Intermediate Results

```csharp
var filteredProducts = products
    .Where(product => product.Price >= 50m)
    .ToList();

filteredProducts.Dump("Filtered Products");
```

This is useful when debugging query logic.

---

# 🧪 Practical Example: Debugging an Order Pipeline

## Classes

```csharp
public class CheckoutOrder
{
    public int Id { get; set; }

    public string CustomerName { get; set; } = "";

    public List<CheckoutItem> Items { get; set; } = new();

    public decimal Discount { get; set; }

    public decimal Total { get; set; }
}

public class CheckoutItem
{
    public string Name { get; set; } = "";

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }
}
```

---

## Processing Code

```csharp
using Dumpify;

var order = new CheckoutOrder
{
    Id = 7004,
    CustomerName = "Isabella Reed",
    Items = new List<CheckoutItem>
    {
        new CheckoutItem { Name = "Notebook", Quantity = 3, UnitPrice = 4.50m },
        new CheckoutItem { Name = "Pen Set", Quantity = 2, UnitPrice = 7.25m },
        new CheckoutItem { Name = "Desk Organizer", Quantity = 1, UnitPrice = 22.00m }
    }
};

order.Dump("Initial Order");

order.Total = order.Items.Sum(item => item.Quantity * item.UnitPrice);

order.Dump("After Total Calculation");

if (order.Total >= 40m)
{
    order.Discount = order.Total * 0.10m;
    order.Total -= order.Discount;
}

order.Dump("After Discount");
```

---

# 🧩 Practical Example: Inspecting Configuration

```csharp
using Dumpify;

var appSettings = new
{
    ApplicationName = "Inventory Portal",
    Environment = "Development",
    MaxUploadSizeMb = 25,
    EnableCaching = true,
    SupportedLanguages = new[]
    {
        "en",
        "fr",
        "es"
    }
};

appSettings.Dump("Application Settings");
```

---

# 🧵 Practical Example: Inspecting Async Results

```csharp
using Dumpify;

var users = await GetUsersAsync();

users.Dump("Users From Async Method");
```

Example async method:

```csharp
static async Task<List<AppUser>> GetUsersAsync()
{
    await Task.Delay(300);

    return new List<AppUser>
    {
        new AppUser { Id = 1, Name = "Kai Morgan", Role = "Admin" },
        new AppUser { Id = 2, Name = "Zara Hill", Role = "Editor" },
        new AppUser { Id = 3, Name = "Leo Brooks", Role = "Viewer" }
    };
}
```

Class:

```csharp
public class AppUser
{
    public int Id { get; set; }

    public string Name { get; set; } = "";

    public string Role { get; set; } = "";
}
```

---

# 📌 Common Patterns

## Dump a variable

```csharp
totalPrice.Dump("Total Price");
```

## Dump an object

```csharp
customer.Dump("Customer");
```

## Dump a list

```csharp
orders.Dump("Orders");
```

## Dump a LINQ result

```csharp
products
    .Where(product => product.Price > 100m)
    .Dump("Expensive Products");
```

## Dump before and after

```csharp
cart.Dump("Before Update");

cart.AddItem("Wireless Charger");

cart.Dump("After Update");
```

---

# 🧾 Quick Reference

| Task | Example |
|---|---|
| Install package | `dotnet add package Dumpify` |
| Import namespace | `using Dumpify;` |
| Dump a value | `price.Dump();` |
| Dump with label | `order.Dump("Order");` |
| Dump a collection | `products.Dump("Products");` |
| Debug only | `#if DEBUG order.Dump(); #endif` |
| Avoid sensitive data | Replace secrets with `"***"` |

