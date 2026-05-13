# C# Generic Types

**Generic types** let you write code that works with **many data types** while keeping **type safety**.

Instead of creating separate versions of a class or method for `int`, `string`, `decimal`, and so on, you can write the logic **once** and let the type be supplied later.

---

# Why Generics Exist

Without generics, code often becomes:

- repetitive
- less safe
- harder to maintain

## Problem Without Generics

Imagine a container class for numbers:

```csharp
public class IntegerBox
{
    public int Value { get; set; }
}
```

Then later you need one for text:

```csharp
public class TextBox
{
    public string Value { get; set; } = "";
}
```

Then one for dates:

```csharp
public class DateBox
{
    public DateTime Value { get; set; }
}
```

This repeats the same structure again and again.

---

# Generic Type Idea

A generic type replaces the specific data type with a **type parameter**.

Usually that parameter is written like:

- `T`
- `TKey`
- `TValue`
- `TItem`

## Example

```csharp
public class Box<T>
{
    public T Value { get; set; }

    public Box(T value)
    {
        Value = value;
    }
}
```

Now the same class can hold many kinds of values.

---

# Using a Generic Type

```csharp
Box<int> quantityBox = new Box<int>(25);
Box<string> nameBox = new Box<string>("Ariana");
Box<DateTime> dateBox = new Box<DateTime>(new DateTime(2026, 3, 14));
```

Each one uses the same generic class, but with a different actual type.

---

# Key Terminology

| Term | Meaning |
|---|---|
| **Generic type** | A type that uses one or more type parameters |
| **Type parameter** | Placeholder like `T` |
| **Type argument** | The actual type used, like `int` or `string` |
| **Constructed type** | A generic type with actual types supplied, like `Box<int>` |

---

# How to Read `Box<T>`

```csharp
Box<T>
```

Read it as:

> “A box of some type `T`.”

If you write:

```csharp
Box<string>
```

that means:

> “A box whose value type is `string`.”

---

# Simple Generic Class

```csharp
public class Crate<T>
{
    public T Item { get; set; }

    public Crate(T item)
    {
        Item = item;
    }

    public void ShowItem()
    {
        Console.WriteLine(Item);
    }
}
```

## Usage

```csharp
Crate<int> scoreCrate = new Crate<int>(88);
Crate<string> cityCrate = new Crate<string>("Shiraz");

scoreCrate.ShowItem();
cityCrate.ShowItem();
```

---

# Benefits of Generic Types

## ✅ 1. Reuse

You write the logic once and use it for many data types.

---

## ✅ 2. Type Safety

The compiler prevents incorrect usage.

```csharp
Crate<int> points = new Crate<int>(50);
```

You cannot later store a `string` in `points.Item`.

---

## ✅ 3. Better Than `object`

Before generics, developers often used `object` to store any value.

```csharp
public class OldContainer
{
    public object Data { get; set; }
}
```

That allows anything, but it loses type safety.

### Problems with `object`

- casting is required
- runtime errors become more likely
- code becomes less clear

### Example

```csharp
OldContainer container = new OldContainer();
container.Data = "hello";

string text = (string)container.Data;
```

This works only if the stored value really is a `string`.

If not, an exception may happen.

With generics:

```csharp
Crate<string> container = new Crate<string>("hello");
string text = container.Item;
```

No cast needed.

---

# Generic Collections

Many built-in .NET collections are generic.

## Common Examples

- `List<T>`
- `Dictionary<TKey, TValue>`
- `Queue<T>`
- `Stack<T>`
- `HashSet<T>`

---

# Example: `List<T>`

```csharp
List<string> animals = new List<string>
{
    "Falcon",
    "Otter",
    "Panda"
};
```

This list can store only `string` values.

```csharp
animals.Add("Koala");
```

But this is invalid:

```csharp
animals.Add(99);
```

❌ because `99` is not a `string`

---

# Example: `Dictionary<TKey, TValue>`

A dictionary stores pairs:

- **key**
- **value**

```csharp
Dictionary<int, string> products = new Dictionary<int, string>
{
    { 101, "Keyboard" },
    { 102, "Monitor" },
    { 103, "Speaker" }
};
```

Here:

- key type = `int`
- value type = `string`

Access:

```csharp
string item = products[102];
```

---

# Multiple Type Parameters

A generic type can have more than one type parameter.

## Example

```csharp
public class Pair<TLeft, TRight>
{
    public TLeft Left { get; set; }
    public TRight Right { get; set; }

    public Pair(TLeft left, TRight right)
    {
        Left = left;
        Right = right;
    }
}
```

## Usage

```csharp
Pair<string, int> userAge = new Pair<string, int>("Nima", 29);
Pair<int, bool> featureFlag = new Pair<int, bool>(7, true);
```

---

# Generic Methods Inside Generic Types

A generic class can also contain regular methods or additional generic methods.

## Example

```csharp
public class Shelf<T>
{
    public T StoredItem { get; set; }

    public Shelf(T item)
    {
        StoredItem = item;
    }

    public void Replace(T newItem)
    {
        StoredItem = newItem;
    }

    public U ConvertTo<U>(Func<T, U> converter)
    {
        return converter(StoredItem);
    }
}
```

## Usage

```csharp
Shelf<int> shelf = new Shelf<int>(64);

string text = shelf.ConvertTo(n => $"Value: {n}");
```

---

# Generic Type Constraints

Sometimes you do **not** want to allow *every* type.

You may want to limit `T` so the generic type can rely on certain capabilities.

This is done with **constraints**.

---

# `where` Constraint

```csharp
where T : ...
```

It tells the compiler what kinds of types are allowed.

---

# Common Constraints

| Constraint | Meaning |
|---|---|
| `where T : class` | `T` must be a reference type |
| `where T : struct` | `T` must be a value type |
| `where T : new()` | `T` must have a parameterless constructor |
| `where T : BaseType` | `T` must inherit from a base class |
| `where T : IInterface` | `T` must implement an interface |

---

# Example: Base Class Constraint

```csharp
public class Entity
{
    public int Id { get; set; }
}
```

```csharp
public class Repository<T> where T : Entity
{
    private readonly List<T> _records = new();

    public void Add(T item)
    {
        _records.Add(item);
    }

    public T? FindById(int id)
    {
        return _records.FirstOrDefault(x => x.Id == id);
    }
}
```

## Usage

```csharp
public class Customer : Entity
{
    public string Name { get; set; } = "";
}
```

```csharp
Repository<Customer> customers = new Repository<Customer>();
customers.Add(new Customer { Id = 301, Name = "Lina" });
```

This works because `Customer` inherits from `Entity`.

---

# Example: `new()` Constraint

If you want to create objects of type `T` inside the generic type:

```csharp
public class Factory<T> where T : new()
{
    public T Create()
    {
        return new T();
    }
}
```

## Usage

```csharp
Factory<StringBuilder> builderFactory = new Factory<StringBuilder>();
StringBuilder builder = builderFactory.Create();
```

Without `where T : new()`, `new T()` would not be allowed.

---

# Example: Interface Constraint

```csharp
public interface IPrintable
{
    void Print();
}
```

```csharp
public class PrinterQueue<T> where T : IPrintable
{
    private readonly List<T> _items = new();

    public void Enqueue(T item)
    {
        _items.Add(item);
    }

    public void PrintAll()
    {
        foreach (T item in _items)
        {
            item.Print();
        }
    }
}
```

Any type used with `PrinterQueue<T>` must implement `IPrintable`.

---

# Default Value of `T`

Inside a generic type, you may not know whether `T` is:

- a reference type
- a value type

The default value of `T` can be written as:

```csharp
default
```

or:

```csharp
default(T)
```

## Example

```csharp
public class Holder<T>
{
    public T ResetValue()
    {
        return default!;
    }
}
```

Possible defaults:

- `null` for reference types
- `0` for numeric value types
- `false` for `bool`
- zeroed struct value for structs

---

# Generic Interfaces

Interfaces can also be generic.

## Example

```csharp
public interface IRepository<T>
{
    void Add(T item);
    T? GetById(int id);
}
```

Implementation:

```csharp
public class MemoryRepository<T> : IRepository<T> where T : Entity
{
    private readonly List<T> _items = new();

    public void Add(T item)
    {
        _items.Add(item);
    }

    public T? GetById(int id)
    {
        return _items.FirstOrDefault(x => x.Id == id);
    }
}
```

---

# Generic Structs

Structs can also be generic.

```csharp
public struct Coordinate<T>
{
    public T X { get; set; }
    public T Y { get; set; }

    public Coordinate(T x, T y)
    {
        X = x;
        Y = y;
    }
}
```

## Usage

```csharp
Coordinate<int> pointA = new Coordinate<int>(12, 48);
Coordinate<double> pointB = new Coordinate<double>(3.5, 7.1);
```

---

# Generic Records

Records may also be generic.

```csharp
public record ApiResult<T>(bool Success, T Data, string Message);
```

## Usage

```csharp
ApiResult<string> response = new ApiResult<string>(true, "Done", "Operation completed");
ApiResult<int> code = new ApiResult<int>(false, -1, "Unavailable");
```

---

# Open vs Closed Generic Types

## Open Generic Type

A type parameter is still unresolved:

```csharp
Box<T>
Dictionary<TKey, TValue>
```

These are **open** generic types.

---

## Closed Generic Type

All type arguments are supplied:

```csharp
Box<int>
Dictionary<string, decimal>
```

These are **closed** generic types.

---

# Nested Example

```csharp
Dictionary<string, List<int>>
```

Read it as:

> dictionary of `string` keys and `List<int>` values

Example:

```csharp
Dictionary<string, List<int>> examScores = new()
{
    ["Math"] = new List<int> { 78, 85, 91 },
    ["Science"] = new List<int> { 88, 90 }
};
```

---

# Type Inference with Constructors

Sometimes the variable declaration makes the type obvious.

```csharp
Crate<string> box = new Crate<string>("Notebook");
```

With target-typed `new`, you can shorten it:

```csharp
Crate<string> box = new("Notebook");
```

---

# Generic Type Naming Conventions

Common names for type parameters:

| Name | Meaning |
|---|---|
| `T` | generic type |
| `TItem` | item type |
| `TKey` | dictionary key type |
| `TValue` | dictionary value type |
| `TResult` | return/result type |

These are conventions, not rules.

---

# Real-World Example: Response Wrapper

```csharp
public class ServiceResponse<T>
{
    public bool IsSuccessful { get; set; }
    public T? Payload { get; set; }
    public string ErrorText { get; set; } = "";
}
```

## Usage with text

```csharp
ServiceResponse<string> textResponse = new ServiceResponse<string>
{
    IsSuccessful = true,
    Payload = "Ready",
    ErrorText = ""
};
```

## Usage with numbers

```csharp
ServiceResponse<int> numericResponse = new ServiceResponse<int>
{
    IsSuccessful = true,
    Payload = 204,
    ErrorText = ""
};
```

Same wrapper, different payload types.

---

# Real-World Example: Generic Cache

```csharp
public class CacheEntry<TKey, TValue>
{
    public TKey Key { get; set; }
    public TValue Value { get; set; }

    public CacheEntry(TKey key, TValue value)
    {
        Key = key;
        Value = value;
    }
}
```

## Usage

```csharp
CacheEntry<string, decimal> priceEntry = new CacheEntry<string, decimal>("Book", 18.75m);
CacheEntry<int, string> userEntry = new CacheEntry<int, string>(901, "Marjan");
```

---

# Generics and Compile-Time Safety

Generics are powerful because errors are caught earlier.

## Example

```csharp
List<int> values = new List<int>();
values.Add(10);
values.Add(20);
```

This is valid.

But this is not:

```csharp
values.Add("oops");
```

The compiler catches the problem immediately.

That is much safer than storing everything as `object`.

---

# Generic Type with Private List

```csharp
public class Bucket<T>
{
    private readonly List<T> _items = new();

    public void Add(T item)
    {
        _items.Add(item);
    }

    public T GetAt(int index)
    {
        return _items[index];
    }

    public int Count => _items.Count;
}
```

## Usage

```csharp
Bucket<string> tags = new Bucket<string>();
tags.Add("backend");
tags.Add("api");

string first = tags.GetAt(0);
int count = tags.Count;
```

---

# Generic Types and Nullable Awareness

In nullable-aware code, be careful with `T`, `T?`, and constraints.

## Example

```csharp
public class OptionalValue<T>
{
    public T? Value { get; set; }
}
```

This can have different meaning depending on whether `T` is:

- reference type
- value type
- constrained or unconstrained

Generic nullability can become advanced, so design carefully.

---

# Common Mistakes

## ❌ Using `object` When a Generic Type Is Better

Bad fit:

```csharp
public class DataHolder
{
    public object Item { get; set; }
}
```

Often better:

```csharp
public class DataHolder<T>
{
    public T Item { get; set; }

    public DataHolder(T item)
    {
        Item = item;
    }
}
```

---

## ❌ Forgetting Constraints

If your generic code assumes `T` has an `Id`, a `Print()` method, or a public constructor, you must declare that requirement with constraints.

---

## ❌ Making Generics Too Complicated

Not every class should be generic.

If the type will only ever work with one specific data type, generics may be unnecessary.

---

# Generic vs Non-Generic Comparison

| Feature | Generic Type | Non-Generic with `object` |
|---|---|---|
| Type safety | Strong | Weak |
| Casting required | Usually no | Often yes |
| Readability | Better | Worse |
| Runtime cast errors | Less likely | More likely |
| Reusability | High | Sometimes awkward |

---

# Mini Example Set

## Generic Wrapper

```csharp
public class Wrapper<T>
{
    public T Data { get; set; }

    public Wrapper(T data)
    {
        Data = data;
    }
}
```

Usage:

```csharp
Wrapper<bool> enabled = new Wrapper<bool>(true);
Wrapper<string> greeting = new Wrapper<string>("Welcome");
```

---

## Generic Pair

```csharp
public class Duo<TFirst, TSecond>
{
    public TFirst First { get; set; }
    public TSecond Second { get; set; }

    public Duo(TFirst first, TSecond second)
    {
        First = first;
        Second = second;
    }
}
```

Usage:

```csharp
Duo<string, decimal> product = new Duo<string, decimal>("Lamp", 39.99m);
```

---

## Generic Queue-Like Type

```csharp
public class Lineup<T>
{
    private readonly Queue<T> _queue = new();

    public void Add(T item)
    {
        _queue.Enqueue(item);
    }

    public T Next()
    {
        return _queue.Dequeue();
    }
}
```

Usage:

```csharp
Lineup<string> callers = new Lineup<string>();
callers.Add("Sara");
callers.Add("Omid");

string current = callers.Next();
```

---

# Syntax Patterns

## Generic Class

```csharp
public class MyType<T>
{
}
```

---

## Multiple Type Parameters

```csharp
public class MyType<TOne, TTwo>
{
}
```

---

## Generic Type with Constraint

```csharp
public class MyType<T> where T : class
{
}
```

---

## Generic Interface

```csharp
public interface IService<T>
{
}
```

---

## Generic Struct

```csharp
public struct Pair<T>
{
}
```

---

# Mental Model

> A generic type is a reusable blueprint where one or more actual data types are supplied later.

For example:

- `List<T>` = a list of some type
- `List<int>` = a list of integers
- `List<string>` = a list of strings

The structure stays the same.

The data type changes.