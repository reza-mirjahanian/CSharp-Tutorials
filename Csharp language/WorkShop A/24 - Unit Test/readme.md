# Unit Testing in C#

## 1. What Is a Unit Test?

A **unit test** checks a small, isolated piece of code to make sure it works correctly.

Usually, a unit is:

- A method
- A class
- A small component with one responsibility

For example, if you have a method that calculates discounts, a unit test can verify that the method returns the correct discount for different inputs.

```csharp
public decimal CalculateDiscount(decimal price, decimal discountPercent)
{
    return price * discountPercent / 100;
}
```

A unit test for this method might check:

- `CalculateDiscount(200, 10)` returns `20`
- `CalculateDiscount(80, 25)` returns `20`
- `CalculateDiscount(150, 0)` returns `0`

---

# 2. Why Unit Testing Matters

Unit tests help you:

- ✅ Catch bugs early
- ✅ Refactor code safely
- ✅ Document expected behavior
- ✅ Improve code design
- ✅ Reduce manual testing
- ✅ Prevent old bugs from returning

> A good unit test tells you:  
> **“This part of the application behaves exactly as expected.”**

---

# 3. Common Unit Testing Frameworks in C#

C# has several popular unit testing frameworks.

| Framework | Description |
|---|---|
| **xUnit** | Modern and widely used in .NET projects |
| **NUnit** | Mature, flexible, and feature-rich |
| **MSTest** | Microsoft’s built-in testing framework |

This lesson mainly uses **xUnit**, because it is common in modern .NET development.

---

# 4. Creating a Simple Class to Test

Imagine you have a class called `PriceCalculator`.

```csharp
public class PriceCalculator
{
    public decimal AddTax(decimal price, decimal taxRate)
    {
        return price + price * taxRate / 100;
    }

    public decimal ApplyDiscount(decimal price, decimal discountRate)
    {
        return price - price * discountRate / 100;
    }
}
```

This class has two methods:

| Method | Purpose |
|---|---|
| `AddTax` | Adds tax to a price |
| `ApplyDiscount` | Reduces a price by a discount percentage |

---

# 5. Creating a Unit Test Project

In a typical .NET solution, you may have:

```text
ShopApp
│
├── ShopApp
│   └── PriceCalculator.cs
│
└── ShopApp.Tests
    └── PriceCalculatorTests.cs
```

You can create an xUnit test project using the .NET CLI:

```bash
dotnet new xunit -n ShopApp.Tests
```

Then add a reference to the main project:

```bash
dotnet add ShopApp.Tests reference ShopApp
```

---

# 6. Writing Your First Unit Test

## Production Code

```csharp
public class PriceCalculator
{
    public decimal AddTax(decimal price, decimal taxRate)
    {
        return price + price * taxRate / 100;
    }
}
```

## Test Code

```csharp
using Xunit;

public class PriceCalculatorTests
{
    [Fact]
    public void AddTax_WhenTaxRateIsTenPercent_ReturnsPriceWithTax()
    {
        // Arrange
        var calculator = new PriceCalculator();

        // Act
        decimal result = calculator.AddTax(300m, 10m);

        // Assert
        Assert.Equal(330m, result);
    }
}
```

---

# 7. The AAA Pattern

Most unit tests follow the **AAA pattern**:

| Step | Meaning | Example |
|---|---|---|
| **Arrange** | Prepare objects and data | Create `PriceCalculator` |
| **Act** | Execute the method being tested | Call `AddTax()` |
| **Assert** | Check the result | Verify result equals `330` |

```csharp
[Fact]
public void ApplyDiscount_WhenDiscountIsTwentyPercent_ReturnsReducedPrice()
{
    // Arrange
    var calculator = new PriceCalculator();

    // Act
    decimal result = calculator.ApplyDiscount(250m, 20m);

    // Assert
    Assert.Equal(200m, result);
}
```

---

# 8. Test Method Naming

A good test name describes:

1. **The method being tested**
2. **The condition**
3. **The expected result**

A useful naming pattern is:

```text
MethodName_WhenCondition_ReturnsExpectedResult
```

Examples:

```csharp
AddTax_WhenTaxRateIsZero_ReturnsOriginalPrice
ApplyDiscount_WhenDiscountIsFiftyPercent_ReturnsHalfPrice
CalculateTotal_WhenCartIsEmpty_ReturnsZero
```

Good test names make failures easy to understand.

---

# 9. Using `[Fact]`

In xUnit, `[Fact]` is used for a test with no parameters.

```csharp
[Fact]
public void AddTax_WhenTaxRateIsZero_ReturnsOriginalPrice()
{
    var calculator = new PriceCalculator();

    decimal result = calculator.AddTax(420m, 0m);

    Assert.Equal(420m, result);
}
```

Use `[Fact]` when the test checks one specific scenario.

---

# 10. Using `[Theory]` for Multiple Test Cases

If you want to test the same method with different inputs, use `[Theory]`.

```csharp
public class PriceCalculator
{
    public decimal ApplyDiscount(decimal price, decimal discountRate)
    {
        return price - price * discountRate / 100;
    }
}
```

```csharp
using Xunit;

public class PriceCalculatorTests
{
    [Theory]
    [InlineData(100, 10, 90)]
    [InlineData(500, 20, 400)]
    [InlineData(80, 25, 60)]
    public void ApplyDiscount_WithVariousDiscountRates_ReturnsExpectedPrice(
        decimal price,
        decimal discountRate,
        decimal expected)
    {
        var calculator = new PriceCalculator();

        decimal result = calculator.ApplyDiscount(price, discountRate);

        Assert.Equal(expected, result);
    }
}
```

## `[Fact]` vs `[Theory]`

| Attribute | Use Case |
|---|---|
| `[Fact]` | One fixed test case |
| `[Theory]` | Same test logic with multiple data sets |

---

# 11. Common Assertions in xUnit

Assertions verify expected behavior.

## Equality

```csharp
Assert.Equal(120, result);
```

## Not Equal

```csharp
Assert.NotEqual(0, result);
```

## True or False

```csharp
Assert.True(user.IsActive);
Assert.False(order.IsCancelled);
```

## Null Checks

```csharp
Assert.Null(customer.MiddleName);
Assert.NotNull(invoice);
```

## Contains

```csharp
Assert.Contains("coffee", productNames);
```

## Empty Collections

```csharp
Assert.Empty(cart.Items);
Assert.NotEmpty(order.Items);
```

## Type Checks

```csharp
Assert.IsType<PremiumCustomer>(customer);
```

---

# 12. Testing Exceptions

Sometimes a method should throw an exception for invalid input.

## Production Code

```csharp
public class BankAccount
{
    public decimal Balance { get; private set; }

    public BankAccount(decimal openingBalance)
    {
        Balance = openingBalance;
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Withdrawal amount must be greater than zero.");
        }

        if (amount > Balance)
        {
            throw new InvalidOperationException("Insufficient account balance.");
        }

        Balance -= amount;
    }
}
```

## Test for Exception

```csharp
[Fact]
public void Withdraw_WhenAmountIsNegative_ThrowsArgumentException()
{
    var account = new BankAccount(900m);

    Assert.Throws<ArgumentException>(() => account.Withdraw(-50m));
}
```

## Test Exception Message

```csharp
[Fact]
public void Withdraw_WhenAmountExceedsBalance_ThrowsExpectedMessage()
{
    var account = new BankAccount(300m);

    var exception = Assert.Throws<InvalidOperationException>(
        () => account.Withdraw(700m)
    );

    Assert.Equal("Insufficient account balance.", exception.Message);
}
```

---

# 13. Testing State Changes

Some methods change the internal state of an object.

```csharp
public class BankAccount
{
    public decimal Balance { get; private set; }

    public BankAccount(decimal openingBalance)
    {
        Balance = openingBalance;
    }

    public void Deposit(decimal amount)
    {
        Balance += amount;
    }
}
```

Test:

```csharp
[Fact]
public void Deposit_WhenAmountIsValid_IncreasesBalance()
{
    var account = new BankAccount(400m);

    account.Deposit(150m);

    Assert.Equal(550m, account.Balance);
}
```

---

# 14. Testing Return Values

Some methods simply return a value.

```csharp
public class ShippingCalculator
{
    public decimal CalculateShipping(decimal orderTotal)
    {
        if (orderTotal >= 1000m)
        {
            return 0m;
        }

        return 45m;
    }
}
```

Test:

```csharp
[Theory]
[InlineData(1000, 0)]
[InlineData(1500, 0)]
[InlineData(300, 45)]
public void CalculateShipping_BasedOnOrderTotal_ReturnsCorrectShippingCost(
    decimal orderTotal,
    decimal expectedShipping)
{
    var calculator = new ShippingCalculator();

    decimal result = calculator.CalculateShipping(orderTotal);

    Assert.Equal(expectedShipping, result);
}
```

---

# 15. Testing Boolean Logic

```csharp
public class AgeValidator
{
    public bool CanCreateAccount(int age)
    {
        return age >= 18;
    }
}
```

```csharp
public class AgeValidatorTests
{
    [Theory]
    [InlineData(18, true)]
    [InlineData(25, true)]
    [InlineData(17, false)]
    public void CanCreateAccount_WhenAgeIsChecked_ReturnsExpectedResult(
        int age,
        bool expected)
    {
        var validator = new AgeValidator();

        bool result = validator.CanCreateAccount(age);

        Assert.Equal(expected, result);
    }
}
```

---

# 16. Testing Collections

## Production Code

```csharp
public class ShoppingCart
{
    private readonly List<string> _items = new();

    public IReadOnlyList<string> Items => _items;

    public void AddItem(string itemName)
    {
        _items.Add(itemName);
    }
}
```

## Tests

```csharp
public class ShoppingCartTests
{
    [Fact]
    public void AddItem_WhenItemIsAdded_CartContainsItem()
    {
        var cart = new ShoppingCart();

        cart.AddItem("Notebook");

        Assert.Contains("Notebook", cart.Items);
    }

    [Fact]
    public void NewCart_WhenCreated_HasNoItems()
    {
        var cart = new ShoppingCart();

        Assert.Empty(cart.Items);
    }
}
```

---

# 17. Testing Private Methods

Usually, you should **not test private methods directly**.

Private methods are implementation details. Instead, test them through public methods.

```csharp
public class InvoiceCalculator
{
    public decimal CalculateFinalAmount(decimal amount)
    {
        decimal serviceFee = CalculateServiceFee(amount);

        return amount + serviceFee;
    }

    private decimal CalculateServiceFee(decimal amount)
    {
        return amount * 0.05m;
    }
}
```

Test the public method:

```csharp
[Fact]
public void CalculateFinalAmount_WhenAmountIsProvided_AddsServiceFee()
{
    var calculator = new InvoiceCalculator();

    decimal result = calculator.CalculateFinalAmount(200m);

    Assert.Equal(210m, result);
}
```

> If a private method is complex and difficult to test through public behavior, it may be a sign that the code should be redesigned.

---

# 18. Unit Tests Should Be Isolated

A unit test should not usually depend on:

- A real database
- A real web API
- The file system
- The current date and time
- Random values
- Network access

These dependencies make tests:

- Slower
- Harder to repeat
- More fragile

Instead, use:

- Interfaces
- Fake implementations
- Mocks
- Dependency injection

---

# 19. Dependency Injection for Testable Code

## Hard-to-Test Code

```csharp
public class WelcomeService
{
    public string CreateMessage(string name)
    {
        var hour = DateTime.Now.Hour;

        if (hour < 12)
        {
            return $"Good morning, {name}.";
        }

        return $"Good afternoon, {name}.";
    }
}
```

This is harder to test because it depends on the real current time.

## Better Design

Create an interface:

```csharp
public interface IClock
{
    DateTime Now { get; }
}
```

Use it in the service:

```csharp
public class WelcomeService
{
    private readonly IClock _clock;

    public WelcomeService(IClock clock)
    {
        _clock = clock;
    }

    public string CreateMessage(string name)
    {
        if (_clock.Now.Hour < 12)
        {
            return $"Good morning, {name}.";
        }

        return $"Good afternoon, {name}.";
    }
}
```

Create a fake clock for tests:

```csharp
public class FakeClock : IClock
{
    public DateTime Now { get; set; }
}
```

Test:

```csharp
[Fact]
public void CreateMessage_WhenTimeIsMorning_ReturnsMorningMessage()
{
    var fakeClock = new FakeClock
    {
        Now = new DateTime(2026, 3, 14, 9, 30, 0)
    };

    var service = new WelcomeService(fakeClock);

    string result = service.CreateMessage("Lina");

    Assert.Equal("Good morning, Lina.", result);
}
```

---

# 20. Mocking Dependencies

A **mock** is a fake object used to verify interactions or provide controlled behavior.

A popular mocking library in C# is **Moq**.

Install it:

```bash
dotnet add package Moq
```

## Example Scenario

You have an email sender interface:

```csharp
public interface IEmailSender
{
    void SendEmail(string recipient, string subject, string body);
}
```

A notification service uses it:

```csharp
public class NotificationService
{
    private readonly IEmailSender _emailSender;

    public NotificationService(IEmailSender emailSender)
    {
        _emailSender = emailSender;
    }

    public void SendWelcomeEmail(string email)
    {
        _emailSender.SendEmail(
            email,
            "Welcome to BlueCart",
            "Your account has been created successfully."
        );
    }
}
```

## Test with Moq

```csharp
using Moq;
using Xunit;

public class NotificationServiceTests
{
    [Fact]
    public void SendWelcomeEmail_WhenCalled_SendsEmailOnce()
    {
        var emailSenderMock = new Mock<IEmailSender>();

        var service = new NotificationService(emailSenderMock.Object);

        service.SendWelcomeEmail("user@example.com");

        emailSenderMock.Verify(sender => sender.SendEmail(
            "user@example.com",
            "Welcome to BlueCart",
            "Your account has been created successfully."
        ), Times.Once);
    }
}
```

---

# 21. Stubs vs Mocks

| Type | Purpose | Example |
|---|---|---|
| **Stub** | Provides fake data | Return a fake user |
| **Mock** | Verifies behavior | Check that email was sent |
| **Fake** | Working simplified implementation | In-memory repository |

## Stub Example

```csharp
public interface ICurrencyRateProvider
{
    decimal GetRate(string currencyCode);
}
```

```csharp
public class CurrencyConverter
{
    private readonly ICurrencyRateProvider _rateProvider;

    public CurrencyConverter(ICurrencyRateProvider rateProvider)
    {
        _rateProvider = rateProvider;
    }

    public decimal ConvertToLocal(decimal amount, string currencyCode)
    {
        decimal rate = _rateProvider.GetRate(currencyCode);

        return amount * rate;
    }
}
```

```csharp
public class FakeRateProvider : ICurrencyRateProvider
{
    public decimal GetRate(string currencyCode)
    {
        return currencyCode == "EUR" ? 48m : 35m;
    }
}
```

```csharp
[Fact]
public void ConvertToLocal_WhenCurrencyIsEuro_ReturnsConvertedAmount()
{
    var converter = new CurrencyConverter(new FakeRateProvider());

    decimal result = converter.ConvertToLocal(10m, "EUR");

    Assert.Equal(480m, result);
}
```

---

# 22. Testing Async Methods

Many C# methods are asynchronous and return `Task` or `Task<T>`.

## Production Code

```csharp
public interface IUserRepository
{
    Task<User?> FindByEmailAsync(string email);
}
```

```csharp
public class User
{
    public string Email { get; set; } = "";
    public string DisplayName { get; set; } = "";
}
```

```csharp
public class UserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<string> GetDisplayNameAsync(string email)
    {
        User? user = await _repository.FindByEmailAsync(email);

        return user?.DisplayName ?? "Guest";
    }
}
```

## Async Test with Moq

```csharp
using Moq;
using Xunit;

public class UserServiceTests
{
    [Fact]
    public async Task GetDisplayNameAsync_WhenUserExists_ReturnsDisplayName()
    {
        var repositoryMock = new Mock<IUserRepository>();

        repositoryMock
            .Setup(repo => repo.FindByEmailAsync("maya@example.com"))
            .ReturnsAsync(new User
            {
                Email = "maya@example.com",
                DisplayName = "Maya"
            });

        var service = new UserService(repositoryMock.Object);

        string result = await service.GetDisplayNameAsync("maya@example.com");

        Assert.Equal("Maya", result);
    }
}
```

## Async Test for Missing Data

```csharp
[Fact]
public async Task GetDisplayNameAsync_WhenUserDoesNotExist_ReturnsGuest()
{
    var repositoryMock = new Mock<IUserRepository>();

    repositoryMock
        .Setup(repo => repo.FindByEmailAsync("ghost@example.com"))
        .ReturnsAsync((User?)null);

    var service = new UserService(repositoryMock.Object);

    string result = await service.GetDisplayNameAsync("ghost@example.com");

    Assert.Equal("Guest", result);
}
```

---

# 23. Testing Async Exceptions

```csharp
public class ReportService
{
    public async Task<string> GenerateReportAsync(int reportId)
    {
        await Task.Delay(10);

        if (reportId <= 0)
        {
            throw new ArgumentException("Report id must be positive.");
        }

        return $"Report-{reportId}";
    }
}
```

```csharp
[Fact]
public async Task GenerateReportAsync_WhenReportIdIsInvalid_ThrowsArgumentException()
{
    var service = new ReportService();

    await Assert.ThrowsAsync<ArgumentException>(
        () => service.GenerateReportAsync(0)
    );
}
```

---

# 24. Testing Floating-Point Values

Floating-point values like `double` can have tiny precision differences.

```csharp
public class GeometryCalculator
{
    public double CalculateCircleArea(double radius)
    {
        return Math.PI * radius * radius;
    }
}
```

Use precision in assertions:

```csharp
[Fact]
public void CalculateCircleArea_WhenRadiusIsTwo_ReturnsApproximateArea()
{
    var calculator = new GeometryCalculator();

    double result = calculator.CalculateCircleArea(2);

    Assert.Equal(12.57, result, precision: 2);
}
```

For financial calculations, prefer `decimal` instead of `double`.

---

# 25. Testing Validation Logic

## Production Code

```csharp
public class PasswordValidator
{
    public bool IsValid(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
        {
            return false;
        }

        if (password.Length < 8)
        {
            return false;
        }

        if (!password.Any(char.IsDigit))
        {
            return false;
        }

        return true;
    }
}
```

## Tests

```csharp
public class PasswordValidatorTests
{
    [Theory]
    [InlineData("River2026", true)]
    [InlineData("tiny7", false)]
    [InlineData("NoDigitsHere", false)]
    [InlineData("", false)]
    [InlineData("   ", false)]
    public void IsValid_WhenPasswordIsChecked_ReturnsExpectedResult(
        string password,
        bool expected)
    {
        var validator = new PasswordValidator();

        bool result = validator.IsValid(password);

        Assert.Equal(expected, result);
    }
}
```

---

# 26. Test Data Builders

When objects are large, tests can become messy.

## Without Builder

```csharp
var customer = new Customer
{
    FirstName = "Nora",
    LastName = "Miles",
    Email = "nora@example.com",
    IsPremium = true,
    LoyaltyPoints = 1200
};
```

## With Builder

```csharp
public class CustomerBuilder
{
    private readonly Customer _customer = new()
    {
        FirstName = "Default",
        LastName = "Customer",
        Email = "default@example.com",
        IsPremium = false,
        LoyaltyPoints = 0
    };

    public CustomerBuilder WithEmail(string email)
    {
        _customer.Email = email;
        return this;
    }

    public CustomerBuilder AsPremium()
    {
        _customer.IsPremium = true;
        return this;
    }

    public CustomerBuilder WithLoyaltyPoints(int points)
    {
        _customer.LoyaltyPoints = points;
        return this;
    }

    public Customer Build()
    {
        return _customer;
    }
}
```

Usage:

```csharp
[Fact]
public void Customer_WhenBuiltAsPremium_HasPremiumStatus()
{
    Customer customer = new CustomerBuilder()
        .WithEmail("sara@example.com")
        .AsPremium()
        .WithLoyaltyPoints(750)
        .Build();

    Assert.True(customer.IsPremium);
    Assert.Equal("sara@example.com", customer.Email);
}
```

---

# 27. Testing Classes with Repositories

## Interface

```csharp
public interface IProductRepository
{
    Product? GetBySku(string sku);
}
```

## Model

```csharp
public class Product
{
    public string Sku { get; set; } = "";
    public string Name { get; set; } = "";
    public decimal Price { get; set; }
}
```

## Service

```csharp
public class ProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public decimal GetProductPrice(string sku)
    {
        Product? product = _repository.GetBySku(sku);

        if (product is null)
        {
            throw new InvalidOperationException("Product was not found.");
        }

        return product.Price;
    }
}
```

## Test

```csharp
[Fact]
public void GetProductPrice_WhenProductExists_ReturnsPrice()
{
    var repositoryMock = new Mock<IProductRepository>();

    repositoryMock
        .Setup(repo => repo.GetBySku("BK-204"))
        .Returns(new Product
        {
            Sku = "BK-204",
            Name = "Desk Lamp",
            Price = 135m
        });

    var service = new ProductService(repositoryMock.Object);

    decimal result = service.GetProductPrice("BK-204");

    Assert.Equal(135m, result);
}
```

## Test Missing Product

```csharp
[Fact]
public void GetProductPrice_WhenProductDoesNotExist_ThrowsException()
{
    var repositoryMock = new Mock<IProductRepository>();

    repositoryMock
        .Setup(repo => repo.GetBySku("UNKNOWN"))
        .Returns((Product?)null);

    var service = new ProductService(repositoryMock.Object);

    Assert.Throws<InvalidOperationException>(
        () => service.GetProductPrice("UNKNOWN")
    );
}
```

---

# 28. Unit Test Project Structure

A clean test project might look like this:

```text
ShopApp.Tests
│
├── Calculators
│   ├── PriceCalculatorTests.cs
│   └── ShippingCalculatorTests.cs
│
├── Services
│   ├── UserServiceTests.cs
│   └── ProductServiceTests.cs
│
├── Validators
│   ├── PasswordValidatorTests.cs
│   └── AgeValidatorTests.cs
│
└── TestHelpers
    ├── CustomerBuilder.cs
    └── FakeClock.cs
```

Recommended naming:

| Item | Naming Example |
|---|---|
| Test project | `ShopApp.Tests` |
| Test class | `PriceCalculatorTests` |
| Test method | `AddTax_WhenRateIsValid_ReturnsTotalWithTax` |
| Test helper folder | `TestHelpers` |

---

# 29. Running Unit Tests

## Run All Tests

```bash
dotnet test
```

## Run Tests from a Specific Project

```bash
dotnet test ShopApp.Tests
```

## Run Tests with Detailed Output

```bash
dotnet test --logger "console;verbosity=detailed"
```

## Run Tests by Name Filter

```bash
dotnet test --filter "PriceCalculatorTests"
```

```bash
dotnet test --filter "AddTax"
```

---

# 30. Code Coverage

**Code coverage** shows how much of your code is executed by tests.

Install coverage collector:

```bash
dotnet add package coverlet.collector
```

Run tests with coverage:

```bash
dotnet test --collect:"XPlat Code Coverage"
```

Coverage can help identify untested areas, but high coverage does not always mean good tests.

> Good tests verify behavior, not just execution.

---

# 31. Characteristics of Good Unit Tests

Good unit tests should be:

| Characteristic | Meaning |
|---|---|
| **Fast** | Run quickly |
| **Isolated** | Do not depend on external systems |
| **Repeatable** | Same result every time |
| **Readable** | Easy to understand |
| **Focused** | Test one behavior |
| **Reliable** | Do not randomly fail |

---

# 32. Bad Unit Test Example

```csharp
[Fact]
public void Test1()
{
    var service = new OrderService();

    var result = service.Process();

    Assert.True(result);
}
```

Problems:

- ❌ Test name is unclear
- ❌ Behavior is vague
- ❌ The meaning of `true` is unknown
- ❌ Setup is incomplete
- ❌ Test does not explain expected behavior

---

# 33. Better Unit Test Example

```csharp
[Fact]
public void Process_WhenOrderHasItems_ReturnsSuccessfulResult()
{
    var order = new Order
    {
        Items =
        {
            new OrderItem
            {
                Name = "Wireless Mouse",
                Quantity = 2,
                UnitPrice = 25m
            }
        }
    };

    var service = new OrderService();

    bool result = service.Process(order);

    Assert.True(result);
}
```

This test is better because:

- ✅ Name explains the scenario
- ✅ Input data is visible
- ✅ Expected behavior is clear
- ✅ Test checks one behavior

---

# 34. Avoid Testing Implementation Details

## Fragile Test

```csharp
[Fact]
public void CalculateTotal_WhenCalled_UsesDiscountMethod()
{
    var calculator = new OrderCalculator();

    decimal result = calculator.CalculateTotal(200m);

    Assert.Equal(180m, result);
}
```

This may look okay, but if the real goal is to check whether an internal method was called, the test becomes fragile.

## Better Focus

Test the result:

```csharp
[Fact]
public void CalculateTotal_WhenCustomerHasDiscount_ReturnsDiscountedTotal()
{
    var calculator = new OrderCalculator();

    decimal result = calculator.CalculateTotal(200m, hasDiscount: true);

    Assert.Equal(180m, result);
}
```

> Unit tests should focus on **observable behavior**, not internal implementation.

---

# 35. Arrange Data Clearly

Instead of hiding important values:

```csharp
[Fact]
public void CalculateTotal_WhenOrderHasTwoItems_ReturnsCorrectTotal()
{
    var order = TestOrderFactory.CreateDefaultOrder();

    var calculator = new OrderCalculator();

    decimal result = calculator.CalculateTotal(order);

    Assert.Equal(160m, result);
}
```

Prefer making important test data visible:

```csharp
[Fact]
public void CalculateTotal_WhenOrderHasTwoItems_ReturnsCorrectTotal()
{
    var order = new Order();

    order.Items.Add(new OrderItem
    {
        Name = "Keyboard",
        Quantity = 1,
        UnitPrice = 70m
    });

    order.Items.Add(new OrderItem
    {
        Name = "Mouse Pad",
        Quantity = 3,
        UnitPrice = 30m
    });

    var calculator = new OrderCalculator();

    decimal result = calculator.CalculateTotal(order);

    Assert.Equal(160m, result);
}
```

---

# 36. Testing Edge Cases

Edge cases are unusual but important inputs.

Examples:

| Type | Example |
|---|---|
| Minimum value | `0`, `1`, empty string |
| Maximum value | Very large number |
| Null value | `null` input |
| Boundary value | Age `17`, `18`, `19` |
| Empty collection | Cart with no items |
| Duplicate values | Same item added twice |

## Example

```csharp
public class UsernameValidator
{
    public bool IsValid(string? username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            return false;
        }

        return username.Length >= 3 && username.Length <= 20;
    }
}
```

```csharp
[Theory]
[InlineData(null, false)]
[InlineData("", false)]
[InlineData("ab", false)]
[InlineData("abc", true)]
[InlineData("twentycharacterslong", true)]
[InlineData("this-name-is-way-too-long", false)]
public void IsValid_WhenUsernameLengthVaries_ReturnsExpectedResult(
    string? username,
    bool expected)
{
    var validator = new UsernameValidator();

    bool result = validator.IsValid(username);

    Assert.Equal(expected, result);
}
```

---

# 37. Testing Boundary Values

Boundary testing checks values around decision points.

```csharp
public class TicketPriceCalculator
{
    public decimal GetPrice(int age)
    {
        if (age < 6)
        {
            return 0m;
        }

        if (age <= 17)
        {
            return 8m;
        }

        if (age >= 65)
        {
            return 6m;
        }

        return 12m;
    }
}
```

```csharp
[Theory]
[InlineData(5, 0)]
[InlineData(6, 8)]
[InlineData(17, 8)]
[InlineData(18, 12)]
[InlineData(64, 12)]
[InlineData(65, 6)]
public void GetPrice_WhenAgeIsAtBoundary_ReturnsCorrectPrice(
    int age,
    decimal expectedPrice)
{
    var calculator = new TicketPriceCalculator();

    decimal result = calculator.GetPrice(age);

    Assert.Equal(expectedPrice, result);
}
```

---

# 38. Avoid Randomness in Tests

Bad example:

```csharp
[Fact]
public void GenerateCode_WhenCalled_ReturnsCode()
{
    var generator = new Random();

    int value = generator.Next(1, 1000);

    Assert.True(value > 0);
}
```

Better design:

```csharp
public interface INumberGenerator
{
    int Next(int min, int max);
}
```

```csharp
public class CodeService
{
    private readonly INumberGenerator _numberGenerator;

    public CodeService(INumberGenerator numberGenerator)
    {
        _numberGenerator = numberGenerator;
    }

    public string GenerateCode()
    {
        int number = _numberGenerator.Next(100, 999);

        return $"CODE-{number}";
    }
}
```

Test:

```csharp
[Fact]
public void GenerateCode_WhenNumberIsFixed_ReturnsExpectedCode()
{
    var numberGeneratorMock = new Mock<INumberGenerator>();

    numberGeneratorMock
        .Setup(generator => generator.Next(100, 999))
        .Returns(482);

    var service = new CodeService(numberGeneratorMock.Object);

    string result = service.GenerateCode();

    Assert.Equal("CODE-482", result);
}
```

---

# 39. Avoid Real Time in Tests

Bad example:

```csharp
[Fact]
public void IsExpired_WhenCurrentDateIsAfterExpiry_ReturnsTrue()
{
    var subscription = new Subscription
    {
        ExpiresAt = DateTime.Now.AddDays(-1)
    };

    Assert.True(subscription.IsExpired());
}
```

This depends on the real system clock.

Better:

```csharp
public class Subscription
{
    public DateTime ExpiresAt { get; set; }

    public bool IsExpired(DateTime currentDate)
    {
        return currentDate > ExpiresAt;
    }
}
```

```csharp
[Fact]
public void IsExpired_WhenCurrentDateIsAfterExpiry_ReturnsTrue()
{
    var subscription = new Subscription
    {
        ExpiresAt = new DateTime(2026, 5, 10)
    };

    bool result = subscription.IsExpired(new DateTime(2026, 5, 11));

    Assert.True(result);
}
```

---

# 40. Avoid Real Databases in Unit Tests

Unit tests should avoid real databases.

Instead of this:

```csharp
public class CustomerService
{
    private readonly RealDatabaseContext _database;

    public CustomerService()
    {
        _database = new RealDatabaseContext();
    }
}
```

Use an interface:

```csharp
public interface ICustomerRepository
{
    Customer? GetById(int id);
}
```

```csharp
public class CustomerService
{
    private readonly ICustomerRepository _repository;

    public CustomerService(ICustomerRepository repository)
    {
        _repository = repository;
    }

    public string GetCustomerName(int id)
    {
        Customer? customer = _repository.GetById(id);

        return customer?.Name ?? "Unknown";
    }
}
```

Test with mock:

```csharp
[Fact]
public void GetCustomerName_WhenCustomerExists_ReturnsName()
{
    var repositoryMock = new Mock<ICustomerRepository>();

    repositoryMock
        .Setup(repo => repo.GetById(42))
        .Returns(new Customer
        {
            Id = 42,
            Name = "Owen"
        });

    var service = new CustomerService(repositoryMock.Object);

    string result = service.GetCustomerName(42);

    Assert.Equal("Owen", result);
}
```

---

# 41. Unit Test vs Integration Test

| Type | Purpose | Dependencies |
|---|---|---|
| **Unit Test** | Tests one small piece of code | Fake or mocked dependencies |
| **Integration Test** | Tests multiple parts working together | May use database, API, file system |
| **End-to-End Test** | Tests the full application flow | Uses real or production-like system |

Example:

| Scenario | Test Type |
|---|---|
| Validate password length | Unit test |
| Check service works with database | Integration test |
| User logs in through browser | End-to-end test |

---

# 42. Testing with MSTest

MSTest uses `[TestClass]` and `[TestMethod]`.

```csharp
using Microsoft.VisualStudio.TestTools.UnitTesting;

[TestClass]
public class PriceCalculatorTests
{
    [TestMethod]
    public void AddTax_WhenRateIsTenPercent_ReturnsTotalWithTax()
    {
        var calculator = new PriceCalculator();

        decimal result = calculator.AddTax(600m, 10m);

        Assert.AreEqual(660m, result);
    }
}
```

Parameterized tests use `[DataTestMethod]` and `[DataRow]`.

```csharp
[DataTestMethod]
[DataRow(100, 10, 90)]
[DataRow(240, 25, 180)]
[DataRow(800, 50, 400)]
public void ApplyDiscount_WithDifferentRates_ReturnsExpectedPrice(
    decimal price,
    decimal discount,
    decimal expected)
{
    var calculator = new PriceCalculator();

    decimal result = calculator.ApplyDiscount(price, discount);

    Assert.AreEqual(expected, result);
}
```

---

# 43. Testing with NUnit

NUnit uses `[Test]`.

```csharp
using NUnit.Framework;

public class PriceCalculatorTests
{
    [Test]
    public void AddTax_WhenRateIsFivePercent_ReturnsTotalWithTax()
    {
        var calculator = new PriceCalculator();

        decimal result = calculator.AddTax(200m, 5m);

        Assert.That(result, Is.EqualTo(210m));
    }
}
```

Parameterized tests use `[TestCase]`.

```csharp
[TestCase(100, 10, 90)]
[TestCase(250, 20, 200)]
[TestCase(900, 30, 630)]
public void ApplyDiscount_WithDifferentRates_ReturnsExpectedPrice(
    decimal price,
    decimal discount,
    decimal expected)
{
    var calculator = new PriceCalculator();

    decimal result = calculator.ApplyDiscount(price, discount);

    Assert.That(result, Is.EqualTo(expected));
}
```

---

# 44. xUnit Lifecycle

xUnit creates a new instance of the test class for each test.

```csharp
public class CounterTests
{
    private int _counter = 0;

    [Fact]
    public void FirstTest()
    {
        _counter++;

        Assert.Equal(1, _counter);
    }

    [Fact]
    public void SecondTest()
    {
        _counter++;

        Assert.Equal(1, _counter);
    }
}
```

Each test gets a fresh instance, so `_counter` starts at `0` for each test.

---

# 45. Shared Setup in xUnit

If several tests need the same setup, use the constructor.

```csharp
public class PriceCalculatorTests
{
    private readonly PriceCalculator _calculator;

    public PriceCalculatorTests()
    {
        _calculator = new PriceCalculator();
    }

    [Fact]
    public void AddTax_WhenRateIsTenPercent_ReturnsTotalWithTax()
    {
        decimal result = _calculator.AddTax(100m, 10m);

        Assert.Equal(110m, result);
    }

    [Fact]
    public void ApplyDiscount_WhenRateIsTwentyPercent_ReturnsReducedPrice()
    {
        decimal result = _calculator.ApplyDiscount(500m, 20m);

        Assert.Equal(400m, result);
    }
}
```

Use shared setup only when it improves readability.

---

# 46. Cleanup in xUnit

Use `IDisposable` for cleanup.

```csharp
public class FileProcessorTests : IDisposable
{
    private readonly string _tempFilePath;

    public FileProcessorTests()
    {
        _tempFilePath = Path.Combine(Path.GetTempPath(), "sample-test-file.txt");
        File.WriteAllText(_tempFilePath, "temporary test content");
    }

    [Fact]
    public void ReadContent_WhenFileExists_ReturnsContent()
    {
        string content = File.ReadAllText(_tempFilePath);

        Assert.Equal("temporary test content", content);
    }

    public void Dispose()
    {
        if (File.Exists(_tempFilePath))
        {
            File.Delete(_tempFilePath);
        }
    }
}
```

> File-based tests are often closer to integration tests than pure unit tests.

---

# 47. Common Mistakes in Unit Testing

| Mistake | Better Approach |
|---|---|
| Testing too much in one test | Test one behavior per test |
| Using unclear test names | Describe condition and expected result |
| Depending on real time | Inject time or pass date as parameter |
| Depending on random values | Mock randomness |
| Depending on database | Use repository interfaces |
| Testing private methods directly | Test public behavior |
| Overusing mocks | Mock only external dependencies |
| Ignoring edge cases | Test boundaries and invalid inputs |
| Writing tests after every bug manually only | Add tests for important behavior continuously |

---

# 48. Practical Example: Full Unit Testing Flow

## Production Code

```csharp
public class OrderItem
{
    public string Name { get; set; } = "";
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
```

```csharp
public class Order
{
    public List<OrderItem> Items { get; } = new();
}
```

```csharp
public class OrderCalculator
{
    public decimal CalculateTotal(Order order)
    {
        if (order.Items.Count == 0)
        {
            return 0m;
        }

        return order.Items.Sum(item => item.Quantity * item.UnitPrice);
    }
}
```

## Tests

```csharp
using Xunit;

public class OrderCalculatorTests
{
    [Fact]
    public void CalculateTotal_WhenOrderHasNoItems_ReturnsZero()
    {
        var order = new Order();
        var calculator = new OrderCalculator();

        decimal result = calculator.CalculateTotal(order);

        Assert.Equal(0m, result);
    }

    [Fact]
    public void CalculateTotal_WhenOrderHasOneItem_ReturnsItemTotal()
    {
        var order = new Order();

        order.Items.Add(new OrderItem
        {
            Name = "USB Cable",
            Quantity = 3,
            UnitPrice = 15m
        });

        var calculator = new OrderCalculator();

        decimal result = calculator.CalculateTotal(order);

        Assert.Equal(45m, result);
    }

    [Fact]
    public void CalculateTotal_WhenOrderHasMultipleItems_ReturnsSumOfAllItems()
    {
        var order = new Order();

        order.Items.Add(new OrderItem
        {
            Name = "Travel Mug",
            Quantity = 2,
            UnitPrice = 40m
        });

        order.Items.Add(new OrderItem
        {
            Name = "Notebook Pack",
            Quantity = 4,
            UnitPrice = 12m
        });

        var calculator = new OrderCalculator();

        decimal result = calculator.CalculateTotal(order);

        Assert.Equal(128m, result);
    }
}
```

---

# 49. Unit Testing Checklist

Before accepting a unit test, ask:

- [ ] Does the test name explain the behavior?
- [ ] Does it follow **Arrange → Act → Assert**?
- [ ] Does it test only one behavior?
- [ ] Is it independent from other tests?
- [ ] Does it avoid real databases, APIs, time, and randomness?
- [ ] Is the expected result clear?
- [ ] Are edge cases covered?
- [ ] Would the test fail if the code were broken?
- [ ] Is the test easy to read?