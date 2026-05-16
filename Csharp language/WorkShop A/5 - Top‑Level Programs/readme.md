# Understanding Top‑Level Programs in .NET

Top-level programs simplify C# console applications by letting you write code **without** the usual `Program` class and `Main` method.  
This feature makes small apps cleaner and reduces boilerplate.

---

# 🆚 Old Style vs. Top‑Level Programs

## Old‑Style Program Structure (pre‑.NET 6 or when disabled)

```csharp
using System;

namespace HelloCS
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }
}
```

This style requires:

- a namespace  
- a `Program` class  
- a `Main` method  
- extra braces and structure  

---

## Top‑Level Program (default in .NET 6+)

```csharp
Console.WriteLine("Hello, World!");
```

No class.  
No `Main`.  
No namespace unless you want to add one for additional types.

---

# 🛠 How It Works Behind the Scenes

When you write top‑level statements, the compiler:

- creates a **hidden `Program` class**,  
- generates a **hidden `<Main>$` method**,  
- wraps your top‑level code inside that method.

> Your simple program is actually transformed into a form similar to the old style—but automatically.

---

# 🆕 A Brief Timeline

### .NET 5  
Introduced top‑level programs as a feature.

### .NET 6  
Console templates **switched to top‑level programs by default**.

### .NET 7+  
Added options to choose between:

- **top‑level statements**, or  
- **traditional Program/Main style**

---

# 🔧 How to Disable Top‑Level Statements

You can choose the older, explicit style.

## Visual Studio  
Check:  
**Do not use top‑level statements**

## .NET CLI  
Use the switch:

```bash
dotnet new console --use-program-main
```

Creates a Program.cs with the full class and Main method.

---

# ⚠️ Important Warning About Namespaces

The auto‑generated `Program` class:

- has **no namespace**,  
- lives in the **global namespace**,  
- does **not** match your project name.

This can matter if you expect `Program` to be in `MyProjectName`.

---

# 📌 Requirements and Rules for Top‑Level Programs

### 1. **Only One Top‑Level File**
- A project can contain only **one** file with top‑level statements.

### 2. **`using` Statements Must Be at the Top**
Example:

```csharp
using System;
using MyLibrary;
```

### 3. **Custom Types Must Be at the Bottom**
Top‑level code goes first.  
Classes, records, interfaces, etc., must come after.

Example:

```csharp
Console.WriteLine("Hello!");

class Helper
{
    public static void SayHi() => Console.WriteLine("Hi!");
}
```

### 4. **Compiler‑Generated Main Method**
If you use top‑level statements, the compiler creates a method named:

```text
<Main>$
```

If you explicitly define your own `Main`, name it normally:

```csharp
static void Main() { }
```

---

# 📂 Top‑Level Program Structure Example

```csharp
using System;

Console.WriteLine("App Starting...");

DoSomething();

class Utilities
{
    public static void DoSomething() =>
        Console.WriteLine("Did something!");
}
```

Top → `using` statements  
Middle → top‑level code  
Bottom → custom types