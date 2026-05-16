# C# Safety Features: `!`, `when`, and `unchecked`

## 1. Null-Forgiving Operator: `!`

The **null-forgiving operator** is written as an exclamation mark after an expression:

```csharp
customer!.Name
```

It tells the C# compiler:

> “Trust me, this value is not `null` here.”

However, it is important to understand that `!` only affects **compiler warnings**.  
It does **not** change runtime behavior.

---

## What Problem Does `!` Solve?

When nullable reference types are enabled, the compiler warns you if it thinks a value might be `null`.

Example:

```csharp
#nullable enable

string? username = GetUsername();

Console.WriteLine(username.Length);
```

The compiler may warn:

```text
CS8602: Dereference of a possibly null reference.
```

That warning appears because `username` is declared as nullable:

```csharp
string? username
```

So the compiler knows it might contain `null`.

---

## Using the Null-Forgiving Operator

You can suppress the warning by writing `!` after the expression:

```csharp
#nullable enable

string? username = GetUsername();

Console.WriteLine(username!.Length);
```

This tells the compiler to stop warning about `username` possibly being `null`.

---

## Important: `!` Does Not Prevent Runtime Errors

The null-forgiving operator does **not** check for `null`.

It does not convert `null` into a valid object.

It does not protect your code.

Example:

```csharp
#nullable enable

string? productCode = null;

Console.WriteLine(productCode!.Length);
```

This compiles without a nullable warning, but at runtime it throws:

```text
System.NullReferenceException
```

Because `productCode` is still actually `null`.

---

## Think of `!` as a Compiler Instruction

| Code | Meaning |
|---|---|
| `value` | Use the value normally |
| `value!` | Use the value and suppress nullable warnings |
| `value?.Property` | Safely access the property only if value is not null |
| `value ?? fallback` | Use fallback if value is null |

---

## Safer Alternatives to `!`

### Use an `if` Statement

```csharp
string? email = FindEmailAddress();

if (email is not null)
{
    Console.WriteLine(email.Length);
}
```

### Use the Null-Conditional Operator: `?.`

```csharp
string? email = FindEmailAddress();

Console.WriteLine(email?.Length);
```

If `email` is `null`, the result is also `null` instead of throwing an exception.

### Use the Null-Coalescing Operator: `??`

```csharp
string? displayName = GetDisplayName();

Console.WriteLine(displayName ?? "Guest User");
```

If `displayName` is `null`, `"Guest User"` is used.

### Throw a Clear Exception

```csharp
string? apiKey = LoadApiKey();

string validApiKey = apiKey ?? throw new InvalidOperationException("API key is missing.");
```

This is often better than using `!`, because the error message is clearer.

---

## When Should You Use `!`?

Use `!` only when you are sure the value cannot be `null`, but the compiler cannot understand that.

Example:

```csharp
#nullable enable

User? currentUser = FindLoggedInUser();

ValidateUserExists(currentUser);

Console.WriteLine(currentUser!.Username);

static void ValidateUserExists(User? user)
{
    if (user is null)
    {
        throw new InvalidOperationException("No user is logged in.");
    }
}

class User
{
    public string Username { get; set; } = "";
}
```

In this example, `ValidateUserExists` throws an exception if `currentUser` is `null`.

However, the compiler may not fully understand that, so `!` is used:

```csharp
currentUser!.Username
```

---

# 2. Exception Filters with `when`

C# allows you to add conditions to `catch` blocks using the `when` keyword.

This is called an **exception filter**.

Basic syntax:

```csharp
try
{
    // Code that might throw an exception
}
catch (SomeException ex) when (condition)
{
    // Handle the exception only if condition is true
}
```

---

## Basic Example

```csharp
try
{
    int result = DivideNumbers(20, 0);
    Console.WriteLine(result);
}
catch (DivideByZeroException ex) when (DateTime.Now.Hour < 12)
{
    Console.WriteLine("Division by zero happened in the morning.");
}
catch (DivideByZeroException ex)
{
    Console.WriteLine("Division by zero happened later in the day.");
}

static int DivideNumbers(int left, int right)
{
    return left / right;
}
```

The first `catch` only runs if both are true:

1. The exception is a `DivideByZeroException`
2. The condition after `when` is `true`

---

## Practical Example: Handling Based on Error Code

```csharp
try
{
    ConnectToService("inventory-api");
}
catch (ServiceException ex) when (ex.ErrorCode == 408)
{
    Console.WriteLine("The service request timed out.");
}
catch (ServiceException ex) when (ex.ErrorCode == 401)
{
    Console.WriteLine("Authentication failed.");
}
catch (ServiceException ex)
{
    Console.WriteLine($"Service error: {ex.Message}");
}

static void ConnectToService(string serviceName)
{
    throw new ServiceException("Request timeout.", 408);
}

class ServiceException : Exception
{
    public int ErrorCode { get; }

    public ServiceException(string message, int errorCode) : base(message)
    {
        ErrorCode = errorCode;
    }
}
```

---

## Why Use `when` Instead of `if` Inside `catch`?

You could write this:

```csharp
try
{
    ProcessPayment(250);
}
catch (PaymentException ex)
{
    if (ex.Status == "Declined")
    {
        Console.WriteLine("Payment was declined.");
    }
    else
    {
        throw;
    }
}
```

But with `when`, the code is cleaner:

```csharp
try
{
    ProcessPayment(250);
}
catch (PaymentException ex) when (ex.Status == "Declined")
{
    Console.WriteLine("Payment was declined.");
}
catch (PaymentException ex) when (ex.Status == "InsufficientFunds")
{
    Console.WriteLine("The account does not have enough funds.");
}
```

---

## Multiple `catch` Blocks with Filters

```csharp
try
{
    LoadDocument("settings.json");
}
catch (FileNotFoundException ex) when (ex.FileName?.EndsWith(".json") == true)
{
    Console.WriteLine("A JSON configuration file is missing.");
}
catch (FileNotFoundException ex)
{
    Console.WriteLine($"A file was not found: {ex.FileName}");
}
catch (UnauthorizedAccessException)
{
    Console.WriteLine("You do not have permission to access this file.");
}
```

---

## Exception Filter Behavior

A `catch` block with `when` only handles the exception if the filter is `true`.

```csharp
catch (IOException ex) when (ex.Message.Contains("locked"))
```

If the exception is an `IOException` but the message does not contain `"locked"`, C# skips that `catch` block and keeps looking for another matching handler.

---

## Exception Filter Table

| Code | Meaning |
|---|---|
| `catch (Exception ex)` | Catch every exception of type `Exception` |
| `catch (IOException ex)` | Catch only `IOException` and derived types |
| `catch (IOException ex) when (condition)` | Catch `IOException` only if `condition` is true |
| `catch (Exception ex) when (ex.Message.Contains("timeout"))` | Catch exceptions whose message contains `"timeout"` |

---

# 3. Disabling Overflow Checks with `unchecked`

C# can check for arithmetic overflow.

An **overflow** happens when a numeric value becomes too large or too small for its data type.

Example:

```csharp
int maximum = int.MaxValue;

int result = maximum + 1;
```

The maximum value of `int` is:

```csharp
2,147,483,647
```

Adding `1` goes beyond what an `int` can store.

---

## `checked` vs `unchecked`

C# provides two keywords for overflow behavior:

| Keyword | Behavior |
|---|---|
| `checked` | Throws an exception if overflow happens |
| `unchecked` | Allows overflow without throwing an exception |

---

## Using `checked`

```csharp
int maximum = int.MaxValue;

checked
{
    int result = maximum + 1;
    Console.WriteLine(result);
}
```

This throws:

```text
System.OverflowException
```

Because the result is too large for an `int`.

---

## Using `unchecked`

```csharp
int maximum = int.MaxValue;

unchecked
{
    int result = maximum + 1;
    Console.WriteLine(result);
}
```

This does **not** throw an exception.

Instead, the value wraps around.

The output is:

```text
-2147483648
```

That is `int.MinValue`.

---

## Why Does It Wrap Around?

An `int` has a fixed range:

| Type | Minimum | Maximum |
|---|---:|---:|
| `int` | `-2,147,483,648` | `2,147,483,647` |

If you go above the maximum value, it wraps around to the minimum value.

```csharp
int.MaxValue + 1 == int.MinValue
```

In an `unchecked` context, this wraparound is allowed.

---

## `unchecked` Statement

You can use `unchecked` with a block:

```csharp
unchecked
{
    int score = int.MaxValue;
    int nextScore = score + 10;

    Console.WriteLine(nextScore);
}
```

Everything inside the block uses unchecked arithmetic.

---

## `unchecked` Expression

You can also use `unchecked` with a single expression:

```csharp
int total = unchecked(int.MaxValue + 5);

Console.WriteLine(total);
```

---

## `checked` Expression

Similarly, `checked` can be used with one expression:

```csharp
int total = checked(int.MaxValue + 5);

Console.WriteLine(total);
```

This throws:

```text
System.OverflowException
```

---

## Example: `byte` Overflow

```csharp
byte level = 255;

unchecked
{
    byte nextLevel = (byte)(level + 1);
    Console.WriteLine(nextLevel);
}
```

Output:

```text
0
```

A `byte` can store values from `0` to `255`.

So after `255`, it wraps back to `0`.

---

## Example: Combining `checked` and `unchecked`

```csharp
checked
{
    int safeValue = 100 + 50;
    Console.WriteLine(safeValue);

    unchecked
    {
        int wrappedValue = int.MaxValue + 3;
        Console.WriteLine(wrappedValue);
    }
}
```

In this example:

1. The outer block uses `checked`
2. The inner block uses `unchecked`
3. Overflow is allowed only inside the `unchecked` block

---

# 4. Comparing the Three Features

| Feature | Syntax | Purpose | Runtime Effect |
|---|---|---|---|
| Null-forgiving operator | `value!` | Suppresses nullable compiler warnings | No direct runtime effect |
| Exception filter | `catch (...) when (...)` | Handles exceptions only when a condition is true | Controls which `catch` block runs |
| Unchecked arithmetic | `unchecked { ... }` | Disables overflow checking | Allows numeric wraparound |

---

# 5. Full Example Using All Three

```csharp
#nullable enable

try
{
    User? user = FindUserById(42);

    string username = user!.Username;

    unchecked
    {
        int loginCount = int.MaxValue;
        int updatedLoginCount = loginCount + 1;

        Console.WriteLine($"User: {username}");
        Console.WriteLine($"Login count: {updatedLoginCount}");
    }
}
catch (NullReferenceException ex) when (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
{
    Console.WriteLine("A null value was used on Friday.");
}
catch (NullReferenceException)
{
    Console.WriteLine("A null value was used.");
}

static User? FindUserById(int id)
{
    return null;
}

class User
{
    public string Username { get; set; } = "default-user";
}
```

## What Happens Here?

1. `FindUserById(42)` returns `null`.
2. `user!.Username` suppresses the compiler warning.
3. At runtime, `user` is still `null`.
4. Accessing `.Username` throws a `NullReferenceException`.
5. The `catch` block with `when` may handle it depending on the day.
6. The `unchecked` block would allow overflow, but it is never reached because the exception happens first.