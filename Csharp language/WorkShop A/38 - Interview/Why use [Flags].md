# 🏷️ Why use `[Flags]` on an enum for combined values?

When an enum is meant to represent **a set of options that can be combined**, applying **`[Flags]`** tells C# and other developers:

> “This enum is intended to be used as a bit field, not just as one single choice.”

---

# ✅ What problem it solves

Normally, an enum looks like a list of **mutually exclusive values**:

```csharp
public enum AccessLevel
{
    Guest = 1,
    User = 2,
    Manager = 3
}
```

This suggests a variable should hold **one value at a time**.

But sometimes you want to combine values:

- Read + Write
- Monday + Wednesday + Friday
- Visible + Enabled + Focused

That is where **bit flags** come in.

---

# 🧩 Example without `[Flags]`

```csharp
public enum Permission
{
    None = 0,
    View = 1,
    Edit = 2,
    Delete = 4
}
```

You can still combine them:

```csharp
Permission p = Permission.View | Permission.Edit;
```

This works mathematically because the values are powers of two.

But without `[Flags]`, the enum does not clearly express its intended use.

---

# 🌟 What `[Flags]` adds

```csharp
[Flags]
public enum Permission
{
    None = 0,
    View = 1,
    Edit = 2,
    Delete = 4
}
```

Now the enum clearly means:

- values can be combined
- each value represents a single bit
- the enum is a set of options, not a single state

---

# 📌 Main reasons to use `[Flags]`

## 1. It makes intent clear

This is the biggest reason.

When another developer sees:

```csharp
[Flags]
public enum Permission
{
    None = 0,
    View = 1,
    Edit = 2,
    Delete = 4
}
```

they immediately understand:

- this enum is meant for combinations
- bitwise operators like `|` and `&` are expected
- values should usually be powers of two

Without `[Flags]`, the combined use is less obvious.

---

## 2. It improves string output

This is one of the most visible benefits.

## Without `[Flags]`

```csharp
public enum Permission
{
    None = 0,
    View = 1,
    Edit = 2,
    Delete = 4
}
```

```csharp
var p = Permission.View | Permission.Edit;
Console.WriteLine(p);
```

Output may be:

```csharp
3
```

That is technically correct, but not very readable.

---

## With `[Flags]`

```csharp
[Flags]
public enum Permission
{
    None = 0,
    View = 1,
    Edit = 2,
    Delete = 4
}
```

```csharp
var p = Permission.View | Permission.Edit;
Console.WriteLine(p);
```

Output becomes:

```csharp
View, Edit
```

That is much clearer.

---

## 3. It makes debugging easier

When inspecting values in logs, debugger windows, or output, combined flags are shown in a readable form.

Instead of seeing:

- `3`
- `5`
- `7`

you see:

- `View, Edit`
- `View, Delete`
- `View, Edit, Delete`

That makes troubleshooting much easier.

---

## 4. It matches the intended bitwise design

A flags enum is usually designed like this:

```csharp
[Flags]
public enum WindowOptions
{
    None = 0,
    Resizable = 1,
    Minimizable = 2,
    Maximizable = 4,
    Fullscreen = 8
}
```

Each value is a separate bit:

| Name | Value | Binary |
|---|---:|---|
| `None` | 0 | `0000` |
| `Resizable` | 1 | `0001` |
| `Minimizable` | 2 | `0010` |
| `Maximizable` | 4 | `0100` |
| `Fullscreen` | 8 | `1000` |

This allows combinations without overlap:

```csharp
var options = WindowOptions.Resizable | WindowOptions.Maximizable;
```

Binary result:

```csharp
0001 | 0100 = 0101
```

---

# ⚠️ Important: `[Flags]` does not create bitwise behavior by itself

This is very important.

The attribute does **not** magically make an enum combinable.

Bitwise combination works because of the **numeric values**.

For example, this still works:

```csharp
public enum Permission
{
    None = 0,
    View = 1,
    Edit = 2,
    Delete = 4
}

var p = Permission.View | Permission.Edit;
```

So what does `[Flags]` do?

- communicates intent
- improves formatting/output
- supports better readability

It is mainly a **semantic and display attribute**, not a behavior engine.

---

# ✅ Why powers of two matter

If you want combined flags to work correctly, values should usually be:

- `1`
- `2`
- `4`
- `8`
- `16`
- `32`

Each one uses a different bit.

## Good flags enum

```csharp
[Flags]
public enum FeatureSet
{
    None = 0,
    Search = 1,
    Export = 2,
    Analytics = 4,
    Notifications = 8
}
```

## Bad design for flags

```csharp
[Flags]
public enum FeatureSet
{
    None = 0,
    Search = 1,
    Export = 2,
    Analytics = 3
}
```

Why bad?

Because `3` overlaps with `1 | 2`.

So `Analytics` would clash with `Search + Export`.

---

# 🧪 Example of combining values

```csharp
[Flags]
public enum FileAction
{
    None = 0,
    Open = 1,
    Save = 2,
    Print = 4
}
```

Combine values:

```csharp
var actions = FileAction.Open | FileAction.Print;
```

Check whether a flag is present:

```csharp
bool canPrint = (actions & FileAction.Print) == FileAction.Print;
```

Or use `HasFlag`:

```csharp
bool canOpen = actions.HasFlag(FileAction.Open);
```

---

# 🆚 Without vs with `[Flags]`

| Aspect | Without `[Flags]` | With `[Flags]` |
|---|---|---|
| Can combine powers-of-two values | Yes | Yes |
| Intent is obvious | No | Yes |
| `ToString()` for combinations is clearer | Usually no | Yes |
| Better for debugging/logging | Less | More |
| Signals bit-field usage | No | Yes |

---

# 📍 Example showing the readability difference

## Without `[Flags]`

```csharp
public enum DaysAvailable
{
    None = 0,
    Monday = 1,
    Tuesday = 2,
    Wednesday = 4
}
```

```csharp
var days = DaysAvailable.Monday | DaysAvailable.Wednesday;
Console.WriteLine(days);
```

Possible output:

```csharp
5
```

---

## With `[Flags]`

```csharp
[Flags]
public enum DaysAvailable
{
    None = 0,
    Monday = 1,
    Tuesday = 2,
    Wednesday = 4
}
```

```csharp
var days = DaysAvailable.Monday | DaysAvailable.Wednesday;
Console.WriteLine(days);
```

Output:

```csharp
Monday, Wednesday
```

Much better.

---

# 🧠 Best practice pattern

When creating a flags enum, the usual pattern is:

```csharp
[Flags]
public enum Permission
{
    None = 0,
    Read = 1,
    Write = 2,
    Execute = 4,
    Share = 8
}
```

## Recommended rules

- apply `[Flags]`
- include `None = 0`
- use powers of two
- combine values with `|`
- test values with `&` or `HasFlag`

---

# 🚫 Common mistake

## Wrong assumption

> “If I use `[Flags]`, I can assign any numbers and combinations will work.”

Not true.

This is wrong:

```csharp
[Flags]
public enum Status
{
    None = 0,
    Ready = 1,
    Running = 2,
    Finished = 3
}
```

Because `3` is already `1 | 2`.

So `Finished` collides with `Ready + Running`.

---

# 🔍 In one sentence

You should apply **`[Flags]`** to an enum when you want to store combined values because it **declares that the enum is intended to be used as a bit field**, making the code **clearer, more readable, and better formatted when combined values are displayed**.