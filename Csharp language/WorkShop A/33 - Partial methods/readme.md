# C# Local Functions

Local functions are **methods declared inside another method**. They help you keep small helper logic close to the code that uses it.

They are especially useful when:

- a helper is only needed in one method
- you want better readability than a long lambda
- you need recursion
- you want to access local variables from the containing method

---

## Basic Idea

A local function is written inside a method body:

```csharp
using System;

class Sample
{
    static void Main()
    {
        int MultiplyByTwo(int n)
        {
            return n * 2;
        }

        Console.WriteLine(MultiplyByTwo(7));
    }
}
```

### What makes it different from a normal method?

- It is **only visible inside the containing member**
- It cannot be called from outside that method
- It can use local variables and parameters from the outer method

---

## Why Use Local Functions?

### 1. Keep helper logic nearby

Instead of creating a separate private method for tiny logic, you can keep it where it belongs.

```csharp
using System;

class InvoiceApp
{
    static void PrintInvoiceTotal(decimal amount, decimal taxRate)
    {
        decimal AddTax(decimal value)
        {
            return value + (value * taxRate);
        }

        Console.WriteLine($"Final total: {AddTax(amount):0.00}");
    }
}
```

Here, `AddTax` uses `taxRate` from the outer method.

---

## Accessing Outer Variables

A local function can capture:

- method parameters
- local variables
- fields and properties of the class

```csharp
using System;

class GreetingTool
{
    static void SendGreeting(string customerName)
    {
        string suffix = "! Welcome aboard";

        string BuildMessage()
        {
            return "Hello, " + customerName + suffix;
        }

        Console.WriteLine(BuildMessage());
    }
}
```

---

## Local Function with Recursion

Local functions are often clearer than lambdas for recursive logic.

```csharp
using System;

class MathDemo
{
    static void Main()
    {
        int Fibonacci(int index)
        {
            if (index <= 1)
                return index;

            return Fibonacci(index - 1) + Fibonacci(index - 2);
        }

        Console.WriteLine(Fibonacci(6));
    }
}
```

---

## Static Local Functions

If a local function does **not need outer variables**, make it `static`.

### Why?

Because a `static` local function:

- cannot capture outer state
- makes intent clearer
- can reduce accidental dependencies

```csharp
using System;

class Converter
{
    static void Main()
    {
        static double ToMiles(double kilometers)
        {
            return kilometers * 0.621371;
        }

        Console.WriteLine(ToMiles(10));
    }
}
```

> **Tip:** Use `static` when the local function is self-contained.

---

## Local Functions vs Lambdas

Both can be used for small logic, but they are not always equally convenient.

### Comparison Table

| Feature | Local Function | Lambda Expression |
|---|---|---|
| Named | Yes | Usually anonymous |
| Recursion | Easy | More awkward |
| Can have attributes | Yes | No |
| Can be `static` | Yes | Yes, in some contexts |
| Readability for complex logic | Often better | Best for short expressions |

### Example: Lambda version

```csharp
Func<int, int> square = x => x * x;
Console.WriteLine(square(5));
```

### Example: Local function version

```csharp
int Square(int x)
{
    return x * x;
}

Console.WriteLine(Square(5));
```

Use a lambda for short delegate-style behavior.  
Use a local function for **structured helper logic**.

---

## Local Functions with Iterators

They are useful when you want validation before `yield return`.

```csharp
using System;
using System.Collections.Generic;

class SequenceBuilder
{
    static IEnumerable<int> CreateRange(int start, int count)
    {
        if (count < 0)
            throw new ArgumentOutOfRangeException(nameof(count));

        return Iterator();

        IEnumerable<int> Iterator()
        {
            for (int i = 0; i < count; i++)
                yield return start + i;
        }
    }
}
```

### Why this pattern helps

Validation happens immediately in the outer method, while the iterator logic stays separate and clean.

---

## Local Functions with Async Code

They also help organize asynchronous methods.

```csharp
using System;
using System.Threading.Tasks;

class Downloader
{
    static async Task<string> GetMessageAsync(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new ArgumentException("User id is required.", nameof(userId));

        return await LoadAsync();

        async Task<string> LoadAsync()
        {
            await Task.Delay(150);
            return $"Profile loaded for {userId}";
        }
    }
}
```

---

## Scope Rules

A local function exists only within its containing block.

```csharp
void Process()
{
    void StepOne()
    {
        Console.WriteLine("Step one");
    }

    StepOne();
}
```

Trying to call `StepOne()` outside `Process()` would fail.

---

## Modifiers You Can Use

Local functions can use some modifiers.

### Common ones

- `static`
- `async`
- `unsafe`

### Example

```csharp
async Task<int> CountLaterAsync()
{
    async Task<int> FetchAsync()
    {
        await Task.Delay(100);
        return 42;
    }

    return await FetchAsync();
}
```

---

## When Local Functions Are a Good Fit

- **validation helpers** used only once
- **recursive algorithms**
- **iterator helpers**
- **async helper routines**
- **small private logic** that would otherwise clutter the class

## When They Are Not Ideal

- when the logic is reused in many methods
- when the function grows too large
- when it deserves a descriptive class-level method
- when too many nested local functions hurt readability

---

# Splitting Classes

C# allows a class to be split across multiple files by using the `partial` keyword.

This is called a **partial class**.

---

## Why Split a Class?

Large classes can become difficult to manage. Splitting them helps organize code by responsibility.

### Common uses

- separating generated code from handwritten code
- grouping properties, methods, and events into different files
- making large types easier to navigate
- allowing multiple developers to work on the same class more comfortably

---

## Basic Partial Class

A partial class is declared with the `partial` keyword in each part.

### File: `Customer.Profile.cs`

```csharp
public partial class Customer
{
    public string FullName { get; set; } = "";
    public string EmailAddress { get; set; } = "";
}
```

### File: `Customer.Actions.cs`

```csharp
using System;

public partial class Customer
{
    public void DisplayProfile()
    {
        Console.WriteLine($"{FullName} - {EmailAddress}");
    }
}
```

These two files are combined by the compiler into **one class**.

---

## Important Rule

> Every part must use the `partial` keyword.

If one part forgets `partial`, the compiler treats it as a different declaration and raises an error.

---

## Partial Class Requirements

All parts must match in key ways.

### They must have the same:

- class name
- namespace
- accessibility level context
- generic parameters, if any

### Example

```csharp
namespace ShopSystem
{
    public partial class OrderManager
    {
        public int CurrentOrderId { get; set; }
    }
}
```

```csharp
namespace ShopSystem
{
    public partial class OrderManager
    {
        public void Reset()
        {
            CurrentOrderId = 0;
        }
    }
}
```

---

## Members Combine Into One Type

Fields, methods, properties, events, nested types, and interfaces all become part of the final compiled class.

```csharp
public partial class Report
{
    public string Title { get; set; } = "";
}
```

```csharp
public partial class Report
{
    public void Print()
    {
        Console.WriteLine(Title);
    }
}
```

---

## Partial Classes and Inheritance

A base class is declared in one of the parts, but it applies to the whole class.

```csharp
public class Entity
{
    public int Id { get; set; }
}
```

```csharp
public partial class Product : Entity
{
    public string Name { get; set; } = "";
}
```

```csharp
public partial class Product
{
    public void Show()
    {
        Console.WriteLine($"{Id}: {Name}");
    }
}
```

### Rule

- the base class must be consistent
- interfaces from different parts are merged

---

## Partial Classes with Interfaces

```csharp
public interface ILoggable
{
    void Log();
}

public interface IPrintable
{
    void Print();
}
```

```csharp
public partial class Document : ILoggable
{
    public void Log()
    {
        Console.WriteLine("Logged document");
    }
}
```

```csharp
public partial class Document : IPrintable
{
    public void Print()
    {
        Console.WriteLine("Printed document");
    }
}
```

The final `Document` class implements **both** interfaces.

---

## Typical File Organization

A common pattern is:

- `Employee.Core.cs`
- `Employee.Validation.cs`
- `Employee.Commands.cs`

### Example

#### `Employee.Core.cs`

```csharp
public partial class Employee
{
    public string Name { get; set; } = "";
    public decimal Salary { get; set; }
}
```

#### `Employee.Validation.cs`

```csharp
using System;

public partial class Employee
{
    public void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw new InvalidOperationException("Name is required.");

        if (Salary < 0)
            throw new InvalidOperationException("Salary cannot be negative.");
    }
}
```

#### `Employee.Commands.cs`

```csharp
using System;

public partial class Employee
{
    public void Save()
    {
        Validate();
        Console.WriteLine("Employee saved.");
    }
}
```

---

## Partial Structs and Interfaces

The `partial` keyword is not limited to classes.

It can also be used with:

- `struct`
- `interface`
- `record`

### Example with a struct

```csharp
public partial struct Coordinates
{
    public int X;
}
```

```csharp
public partial struct Coordinates
{
    public int Y;
}
```

---

## Partial Records

```csharp
public partial record ProductInfo(string Code);
```

```csharp
public partial record ProductInfo
{
    public decimal Price { get; init; }
}
```

---

## Generated Code Scenario

One of the most common reasons for partial classes is to keep generated code separate from custom code.

### Generated file

```csharp
public partial class DashboardView
{
    private string _layoutName = "MainLayout";
}
```

### Your file

```csharp
public partial class DashboardView
{
    public void Render()
    {
        Console.WriteLine($"Rendering {_layoutName}");
    }
}
```

This prevents your manual changes from being overwritten when the generated file is recreated.

---

## Best Practices for Partial Classes

### Good practices

- split by **responsibility**
- use clear file names
- keep all parts in the same namespace
- use partial classes mainly when there is a real organizational benefit

### Avoid

- splitting small classes without reason
- scattering related logic too much
- making the class harder to follow across many files

---

# Partial Methods

A **partial method** is a method whose declaration can appear in one part of a partial type and whose implementation can appear in another part.

They are mainly used with **partial classes/structs**.

---

## Why Partial Methods Exist

They are useful when one part of a type wants to provide a **hook** that another part may implement.

This is especially common in generated code.

> Think of a partial method as an optional extension point inside a partial type.

---

## Basic Pattern

### Declaring the method

```csharp
public partial class Account
{
    partial void OnCreated();
}
```

### Implementing the method

```csharp
using System;

public partial class Account
{
    partial void OnCreated()
    {
        Console.WriteLine("Account was created.");
    }
}
```

The compiler combines them into one method.

---

## Calling a Partial Method

```csharp
public partial class Account
{
    public Account()
    {
        OnCreated();
    }

    partial void OnCreated();
}
```

If the implementation exists, it runs.  
If not, behavior depends on the method form and language rules.

---

## Traditional Partial Method Restrictions

Originally, partial methods had strong restrictions. They had to be:

- implicitly `private`
- `void`
- without `out` parameters

This made them lightweight optional hooks.

### Example

```csharp
public partial class Purchase
{
    partial void OnAmountChanged();

    private decimal _amount;

    public decimal Amount
    {
        get => _amount;
        set
        {
            _amount = value;
            OnAmountChanged();
        }
    }
}
```

```csharp
using System;

public partial class Purchase
{
    partial void OnAmountChanged()
    {
        Console.WriteLine("Amount changed.");
    }
}
```

---

## Modern Partial Methods

Newer C# versions allow broader use of partial methods.

They can now:

- have accessibility modifiers
- return values
- use `out` parameters

### But there is a rule

> If a partial method has an explicit accessibility modifier or otherwise requires a real callable member, it **must** have an implementation.

---

## Example with Accessibility

```csharp
public partial class TaxCalculator
{
    public partial decimal ComputeTax(decimal amount);
}
```

```csharp
public partial class TaxCalculator
{
    public partial decimal ComputeTax(decimal amount)
    {
        return amount * 0.18m;
    }
}
```

Because it is `public`, an implementation is required.

---

## Example of Optional Hook Style

This is the classic generated-code pattern:

```csharp
public partial class CustomerForm
{
    partial void BeforeRender();

    public void Render()
    {
        BeforeRender();
        Console.WriteLine("Rendering form...");
    }
}
```

### Optional implementation

```csharp
using System;

public partial class CustomerForm
{
    partial void BeforeRender()
    {
        Console.WriteLine("Preparing data...");
    }
}
```

If this implementation were missing, the call could be omitted by the compiler in the traditional form.

---

## Partial Methods vs Normal Methods

| Feature | Partial Method | Normal Method |
|---|---|---|
| Requires partial type | Yes | No |
| Can be split across files | Yes | No |
| Useful for generated hooks | Very much | Less specialized |
| May be optional in classic form | Yes | No |

---

## Partial Methods and Generated Code

Generated code often declares methods like:

- `OnLoaded()`
- `OnNameUpdated()`
- `OnBeforeSave()`

Then your own partial file can implement them.

### Example

#### Generated part

```csharp
public partial class Profile
{
    partial void OnNicknameChanged();

    private string _nickname = "";

    public string Nickname
    {
        get => _nickname;
        set
        {
            _nickname = value;
            OnNicknameChanged();
        }
    }
}
```

#### Custom part

```csharp
using System;

public partial class Profile
{
    partial void OnNicknameChanged()
    {
        Console.WriteLine($"Nickname updated to: {Nickname}");
    }
}
```

---

## Rules to Remember

### For partial classes

- all parts must use `partial`
- all parts must represent the same type
- the compiler merges them into one type

### For partial methods

- they must be inside a partial type
- declaration and implementation signatures must match
- modern partial methods can be more powerful, but some forms require implementation

---

## Good Use Cases for Partial Methods

- **generated code hooks**
- **customization points**
- **property change callbacks**
- **validation steps triggered from generated members**

## Poor Use Cases

- general-purpose application logic
- replacing normal methods without a real need
- designs where splitting implementation makes code harder to find

---

## Quick Mental Model

### `local function`
A helper method **inside another method**

### `partial class`
One class **split across multiple files**

### `partial method`
One method declaration and implementation **split across parts of a partial type**