# 🔐 C# Access Modifiers

Access modifiers control **where a type or member can be used**.

They answer questions like:

- Who can see this class?
- Which code can call this method?
- Can derived classes access this member?
- Is it visible only inside this assembly?

In C#, there are **seven** access modifier keywords or keyword combinations used for **types or members**:

1. `public`
2. `private`
3. `protected`
4. `internal`
5. `protected internal`
6. `private protected`
7. `file`

---

# 1. `public`

## Meaning

`public` means the type or member is accessible **from anywhere** that can reference it.

- same class
- derived class
- same assembly
- other assemblies

## Example

```csharp
public class ReportPrinter
{
    public void Print()
    {
        Console.WriteLine("Printing report...");
    }
}
```

```csharp
var printer = new ReportPrinter();
printer.Print();
```

## Use it for

- API surfaces
- classes meant to be used by other projects
- methods/properties intentionally exposed to callers

---

# 2. `private`

## Meaning

`private` means accessible **only inside the containing type**.

This is the **most restrictive** common modifier for members.

## Example

```csharp
public class BankAccount
{
    private decimal _balance = 250m;

    private void ApplyMonthlyFee()
    {
        _balance -= 5m;
    }
}
```

Outside code cannot do this:

```csharp
var account = new BankAccount();
// account._balance = 0;         // Not allowed
// account.ApplyMonthlyFee();    // Not allowed
```

## Use it for

- internal implementation details
- helper methods
- backing fields
- logic you do not want external code to depend on

> `private` is for hiding details inside the type itself.

---

# 3. `protected`

## Meaning

`protected` means accessible:

- inside the containing class
- inside types derived from that class

But **not** from unrelated outside code.

## Example

```csharp
public class Appliance
{
    protected void StartMotor()
    {
        Console.WriteLine("Motor started");
    }
}

public class WashingMachine : Appliance
{
    public void RunCycle()
    {
        StartMotor(); // Allowed
    }
}
```

Outside code still cannot call it directly:

```csharp
var machine = new WashingMachine();
// machine.StartMotor(); // Not allowed
```

## Use it for

- base class members intended for subclasses
- extensibility points in inheritance hierarchies

---

# 4. `internal`

## Meaning

`internal` means accessible **only within the same assembly**.

An assembly is usually a compiled `.dll` or `.exe`.

So if code is in the same project/assembly, it can access the member or type.  
If it is in a different assembly, it cannot.

## Example

```csharp
internal class CacheStore
{
    internal void ClearAll()
    {
        Console.WriteLine("Cache cleared");
    }
}
```

This is accessible anywhere inside the same assembly, but not from another project referencing it.

## Use it for

- implementation types that should stay inside the project
- helper classes not meant for external consumers
- internal architecture pieces

---

# 5. `protected internal`

## Meaning

`protected internal` means accessible by either of these:

- any code in the **same assembly**
- **derived types** in any assembly

Think of it as:

> **protected OR internal**

## Example

```csharp
public class DocumentBase
{
    protected internal void MarkDirty()
    {
        Console.WriteLine("Document marked as modified");
    }
}
```

This method can be used by:

- any class in the same assembly
- subclasses even in another assembly

## Use it for

- members meant for inheritance
- members also allowed broadly inside the current assembly

---

# 6. `private protected`

## Meaning

`private protected` means accessible only by:

- the containing class
- derived classes **in the same assembly**

Think of it as:

> **protected AND internal**

It is more restrictive than `protected internal`.

## Example

```csharp
public class DeviceBase
{
    private protected void ResetCore()
    {
        Console.WriteLine("Core reset");
    }
}

public class RouterDevice : DeviceBase
{
    public void Restart()
    {
        ResetCore(); // Allowed if in same assembly
    }
}
```

But a derived type in another assembly cannot access it.

## Use it for

- inheritance-based access that must stay inside the assembly
- framework internals where subclass access should not leak across assemblies

---

# 7. `file`

## Meaning

`file` means the type is accessible **only within the same source file**.

This modifier applies to **types**, not normal members like methods or properties.

It was added to support file-local helper types.

## Example

```csharp
file class CsvRowParser
{
    public string[] Parse(string line) => line.Split(',');
}
```

Only code in that same `.cs` file can use `CsvRowParser`.

Code in another file in the same project cannot access it.

## Use it for

- tiny helper types
- implementation details that should stay local to one file
- avoiding name collisions in large projects

---

# 📊 Quick comparison table

| Modifier | Same Type | Derived Type | Same Assembly | Other Assembly | Notes |
|---|---:|---:|---:|---:|---|
| `public` | ✅ | ✅ | ✅ | ✅ | Accessible everywhere |
| `private` | ✅ | ❌ | ❌ | ❌ | Only inside containing type |
| `protected` | ✅ | ✅ | Only through inheritance | Only through inheritance | For subclasses |
| `internal` | ✅ | ✅* | ✅ | ❌ | Assembly-only |
| `protected internal` | ✅ | ✅ | ✅ | ✅ through inheritance | `protected OR internal` |
| `private protected` | ✅ | ✅ in same assembly | Limited | ❌ | `protected AND internal` |
| `file` | file-local type only | file-local | file-local | ❌ | Only for types |

> `*` A derived type in the same assembly can access an `internal` member because it is in the same assembly, not because it is derived.

---

# 🧩 Type-level vs member-level usage

Not all modifiers can be used everywhere.

## Commonly used on members

- `public`
- `private`
- `protected`
- `internal`
- `protected internal`
- `private protected`

Examples of members:

- fields
- methods
- properties
- events
- nested types

## Commonly used on types

- `public`
- `internal`
- `file`

For top-level types:

- `public`
- `internal`
- `file`

You cannot normally declare a top-level class as:

- `private`
- `protected`
- `private protected`
- `protected internal`

Those are for members and nested types, not ordinary top-level types.

---

# 🏗️ Nested type example

A nested type is a type declared inside another type.

```csharp
public class Engine
{
    private class InternalState
    {
    }
}
```

Here, `InternalState` is accessible only inside `Engine`.

Nested types can use modifiers like:

- `private`
- `protected`
- `public`
- `internal`
- `protected internal`
- `private protected`

---

# 🧠 Easy way to remember

## Broadest to most restrictive

A useful mental order is:

1. `public`
2. `protected internal`
3. `internal`
4. `protected`
5. `private protected`
6. `private`

`file` is a special case because it is **file-scoped for types**, not assembly-scoped or inheritance-scoped in the usual sense.

---

# 🔍 `protected internal` vs `private protected`

These two are often confused.

## `protected internal`

Accessible if **either** condition is true:

- same assembly
- derived class

### Think:

> wider access

```csharp
protected internal void SyncState() { }
```

---

## `private protected`

Accessible only if **both ideas are satisfied in practice**:

- derived class
- same assembly

### Think:

> narrower access

```csharp
private protected void SyncState() { }
```

---

# 🧪 Small comparison example

```csharp
public class BaseService
{
    protected internal void OpenWide() { }
    private protected void OpenNarrow() { }
}
```

## Who can access `OpenWide()`?

- classes in same assembly ✅
- derived classes in same assembly ✅
- derived classes in another assembly ✅

## Who can access `OpenNarrow()`?

- containing class ✅
- derived classes in same assembly ✅
- non-derived classes in same assembly ❌
- derived classes in another assembly ❌

---

# 📌 Defaults to remember

If you omit an access modifier, defaults apply.

## Top-level types

Default is:

```csharp
internal
```

Example:

```csharp
class UtilityTool
{
}
```

This is the same as:

```csharp
internal class UtilityTool
{
}
```

## Members of a class

Default is:

```csharp
private
```

Example:

```csharp
public class Sample
{
    void Execute()
    {
    }
}
```

This is the same as:

```csharp
public class Sample
{
    private void Execute()
    {
    }
}
```

---

# 🛠️ Practical examples

## `public` API member

```csharp
public class MailSender
{
    public void Send(string recipient)
    {
        Console.WriteLine($"Sending mail to {recipient}");
    }
}
```

## `private` helper

```csharp
public class MailSender
{
    public void Send(string recipient)
    {
        Validate(recipient);
        Console.WriteLine($"Sending mail to {recipient}");
    }

    private void Validate(string recipient)
    {
        if (string.IsNullOrWhiteSpace(recipient))
            throw new ArgumentException("Recipient is required.");
    }
}
```

## `internal` support type

```csharp
internal class RetryPolicy
{
    public int MaxAttempts { get; set; } = 3;
}
```

## `protected` extensibility hook

```csharp
public class Importer
{
    public void Run()
    {
        BeforeRun();
        Console.WriteLine("Importing...");
    }

    protected virtual void BeforeRun()
    {
    }
}
```

---

# 🚫 Common mistakes

## 1. Thinking `protected` means “same assembly”

It does **not**.

`protected` is about **inheritance**, not project boundaries.

---

## 2. Thinking `internal` means “same namespace”

It does **not**.

`internal` means **same assembly**, even across different namespaces.

---

## 3. Confusing `protected internal` with `private protected`

- `protected internal` = broader
- `private protected` = narrower

---

## 4. Using `public` too often

Not everything should be exposed.

A good design often starts with the **most restrictive** access possible, then widens only if needed.

---

# 🗂️ Rule of thumb

| If you want... | Use |
|---|---|
| Everyone to access it | `public` |
| Only this type to access it | `private` |
| This type and subclasses | `protected` |
| Only this assembly | `internal` |
| Same assembly or any subclass | `protected internal` |
| Only subclasses in this assembly | `private protected` |
| Type usable only in this file | `file` |

---

# 🧪 Tiny example showing several together

```csharp
file class FileHelper
{
    public static string Normalize(string text) => text.Trim();
}

public class Processor
{
    private int _count;

    internal void Start()
    {
        _count = 1;
    }

    protected void LogCore()
    {
        Console.WriteLine("Core log");
    }

    public void Run()
    {
        Start();
        Console.WriteLine(FileHelper.Normalize("  running  "));
    }
}
```