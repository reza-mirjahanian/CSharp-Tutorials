# C# `partial Program` Class, `static` Methods, and `<ItemGroup>`

## 1. What Is `Program` in C#?

In many C# applications, `Program` is the class that contains the application entry point.

Traditionally, a C# program starts with a `Main` method:

```csharp
using System;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Application started.");
    }
}
```

The `Main` method is the first method executed when the application runs.

---

## 2. Modern C# Top-Level Statements

In newer C# versions, especially C# 9 and later, you can write code without explicitly declaring the `Program` class.

Example:

```csharp
Console.WriteLine("Welcome to the inventory system.");
```

Even though you do not see it, the compiler internally creates something similar to this:

```csharp
public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the inventory system.");
    }
}
```

So this:

```csharp
Console.WriteLine("Welcome to the inventory system.");
```

is a shorter version of writing a full `Program` class.

---

# 3. What Does `partial Program` Mean?

The keyword `partial` means that a class can be split across multiple files.

A `partial` class allows you to define one class in several places.

## Example: One Class Split into Multiple Files

### File: `Program.cs`

```csharp
public partial class Program
{
    public static void Main(string[] args)
    {
        ShowGreeting();
    }
}
```

### File: `Program.Helpers.cs`

```csharp
public partial class Program
{
    public static void ShowGreeting()
    {
        Console.WriteLine("Hello from the helper method.");
    }
}
```

Together, these two files become one complete class:

```csharp
public class Program
{
    public static void Main(string[] args)
    {
        ShowGreeting();
    }

    public static void ShowGreeting()
    {
        Console.WriteLine("Hello from the helper method.");
    }
}
```

---

# 4. Why Use `partial Program`?

`partial Program` is useful when you want to extend the automatically generated `Program` class.

This is common in:

- ASP.NET Core applications
- Minimal API projects
- Integration testing
- Generated code scenarios
- Large applications where startup logic is split into multiple files

---

# 5. `partial Program` with Top-Level Statements

When you use top-level statements, the compiler creates a hidden `Program` class.

Example:

```csharp
var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/", () => "Store API is running.");

app.Run();
```

The compiler treats this as if there is a `Program` class.

However, if you want to access this hidden `Program` class from another project, such as a test project, you may need to declare it as `partial`.

You can add this at the bottom of `Program.cs`:

```csharp
public partial class Program
{
}
```

Full example:

```csharp
var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/", () => "Store API is running.");

app.Run();

public partial class Program
{
}
```

This allows test projects to reference the `Program` class.

---

# 6. Example: `partial Program` in a Minimal API

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IProductService, ProductService>();

var app = builder.Build();

app.MapGet("/products", (IProductService service) =>
{
    return service.GetProducts();
});

app.Run();

public partial class Program
{
}
```

## Supporting Classes

```csharp
public interface IProductService
{
    List<string> GetProducts();
}
```

```csharp
public class ProductService : IProductService
{
    public List<string> GetProducts()
    {
        return new List<string>
        {
            "Notebook",
            "Desk Lamp",
            "Wireless Mouse"
        };
    }
}
```

---

# 7. Why Is `public partial class Program` Often Empty?

You may see this:

```csharp
public partial class Program
{
}
```

It looks useless, but it has an important purpose.

It makes the generated `Program` class accessible to other code.

For example, integration tests may need this:

```csharp
WebApplicationFactory<Program>
```

Without declaring `Program` as `public partial`, test projects may not be able to access it.

---

# 8. `partial Program` for Integration Testing

In ASP.NET Core integration tests, you may see code like this:

```csharp
public class ProductApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ProductApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task ProductsEndpoint_ReturnsSuccess()
    {
        var response = await _client.GetAsync("/products");

        response.EnsureSuccessStatusCode();
    }
}
```

For this to work, the application project usually contains:

```csharp
public partial class Program
{
}
```

---

# 9. What Is a `static` Function in C#?

In C#, functions inside classes are usually called **methods**.

So instead of saying:

> static function

C# developers usually say:

> **static method**

A `static` method belongs to the class itself, not to an object created from the class.

---

# 10. Regular Method vs `static` Method

## Regular Instance Method

```csharp
public class Calculator
{
    public int Add(int firstNumber, int secondNumber)
    {
        return firstNumber + secondNumber;
    }
}
```

To use it, you must create an object:

```csharp
var calculator = new Calculator();

int result = calculator.Add(8, 4);

Console.WriteLine(result);
```

## Static Method

```csharp
public class Calculator
{
    public static int Add(int firstNumber, int secondNumber)
    {
        return firstNumber + secondNumber;
    }
}
```

To use it, you do **not** create an object:

```csharp
int result = Calculator.Add(8, 4);

Console.WriteLine(result);
```

---

# 11. Static Method Syntax

```csharp
public static returnType MethodName(parameters)
{
    // method body
}
```

Example:

```csharp
public static decimal CalculateDiscount(decimal price, decimal discountRate)
{
    return price - price * discountRate;
}
```

Usage:

```csharp
decimal finalPrice = PricingHelper.CalculateDiscount(120m, 0.15m);

Console.WriteLine(finalPrice);
```

---

# 12. Static Method Example

```csharp
public class TextFormatter
{
    public static string MakeTitle(string text)
    {
        return text.Trim().ToUpper();
    }
}
```

Usage:

```csharp
string title = TextFormatter.MakeTitle(" monthly report ");

Console.WriteLine(title);
```

Output:

```text
MONTHLY REPORT
```

---

# 13. Static Methods in `Program`

You can define static methods inside the `Program` class.

```csharp
public class Program
{
    public static void Main(string[] args)
    {
        DisplayMenu();

        int total = AddNumbers(14, 6);

        Console.WriteLine($"Total: {total}");
    }

    public static void DisplayMenu()
    {
        Console.WriteLine("1. View orders");
        Console.WriteLine("2. Add product");
        Console.WriteLine("3. Exit");
    }

    public static int AddNumbers(int firstNumber, int secondNumber)
    {
        return firstNumber + secondNumber;
    }
}
```

---

# 14. Static Methods in `partial Program`

Because all partial parts become one class, static methods can be placed in another file.

## File: `Program.cs`

```csharp
public partial class Program
{
    public static void Main(string[] args)
    {
        ShowStartupMessage();

        int tax = CalculateTax(200, 10);

        Console.WriteLine($"Tax amount: {tax}");
    }
}
```

## File: `Program.Utilities.cs`

```csharp
public partial class Program
{
    public static void ShowStartupMessage()
    {
        Console.WriteLine("Finance tool is starting.");
    }

    public static int CalculateTax(int amount, int taxRate)
    {
        return amount * taxRate / 100;
    }
}
```

---

# 15. Static Methods with Top-Level Statements

With top-level statements, you can define local methods below the main code.

```csharp
Console.WriteLine("Starting report generator...");

string reportName = CreateReportName("sales");

Console.WriteLine(reportName);

static string CreateReportName(string category)
{
    return $"{category}-report-{DateTime.Now:yyyyMMdd}";
}
```

Output example:

```text
Starting report generator...
sales-report-20260501
```

The method `CreateReportName` is static because top-level methods cannot access instance members of a class.

---

# 16. Static Method Rules

| Rule | Explanation |
|---|---|
| Belongs to class | A static method is called through the class name. |
| No object required | You do not need `new` to call it. |
| Cannot directly access instance members | It cannot use non-static fields or methods directly. |
| Good for utility logic | Useful for helpers like formatting, calculations, validation, etc. |

---

# 17. Example: Static Utility Class

A class that only contains static helper methods can be declared as `static`.

```csharp
public static class OrderHelper
{
    public static bool IsLargeOrder(decimal amount)
    {
        return amount >= 1000m;
    }

    public static decimal AddServiceFee(decimal amount)
    {
        return amount + 25m;
    }
}
```

Usage:

```csharp
decimal orderAmount = 1450m;

if (OrderHelper.IsLargeOrder(orderAmount))
{
    decimal finalAmount = OrderHelper.AddServiceFee(orderAmount);

    Console.WriteLine($"Final amount: {finalAmount}");
}
```

---

# 18. Static Class vs Static Method

| Feature | Static Method | Static Class |
|---|---|---|
| Meaning | A method that belongs to the class | A class that cannot be instantiated |
| Can be inside normal class? | Yes | No, it is the whole class |
| Can contain instance members? | Not applicable | No |
| Example | `Math.Round(12.5)` | `Math` |

Example of a static method inside a normal class:

```csharp
public class Invoice
{
    public int Id { get; set; }

    public static string GenerateInvoiceCode()
    {
        return $"INV-{Guid.NewGuid().ToString()[..8]}";
    }
}
```

Example of a static class:

```csharp
public static class CurrencyFormatter
{
    public static string FormatUsd(decimal amount)
    {
        return $"${amount:N2}";
    }
}
```

---

# 19. What Is `<ItemGroup>`?

`<ItemGroup>` is used inside a `.csproj` file.

A `.csproj` file is the project file for a C# project.

It tells .NET:

- Which files belong to the project
- Which NuGet packages are used
- Which project references exist
- Which content files should be copied
- Which resources should be embedded

---

# 20. Example `.csproj` File

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net9.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

</Project>
```

---

# 21. `<PropertyGroup>` vs `<ItemGroup>`

| Element | Purpose | Example |
|---|---|---|
| `<PropertyGroup>` | Stores project settings | Target framework, nullable mode, output type |
| `<ItemGroup>` | Stores collections of items | Packages, references, files, resources |

Example:

```xml
<PropertyGroup>
  <TargetFramework>net9.0</TargetFramework>
  <Nullable>enable</Nullable>
</PropertyGroup>
```

```xml
<ItemGroup>
  <PackageReference Include="Humanizer" Version="2.14.1" />
</ItemGroup>
```

---

# 22. `<ItemGroup>` for NuGet Packages

To add NuGet packages, use `<PackageReference>` inside `<ItemGroup>`.

```xml
<ItemGroup>
  <PackageReference Include="Dapper" Version="2.1.66" />
  <PackageReference Include="FluentValidation" Version="11.11.0" />
</ItemGroup>
```

This means the project depends on these packages.

---

# 23. Full Example with Package References

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Dapper" Version="2.1.66" />
    <PackageReference Include="FluentValidation" Version="11.11.0" />
  </ItemGroup>

</Project>
```

---

# 24. `<ItemGroup>` for Project References

If one project depends on another project, use `<ProjectReference>`.

Example solution structure:

```text
ShopSolution/
├── Shop.Api/
│   └── Shop.Api.csproj
├── Shop.Services/
│   └── Shop.Services.csproj
└── Shop.Data/
    └── Shop.Data.csproj
```

Inside `Shop.Api.csproj`:

```xml
<ItemGroup>
  <ProjectReference Include="..\Shop.Services\Shop.Services.csproj" />
  <ProjectReference Include="..\Shop.Data\Shop.Data.csproj" />
</ItemGroup>
```

This means `Shop.Api` can use classes from `Shop.Services` and `Shop.Data`.

---

# 25. `<ItemGroup>` for Content Files

You can include files that should be copied to the output folder.

```xml
<ItemGroup>
  <Content Include="Files\welcome-message.txt">
    <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
  </Content>
</ItemGroup>
```

This copies the file to the output directory when the project builds.

---

# 26. Common `CopyToOutputDirectory` Values

| Value | Meaning |
|---|---|
| `Never` | Do not copy the file |
| `Always` | Always copy the file |
| `PreserveNewest` | Copy only if the source file is newer |

Example:

```xml
<ItemGroup>
  <Content Include="Assets\sample-data.json">
    <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
  </Content>
</ItemGroup>
```

---

# 27. `<ItemGroup>` for Embedded Resources

An embedded resource is stored inside the compiled assembly.

```xml
<ItemGroup>
  <EmbeddedResource Include="Templates\invoice-template.html" />
</ItemGroup>
```

You can use this for:

- Email templates
- Static text files
- Small configuration templates
- Localization resources

---

# 28. `<ItemGroup>` for Compile Items

In SDK-style projects, `.cs` files are included automatically by default.

So usually you do **not** need this:

```xml
<ItemGroup>
  <Compile Include="Services\OrderService.cs" />
</ItemGroup>
```

However, you may use it in special cases.

For example, excluding a file:

```xml
<ItemGroup>
  <Compile Remove="Experiments\OldDiscountCalculator.cs" />
</ItemGroup>
```

Or including generated code from a specific folder:

```xml
<ItemGroup>
  <Compile Include="Generated\Models\*.cs" />
</ItemGroup>
```

---

# 29. Multiple `<ItemGroup>` Blocks

A `.csproj` file can have more than one `<ItemGroup>`.

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Serilog.AspNetCore" Version="9.0.0" />
  </ItemGroup>

  <ItemGroup>
    <ProjectReference Include="..\Store.Core\Store.Core.csproj" />
  </ItemGroup>

  <ItemGroup>
    <Content Include="Config\default-settings.json">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </Content>
  </ItemGroup>

</Project>
```

This is valid.

You can also combine them:

```xml
<ItemGroup>
  <PackageReference Include="Serilog.AspNetCore" Version="9.0.0" />
  <ProjectReference Include="..\Store.Core\Store.Core.csproj" />
  <Content Include="Config\default-settings.json">
    <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
  </Content>
</ItemGroup>
```

---

# 30. Practical Example Combining All Topics

## Project Structure

```text
OrderTracker/
├── OrderTracker.Api/
│   ├── Program.cs
│   ├── Program.Helpers.cs
│   ├── appsettings.json
│   └── OrderTracker.Api.csproj
└── OrderTracker.Tests/
    └── OrderTracker.Tests.csproj
```

---

## `Program.cs`

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IOrderService, OrderService>();

var app = builder.Build();

app.MapGet("/", () => "Order Tracker API is online.");

app.MapGet("/orders", (IOrderService service) =>
{
    return service.GetRecentOrders();
});

app.MapGet("/health", () =>
{
    return Program.GetHealthStatus();
});

app.Run();

public partial class Program
{
}
```

---

## `Program.Helpers.cs`

```csharp
public partial class Program
{
    public static string GetHealthStatus()
    {
        return "Healthy";
    }

    public static string CreateTrackingCode()
    {
        return $"TRK-{DateTime.UtcNow:yyyyMMddHHmmss}";
    }
}
```

---

## `IOrderService.cs`

```csharp
public interface IOrderService
{
    List<string> GetRecentOrders();
}
```

---

## `OrderService.cs`

```csharp
public class OrderService : IOrderService
{
    public List<string> GetRecentOrders()
    {
        return new List<string>
        {
            "Order-2041",
            "Order-2042",
            "Order-2043"
        };
    }
}
```

---

## `OrderTracker.Api.csproj`

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

  <ItemGroup>
    <PackageReference Include="Serilog.AspNetCore" Version="9.0.0" />
  </ItemGroup>

  <ItemGroup>
    <Content Include="appsettings.json">
      <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </Content>
  </ItemGroup>

</Project>
```

---

# 31. Practical Meaning of Each Part

## `public partial class Program`

```csharp
public partial class Program
{
}
```

Means:

- The `Program` class can be split across files.
- The compiler-generated `Program` class can be extended.
- Test projects can access `Program`.
- Static helper methods can be added in another file.

---

## `static string GetHealthStatus()`

```csharp
public static string GetHealthStatus()
{
    return "Healthy";
}
```

Means:

- The method belongs to `Program`.
- It can be called without creating a `Program` object.
- It can be called like this:

```csharp
Program.GetHealthStatus();
```

---

## `<ItemGroup>`

```xml
<ItemGroup>
  <PackageReference Include="Serilog.AspNetCore" Version="9.0.0" />
</ItemGroup>
```

Means:

- This project uses the `Serilog.AspNetCore` NuGet package.
- The package will be restored during build or restore.
- The project can use APIs from that package.

---

# 32. Common Mistakes

## Mistake 1: Calling a Static Method Through an Object

❌ Avoid this:

```csharp
var formatter = new TextFormatter();

string value = formatter.MakeTitle("daily notes");
```

If `MakeTitle` is static, call it through the class:

✅ Correct:

```csharp
string value = TextFormatter.MakeTitle("daily notes");
```

---

## Mistake 2: Accessing Instance Fields from Static Methods

❌ Incorrect:

```csharp
public class Counter
{
    private int _count = 0;

    public static void Increase()
    {
        _count++;
    }
}
```

A static method cannot directly access `_count` because `_count` belongs to an object.

✅ Correct option 1: Make the field static

```csharp
public class Counter
{
    private static int _count = 0;

    public static void Increase()
    {
        _count++;
    }
}
```

✅ Correct option 2: Use an instance method

```csharp
public class Counter
{
    private int _count = 0;

    public void Increase()
    {
        _count++;
    }
}
```

---

## Mistake 3: Forgetting `partial`

❌ This may cause problems in some testing scenarios:

```csharp
var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/", () => "API is available.");

app.Run();
```

✅ Better for integration testing:

```csharp
var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/", () => "API is available.");

app.Run();

public partial class Program
{
}
```

---

## Mistake 4: Putting `<ItemGroup>` Outside `<Project>`

❌ Incorrect:

```xml
<ItemGroup>
  <PackageReference Include="CsvHelper" Version="33.0.1" />
</ItemGroup>

<Project Sdk="Microsoft.NET.Sdk">
</Project>
```

✅ Correct:

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <ItemGroup>
    <PackageReference Include="CsvHelper" Version="33.0.1" />
  </ItemGroup>

</Project>
```

---

## Mistake 5: Using `<ItemGroup>` for Settings

❌ Incorrect:

```xml
<ItemGroup>
  <TargetFramework>net9.0</TargetFramework>
</ItemGroup>
```

✅ Correct:

```xml
<PropertyGroup>
  <TargetFramework>net9.0</TargetFramework>
</PropertyGroup>
```

---

# 33. Quick Reference

| Concept | Meaning | Example |
|---|---|---|
| `Program` | Main application class | `public class Program` |
| `partial` | Allows class to be split across files | `public partial class Program` |
| `static` method | Method called on class, not object | `Calculator.Add(4, 5)` |
| `<ItemGroup>` | Collection of project items in `.csproj` | Packages, references, content files |
| `<PackageReference>` | Adds NuGet package | `Include="Dapper"` |
| `<ProjectReference>` | References another project | `Include="..\Core\Core.csproj"` |
| `<Content>` | Adds content file | `Include="settings.json"` |
| `<Compile>` | Controls source code files | `Remove="OldFile.cs"` |