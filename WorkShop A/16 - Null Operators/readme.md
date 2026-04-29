# C# Null Operators: `?.`, `??`, and `??=`

## 1. Null-Conditional Operator: `?.`

### Meaning

The **null-conditional operator** checks whether an object is `null` before accessing one of its members.

> If the object is `null`, the whole expression returns `null`.  
> If the object is not `null`, the member is accessed normally.

---

## Syntax

```csharp
objectName?.MemberName
```

You can use it with:

- **Properties**
- **Methods**
- **Fields**
- **Indexers**
- **Events**

---

## Example Without `?.`

```csharp
Customer customer = null;

string city = customer.Address.City; // ❌ Runtime error
```

This causes a `NullReferenceException` because `customer` is `null`.

---

## Example With `?.`

```csharp
Customer customer = null;

string city = customer?.Address?.City;
```

### What happens?

1. `customer` is checked.
2. Since `customer` is `null`, the expression returns `null`.
3. No exception is thrown.

---

## Example with a Method

```csharp
User user = null;

user?.SendWelcomeEmail();
```

If `user` is `null`, the method is **not called**.

---

## Example with Arrays or Lists

```csharp
List<string> colors = null;

string firstColor = colors?[0];
```

If `colors` is `null`, `firstColor` becomes `null`.

---

## Important Detail

The result of `?.` is often nullable.

```csharp
Order order = null;

int? itemCount = order?.Items.Count;
```

Because `order` might be `null`, the result must be able to store `null`.

So this works:

```csharp
int? itemCount = order?.Items.Count;
```

But this does not:

```csharp
int itemCount = order?.Items.Count; // ❌ Possible null value
```

---

# 2. Null-Coalescing Operator: `??`

## Meaning

The **null-coalescing operator** provides a fallback value.

> If the value on the left is `null`, return the value on the right.  
> If the value on the left is not `null`, return the left value.

---

## Syntax

```csharp
leftValue ?? fallbackValue
```

---

## Basic Example

```csharp
string nickname = null;

string displayName = nickname ?? "Guest";
```

### Result

```csharp
displayName = "Guest";
```

Because `nickname` is `null`, `"Guest"` is used instead.

---

## Example When Left Side Is Not Null

```csharp
string nickname = "Luna";

string displayName = nickname ?? "Guest";
```

### Result

```csharp
displayName = "Luna";
```

Because `nickname` already has a value, the fallback is ignored.

---

## Common Use with `?.`

The `?.` and `??` operators are often used together.

```csharp
Customer customer = null;

string city = customer?.Address?.City ?? "Unknown city";
```

### Meaning

- Try to get `customer.Address.City`.
- If anything in the chain is `null`, use `"Unknown city"`.

---

## Example with Numbers

```csharp
int? discountPercent = null;

int finalDiscount = discountPercent ?? 0;
```

### Result

```csharp
finalDiscount = 0;
```

Because `discountPercent` is `null`, the fallback value `0` is used.

---

## Example with Method Return Values

```csharp
string GetPreferredLanguage()
{
    return null;
}

string language = GetPreferredLanguage() ?? "English";
```

### Result

```csharp
language = "English";
```

---

# 3. Null-Coalescing Assignment Operator: `??=`

## Meaning

The **null-coalescing assignment operator** assigns a value only if the variable is currently `null`.

> If the variable on the left is `null`, assign the value on the right.  
> If the variable on the left already has a value, leave it unchanged.

---

## Syntax

```csharp
variable ??= fallbackValue;
```

---

## Basic Example

```csharp
string theme = null;

theme ??= "Light";
```

### Result

```csharp
theme = "Light";
```

Because `theme` was `null`, `"Light"` was assigned to it.

---

## Example When Variable Is Not Null

```csharp
string theme = "Dark";

theme ??= "Light";
```

### Result

```csharp
theme = "Dark";
```

Because `theme` already had a value, it was not changed.

---

# Difference Between `??` and `??=`

| Operator | Purpose | Changes the variable? | Example |
|---|---|---:|---|
| `??` | Returns a fallback value if the left side is `null` | ❌ No | `name ?? "Guest"` |
| `??=` | Assigns a fallback value if the left side is `null` | ✅ Yes | `name ??= "Guest";` |

---

## `??` Example

```csharp
string username = null;

string label = username ?? "Anonymous";
```

### Result

```csharp
username = null;
label = "Anonymous";
```

`username` is still `null`.  
Only `label` receives the fallback value.

---

## `??=` Example

```csharp
string username = null;

username ??= "Anonymous";
```

### Result

```csharp
username = "Anonymous";
```

Here, `username` itself is updated.

---

# Combined Examples

## Example 1: Safe Property Access with Default Value

```csharp
Profile profile = null;

string bio = profile?.Bio ?? "No biography available";
```

### Meaning

1. Try to read `profile.Bio`.
2. If `profile` is `null`, return `null`.
3. Since the result is `null`, use `"No biography available"`.

---

## Example 2: Initialize a List Only If Needed

```csharp
List<string> messages = null;

messages ??= new List<string>();

messages.Add("Welcome back!");
```

### Why this is useful

Without `??=`, you might write:

```csharp
if (messages == null)
{
    messages = new List<string>();
}

messages.Add("Welcome back!");
```

With `??=`, the code is shorter and clearer:

```csharp
messages ??= new List<string>();
messages.Add("Welcome back!");
```

---

## Example 3: Safe Method Call with Fallback

```csharp
Account account = null;

string status = account?.GetStatus() ?? "Inactive";
```

### Meaning

- If `account` is not `null`, call `GetStatus()`.
- If `account` is `null`, the method is skipped.
- If the result is `null`, use `"Inactive"`.

---

# Operator Behavior Table

| Operator | Name | What it does | Example | Possible result |
|---|---|---|---|---|
| `?.` | Null-conditional | Safely accesses a member | `user?.Email` | `null` or email value |
| `??` | Null-coalescing | Provides fallback value | `email ?? "none"` | email value or `"none"` |
| `??=` | Null-coalescing assignment | Assigns fallback if variable is `null` | `email ??= "none"` | updates `email` if needed |

---

# Visual Flow

## `?.`

```csharp
customer?.Name
```

```text
Is customer null?
├── Yes → return null
└── No  → return customer.Name
```

---

## `??`

```csharp
name ?? "Guest"
```

```text
Is name null?
├── Yes → return "Guest"
└── No  → return name
```

---

## `??=`

```csharp
name ??= "Guest";
```

```text
Is name null?
├── Yes → name = "Guest"
└── No  → keep current value
```

---

# Practical Example

```csharp
class Customer
{
    public string Name { get; set; }
    public Cart ShoppingCart { get; set; }
}

class Cart
{
    public int? ItemCount { get; set; }
}

Customer customer = new Customer
{
    Name = "Mina",
    ShoppingCart = null
};

int itemCount = customer?.ShoppingCart?.ItemCount ?? 0;

Console.WriteLine(itemCount);
```

### Output

```text
0
```

### Explanation

```csharp
customer?.ShoppingCart?.ItemCount ?? 0
```

1. `customer` is not `null`.
2. `ShoppingCart` is `null`.
3. `?.ItemCount` returns `null`.
4. `?? 0` changes the final result to `0`.

---

# Common Patterns

## Provide a Default String

```csharp
string title = article?.Title ?? "Untitled";
```

---

## Provide an Empty List

```csharp
List<Product> products = catalog?.Products ?? new List<Product>();
```

---

## Initialize a Property

```csharp
settings.ApiEndpoint ??= "https://api.example.test";
```

---

## Safely Call a Method

```csharp
logger?.Write("Application started");
```

---

## Safely Get Nested Data

```csharp
string country = user?.Profile?.Address?.Country ?? "Not specified";
```

---

# Key Notes

## `?.` Does Not Replace `??`

These two are different:

```csharp
user?.Name
```

This safely accesses `Name`.

```csharp
userName ?? "Guest"
```

This provides a fallback value.

They can be combined:

```csharp
string name = user?.Name ?? "Guest";
```

---

## `??=` Requires an Assignable Variable

This works:

```csharp
string mode = null;

mode ??= "Standard";
```

This does not work:

```csharp
GetMode() ??= "Standard"; // ❌ Invalid
```

Because the left side of `??=` must be something that can be assigned to.

---

## Right Side Is Only Evaluated When Needed

```csharp
string token = "abc123";

token ??= GenerateNewToken();
```

Since `token` is not `null`, `GenerateNewToken()` is **not called**.

The same idea applies to `??`:

```csharp
string token = "abc123";

string result = token ?? GenerateNewToken();
```

`GenerateNewToken()` is skipped because `token` already has a value.