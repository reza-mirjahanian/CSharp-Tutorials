# 🧩 Enum Types and Flags in C#

## 1. What Is an `enum`?

An **`enum`** is a special value type in C# that lets you define a set of **named constants**.

Instead of using unclear numbers like this:

```csharp
int status = 2;
```

You can use meaningful names:

```csharp
OrderStatus status = OrderStatus.Shipped;
```

This makes code easier to read, safer, and less error-prone.

---

## 2. Basic `enum` Syntax

```csharp
enum OrderStatus
{
    Pending,
    Processing,
    Shipped,
    Delivered,
    Cancelled
}
```

You can use it like this:

```csharp
OrderStatus currentStatus = OrderStatus.Processing;

Console.WriteLine(currentStatus);
```

Output:

```text
Processing
```

---

## 3. Default Numeric Values

By default, enum members are assigned integer values starting from `0`.

```csharp
enum OrderStatus
{
    Pending,     // 0
    Processing,  // 1
    Shipped,     // 2
    Delivered,   // 3
    Cancelled    // 4
}
```

You can cast an enum value to `int`:

```csharp
OrderStatus status = OrderStatus.Shipped;

int numericValue = (int)status;

Console.WriteLine(numericValue);
```

Output:

```text
2
```

---

## 4. Assigning Custom Values

You can manually assign numeric values to enum members.

```csharp
enum PaymentStatus
{
    Unknown = 0,
    Authorized = 10,
    Captured = 20,
    Refunded = 30,
    Failed = 40
}
```

Example:

```csharp
PaymentStatus status = PaymentStatus.Refunded;

Console.WriteLine((int)status);
```

Output:

```text
30
```

---

## 5. Auto-Increment After Custom Values

If you assign a value to one enum member, the following members continue from that value.

```csharp
enum SupportTicketPriority
{
    Low = 5,
    Medium,   // 6
    High,     // 7
    Critical  // 8
}
```

Example:

```csharp
Console.WriteLine((int)SupportTicketPriority.High);
```

Output:

```text
7
```

---

## 6. Why Use Enums?

Enums are useful when a variable should only contain one value from a fixed set of options.

### ✅ Benefits

| Benefit | Explanation |
|---|---|
| **Readability** | `OrderStatus.Shipped` is clearer than `2` |
| **Type safety** | Prevents mixing unrelated values |
| **Maintainability** | Easier to update names and meanings |
| **IntelliSense support** | IDEs can suggest valid enum values |
| **Self-documenting code** | The code explains itself better |

---

## 7. Enum Example in a Class

```csharp
enum AccountType
{
    Standard,
    Premium,
    Enterprise
}

class CustomerAccount
{
    public string OwnerName { get; set; }
    public AccountType Type { get; set; }

    public decimal GetMonthlyFee()
    {
        return Type switch
        {
            AccountType.Standard => 8.99m,
            AccountType.Premium => 18.49m,
            AccountType.Enterprise => 74.95m,
            _ => 0m
        };
    }
}
```

Usage:

```csharp
var account = new CustomerAccount
{
    OwnerName = "Nora",
    Type = AccountType.Premium
};

Console.WriteLine(account.GetMonthlyFee());
```

Output:

```text
18.49
```

---

# 🔢 Enum Underlying Types

## 1. Default Underlying Type

By default, an enum uses `int` as its underlying type.

```csharp
enum FileState
{
    Created,
    Opened,
    Closed
}
```

This is equivalent to:

```csharp
enum FileState : int
{
    Created,
    Opened,
    Closed
}
```

---

## 2. Supported Underlying Types

An enum can use the following integral types:

| Type | Size |
|---|---:|
| `byte` | 8-bit unsigned |
| `sbyte` | 8-bit signed |
| `short` | 16-bit signed |
| `ushort` | 16-bit unsigned |
| `int` | 32-bit signed |
| `uint` | 32-bit unsigned |
| `long` | 64-bit signed |
| `ulong` | 64-bit unsigned |

---

## 3. Choosing a Smaller Underlying Type

```csharp
enum StarRating : byte
{
    One = 1,
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5
}
```

This can be useful when you need to reduce memory usage or match an external data format.

---

## 4. Underlying Type Example

```csharp
StarRating rating = StarRating.Four;

byte value = (byte)rating;

Console.WriteLine(value);
```

Output:

```text
4
```

---

# 🧠 Working with Enums

## 1. Comparing Enum Values

```csharp
OrderStatus status = OrderStatus.Delivered;

if (status == OrderStatus.Delivered)
{
    Console.WriteLine("The order has arrived.");
}
```

---

## 2. Using Enums in `switch`

```csharp
enum NotificationType
{
    Email,
    Sms,
    Push,
    InApp
}
```

```csharp
NotificationType notification = NotificationType.Push;

switch (notification)
{
    case NotificationType.Email:
        Console.WriteLine("Sending an email notification.");
        break;

    case NotificationType.Sms:
        Console.WriteLine("Sending an SMS notification.");
        break;

    case NotificationType.Push:
        Console.WriteLine("Sending a push notification.");
        break;

    case NotificationType.InApp:
        Console.WriteLine("Showing an in-app notification.");
        break;

    default:
        Console.WriteLine("Unknown notification type.");
        break;
}
```

---

## 3. Using Enums with Switch Expressions

```csharp
enum DeliverySpeed
{
    Economy,
    Standard,
    Express,
    SameDay
}
```

```csharp
DeliverySpeed speed = DeliverySpeed.Express;

decimal fee = speed switch
{
    DeliverySpeed.Economy => 2.75m,
    DeliverySpeed.Standard => 5.50m,
    DeliverySpeed.Express => 12.25m,
    DeliverySpeed.SameDay => 24.99m,
    _ => throw new ArgumentOutOfRangeException()
};

Console.WriteLine(fee);
```

Output:

```text
12.25
```

---

# 🔁 Enum Conversion

## 1. Enum to Integer

```csharp
enum InvoiceState
{
    Draft = 1,
    Sent = 2,
    Paid = 3,
    Overdue = 4
}
```

```csharp
InvoiceState state = InvoiceState.Paid;

int value = (int)state;

Console.WriteLine(value);
```

Output:

```text
3
```

---

## 2. Integer to Enum

```csharp
int value = 4;

InvoiceState state = (InvoiceState)value;

Console.WriteLine(state);
```

Output:

```text
Overdue
```

---

## 3. Be Careful with Invalid Numeric Values

C# allows casting a number to an enum even if the number does not match a named member.

```csharp
InvoiceState state = (InvoiceState)99;

Console.WriteLine(state);
```

Output:

```text
99
```

> ⚠️ An enum variable can technically hold an unnamed numeric value.  
> Use validation when converting from user input, databases, or external APIs.

---

## 4. Checking Whether a Value Is Defined

Use `Enum.IsDefined`.

```csharp
int input = 3;

bool isValid = Enum.IsDefined(typeof(InvoiceState), input);

Console.WriteLine(isValid);
```

Output:

```text
True
```

Example with invalid value:

```csharp
int input = 77;

if (Enum.IsDefined(typeof(InvoiceState), input))
{
    InvoiceState state = (InvoiceState)input;
    Console.WriteLine(state);
}
else
{
    Console.WriteLine("Invalid invoice state.");
}
```

---

# 📝 Parsing Enum Values from Strings

## 1. Using `Enum.Parse`

```csharp
string text = "Paid";

InvoiceState state = (InvoiceState)Enum.Parse(typeof(InvoiceState), text);

Console.WriteLine(state);
```

Output:

```text
Paid
```

---

## 2. Case-Insensitive Parsing

```csharp
string text = "overdue";

InvoiceState state = (InvoiceState)Enum.Parse(
    typeof(InvoiceState),
    text,
    ignoreCase: true
);

Console.WriteLine(state);
```

Output:

```text
Overdue
```

---

## 3. Safer Parsing with `Enum.TryParse`

`Enum.TryParse` is usually better because it does not throw an exception when parsing fails.

```csharp
string input = "Sent";

bool success = Enum.TryParse(input, out InvoiceState state);

if (success)
{
    Console.WriteLine($"Parsed value: {state}");
}
else
{
    Console.WriteLine("Could not parse invoice state.");
}
```

Output:

```text
Parsed value: Sent
```

---

## 4. Case-Insensitive `TryParse`

```csharp
string input = "draft";

bool success = Enum.TryParse(
    input,
    ignoreCase: true,
    out InvoiceState state
);

if (success)
{
    Console.WriteLine(state);
}
```

Output:

```text
Draft
```

---

## 5. `TryParse` Still Allows Numeric Strings

```csharp
string input = "42";

bool success = Enum.TryParse(input, out InvoiceState state);

Console.WriteLine(success);
Console.WriteLine(state);
```

Output:

```text
True
42
```

> ⚠️ `Enum.TryParse` checks whether parsing succeeded, but it does **not always guarantee** that the parsed value is a named enum member.

Use `Enum.IsDefined` if you need strict validation.

```csharp
string input = "42";

if (Enum.TryParse(input, out InvoiceState state) &&
    Enum.IsDefined(typeof(InvoiceState), state))
{
    Console.WriteLine($"Valid state: {state}");
}
else
{
    Console.WriteLine("Invalid invoice state.");
}
```

---

# 📋 Getting Enum Names and Values

## 1. Get All Enum Names

```csharp
string[] names = Enum.GetNames(typeof(InvoiceState));

foreach (string name in names)
{
    Console.WriteLine(name);
}
```

Output:

```text
Draft
Sent
Paid
Overdue
```

---

## 2. Get All Enum Values

```csharp
InvoiceState[] states = (InvoiceState[])Enum.GetValues(typeof(InvoiceState));

foreach (InvoiceState state in states)
{
    Console.WriteLine($"{state} = {(int)state}");
}
```

Output:

```text
Draft = 1
Sent = 2
Paid = 3
Overdue = 4
```

---

## 3. Generic Version

In modern C#, you can use generic enum methods.

```csharp
foreach (InvoiceState state in Enum.GetValues<InvoiceState>())
{
    Console.WriteLine(state);
}
```

```csharp
foreach (string name in Enum.GetNames<InvoiceState>())
{
    Console.WriteLine(name);
}
```

---

# 🏷️ Enum Naming Conventions

## 1. Enum Type Names

Use **PascalCase** for enum type names.

```csharp
enum CustomerLevel
{
    Bronze,
    Silver,
    Gold
}
```

✅ Good:

```csharp
CustomerLevel
```

❌ Avoid:

```csharp
customer_level
customerLevelEnum
```

---

## 2. Enum Member Names

Use **PascalCase** for enum members.

```csharp
enum AccessLevel
{
    Guest,
    Member,
    Moderator,
    Administrator
}
```

✅ Good:

```csharp
Administrator
```

❌ Avoid:

```csharp
administrator
ADMINISTRATOR
admin_user
```

---

## 3. Avoid Adding `Enum` to the Name

Usually, do not name an enum like this:

```csharp
enum OrderStatusEnum
{
    Pending,
    Shipped
}
```

Prefer:

```csharp
enum OrderStatus
{
    Pending,
    Shipped
}
```

---

## 4. Singular vs. Plural Names

| Enum Type | Recommended Name |
|---|---|
| Normal enum | Singular |
| Flags enum | Plural |

Normal enum:

```csharp
enum PaymentMethod
{
    Card,
    BankTransfer,
    DigitalWallet
}
```

Flags enum:

```csharp
[Flags]
enum FilePermissions
{
    Read = 1,
    Write = 2,
    Execute = 4
}
```

---

# 🚦 Enum Default Values

## 1. Default Enum Value Is `0`

The default value of an enum is always the value with numeric value `0`.

```csharp
enum TaskState
{
    New = 0,
    Running = 1,
    Completed = 2
}
```

```csharp
TaskState state = default;

Console.WriteLine(state);
```

Output:

```text
New
```

---

## 2. Include a Zero Value

It is usually good practice to include a meaningful `0` value.

```csharp
enum RegistrationStatus
{
    Unknown = 0,
    Started = 1,
    Verified = 2,
    Rejected = 3
}
```

This avoids confusing default values.

```csharp
RegistrationStatus status = default;

Console.WriteLine(status);
```

Output:

```text
Unknown
```

---

## 3. Problem Without a Zero Member

```csharp
enum ReportFormat
{
    Pdf = 1,
    Csv = 2,
    Html = 3
}
```

```csharp
ReportFormat format = default;

Console.WriteLine(format);
```

Output:

```text
0
```

> ⚠️ Since no member has value `0`, the enum prints `0`, which may be confusing.

---

# 🚩 Flags Enums

## 1. What Is a Flags Enum?

A **flags enum** represents a combination of multiple options.

A normal enum usually means:

> Choose **one** value.

A flags enum means:

> Choose **zero, one, or many** values.

Example use cases:

- File permissions
- User roles
- Feature toggles
- Days of the week
- Notification preferences
- Formatting options

---

## 2. Normal Enum vs. Flags Enum

### Normal Enum

```csharp
enum ShippingMethod
{
    Standard,
    Express,
    Overnight
}
```

A package should usually have one shipping method:

```csharp
ShippingMethod method = ShippingMethod.Express;
```

---

### Flags Enum

```csharp
[Flags]
enum NotificationOptions
{
    None = 0,
    Email = 1,
    Sms = 2,
    Push = 4,
    InApp = 8
}
```

A user can choose multiple notification options:

```csharp
NotificationOptions options =
    NotificationOptions.Email |
    NotificationOptions.Push;
```

---

# 🧮 Why Flags Use Powers of Two

Flags should use values that are powers of two:

| Flag | Decimal | Binary |
|---|---:|---:|
| `Email` | `1` | `0001` |
| `Sms` | `2` | `0010` |
| `Push` | `4` | `0100` |
| `InApp` | `8` | `1000` |

Each flag uses a separate bit.

```csharp
[Flags]
enum NotificationOptions
{
    None = 0,
    Email = 1,
    Sms = 2,
    Push = 4,
    InApp = 8
}
```

---

## 1. Combining Flags with Bitwise OR `|`

```csharp
NotificationOptions options =
    NotificationOptions.Email |
    NotificationOptions.Sms;
```

Binary view:

```text
Email = 0001
Sms   = 0010
--------------
Both  = 0011
```

The result contains both flags.

---

## 2. Checking Flags with `HasFlag`

```csharp
NotificationOptions options =
    NotificationOptions.Email |
    NotificationOptions.Push;

if (options.HasFlag(NotificationOptions.Email))
{
    Console.WriteLine("Email notifications are enabled.");
}

if (options.HasFlag(NotificationOptions.Sms))
{
    Console.WriteLine("SMS notifications are enabled.");
}
```

Output:

```text
Email notifications are enabled.
```

---

## 3. Checking Flags with Bitwise AND `&`

Many developers prefer `&` for flag checks.

```csharp
NotificationOptions options =
    NotificationOptions.Email |
    NotificationOptions.Push;

bool hasPush = (options & NotificationOptions.Push) == NotificationOptions.Push;

Console.WriteLine(hasPush);
```

Output:

```text
True
```

Binary view:

```text
Options = 0101
Push    = 0100
--------------
Result  = 0100
```

---

## 4. Adding a Flag

Use bitwise OR `|`.

```csharp
NotificationOptions options = NotificationOptions.Email;

options |= NotificationOptions.Sms;

Console.WriteLine(options);
```

Output:

```text
Email, Sms
```

---

## 5. Removing a Flag

Use bitwise AND `&` with bitwise complement `~`.

```csharp
NotificationOptions options =
    NotificationOptions.Email |
    NotificationOptions.Sms |
    NotificationOptions.Push;

options &= ~NotificationOptions.Sms;

Console.WriteLine(options);
```

Output:

```text
Email, Push
```

---

## 6. Toggling a Flag

Use bitwise XOR `^`.

```csharp
NotificationOptions options =
    NotificationOptions.Email |
    NotificationOptions.Push;

options ^= NotificationOptions.Push;

Console.WriteLine(options);
```

Output:

```text
Email
```

Toggling again adds it back:

```csharp
options ^= NotificationOptions.Push;

Console.WriteLine(options);
```

Output:

```text
Email, Push
```

---

# ⚙️ The `[Flags]` Attribute

## 1. What Does `[Flags]` Do?

The `[Flags]` attribute tells C# that an enum is intended to be used as a bit field.

```csharp
[Flags]
enum FilePermissions
{
    None = 0,
    Read = 1,
    Write = 2,
    Execute = 4
}
```

> ✅ `[Flags]` mainly affects how enum values are displayed as strings.  
> The bitwise operations work even without `[Flags]`, but the output is much clearer with it.

---

## 2. Without `[Flags]`

```csharp
enum FilePermissions
{
    None = 0,
    Read = 1,
    Write = 2,
    Execute = 4
}
```

```csharp
FilePermissions permissions =
    FilePermissions.Read |
    FilePermissions.Write;

Console.WriteLine(permissions);
```

Possible output:

```text
3
```

---

## 3. With `[Flags]`

```csharp
[Flags]
enum FilePermissions
{
    None = 0,
    Read = 1,
    Write = 2,
    Execute = 4
}
```

```csharp
FilePermissions permissions =
    FilePermissions.Read |
    FilePermissions.Write;

Console.WriteLine(permissions);
```

Output:

```text
Read, Write
```

---

# 🧱 Designing a Good Flags Enum

## 1. Always Include `None = 0`

```csharp
[Flags]
enum DocumentAccess
{
    None = 0,
    View = 1,
    Edit = 2,
    Comment = 4,
    Share = 8,
    Archive = 16
}
```

`None` means no flags are selected.

```csharp
DocumentAccess access = DocumentAccess.None;
```

---

## 2. Use Powers of Two

✅ Good:

```csharp
[Flags]
enum DeviceFeatures
{
    None = 0,
    Bluetooth = 1,
    WiFi = 2,
    Gps = 4,
    Camera = 8
}
```

❌ Bad:

```csharp
[Flags]
enum DeviceFeatures
{
    None = 0,
    Bluetooth = 1,
    WiFi = 2,
    Gps = 3,
    Camera = 4
}
```

Why is `Gps = 3` bad?

```text
Bluetooth = 0001
WiFi      = 0010
Gps       = 0011
```

`Gps` accidentally overlaps with `Bluetooth | WiFi`.

---

## 3. Use Shift Operators for Clarity

Instead of manually writing `1`, `2`, `4`, `8`, you can use bit shifting.

```csharp
[Flags]
enum ExportOptions
{
    None = 0,
    IncludeHeaders = 1 << 0,    // 1
    IncludeTotals = 1 << 1,     // 2
    CompressFile = 1 << 2,      // 4
    EncryptFile = 1 << 3        // 8
}
```

This makes it clear that each member uses a different bit.

---

## 4. Define Common Combinations

You can create named combinations for frequently used groups.

```csharp
[Flags]
enum DocumentAccess
{
    None = 0,
    View = 1,
    Edit = 2,
    Comment = 4,
    Share = 8,
    Archive = 16,

    Basic = View | Comment,
    Contributor = View | Edit | Comment,
    Manager = View | Edit | Comment | Share,
    Full = View | Edit | Comment | Share | Archive
}
```

Usage:

```csharp
DocumentAccess access = DocumentAccess.Contributor;

Console.WriteLine(access);
```

Output:

```text
Contributor
```

Checking individual permissions:

```csharp
bool canEdit = access.HasFlag(DocumentAccess.Edit);

Console.WriteLine(canEdit);
```

Output:

```text
True
```

---

# 🧪 Practical Flags Example: User Permissions

## 1. Define the Flags Enum

```csharp
[Flags]
enum UserPermissions
{
    None = 0,
    ReadReports = 1,
    CreateReports = 2,
    EditReports = 4,
    DeleteReports = 8,
    ManageUsers = 16,
    ConfigureSystem = 32
}
```

---

## 2. Assign Multiple Permissions

```csharp
UserPermissions permissions =
    UserPermissions.ReadReports |
    UserPermissions.CreateReports |
    UserPermissions.EditReports;

Console.WriteLine(permissions);
```

Output:

```text
ReadReports, CreateReports, EditReports
```

---

## 3. Check for a Permission

```csharp
if (permissions.HasFlag(UserPermissions.EditReports))
{
    Console.WriteLine("The user can edit reports.");
}
```

Output:

```text
The user can edit reports.
```

---

## 4. Add a Permission

```csharp
permissions |= UserPermissions.DeleteReports;

Console.WriteLine(permissions);
```

Output:

```text
ReadReports, CreateReports, EditReports, DeleteReports
```

---

## 5. Remove a Permission

```csharp
permissions &= ~UserPermissions.CreateReports;

Console.WriteLine(permissions);
```

Output:

```text
ReadReports, EditReports, DeleteReports
```

---

## 6. Check for Multiple Permissions

```csharp
UserPermissions required =
    UserPermissions.ReadReports |
    UserPermissions.EditReports;

bool hasRequired = (permissions & required) == required;

Console.WriteLine(hasRequired);
```

Output:

```text
True
```

---

## 7. Check for Any Permission

```csharp
UserPermissions moderation =
    UserPermissions.DeleteReports |
    UserPermissions.ManageUsers;

bool hasAnyModerationPermission = (permissions & moderation) != 0;

Console.WriteLine(hasAnyModerationPermission);
```

Output:

```text
True
```

---

# 🧰 Bitwise Operators for Flags

| Operator | Name | Purpose | Example |
|---|---|---|---|
| `|` | OR | Add/combine flags | `Read | Write` |
| `&` | AND | Check whether flags exist | `value & Read` |
| `~` | NOT | Invert flags | `~Write` |
| `^` | XOR | Toggle flags | `value ^ Execute` |

---

## 1. OR `|`: Combine Flags

```csharp
FilePermissions permissions =
    FilePermissions.Read |
    FilePermissions.Write;
```

---

## 2. AND `&`: Check Flags

```csharp
bool canRead =
    (permissions & FilePermissions.Read) == FilePermissions.Read;
```

---

## 3. NOT `~`: Remove Flags

```csharp
permissions &= ~FilePermissions.Write;
```

---

## 4. XOR `^`: Toggle Flags

```csharp
permissions ^= FilePermissions.Execute;
```

---

# 📦 Flags with Methods

## 1. Passing Flags to a Method

```csharp
[Flags]
enum ReportOptions
{
    None = 0,
    IncludeCharts = 1,
    IncludeRawData = 2,
    IncludeAuditTrail = 4,
    SendByEmail = 8
}
```

```csharp
class ReportGenerator
{
    public void Generate(ReportOptions options)
    {
        if (options.HasFlag(ReportOptions.IncludeCharts))
        {
            Console.WriteLine("Adding charts...");
        }

        if (options.HasFlag(ReportOptions.IncludeRawData))
        {
            Console.WriteLine("Adding raw data...");
        }

        if (options.HasFlag(ReportOptions.IncludeAuditTrail))
        {
            Console.WriteLine("Adding audit trail...");
        }

        if (options.HasFlag(ReportOptions.SendByEmail))
        {
            Console.WriteLine("Emailing the report...");
        }
    }
}
```

Usage:

```csharp
var generator = new ReportGenerator();

generator.Generate(
    ReportOptions.IncludeCharts |
    ReportOptions.SendByEmail
);
```

Output:

```text
Adding charts...
Emailing the report...
```

---

# 🧾 Flags with Strings

## 1. Converting Flags to a String

```csharp
FilePermissions permissions =
    FilePermissions.Read |
    FilePermissions.Execute;

string text = permissions.ToString();

Console.WriteLine(text);
```

Output:

```text
Read, Execute
```

---

## 2. Parsing Flags from a String

```csharp
string input = "Read, Write";

FilePermissions permissions =
    Enum.Parse<FilePermissions>(input);

Console.WriteLine(permissions);
```

Output:

```text
Read, Write
```

---

## 3. Safer Parsing with `TryParse`

```csharp
string input = "Read, Execute";

if (Enum.TryParse(input, out FilePermissions permissions))
{
    Console.WriteLine($"Permissions: {permissions}");
}
else
{
    Console.WriteLine("Invalid permissions.");
}
```

Output:

```text
Permissions: Read, Execute
```

---

# 🧨 Common Mistakes with Enums

## 1. Using Magic Numbers Instead of Enums

❌ Avoid:

```csharp
int orderStatus = 3;

if (orderStatus == 3)
{
    Console.WriteLine("Order delivered.");
}
```

✅ Prefer:

```csharp
OrderStatus orderStatus = OrderStatus.Delivered;

if (orderStatus == OrderStatus.Delivered)
{
    Console.WriteLine("Order delivered.");
}
```

---

## 2. Forgetting a Zero Value

❌ Avoid:

```csharp
enum AccountState
{
    Active = 1,
    Suspended = 2,
    Closed = 3
}
```

✅ Prefer:

```csharp
enum AccountState
{
    Unknown = 0,
    Active = 1,
    Suspended = 2,
    Closed = 3
}
```

---

## 3. Using Flags Without Powers of Two

❌ Avoid:

```csharp
[Flags]
enum PrintOptions
{
    None = 0,
    DoubleSided = 1,
    Color = 2,
    Stapled = 3
}
```

✅ Prefer:

```csharp
[Flags]
enum PrintOptions
{
    None = 0,
    DoubleSided = 1,
    Color = 2,
    Stapled = 4
}
```

---

## 4. Using an Enum When a Class Is Better

Enums are good for fixed sets of values.

✅ Good enum use:

```csharp
enum TemperatureUnit
{
    Celsius,
    Fahrenheit,
    Kelvin
}
```

But if each option needs lots of behavior, data, or frequent changes, a class may be better.

❌ Possible overuse:

```csharp
enum SubscriptionPlan
{
    Starter,
    Growth,
    Business,
    Enterprise
}
```

If each plan has different limits, prices, rules, discounts, and features, you may prefer classes or configuration.

---

# 🧬 Enum as Method Parameters and Return Values

## 1. Enum as a Parameter

```csharp
enum LogLevel
{
    Trace,
    Debug,
    Info,
    Warning,
    Error
}
```

```csharp
void WriteLog(LogLevel level, string message)
{
    Console.WriteLine($"[{level}] {message}");
}
```

Usage:

```csharp
WriteLog(LogLevel.Warning, "Disk space is below 15%.");
```

Output:

```text
[Warning] Disk space is below 15%.
```

---

## 2. Enum as a Return Value

```csharp
enum DiscountCategory
{
    None,
    Seasonal,
    Loyalty,
    Clearance
}
```

```csharp
DiscountCategory GetDiscountCategory(decimal orderTotal)
{
    if (orderTotal >= 500m)
    {
        return DiscountCategory.Loyalty;
    }

    if (orderTotal >= 150m)
    {
        return DiscountCategory.Seasonal;
    }

    return DiscountCategory.None;
}
```

Usage:

```csharp
DiscountCategory category = GetDiscountCategory(275m);

Console.WriteLine(category);
```

Output:

```text
Seasonal
```

---

# 🧩 Enum in Properties

```csharp
enum ProductCondition
{
    Unknown = 0,
    New = 1,
    Refurbished = 2,
    Used = 3
}
```

```csharp
class Product
{
    public string Name { get; set; }
    public ProductCondition Condition { get; set; }
}
```

Usage:

```csharp
var product = new Product
{
    Name = "Wireless Keyboard",
    Condition = ProductCondition.Refurbished
};

Console.WriteLine($"{product.Name}: {product.Condition}");
```

Output:

```text
Wireless Keyboard: Refurbished
```

---

# 🧮 Enum Formatting

## 1. General Format: `G`

```csharp
InvoiceState state = InvoiceState.Paid;

Console.WriteLine(state.ToString("G"));
```

Output:

```text
Paid
```

---

## 2. Decimal Format: `D`

```csharp
InvoiceState state = InvoiceState.Paid;

Console.WriteLine(state.ToString("D"));
```

Output:

```text
3
```

---

## 3. Hexadecimal Format: `X`

```csharp
InvoiceState state = InvoiceState.Paid;

Console.WriteLine(state.ToString("X"));
```

Possible output:

```text
00000003
```

---

## 4. Flags Format: `F`

```csharp
FilePermissions permissions =
    FilePermissions.Read |
    FilePermissions.Write;

Console.WriteLine(permissions.ToString("F"));
```

Output:

```text
Read, Write
```

---

# 🏗️ Full Example: Feature Flags

## 1. Define the Enum

```csharp
[Flags]
enum ApplicationFeatures
{
    None = 0,
    DarkMode = 1 << 0,
    CloudSync = 1 << 1,
    OfflineAccess = 1 << 2,
    AdvancedSearch = 1 << 3,
    BetaDashboard = 1 << 4
}
```

---

## 2. Create a Class That Uses the Flags

```csharp
class UserProfile
{
    public string Username { get; set; }
    public ApplicationFeatures EnabledFeatures { get; private set; }

    public UserProfile(string username)
    {
        Username = username;
        EnabledFeatures = ApplicationFeatures.None;
    }

    public void EnableFeature(ApplicationFeatures feature)
    {
        EnabledFeatures |= feature;
    }

    public void DisableFeature(ApplicationFeatures feature)
    {
        EnabledFeatures &= ~feature;
    }

    public bool HasFeature(ApplicationFeatures feature)
    {
        return (EnabledFeatures & feature) == feature;
    }
}
```

---

## 3. Use the Class

```csharp
var profile = new UserProfile("mina_47");

profile.EnableFeature(ApplicationFeatures.DarkMode);
profile.EnableFeature(ApplicationFeatures.CloudSync);
profile.EnableFeature(ApplicationFeatures.AdvancedSearch);

Console.WriteLine(profile.EnabledFeatures);
```

Output:

```text
DarkMode, CloudSync, AdvancedSearch
```

---

## 4. Check a Feature

```csharp
if (profile.HasFeature(ApplicationFeatures.CloudSync))
{
    Console.WriteLine("Cloud sync is available.");
}
```

Output:

```text
Cloud sync is available.
```

---

## 5. Disable a Feature

```csharp
profile.DisableFeature(ApplicationFeatures.DarkMode);

Console.WriteLine(profile.EnabledFeatures);
```

Output:

```text
CloudSync, AdvancedSearch
```

---

# 🧠 Quick Reference

## Normal Enum

```csharp
enum OrderStatus
{
    Unknown = 0,
    Pending = 1,
    Processing = 2,
    Shipped = 3,
    Delivered = 4
}
```

Use when the value should be **one option**.

```csharp
OrderStatus status = OrderStatus.Shipped;
```

---

## Flags Enum

```csharp
[Flags]
enum FilePermissions
{
    None = 0,
    Read = 1,
    Write = 2,
    Execute = 4
}
```

Use when the value can be **multiple options combined**.

```csharp
FilePermissions permissions =
    FilePermissions.Read |
    FilePermissions.Write;
```

---

## Common Flag Operations

| Task | Code |
|---|---|
| Add a flag | `value |= SomeFlag;` |
| Remove a flag | `value &= ~SomeFlag;` |
| Toggle a flag | `value ^= SomeFlag;` |
| Check one flag | `(value & SomeFlag) == SomeFlag` |
| Check any flag | `(value & group) != 0` |
| Check all flags | `(value & group) == group` |