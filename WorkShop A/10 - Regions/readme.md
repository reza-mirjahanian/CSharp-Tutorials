# 🧩 Regions, Statements, Blocks, and Brace Styles in C#

---

# 1. `#region` and `#endregion`

## What Is a Region?

A **region** is a labeled section of code that can be **collapsed** or **expanded** in many code editors.

Regions are created using:

```csharp
#region Region name

// Code goes here

#endregion
```

They do **not** affect how the program runs. They are only used to help organize code visually.

---

## Example

```csharp
#region Three variables that store the number 2 million.

int decimalNotation = 2_000_000;
int binaryNotation = 0b_0001_1110_1000_0100_1000_0000;
int hexadecimalNotation = 0x_001E_8480;

#endregion
```

---

## What This Code Does

The region groups three variables that all store the same value:

```csharp
2_000_000
```

but in different number formats.

| Variable | Number Format | Value |
|---|---:|---:|
| `decimalNotation` | Decimal | `2_000_000` |
| `binaryNotation` | Binary | `2,000,000` |
| `hexadecimalNotation` | Hexadecimal | `2,000,000` |

---

## Number Formats in the Example

### Decimal notation

```csharp
int decimalNotation = 2_000_000;
```

This is the usual base-10 format.

The underscores make the number easier to read:

```csharp
2_000_000
```

is the same as:

```csharp
2000000
```

---

### Binary notation

```csharp
int binaryNotation = 0b_0001_1110_1000_0100_1000_0000;
```

Binary numbers start with:

```csharp
0b
```

Binary uses only:

```text
0 and 1
```

---

### Hexadecimal notation

```csharp
int hexadecimalNotation = 0x_001E_8480;
```

Hexadecimal numbers start with:

```csharp
0x
```

Hexadecimal uses:

```text
0 1 2 3 4 5 6 7 8 9 A B C D E F
```

---

# 2. Why Use Regions?

Regions are useful when you want to group related code together.

For example:

```csharp
#region Input validation

if (string.IsNullOrWhiteSpace(name))
{
  Console.WriteLine("Name is required.");
}

if (age < 0)
{
  Console.WriteLine("Age cannot be negative.");
}

#endregion
```

---

## Regions as Collapsible Commented Blocks

A region works like a **commented label** around a block of code.

For example:

```csharp
#region Calculate final price

decimal subtotal = 100m;
decimal tax = subtotal * 0.2m;
decimal total = subtotal + tax;

Console.WriteLine(total);

#endregion
```

In an editor, this can usually be collapsed to something like:

```csharp
#region Calculate final price
```

This makes large files easier to navigate.

---

# 3. When Regions Are Helpful

## ✅ Good Uses

Regions can be helpful for:

- Grouping related code in learning examples
- Organizing long files temporarily
- Separating setup code from main logic
- Making demonstration code easier to read
- Grouping fields, constructors, methods, or properties

Example:

```csharp
#region Fields

private string name;
private int age;

#endregion

#region Constructors

public Person(string name, int age)
{
  this.name = name;
  this.age = age;
}

#endregion

#region Methods

public void SayHello()
{
  Console.WriteLine($"Hello, my name is {name}.");
}

#endregion
```

---

## ⚠️ Use Regions Carefully

Regions can sometimes hide messy code instead of improving it.

If a file needs many regions, it may be a sign that the code should be split into:

- Smaller methods
- Smaller classes
- Separate files
- Better-organized types

---

## Practical Advice

> Use regions when they make code easier to navigate, but do not use them to hide poorly organized code.

---

# 4. Statements in C#

## What Is a Statement?

A **statement** is a single instruction in C#.

Most statements end with a semicolon:

```csharp
;
```

---

## Example

```csharp
Console.WriteLine("Hello World!");
```

This is a statement.

It tells the program to print:

```text
Hello World!
```

---

## Semicolon Indicates the End of a Statement

```csharp
using System;
```

The semicolon tells C#:

> This instruction is finished.

---

## More Statement Examples

```csharp
int age = 30;
```

```csharp
string name = "Alice";
```

```csharp
Console.WriteLine(name);
```

```csharp
age++;
```

Each one is a separate statement.

---

# 5. Blocks in C#

## What Is a Block?

A **block** is a group of code surrounded by curly braces:

```csharp
{
  // Code goes here
}
```

---

## Opening and Closing Braces

| Symbol | Meaning |
|---|---|
| `{` | Starts a block |
| `}` | Ends a block |

---

## Example Block

```csharp
{
  Console.WriteLine("Inside a block");
}
```

---

## Blocks Usually Belong To Something

Blocks are commonly used with:

- Namespaces
- Classes
- Methods
- `if` statements
- Loops
- `try/catch` statements

---

# 6. Example: Statements and Blocks in a Console App

```csharp
using System; // A semicolon indicates the end of a statement.

namespace Basics
{ // An open brace indicates the start of a block.

  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Hello World!"); // A statement.
    }
  }

} // A close brace indicates the end of a block.
```

---

# 7. Understanding the Example

## `using` Statement

```csharp
using System;
```

This imports the `System` namespace so the program can use classes like:

```csharp
Console
```

The statement ends with:

```csharp
;
```

---

## Namespace Block

```csharp
namespace Basics
{
  ...
}
```

The namespace groups related code under the name:

```csharp
Basics
```

The code inside the braces belongs to that namespace.

---

## Class Block

```csharp
class Program
{
  ...
}
```

This defines a class named:

```csharp
Program
```

The class body is inside the braces.

---

## Method Block

```csharp
static void Main(string[] args)
{
  ...
}
```

This defines the `Main` method.

In older-style console apps, `Main` is the entry point of the program.

That means the program starts running here.

---

## Statement Inside the Method

```csharp
Console.WriteLine("Hello World!");
```

This statement prints text to the console.

---

# 8. Nested Blocks

Blocks can be placed inside other blocks.

Example:

```csharp
namespace Basics
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Hello World!");
    }
  }
}
```

This has multiple levels:

1. `namespace Basics`
   1. `class Program`
      1. `Main` method
         1. `Console.WriteLine` statement

---

## Visual Structure

```text
namespace
└── class
    └── method
        └── statement
```

---

# 9. C# Brace Style

C# commonly uses a brace style where the opening brace and closing brace are placed on their own lines.

## Common C# Style

```csharp
if (x < 3)
{
  // Do something if x is less than 3.
}
```

---

## Key Features of This Style

- The opening brace `{` is on its own line.
- The closing brace `}` is on its own line.
- The braces line up vertically.
- The code inside the block is indented.

---

## Example with a Method

```csharp
static void SayHello()
{
  Console.WriteLine("Hello!");
}
```

---

## Example with a Class

```csharp
class Person
{
  public string Name { get; set; }
}
```

---

# 10. JavaScript-Style Braces

Some languages, such as JavaScript, often place the opening brace at the end of the first line.

```csharp
if (x < 3) {
  // Do something if x is less than 3.
}
```

This style also works in C#.

---

## Comparison

| Style | Example |
|---|---|
| Common C# style | `if (x < 3)` followed by `{` on the next line |
| JavaScript-style | `if (x < 3) {` on the same line |

---

## Common C# Style

```csharp
if (x < 3)
{
  Console.WriteLine("x is less than 3.");
}
```

---

## JavaScript-Style Formatting

```csharp
if (x < 3) {
  Console.WriteLine("x is less than 3.");
}
```

---

# 11. Does the Compiler Care About Brace Style?

No.

The C# compiler does **not** care whether you write:

```csharp
if (x < 3)
{
  Console.WriteLine("Small");
}
```

or:

```csharp
if (x < 3) {
  Console.WriteLine("Small");
}
```

Both are valid C#.

---

## What the Compiler Cares About

The compiler cares about:

- Correct syntax
- Matching braces
- Semicolons where required
- Valid statements
- Valid types and names

---

## What the Compiler Does Not Care About

The compiler usually does not care about:

- Blank lines
- Indentation
- Whether `{` is on the same line or next line
- Extra spaces between tokens

---

# 12. Readability Matters

Although the compiler does not care about formatting, humans do.

Good formatting makes code easier to:

- Read
- Understand
- Debug
- Maintain
- Review

---

## Less Readable

```csharp
if(x<3){Console.WriteLine("Small");}
```

This works, but it is harder to read.

---

## More Readable

```csharp
if (x < 3)
{
  Console.WriteLine("Small");
}
```

This is easier to understand.

---

# 13. Recommended Formatting Habits

## ✅ Use Consistent Brace Style

Choose one style and use it consistently.

For C#, the common convention is:

```csharp
if (condition)
{
  // Code
}
```

---

## ✅ Indent Code Inside Blocks

```csharp
if (isValid)
{
  Console.WriteLine("Valid");
}
```

Avoid:

```csharp
if (isValid)
{
Console.WriteLine("Valid");
}
```

---

## ✅ Keep Related Code Together

```csharp
#region Customer details

string firstName = "Sara";
string lastName = "Adams";
int age = 32;

#endregion
```

---

## ✅ Avoid Overusing Regions

Instead of this:

```csharp
#region Very long calculation

// hundreds of lines of code

#endregion
```

Prefer moving logic into a method:

```csharp
CalculateTotal();
```

Then define:

```csharp
static decimal CalculateTotal()
{
  // calculation code
}
```

Methods naturally create named, reusable blocks of code.

---

# 14. Regions vs Methods

## Region

A region only organizes code visually.

```csharp
#region Print greeting

Console.WriteLine("Hello");
Console.WriteLine("Welcome");

#endregion
```

The region does not create reusable behavior.

---

## Method

A method creates reusable behavior.

```csharp
static void PrintGreeting()
{
  Console.WriteLine("Hello");
  Console.WriteLine("Welcome");
}
```

You can call it multiple times:

```csharp
PrintGreeting();
PrintGreeting();
PrintGreeting();
```

---

## Comparison

| Feature | Region | Method |
|---|---:|---:|
| Collapsible in editor | ✅ | ✅ |
| Has a name | ✅ | ✅ |
| Can be reused | ❌ | ✅ |
| Improves program structure | Limited | ✅ |
| Affects runtime behavior | ❌ | ✅ |
| Helps organize long code | ✅ | ✅ |

---

# 15. Practical Examples

## Example 1: Region Around Variables

```csharp
#region User information

string username = "admin";
string email = "admin@example.com";
bool isActive = true;

#endregion
```

---

## Example 2: Region Around Setup Code

```csharp
#region Console setup

Console.Title = "My Application";
Console.ForegroundColor = ConsoleColor.Green;
Console.Clear();

#endregion
```

---

## Example 3: Blocks Inside an `if` Statement

```csharp
int score = 85;

if (score >= 50)
{
  Console.WriteLine("Passed");
}
else
{
  Console.WriteLine("Failed");
}
```

The `if` block runs when the condition is true.

The `else` block runs when the condition is false.

---

## Example 4: Blocks Inside a Loop

```csharp
for (int i = 1; i <= 3; i++)
{
  Console.WriteLine(i);
}
```

The block runs once for each loop iteration.

Output:

```text
1
2
3
```

---

# 16. Important Terms

| Term | Meaning | Example |
|---|---|---|
| Statement | A single instruction | `Console.WriteLine("Hi");` |
| Semicolon | Ends many statements | `;` |
| Block | A group of statements | `{ ... }` |
| Opening brace | Starts a block | `{` |
| Closing brace | Ends a block | `}` |
| Region | A collapsible labeled code section | `#region Name` |
| Indentation | Spaces used to show nesting | Code inside `{ }` is indented |

---

# 17. Clean Example Combining Everything

```csharp
using System;

namespace Basics
{
  class Program
  {
    static void Main(string[] args)
    {
      #region Greeting message

      Console.WriteLine("Hello World!");

      #endregion

      int x = 2;

      if (x < 3)
      {
        Console.WriteLine("x is less than 3.");
      }
    }
  }
}
```

---

## What This Example Contains

| Part | Code |
|---|---|
| `using` statement | `using System;` |
| Namespace block | `namespace Basics { ... }` |
| Class block | `class Program { ... }` |
| Method block | `static void Main(...) { ... }` |
| Region | `#region Greeting message ... #endregion` |
| Statement | `Console.WriteLine("Hello World!");` |
| `if` block | `if (x < 3) { ... }` |