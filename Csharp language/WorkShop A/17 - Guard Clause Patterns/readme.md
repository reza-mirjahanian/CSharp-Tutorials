# Early Return / Guard Clause Style `if` Statements

## What Is an Early Return?

An **early return** means exiting a method as soon as you know there is nothing more to do.

Instead of writing deeply nested `if` statements, you check special cases first and `return` immediately.

---

## Basic Idea

### Nested style

```csharp
void SendInvoice(Customer customer)
{
    if (customer != null)
    {
        if (customer.IsActive)
        {
            if (customer.Email != null)
            {
                Console.WriteLine($"Invoice sent to {customer.Email}");
            }
        }
    }
}
```

This works, but the main logic is buried inside multiple levels of indentation.

---

### Early return style

```csharp
void SendInvoice(Customer customer)
{
    if (customer == null)
        return;

    if (!customer.IsActive)
        return;

    if (customer.Email == null)
        return;

    Console.WriteLine($"Invoice sent to {customer.Email}");
}
```

Now the invalid cases are handled first, and the main action is easy to see.

---

# Guard Clauses

## Meaning

A **guard clause** is an `if` statement placed near the start of a method to protect the rest of the code from invalid or unwanted conditions.

> A guard clause says:  
> “If this condition is not acceptable, stop here.”

---

## Common Shape

```csharp
void DoSomething(Input input)
{
    if (input == null)
        return;

    // Main logic here
}
```

Or with exceptions:

```csharp
void SaveUser(User user)
{
    if (user == null)
        throw new ArgumentNullException(nameof(user));

    // Main logic here
}
```

---

# Why Use Early Returns?

## 1. Less Nesting

### Without early return

```csharp
void ProcessPayment(Order order)
{
    if (order != null)
    {
        if (order.TotalAmount > 0)
        {
            if (!order.IsCancelled)
            {
                ChargeCard(order);
            }
        }
    }
}
```

### With early return

```csharp
void ProcessPayment(Order order)
{
    if (order == null)
        return;

    if (order.TotalAmount <= 0)
        return;

    if (order.IsCancelled)
        return;

    ChargeCard(order);
}
```

The second version is flatter and easier to scan.

---

## 2. Main Logic Stays at the Normal Indentation Level

```csharp
void PublishArticle(Article article)
{
    if (article == null)
        return;

    if (string.IsNullOrWhiteSpace(article.Title))
        return;

    if (article.IsDraft)
        return;

    Publish(article);
}
```

The important action is clear:

```csharp
Publish(article);
```

It is not hidden inside several nested blocks.

---

## 3. Invalid Cases Are Handled Immediately

```csharp
void ApplyDiscount(Customer customer)
{
    if (customer == null)
        return;

    if (!customer.HasMembership)
        return;

    customer.DiscountPercent = 10;
}
```

This reads like a checklist:

1. No customer? Stop.
2. No membership? Stop.
3. Otherwise, apply discount.

---

# Early Return with `void` Methods

For methods that return nothing, use plain `return;`.

```csharp
void SendNotification(User user)
{
    if (user == null)
        return;

    if (!user.AllowNotifications)
        return;

    Console.WriteLine($"Notification sent to {user.Email}");
}
```

---

# Early Return with Return Values

If the method returns a value, each early return must return a valid value.

```csharp
decimal CalculateShipping(Order order)
{
    if (order == null)
        return 0;

    if (order.TotalAmount <= 0)
        return 0;

    if (order.TotalAmount >= 100)
        return 0;

    return 8.99m;
}
```

---

## Example with Boolean Result

```csharp
bool CanLogin(User user)
{
    if (user == null)
        return false;

    if (!user.IsActive)
        return false;

    if (user.IsLocked)
        return false;

    return true;
}
```

This is clearer than:

```csharp
bool CanLogin(User user)
{
    if (user != null)
    {
        if (user.IsActive)
        {
            if (!user.IsLocked)
            {
                return true;
            }
        }
    }

    return false;
}
```

---

# Guard Clauses with Exceptions

Sometimes returning silently is not enough.

If invalid input is a programming error, throw an exception.

```csharp
void RegisterProduct(Product product)
{
    if (product == null)
        throw new ArgumentNullException(nameof(product));

    if (string.IsNullOrWhiteSpace(product.Name))
        throw new ArgumentException("Product name is required.", nameof(product));

    SaveProduct(product);
}
```

---

## Return vs Throw

| Situation | Prefer |
|---|---|
| Invalid input means “nothing to do” | `return` |
| Invalid input means caller made a mistake | `throw` |
| Business rule fails normally | `return false` or a result object |
| Data is missing unexpectedly | Often `throw` |
| Optional operation cannot continue | `return` |

---

# Guard Clauses with Collections

## Checking for `null`

```csharp
void PrintNames(List<string> names)
{
    if (names == null)
        return;

    foreach (string name in names)
    {
        Console.WriteLine(name);
    }
}
```

---

## Checking for Empty Collection

```csharp
void PrintNames(List<string> names)
{
    if (names == null)
        return;

    if (names.Count == 0)
        return;

    foreach (string name in names)
    {
        Console.WriteLine(name);
    }
}
```

---

## Using Pattern Matching

```csharp
void PrintNames(List<string> names)
{
    if (names is null or { Count: 0 })
        return;

    foreach (string name in names)
    {
        Console.WriteLine(name);
    }
}
```

---

# Guard Clauses with Strings

```csharp
void SetDisplayName(string name)
{
    if (string.IsNullOrWhiteSpace(name))
        return;

    Console.WriteLine($"Display name set to {name}");
}
```

---

## With Exception

```csharp
void CreateCategory(string title)
{
    if (string.IsNullOrWhiteSpace(title))
        throw new ArgumentException("Category title is required.", nameof(title));

    Console.WriteLine($"Category created: {title}");
}
```

---

# Combining Conditions

Sometimes multiple guard clauses can be combined.

```csharp
bool CanAccessReport(User user)
{
    if (user == null || !user.IsActive || user.IsSuspended)
        return false;

    return true;
}
```

This is fine when the conditions are simple.

---

## Separate Conditions Can Be Clearer

```csharp
bool CanAccessReport(User user)
{
    if (user == null)
        return false;

    if (!user.IsActive)
        return false;

    if (user.IsSuspended)
        return false;

    return true;
}
```

This version is easier to debug and extend.

---

# Early Return with `else`

With early returns, `else` is often unnecessary.

## Less preferred

```csharp
string GetStatus(Order order)
{
    if (order == null)
    {
        return "Missing";
    }
    else
    {
        return order.IsPaid ? "Paid" : "Unpaid";
    }
}
```

---

## Cleaner

```csharp
string GetStatus(Order order)
{
    if (order == null)
        return "Missing";

    return order.IsPaid ? "Paid" : "Unpaid";
}
```

Once the `if` returns, the rest of the method only runs when the condition was false.

---

# Early Return with Async Methods

Early return works the same way in `async` methods.

```csharp
async Task SendReceiptAsync(Order order)
{
    if (order == null)
        return;

    if (!order.IsPaid)
        return;

    await EmailService.SendAsync(order.CustomerEmail, "Your receipt is ready.");
}
```

---

## Async Method Returning a Value

```csharp
async Task<bool> TrySendReceiptAsync(Order order)
{
    if (order == null)
        return false;

    if (!order.IsPaid)
        return false;

    await EmailService.SendAsync(order.CustomerEmail, "Your receipt is ready.");

    return true;
}
```

---

# Guard Clauses in Constructors

Guard clauses are very common in constructors.

```csharp
class InvoiceService
{
    private readonly PaymentGateway _paymentGateway;

    public InvoiceService(PaymentGateway paymentGateway)
    {
        if (paymentGateway == null)
            throw new ArgumentNullException(nameof(paymentGateway));

        _paymentGateway = paymentGateway;
    }
}
```

---

## Shorter Version

```csharp
class InvoiceService
{
    private readonly PaymentGateway _paymentGateway;

    public InvoiceService(PaymentGateway paymentGateway)
    {
        _paymentGateway = paymentGateway 
            ?? throw new ArgumentNullException(nameof(paymentGateway));
    }
}
```

---

# Guard Clauses with `??`

The null-coalescing operator can make guard clauses shorter.

```csharp
void UpdateProfile(User user)
{
    user = user ?? throw new ArgumentNullException(nameof(user));

    Console.WriteLine($"Updating profile for {user.Name}");
}
```

More commonly:

```csharp
void UpdateProfile(User user)
{
    if (user == null)
        throw new ArgumentNullException(nameof(user));

    Console.WriteLine($"Updating profile for {user.Name}");
}
```

---

# Guard Clauses with `?.` and `??`

You can combine guard clauses with null-safe access.

```csharp
void SendMessage(User user)
{
    string email = user?.Email ?? "";

    if (string.IsNullOrWhiteSpace(email))
        return;

    Console.WriteLine($"Message sent to {email}");
}
```

---

# Realistic Example

## Nested Version

```csharp
void CompleteOrder(Order order)
{
    if (order != null)
    {
        if (!order.IsCancelled)
        {
            if (order.Items != null && order.Items.Count > 0)
            {
                if (order.Customer != null)
                {
                    if (!string.IsNullOrWhiteSpace(order.Customer.Email))
                    {
                        order.Status = "Completed";
                        SendConfirmation(order.Customer.Email);
                    }
                }
            }
        }
    }
}
```

---

## Guard Clause Version

```csharp
void CompleteOrder(Order order)
{
    if (order == null)
        return;

    if (order.IsCancelled)
        return;

    if (order.Items == null || order.Items.Count == 0)
        return;

    if (order.Customer == null)
        return;

    if (string.IsNullOrWhiteSpace(order.Customer.Email))
        return;

    order.Status = "Completed";
    SendConfirmation(order.Customer.Email);
}
```

---

# Same Example with Exceptions for Invalid Data

```csharp
void CompleteOrder(Order order)
{
    if (order == null)
        throw new ArgumentNullException(nameof(order));

    if (order.IsCancelled)
        throw new InvalidOperationException("Cancelled orders cannot be completed.");

    if (order.Items == null || order.Items.Count == 0)
        throw new InvalidOperationException("Order must contain at least one item.");

    if (order.Customer == null)
        throw new InvalidOperationException("Order must have a customer.");

    if (string.IsNullOrWhiteSpace(order.Customer.Email))
        throw new InvalidOperationException("Customer email is required.");

    order.Status = "Completed";
    SendConfirmation(order.Customer.Email);
}
```

---

# Formatting Styles

## With Braces

```csharp
void ActivateAccount(Account account)
{
    if (account == null)
    {
        return;
    }

    if (account.IsActive)
    {
        return;
    }

    account.IsActive = true;
}
```

---

## Without Braces

```csharp
void ActivateAccount(Account account)
{
    if (account == null)
        return;

    if (account.IsActive)
        return;

    account.IsActive = true;
}
```

Both are valid. Many teams prefer braces because they reduce mistakes when adding lines later.

---

# Common Guard Clause Patterns

## Null Check

```csharp
if (customer == null)
    return;
```

---

## Empty String Check

```csharp
if (string.IsNullOrWhiteSpace(title))
    return;
```

---

## Empty Collection Check

```csharp
if (items == null || items.Count == 0)
    return;
```

---

## Invalid Number Check

```csharp
if (quantity <= 0)
    return;
```

---

## Already Done Check

```csharp
if (invoice.IsPaid)
    return;
```

---

## Permission Check

```csharp
if (!user.CanEdit)
    return;
```

---

# Before and After

## Before

```csharp
void AddReview(Product product, Review review)
{
    if (product != null)
    {
        if (review != null)
        {
            if (!string.IsNullOrWhiteSpace(review.Text))
            {
                product.Reviews.Add(review);
            }
        }
    }
}
```

---

## After

```csharp
void AddReview(Product product, Review review)
{
    if (product == null)
        return;

    if (review == null)
        return;

    if (string.IsNullOrWhiteSpace(review.Text))
        return;

    product.Reviews.Add(review);
}
```

---

# When Early Return Is Especially Useful

| Situation | Example |
|---|---|
| Validating input | `if (user == null) return;` |
| Checking permissions | `if (!user.CanDelete) return;` |
| Skipping unnecessary work | `if (items.Count == 0) return;` |
| Avoiding nested logic | Replace nested `if` blocks |
| Handling edge cases first | `if (order.IsCancelled) return;` |
| Exiting async operations early | `if (!shouldSend) return;` |

---

# When to Be Careful

## Too Many Returns in Complex Methods

Early returns usually improve readability, but too many return paths in a very long method can make behavior harder to track.

```csharp
void ProcessFile(FileInfo file)
{
    if (file == null)
        return;

    if (!file.Exists)
        return;

    if (file.Length == 0)
        return;

    if (file.Extension != ".csv")
        return;

    ImportFile(file);
}
```

This is fine.

But if a method has dozens of returns mixed throughout a large body, consider splitting it into smaller methods.

---

## Cleanup Code

If your method must always run cleanup code, use `try/finally`, `using`, or similar patterns.

```csharp
void WriteReport(Stream stream)
{
    if (stream == null)
        return;

    using StreamWriter writer = new StreamWriter(stream);

    writer.WriteLine("Report started");

    if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
        return;

    writer.WriteLine("Report finished");
}
```

The `using` statement still disposes the writer even with early return.

---

# Practical Rule

Use guard clauses for conditions that make the rest of the method unnecessary or invalid.

```csharp
void HandleRequest(Request request)
{
    if (request == null)
        return;

    if (!request.IsAuthenticated)
        return;

    if (request.Payload == null)
        return;

    ProcessPayload(request.Payload);
}
```