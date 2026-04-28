# Implicit and Global Namespace Imports in C#

## Why `using` statements exist

In C#, a file often needs access to types from namespaces like `System`, `System.Collections.Generic`, or `System.Linq`.

Traditionally, each `.cs` file had to start with its own `using` statements:

```csharp
using System;
using System.Collections.Generic;
using System.Linq;
```

Without these imports, you would need to write fully qualified names:

```csharp
System.Console.WriteLine("Hello");
```

---

# Implicit Global Usings

Starting with **.NET 6** and **C# 10**, projects can automatically import a set of common namespaces for you.

This means:

- you write fewer `using` statements
- many common namespaces are available in all `.cs` files
- the compiler generates these imports behind the scenes

## Where they come from

When your project targets **.NET 6 or later**, the build process generates a file like:

```text
<ProjectName>.GlobalUsings.g.cs
```

You can typically find it in a folder like:

```text
obj\Debug\net10.0
```

This generated file contains **global using directives** that apply to the whole project.

> A **global using** makes a namespace available in every C# file in the project.

---

# What “implicit” means

**Implicit usings** are namespace imports added automatically by the SDK based on the kind of project you are building.

Examples of commonly imported namespaces include:

- `System`
- `System.Collections.Generic`
- `System.Linq`

The exact list depends on the **SDK** your project uses.

## SDK matters

Different project types bring different default namespaces.

For example:

- a **console app** may get one set of implicit usings
- a **web app** may get additional ASP.NET-related namespaces
- a **class library** may get a smaller set

So the actual implicit imports are **not identical for every project**.

---

# Global Usings vs Normal Usings

## Normal `using`

A normal `using` only affects the current file:

```csharp
using System.Text;
```

This works only in the file where it appears.

## Global `using`

A global `using` affects the entire project:

```csharp
global using System.Text;
```

Now every `.cs` file in the project can use `System.Text` types.

---

# Customizing Implicit and Global Usings

You can control these imports in your project file (`.csproj`) by adding entries inside an `ItemGroup`.

## Example

```xml
<ItemGroup>
  <Using Remove="System.Threading" />
  <Using Include="System.Numerics" />
  <Using Include="System.Console" Static="true" />
  <Using Include="System.Environment" Alias="Env" />
</ItemGroup>
```

---

# Understanding Each Line

## 1. Remove an implicitly imported namespace

```xml
<Using Remove="System.Threading" />
```

This removes `System.Threading` from the set of global/implicit imports.

### Use this when:

- you do not want that namespace available everywhere
- you want to reduce ambiguity
- you want tighter control over imported namespaces

---

## 2. Add a namespace globally

```xml
<Using Include="System.Numerics" />
```

This globally imports `System.Numerics`.

Now types like `BigInteger` can be used in any file without adding a local `using`.

### Example

```csharp
BigInteger big = BigInteger.Parse("12345678901234567890");
```

---

## 3. Add a static global using

```xml
<Using Include="System.Console" Static="true" />
```

This acts like:

```csharp
global using static System.Console;
```

It lets you call static members directly without writing `Console.`

### Example

Instead of:

```csharp
Console.WriteLine("Hello");
```

you can write:

```csharp
WriteLine("Hello");
```

> `Static="true"` is for importing **static members** of a type.

---

## 4. Add an alias

```xml
<Using Include="System.Environment" Alias="Env" />
```

This creates an alias named `Env` for `System.Environment`.

It behaves like:

```csharp
global using Env = System.Environment;
```

### Example

```csharp
string path = Env.CurrentDirectory;
```

This is useful when:

- a type name is long
- you want clearer code
- you need to avoid naming conflicts

---

# Equivalent C# Forms

The XML entries in the project file correspond to these C# forms:

| Project File Entry | Equivalent C# |
|---|---|
| `<Using Include="System.Numerics" />` | `global using System.Numerics;` |
| `<Using Include="System.Console" Static="true" />` | `global using static System.Console;` |
| `<Using Include="System.Environment" Alias="Env" />` | `global using Env = System.Environment;` |
| `<Using Remove="System.Threading" />` | Removes that generated global using |

---

# `ItemGroup` vs `ImportGroup`

These are **not the same**.

## `ItemGroup`

`ItemGroup` is used to define items in the project, including `Using` entries.

Example:

```xml
<ItemGroup>
  <Using Include="System.Numerics" />
</ItemGroup>
```

## `ImportGroup`

`ImportGroup` is used for grouping imported MSBuild files, not for defining `Using` items.

So for global/implicit namespace configuration, use:

```xml
<ItemGroup>
```

**not**

```xml
<ImportGroup>
```

> If you are adding or removing namespace imports in a `.csproj` file, `ItemGroup` is the correct place.

---

# Practical Mental Model

Think of namespace imports at three levels:

1. **Per file**
   - regular `using`
   - only affects one `.cs` file

2. **Per project**
   - `global using`
   - affects all `.cs` files

3. **Automatically added by the SDK**
   - implicit usings
   - generated for you based on project type

---

# Example: Before and After

## Before implicit/global usings

Every file might need this:

```csharp
using System;
using System.Collections.Generic;
using System.Linq;

namespace DemoApp;

public class Program
{
    public static void Main()
    {
        Console.WriteLine("Hello");
    }
}
```

## After implicit/global usings

The file can become much cleaner:

```csharp
namespace DemoApp;

public class Program
{
    public static void Main()
    {
        WriteLine("Hello");
    }
}
```

This works if:

- `System` is implicitly imported
- `System.Console` is globally imported as a static using

---

# When this is especially useful

## ✅ Good use cases

- projects with many files
- commonly used namespaces across the whole app
- reducing repetitive boilerplate
- improving consistency in team projects

## ⚠️ Be careful with

- importing too many namespaces globally
- static imports that hide where methods come from
- aliases that are unclear or too short

---

# Quick Reference

## Common forms

### File-only import

```csharp
using System.Text;
```

### Project-wide import

```csharp
global using System.Text;
```

### Project-wide static import

```csharp
global using static System.Console;
```

### Project-wide alias

```csharp
global using Env = System.Environment;
```

---

# Project File Pattern

```xml
<ItemGroup>
  <Using Remove="Some.Namespace" />
  <Using Include="Some.Namespace" />
  <Using Include="Some.Type" Static="true" />
  <Using Include="Some.TypeOrNamespace" Alias="ShortName" />
</ItemGroup>
```

---

# Key Terms

| Term | Meaning |
|---|---|
| `using` | Imports a namespace or type for one file |
| `global using` | Imports a namespace or type for the entire project |
| implicit usings | Global imports automatically generated by the SDK |
| `Static="true"` | Imports static members of a type |
| `Alias="Name"` | Creates a shorter or alternate name |
| `Remove="..."` | Removes an implicit/global using |
| `ItemGroup` | The correct MSBuild group for `Using` items |

---

# Mini Examples

## Global namespace import

```xml
<Using Include="System.Text.Json" />
```

Use anywhere:

```csharp
JsonSerializer.Serialize(new { Name = "Ava" });
```

## Global static import

```xml
<Using Include="System.Math" Static="true" />
```

Use anywhere:

```csharp
double x = Sqrt(16);
```

## Global alias

```xml
<Using Include="System.ConsoleColor" Alias="CC" />
```

Use anywhere:

```csharp
CC color = CC.Green;
```