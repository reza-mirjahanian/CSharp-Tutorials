# C# Working with `null` Values

In C#, `null` means **“there is no object/value here.”**  
It is commonly used with **reference types** and **nullable value types**.

Understanding `null` is important because many runtime errors happen when code tries to use something that is actually `null`.

---

## 1. What `null` Means

A variable that contains `null` does **not** point to a real object in memory.

### Example

```csharp
string? title = null;
```

Here:

- `title` can either contain:
  - a real `string`
  - or `null`

If you try to use `title` like a normal string without checking it first, your program may fail.

---

## 2. Why `null` Can Be Dangerous

A common error in C# is:

> **`NullReferenceException`**  
> “You tried to use an object that does not exist.”

### Example

```csharp
string? customerName = null;
Console.WriteLine(customerName.Length);
```

This causes a problem because `customerName` is `null`, and `null` has no `Length`.

---

# Nullable Reference Types

Nullable reference types were introduced to help developers **detect possible `null` problems earlier**, often at compile time.

---

## 3. Reference Types Before Nullable Reference Types

Traditionally, all reference types could hold `null`.

```csharp
string city = null;
```

This was allowed in older C# projects, but it was risky because `city` looked like a normal string even though it could be `null`.

---

## 4. Reference Types with Nullable Annotations

With nullable reference types enabled:

- `string` means **should not be `null`**
- `string?` means **may be `null`**

### Example

```csharp
string userId = "A102";
string? nickname = null;
```

### Meaning

| Type | Meaning |
|---|---|
| `string` | Non-nullable reference type |
| `string?` | Nullable reference type |

If you assign `null` to a non-nullable variable, the compiler warns you.

```csharp
string productCode = null; // warning
```

---

## 5. Why Nullable Reference Types Are Useful

They help you:

- **document intent**
- **reduce null-related bugs**
- **get compiler warnings before runtime**
- **write safer APIs**

### Compare the Intent

```csharp
string firstName = "Sara";
string? middleName = null;
```

This tells readers:

- `firstName` is expected to always have a value
- `middleName` is optional

---

# Disabling `null`

Sometimes nullable reference types are enabled in a project, but you may want to disable the feature in a file or section of code.

---

## 6. Disable Nullable Context

You can disable nullable analysis with:

```csharp
#nullable disable
```

After this, reference types behave like the older style.

### Example

```csharp
#nullable disable

string message = null;
Console.WriteLine(message.Length);
```

The compiler becomes less strict here, even though the code is unsafe.

---

## 7. Re-Enable Nullable Context

You can turn it back on with:

```csharp
#nullable enable
```

### Example

```csharp
#nullable enable

string label = "Ready";
string? note = null;
```

---

## 8. Disable for Part of a File

You can switch nullable analysis on and off in different parts of the same file.

```csharp
#nullable enable
string email = "info@site.test";
string? backupEmail = null;

#nullable disable
string oldData = null;
```

This can be useful when working with:

- older codebases
- external libraries
- migration scenarios

---

## 9. Project-Level Nullable Setting

Nullable reference types can also be controlled in the project file.

### Example

```xml
<Nullable>enable</Nullable>
```

Other possible values include:

- `enable`
- `disable`
- `warnings`
- `annotations`

### Simple Table

| Setting | Effect |
|---|---|
| `enable` | Enables annotations and warnings |
| `disable` | Turns nullable reference types off |
| `warnings` | Shows warnings without full annotation behavior |
| `annotations` | Uses annotations without warning analysis |

---

# Checking for `null`

Even with nullable reference types, you still need to check values when they may be missing.

---

## 10. Basic `null` Check with `if`

The most direct approach:

```csharp
string? description = GetDescription();

if (description != null)
{
    Console.WriteLine(description.Length);
}
```

This is safe because `description` is checked before use.

---

## 11. Checking for `null` with `== null`

You can also test whether a variable is `null`.

```csharp
if (description == null)
{
    Console.WriteLine("No description available.");
}
```

---

## 12. Null-Conditional Operator `?.`

The `?.` operator accesses a member **only if the object is not `null`**.

### Example

```csharp
string? code = null;
Console.WriteLine(code?.Length);
```

If `code` is `null`, the result is also `null` instead of throwing an exception.

### Another Example

```csharp
Order? currentOrder = FindOrder();
Console.WriteLine(currentOrder?.CustomerName);
```

This is much safer than directly writing:

```csharp
Console.WriteLine(currentOrder.CustomerName);
```

---

## 13. Null-Coalescing Operator `??`

The `??` operator provides a fallback value when something is `null`.

### Syntax

```csharp
valueIfNullable ?? fallbackValue
```

### Example

```csharp
string? theme = null;
string activeTheme = theme ?? "Default";
Console.WriteLine(activeTheme);
```

If `theme` is `null`, `activeTheme` becomes `"Default"`.

---

## 14. Null-Coalescing Assignment `??=`

This assigns a value **only if the variable is currently `null`**.

### Example

```csharp
string? folderPath = null;
folderPath ??= "/var/app/data";
Console.WriteLine(folderPath);
```

If `folderPath` already has a value, it is not changed.

---

## 15. Null-Forgiving Operator `!`

The `!` operator tells the compiler:

> *“I know this is not null here, even if you think it might be.”*

### Example

```csharp
string? input = GetInput();
Console.WriteLine(input!.Length);
```

This removes the warning, but it does **not** make the value safe at runtime.

⚠️ If `input` is actually `null`, the program can still fail.

Use this operator carefully.

---

## 16. Pattern Matching for `null`

A modern style is to use pattern matching:

```csharp
if (token is not null)
{
    Console.WriteLine(token.Length);
}
```

This is clear and expressive.

You can also write:

```csharp
if (token is null)
{
    Console.WriteLine("Token is missing.");
}
```

---

## 17. Guard Clauses for `null`

Guard clauses stop invalid data early.

### Example

```csharp
void PrintCode(string? code)
{
    if (code is null)
    {
        throw new ArgumentNullException(nameof(code));
    }

    Console.WriteLine(code.ToUpper());
}
```

This makes the method safer and easier to read.

---

## 18. `ArgumentNullException.ThrowIfNull`

Modern C# provides a shorter way to validate arguments.

```csharp
void SendMessage(string? text)
{
    ArgumentNullException.ThrowIfNull(text);
    Console.WriteLine(text.Length);
}
```

This is a clean way to protect methods from `null` input.

---

## 19. Safe Access in Chains

When working with nested objects, `?.` is especially useful.

### Example

```csharp
Console.WriteLine(session?.User?.Profile?.DisplayName);
```

This avoids multiple manual checks like:

```csharp
if (session != null &&
    session.User != null &&
    session.User.Profile != null)
{
    Console.WriteLine(session.User.Profile.DisplayName);
}
```

---

## 20. Combining `?.` and `??`

These operators are often used together.

```csharp
string displayText = account?.OwnerName ?? "Unknown Owner";
Console.WriteLine(displayText);
```

### How it works

1. `account?.OwnerName`
   - returns the owner name if `account` is not `null`
   - otherwise returns `null`
2. `?? "Unknown Owner"`
   - uses the fallback text if the first result is `null`

---

## 21. Real-World Example

```csharp
#nullable enable

class Member
{
    public string FullName { get; set; } = "";
    public string? ContactEmail { get; set; }
}

class Program
{
    static void Main()
    {
        Member member = new Member
        {
            FullName = "Nima Ahmadi",
            ContactEmail = null
        };

        Console.WriteLine($"Name: {member.FullName}");
        Console.WriteLine($"Email: {member.ContactEmail ?? "not provided"}");
        Console.WriteLine($"Email length: {member.ContactEmail?.Length}");
    }
}
```

### What to notice

- `FullName` is non-nullable
- `ContactEmail` is nullable
- `??` provides fallback text
- `?.` safely reads `Length`

---

## 22. Best Practices

### ✅ Recommended

- Use **nullable reference types**
- Mark optional references with `?`
- Check nullable values before using them
- Use `??` for sensible defaults
- Use guard clauses for method parameters
- Prefer compiler warnings over ignoring problems

### ❌ Avoid

- Disabling nullable support without a reason
- Using `!` too often
- Assuming a value is never `null`
- Hiding null problems instead of fixing them

---

## 23. Quick Reference Table

| Feature | Example | Purpose |
|---|---|---|
| Nullable reference type | `string? note` | Variable may be `null` |
| Non-nullable reference type | `string note` | Variable should not be `null` |
| Null check | `if (note != null)` | Ensure safety before use |
| Null-conditional | `note?.Length` | Safe member access |
| Null-coalescing | `note ?? "empty"` | Fallback value |
| Null-coalescing assignment | `note ??= "new"` | Assign only if `null` |
| Null-forgiving | `note!` | Suppress compiler warning |
| Disable nullable context | `#nullable disable` | Turn off nullable analysis |
| Enable nullable context | `#nullable enable` | Turn on nullable analysis |

---

## 24. Mini Example Set

### Nullable variable

```csharp
string? remark = null;
```

### Non-nullable variable

```csharp
string category = "Books";
```

### Safe check

```csharp
if (remark != null)
{
    Console.WriteLine(remark);
}
```

### Fallback value

```csharp
Console.WriteLine(remark ?? "No remark");
```

### Safe property access

```csharp
Console.WriteLine(remark?.Length);
```

### Throw if missing

```csharp
ArgumentNullException.ThrowIfNull(category);
```