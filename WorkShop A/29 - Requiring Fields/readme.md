# C#: Requiring Fields During Object Creation

When you create an object, some data is often *essential* from the very beginning.

For example, if you create a `Book`, you may want every `Book` to always have:

- a title
- an author
- a page count

A good way to enforce this is by using a **constructor**.

---

## What Is a Constructor?

A **constructor** is a special method that runs automatically when an object is created.

It is used to:

- set up required values
- initialize fields or properties
- make sure the object starts in a valid state

### Example

```csharp
public class Book
{
    private string _title;
    private string _author;

    public Book(string title, string author)
    {
        _title = title;
        _author = author;
    }
}
```

### Creating an Object

```csharp
Book novel = new Book("The Silent Forest", "Mina Hart");
```

Here:

- `"The Silent Forest"` is passed into `title`
- `"Mina Hart"` is passed into `author`
- the constructor stores those values in the object's fields

---

# Initializing Fields with Constructors

Constructors are commonly used to assign incoming values to fields.

## Basic Pattern

```csharp
public class Vehicle
{
    private string _brand;
    private int _yearBuilt;

    public Vehicle(string brand, int yearBuilt)
    {
        _brand = brand;
        _yearBuilt = yearBuilt;
    }
}
```

### Instantiation

```csharp
Vehicle car = new Vehicle("Nordic Motors", 2024);
```

This guarantees that every `Vehicle` object starts with both:

- a brand
- a production year

---

## Why This Matters

Without a constructor, an object might be created without meaningful data.

### Less Safe Design

```csharp
public class Vehicle
{
    private string _brand;
    private int _yearBuilt;
}
```

Now someone could write:

```csharp
Vehicle car = new Vehicle();
```

That may leave fields with default values such as:

- `null` for strings
- `0` for integers

> Constructors help prevent incomplete or invalid objects.

---

# Requiring Fields to Be Set

If a constructor requires parameters, the caller must provide them.

## Example: Required Data

```csharp
public class Student
{
    private string _fullName;
    private int _gradeLevel;

    public Student(string fullName, int gradeLevel)
    {
        _fullName = fullName;
        _gradeLevel = gradeLevel;
    }
}
```

### Valid Usage

```csharp
Student learner = new Student("Aria Nolan", 11);
```

### Invalid Usage

```csharp
Student learner = new Student();
```

The second example fails because the class does **not** define a parameterless constructor.

That means C# requires the caller to provide:

1. `fullName`
2. `gradeLevel`

---

## Real Benefit

This design makes your classes safer because:

- objects are fully initialized immediately
- required information cannot be forgotten
- bugs caused by missing values are reduced

---

# Defining Multiple Constructors

A class can have **more than one constructor**.

This is useful when you want to support different ways of creating the same object.

This is called **constructor overloading**.

---

## Example: One Class, Different Creation Options

```csharp
public class Rectangle
{
    private int _width;
    private int _height;

    public Rectangle(int width, int height)
    {
        _width = width;
        _height = height;
    }

    public Rectangle(int size)
    {
        _width = size;
        _height = size;
    }
}
```

### How It Works

You can now create rectangles in two ways:

#### 1. Width and height separately

```csharp
Rectangle panel = new Rectangle(12, 7);
```

#### 2. One value for a square

```csharp
Rectangle tile = new Rectangle(6);
```

---

## Why Multiple Constructors Are Useful

They let you:

- offer flexibility to the user of your class
- keep object creation easy
- support common shortcuts

---

# Example with Default-Like Behavior

Sometimes one constructor provides full detail, and another provides a simpler option.

```csharp
public class Account
{
    private string _ownerName;
    private decimal _balance;

    public Account(string ownerName, decimal balance)
    {
        _ownerName = ownerName;
        _balance = balance;
    }

    public Account(string ownerName)
    {
        _ownerName = ownerName;
        _balance = 50.00m;
    }
}
```

### Usage

```csharp
Account first = new Account("Jordan Lee", 275.50m);
Account second = new Account("Sam Rivera");
```

In this design:

- the first account starts with a custom balance
- the second account starts with a default balance of `50.00m`

---

# Constructor Rules to Remember

## Key Points

- A constructor has the **same name** as the class.
- A constructor does **not** have a return type.
- Constructors can take parameters.
- If you define your own constructor, C# does **not** automatically provide a parameterless one.
- You can define multiple constructors as long as their parameter lists are different.

---

# A More Complete Example

```csharp
public class Movie
{
    private string _name;
    private string _director;
    private int _durationMinutes;

    public Movie(string name, string director, int durationMinutes)
    {
        _name = name;
        _director = director;
        _durationMinutes = durationMinutes;
    }

    public Movie(string name, string director)
    {
        _name = name;
        _director = director;
        _durationMinutes = 100;
    }
}
```

## Object Creation Options

```csharp
Movie firstFilm = new Movie("Shadow Signal", "Lena Brooks", 128);
Movie secondFilm = new Movie("Glass Harbor", "Evan Cole");
```

### Result

| Object | Name | Director | Duration |
|---|---|---|---:|
| `firstFilm` | `Shadow Signal` | `Lena Brooks` | `128` |
| `secondFilm` | `Glass Harbor` | `Evan Cole` | `100` |

---

# Constructor Parameters vs Fields

It is important to understand the difference between:

- **parameters**: temporary input values passed into the constructor
- **fields**: variables stored inside the object

## Example

```csharp
public class Laptop
{
    private string _model;

    public Laptop(string model)
    {
        _model = model;
    }
}
```

### In This Example

- `model` → constructor parameter
- `_model` → field inside the object

The constructor copies the parameter value into the field.

---

# A Cleaner Mental Model

Think of object creation like filling out a required form:

- the **constructor parameters** are the form inputs
- the **fields** are where the object stores that information permanently

If the form requires certain blanks to be filled in, the object cannot be created until they are provided.

---

# Common Pattern

## Requiring essential values

```csharp
public class Employee
{
    private string _employeeName;
    private int _employeeId;

    public Employee(string employeeName, int employeeId)
    {
        _employeeName = employeeName;
        _employeeId = employeeId;
    }
}
```

## Allowing multiple creation styles

```csharp
public class Employee
{
    private string _employeeName;
    private int _employeeId;
    private string _department;

    public Employee(string employeeName, int employeeId, string department)
    {
        _employeeName = employeeName;
        _employeeId = employeeId;
        _department = department;
    }

    public Employee(string employeeName, int employeeId)
    {
        _employeeName = employeeName;
        _employeeId = employeeId;
        _department = "General Operations";
    }
}
```

### Usage

```csharp
Employee one = new Employee("Nora Patel", 1042, "Engineering");
Employee two = new Employee("Leo Grant", 1043);
```

---

# Quick Reference Table

| Concept | Meaning | Example |
|---|---|---|
| **Constructor** | Runs when an object is created | `public Book(string title)` |
| **Required fields** | Values that must be supplied at creation | `new Student("Aria Nolan", 11)` |
| **Field initialization** | Assigning constructor inputs to fields | `_title = title;` |
| **Multiple constructors** | Different ways to create the same class | `Rectangle(int width, int height)` and `Rectangle(int size)` |

---

# Practical Tips

## ✅ Good uses of constructors

- requiring important values
- setting safe starting data
- supporting multiple initialization options

## ⚠️ Be careful about

- creating too many constructor overloads
- allowing objects to start with meaningless defaults
- forgetting that a custom constructor removes the automatic empty constructor

---

# Example: Full Demonstration

```csharp
public class Playlist
{
    private string _name;
    private string _creator;
    private int _trackCount;

    public Playlist(string name, string creator, int trackCount)
    {
        _name = name;
        _creator = creator;
        _trackCount = trackCount;
    }

    public Playlist(string name, string creator)
    {
        _name = name;
        _creator = creator;
        _trackCount = 8;
    }
}
```

## Creating Objects

```csharp
Playlist roadTrip = new Playlist("Road Trip Mix", "Dina Shaw", 24);
Playlist starterList = new Playlist("Fresh Picks", "Owen Park");
```

## What Each Constructor Does

- `Playlist(string name, string creator, int trackCount)`
  - requires all details
- `Playlist(string name, string creator)`
  - fills in `trackCount` automatically with `8`

---

# One Important Detail

If you want both:

- a constructor that requires values
- *and*
- a constructor with no parameters

you must define both explicitly.

## Example

```csharp
public class Device
{
    private string _category;
    private int _version;

    public Device()
    {
        _category = "Unknown";
        _version = 1;
    }

    public Device(string category, int version)
    {
        _category = category;
        _version = version;
    }
}
```

### Usage

```csharp
Device a = new Device();
Device b = new Device("Tablet", 3);
```