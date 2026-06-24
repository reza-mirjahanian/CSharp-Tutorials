# C# Types: **Value Types** vs **Reference Types**

In C#, most types fall into two broad categories:

1. **Value types**
2. **Reference types**

Understanding the difference helps you reason about:

- Memory allocation
- Default values
- The `new` keyword
- Assignment behavior
- `null`
- Constructors

---

## 1. Value Types

**Value types** store their actual data directly in the variable.

Common examples include:

| Type | Example | .NET Type |
|---|---:|---|
| `int` | `42` | `System.Int32` |
| `short` | `12` | `System.Int16` |
| `long` | `9_000_000_000` | `System.Int64` |
| `bool` | `true` | `System.Boolean` |
| `DateTime` | `new DateTime(...)` | `System.DateTime` |
| `Point` | `new Point(...)` | `System.Drawing.Point` |
| `struct` types | custom structs | user-defined value types |

---

### Key idea

> A value type variable directly contains its value.

For example:

```csharp
int score = 90;
```

The variable `score` directly stores the integer value `90`.

---

## 2. Reference Types

**Reference types** do not store the object directly in the variable.

Instead, the variable stores a **reference** to an object located elsewhere in memory.

Common examples include:

| Type | Example |
|---|---|
| `string` | `"Hello"` |
| Arrays | `int[] numbers` |
| Classes | `Customer`, `Person`, `Order` |
| Interfaces | `IDisposable`, `IEnumerable` |
| Delegates | `Action`, `Func<int>` |

---

### Key idea

> A reference type variable stores a reference to an object, not the object itself.

For example:

```csharp
Customer customer = new Customer();
```

Here:

- `customer` stores a reference
- The actual `Customer` object is created using `new`

---

# The `new` Keyword

The `new` keyword is used to call a constructor.

It can be used with both:

- **Value types**
- **Reference types**

But it plays a slightly different role depending on the type.

---

## `new` with Value Types

Value types do **not always require** `new`.

Many value types can be assigned using literals:

```csharp
int level = 8;
long cityPopulation = 3_750_000;
bool isEnabled = true;
```

However, some value types do not have literal syntax.

For those, you often use `new`.

```csharp
DateTime meetingDate = new DateTime(2026, 6, 15);
Point position = new Point(25, 40);
```

---

### Target-Typed `new`

Modern C# allows a shorter form when the type is already known:

```csharp
DateTime meetingDate = new(2026, 6, 15);
Point position = new(25, 40);
```

This is equivalent to:

```csharp
DateTime meetingDate = new DateTime(2026, 6, 15);
Point position = new Point(25, 40);
```

---

## `new` with Reference Types

Reference types usually need `new` to create an actual object.

```csharp
Customer customer = new Customer();
```

Without `new`, the variable can exist, but it does not refer to an object yet.

```csharp
Customer customer;
```

At this point, for a **local variable**, `customer` is declared but not usable until assigned.

```csharp
Customer customer;

customer.Name = "Mina"; // ❌ Compile-time error
```

You must assign it first:

```csharp
Customer customer = new Customer();

customer.Name = "Mina"; // ✅ Works
```

---

# Important: Local Variables vs Fields

C# treats **local variables** and **fields** differently.

---

## Local Variables

A local variable is declared inside a method, constructor, or block.

```csharp
void PrintReport()
{
    int count;
    Customer customer;
}
```

Local variables are **not automatically initialized for use**.

You must assign them before reading from them.

```csharp
void PrintReport()
{
    int count;

    Console.WriteLine(count); // ❌ Compile-time error
}
```

Correct:

```csharp
void PrintReport()
{
    int count = 0;

    Console.WriteLine(count); // ✅ Works
}
```

---

## Fields

A field belongs to a class or struct.

Fields are automatically initialized to their default values.

```csharp
class Report
{
    public int PageCount;
    public DateTime CreatedAt;
    public Customer Owner;
}
```

The default values are:

| Field | Default Value |
|---|---|
| `PageCount` | `0` |
| `CreatedAt` | `0001-01-01 00:00:00` |
| `Owner` | `null` |

---

# Example: Declaring Variables

Consider this code:

```csharp
using System;
using System.Drawing;

short temperature;
long totalDownloads;
DateTime scheduledAt;
Point screenPosition;
Employee manager;
```

These variables have been declared, but because they are **local variables**, you cannot read them until they are assigned.

For example:

```csharp
Console.WriteLine(temperature); // ❌ Compile-time error
Console.WriteLine(manager);     // ❌ Compile-time error
```

You must initialize them first.

---

# Initializing Value Types

Value types can be initialized using literals when literals exist.

```csharp
short temperature = 18;
long totalDownloads = 12_500_000;
```

Some value types require constructor calls because C# has no literal syntax for them.

```csharp
DateTime scheduledAt = new DateTime(2026, 7, 4);
Point screenPosition = new Point(320, 180);
```

Or using target-typed `new`:

```csharp
DateTime scheduledAt = new(2026, 7, 4);
Point screenPosition = new(320, 180);
```

---

# Initializing Reference Types

Suppose we have this class:

```csharp
public class Employee
{
    public string FirstName;
    public string LastName;
    public int YearsOfExperience;

    public Employee()
    {
        FirstName = "";
        LastName = "";
        YearsOfExperience = 0;
    }

    public Employee(string firstName, string lastName, int yearsOfExperience)
    {
        FirstName = firstName;
        LastName = lastName;
        YearsOfExperience = yearsOfExperience;
    }
}
```

You can create an `Employee` object using the default constructor:

```csharp
Employee manager = new Employee();
```

Or using target-typed `new`:

```csharp
Employee manager = new();
```

You can also use a constructor with arguments:

```csharp
Employee manager = new Employee("Sara", "Ahmadi", 9);
```

Or shorter:

```csharp
Employee manager = new("Sara", "Ahmadi", 9);
```

---

# Full Example

```csharp
using System;
using System.Drawing;

public class Employee
{
    public string FirstName;
    public string LastName;
    public int YearsOfExperience;

    public Employee()
    {
        FirstName = "";
        LastName = "";
        YearsOfExperience = 0;
    }

    public Employee(string firstName, string lastName, int yearsOfExperience)
    {
        FirstName = firstName;
        LastName = lastName;
        YearsOfExperience = yearsOfExperience;
    }
}

public class Program
{
    public static void Main()
    {
        short temperature = 18;
        long totalDownloads = 12_500_000;

        DateTime scheduledAt = new(2026, 7, 4);
        Point screenPosition = new(320, 180);

        Employee manager = new();
        Employee leadDeveloper = new("Sara", "Ahmadi", 9);

        Console.WriteLine(temperature);
        Console.WriteLine(totalDownloads);
        Console.WriteLine(scheduledAt);
        Console.WriteLine(screenPosition);
        Console.WriteLine(leadDeveloper.FirstName);
    }
}
```

---

# Memory Model: Simplified View

A common simplified explanation is:

| Type Category | Variable Stores | Object Data Usually Lives |
|---|---|---|
| Value type | The actual value | Directly in the variable location |
| Reference type | A reference/address | On the managed heap |

---

## Value Type Example

```csharp
int points = 75;
```

The variable `points` directly contains:

```text
75
```

---

## Reference Type Example

```csharp
Employee developer = new("Nima", "Karimi", 5);
```

The variable `developer` contains a reference to an object.

Conceptually:

```text
developer ───────────────► Employee object
                            FirstName = "Nima"
                            LastName = "Karimi"
                            YearsOfExperience = 5
```

---

# Default Values

Every type in C# has a default value.

## Default Values for Common Value Types

| Type | Default Value |
|---|---|
| `int` | `0` |
| `short` | `0` |
| `long` | `0` |
| `bool` | `false` |
| `char` | `'\0'` |
| `double` | `0.0` |
| `decimal` | `0.0M` |
| `DateTime` | `0001-01-01 00:00:00` |
| `Point` | `X = 0`, `Y = 0` |

---

## Default Value for Reference Types

Most reference types default to:

```csharp
null
```

Example:

```csharp
Employee manager = null;
```

This means `manager` does not currently refer to an `Employee` object.

---

# Using `default`

You can explicitly assign the default value of a type using `default`.

```csharp
int count = default;
DateTime createdAt = default;
Point position = default;
Employee employee = default;
```

Equivalent to:

```csharp
int count = 0;
DateTime createdAt = new DateTime();
Point position = new Point();
Employee employee = null;
```

---

# Comparing Value Types and Reference Types

| Feature | Value Types | Reference Types |
|---|---|---|
| Stores | Actual value | Reference to an object |
| Examples | `int`, `bool`, `DateTime`, `Point`, `struct` | `string`, arrays, classes |
| Can be `null` by default? | No, unless nullable | Yes |
| Usually needs `new`? | Not always | Usually yes |
| Assignment copies | The value | The reference |
| Default value | Usually zero-like value | `null` |

---

# Assignment Behavior

## Value Type Assignment Copies the Value

```csharp
int first = 10;
int second = first;

second = 25;

Console.WriteLine(first);  // 10
Console.WriteLine(second); // 25
```

Changing `second` does not affect `first`.

They are separate values.

---

## Reference Type Assignment Copies the Reference

```csharp
Employee firstEmployee = new("Leila", "Rahimi", 4);
Employee secondEmployee = firstEmployee;

secondEmployee.FirstName = "Maryam";

Console.WriteLine(firstEmployee.FirstName);  // Maryam
Console.WriteLine(secondEmployee.FirstName); // Maryam
```

Both variables refer to the same object.

Conceptually:

```text
firstEmployee  ───────┐
                      ├────► Employee object
secondEmployee ───────┘       FirstName = "Maryam"
                              LastName = "Rahimi"
                              YearsOfExperience = 4
```

---

# Nullable Value Types

Normally, value types cannot be `null`.

```csharp
int count = null; // ❌ Error
```

But you can make a value type nullable using `?`.

```csharp
int? count = null;
DateTime? finishedAt = null;
Point? optionalPosition = null;
```

This is useful when a value may be missing.

```csharp
int? rating = null;

if (rating.HasValue)
{
    Console.WriteLine(rating.Value);
}
else
{
    Console.WriteLine("No rating yet.");
}
```

---

# Constructors and `new`

A constructor initializes a new value or object.

---

## Value Type Constructor

```csharp
DateTime deadline = new DateTime(2026, 8, 20);
```

This creates a `DateTime` value representing:

```text
August 20, 2026
```

Shorter syntax:

```csharp
DateTime deadline = new(2026, 8, 20);
```

---

## Reference Type Constructor

```csharp
Employee tester = new Employee("Omid", "Moradi", 3);
```

This creates an `Employee` object and initializes its state:

```text
FirstName = "Omid"
LastName = "Moradi"
YearsOfExperience = 3
```

Shorter syntax:

```csharp
Employee tester = new("Omid", "Moradi", 3);
```

---

# Default Constructors

## Value Types

All value types have a default value.

```csharp
int number = new int();
DateTime date = new DateTime();
Point point = new Point();
```

These are equivalent to:

```csharp
int number = 0;
DateTime date = default;
Point point = default;
```

With target-typed `new`:

```csharp
int number = new();
DateTime date = new();
Point point = new();
```

---

## Reference Types

A class can have a default constructor.

```csharp
Employee employee = new Employee();
```

Or:

```csharp
Employee employee = new();
```

This creates an object with whatever initial values the constructor assigns.

---

# `null` and Reference Types

A reference type variable can refer to no object.

```csharp
Employee employee = null;
```

Trying to use members through a `null` reference causes a runtime error.

```csharp
Employee employee = null;

Console.WriteLine(employee.FirstName); // ❌ NullReferenceException
```

Correct approach:

```csharp
Employee employee = new("Ava", "Ebrahimi", 6);

Console.WriteLine(employee.FirstName); // ✅ Ava
```

Or check for `null`:

```csharp
Employee employee = null;

if (employee is not null)
{
    Console.WriteLine(employee.FirstName);
}
else
{
    Console.WriteLine("No employee assigned.");
}
```

---

# Practical Example with Comments

```csharp
using System;
using System.Drawing;

public class Product
{
    public string Name;
    public decimal Price;

    public Product()
    {
        Name = "Unnamed";
        Price = 0;
    }

    public Product(string name, decimal price)
    {
        Name = name;
        Price = price;
    }
}

public class Program
{
    public static void Main()
    {
        // Value types initialized with literals
        int quantity = 12;
        decimal discount = 0.15M;

        // Value types initialized with constructors
        DateTime saleDate = new(2026, 9, 10);
        Point labelPosition = new(50, 75);

        // Reference type initialized with default constructor
        Product emptyProduct = new();

        // Reference type initialized with constructor arguments
        Product keyboard = new("Mechanical Keyboard", 89.99M);

        Console.WriteLine(quantity);
        Console.WriteLine(discount);
        Console.WriteLine(saleDate);
        Console.WriteLine(labelPosition);
        Console.WriteLine(emptyProduct.Name);
        Console.WriteLine(keyboard.Name);
    }
}
```

---

# Key Rules to Remember

## Rule 1: Value types store values

```csharp
int number = 100;
```

The variable directly contains `100`.

---

## Rule 2: Reference types store references

```csharp
Product product = new("Desk Lamp", 34.50M);
```

The variable stores a reference to a `Product` object.

---

## Rule 3: `new` calls a constructor

```csharp
DateTime date = new(2026, 12, 1);
Product product = new("Notebook", 4.99M);
```

In both cases, `new` calls a constructor.

---

## Rule 4: Reference types need an object before you use members

```csharp
Product product;

product.Name = "Monitor"; // ❌ Error
```

Correct:

```csharp
Product product = new();

product.Name = "Monitor"; // ✅ Works
```

---

## Rule 5: Local variables must be assigned before use

```csharp
int amount;

Console.WriteLine(amount); // ❌ Error
```

Correct:

```csharp
int amount = 0;

Console.WriteLine(amount); // ✅ Works
```

---

# Modern Syntax vs Older Syntax

## Modern Target-Typed `new`

```csharp
DateTime startDate = new(2026, 5, 12);
Point offset = new(15, 30);
Product chair = new("Office Chair", 149.99M);
```

## Older Explicit Syntax

```csharp
DateTime startDate = new DateTime(2026, 5, 12);
Point offset = new Point(15, 30);
Product chair = new Product("Office Chair", 149.99M);
```

Both forms are valid.

The target-typed form works when C# can clearly determine the type from context.