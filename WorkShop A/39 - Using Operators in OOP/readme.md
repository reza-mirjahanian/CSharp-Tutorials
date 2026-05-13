# Using Operators in OOP

## 1. What Are Operators in C#?

In C#, **operators** are special symbols that perform operations on values or objects.

Examples:

```csharp
int total = 10 + 5;
bool isAdult = age >= 18;
string fullName = firstName + " " + lastName;
```

Common operators include:

| Category | Operators | Example |
|---|---|---|
| Arithmetic | `+`, `-`, `*`, `/`, `%` | `a + b` |
| Comparison | `==`, `!=`, `<`, `>`, `<=`, `>=` | `x > y` |
| Logical | `&&`, `||`, `!` | `isValid && isActive` |
| Assignment | `=`, `+=`, `-=`, `*=`, `/=` | `count += 2` |
| Null-related | `??`, `?.`, `??=` | `name ?? "Unknown"` |
| Type-related | `is`, `as`, `typeof` | `obj is Customer` |

In object-oriented programming, operators can be used not only with primitive values like `int` and `double`, but also with **custom classes and structs**.

---

# 2. Operators and Objects

By default, operators do not always know how to work with your custom objects.

For example:

```csharp
Money price1 = new Money(50);
Money price2 = new Money(30);

Money total = price1 + price2;
```

The expression above will not work unless we tell C# what `+` means for the `Money` type.

This is done using **operator overloading**.

---

# 3. Operator Overloading

## What Is Operator Overloading?

**Operator overloading** means defining custom behavior for operators when they are used with your own types.

For example, you can make this possible:

```csharp
Distance d1 = new Distance(12);
Distance d2 = new Distance(8);

Distance result = d1 + d2;
```

Instead of writing:

```csharp
Distance result = d1.Add(d2);
```

Operator overloading can make code more natural and readable when used correctly.

---

# 4. Basic Syntax of Operator Overloading

Operator overloads are declared using:

```csharp
public static ReturnType operator OperatorSymbol(ParameterList)
{
    // Implementation
}
```

Example:

```csharp
public static Distance operator +(Distance left, Distance right)
{
    return new Distance(left.Meters + right.Meters);
}
```

Important rules:

| Rule | Description |
|---|---|
| Must be `public` | Operator overloads must be public |
| Must be `static` | Operator overloads belong to the type, not an instance |
| At least one parameter must be the containing type | You cannot overload operators only for built-in types |
| Cannot create new operators | You can only overload existing operators |
| Cannot change precedence | Operator precedence is fixed by C# |

---

# 5. Example: Overloading `+` for a `Money` Class

## Class Definition

```csharp
public class Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static Money operator +(Money left, Money right)
    {
        if (left.Currency != right.Currency)
        {
            throw new InvalidOperationException("Cannot add money values with different currencies.");
        }

        return new Money(left.Amount + right.Amount, left.Currency);
    }

    public override string ToString()
    {
        return $"{Amount} {Currency}";
    }
}
```

## Usage

```csharp
Money lunch = new Money(14.75m, "USD");
Money coffee = new Money(4.25m, "USD");

Money total = lunch + coffee;

Console.WriteLine(total);
```

Output:

```text
19.00 USD
```

## Explanation

The operator method:

```csharp
public static Money operator +(Money left, Money right)
```

tells C# how to add two `Money` objects.

So this:

```csharp
Money total = lunch + coffee;
```

is internally treated similarly to:

```csharp
Money total = Money.operator +(lunch, coffee);
```

---

# 6. Overloading the `-` Operator

You can also define subtraction.

```csharp
public class WalletBalance
{
    public decimal Amount { get; }

    public WalletBalance(decimal amount)
    {
        Amount = amount;
    }

    public static WalletBalance operator -(WalletBalance left, WalletBalance right)
    {
        return new WalletBalance(left.Amount - right.Amount);
    }

    public override string ToString()
    {
        return $"Balance: {Amount}";
    }
}
```

Usage:

```csharp
WalletBalance current = new WalletBalance(250.00m);
WalletBalance spent = new WalletBalance(45.50m);

WalletBalance remaining = current - spent;

Console.WriteLine(remaining);
```

Output:

```text
Balance: 204.50
```

---

# 7. Overloading Unary Operators

Some operators use only one operand.

Examples:

| Operator | Meaning |
|---|---|
| `+x` | Unary plus |
| `-x` | Unary minus |
| `!x` | Logical negation |
| `++x` | Increment |
| `--x` | Decrement |

---

## Example: Unary `-`

```csharp
public class Temperature
{
    public double Celsius { get; }

    public Temperature(double celsius)
    {
        Celsius = celsius;
    }

    public static Temperature operator -(Temperature value)
    {
        return new Temperature(-value.Celsius);
    }

    public override string ToString()
    {
        return $"{Celsius}°C";
    }
}
```

Usage:

```csharp
Temperature outside = new Temperature(7.5);
Temperature inverted = -outside;

Console.WriteLine(inverted);
```

Output:

```text
-7.5°C
```

---

# 8. Overloading `++` and `--`

The increment and decrement operators are useful for types that represent measurable values.

```csharp
public class Level
{
    public int Value { get; }

    public Level(int value)
    {
        Value = value;
    }

    public static Level operator ++(Level level)
    {
        return new Level(level.Value + 1);
    }

    public static Level operator --(Level level)
    {
        return new Level(level.Value - 1);
    }

    public override string ToString()
    {
        return $"Level {Value}";
    }
}
```

Usage:

```csharp
Level playerLevel = new Level(3);

playerLevel++;

Console.WriteLine(playerLevel);
```

Output:

```text
Level 4
```

> In C#, overloading `++` automatically supports both prefix and postfix syntax.

Example:

```csharp
++playerLevel;
playerLevel++;
```

---

# 9. Overloading Comparison Operators

Comparison operators are often used for value objects such as:

- `Money`
- `Distance`
- `Score`
- `Temperature`
- `DateRange`
- `VersionNumber`

Common comparison operators:

| Operator | Meaning |
|---|---|
| `==` | Equal |
| `!=` | Not equal |
| `<` | Less than |
| `>` | Greater than |
| `<=` | Less than or equal |
| `>=` | Greater than or equal |

---

## Important Rule

Some comparison operators must be overloaded in pairs.

| If You Overload | You Should Also Overload |
|---|---|
| `==` | `!=` |
| `<` | `>` |
| `<=` | `>=` |

---

# 10. Example: Overloading `==` and `!=`

```csharp
public class ProductCode
{
    public string Value { get; }

    public ProductCode(string value)
    {
        Value = value;
    }

    public static bool operator ==(ProductCode left, ProductCode right)
    {
        if (ReferenceEquals(left, right))
        {
            return true;
        }

        if (left is null || right is null)
        {
            return false;
        }

        return left.Value == right.Value;
    }

    public static bool operator !=(ProductCode left, ProductCode right)
    {
        return !(left == right);
    }

    public override bool Equals(object? obj)
    {
        return obj is ProductCode other && Value == other.Value;
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public override string ToString()
    {
        return Value;
    }
}
```

Usage:

```csharp
ProductCode first = new ProductCode("BOOK-204");
ProductCode second = new ProductCode("BOOK-204");
ProductCode third = new ProductCode("PEN-918");

Console.WriteLine(first == second);
Console.WriteLine(first == third);
Console.WriteLine(first != third);
```

Output:

```text
True
False
True
```

---

## Why Override `Equals()` and `GetHashCode()`?

When you overload `==` and `!=`, you should usually also override:

```csharp
Equals()
GetHashCode()
```

Because these are used by collections such as:

```csharp
Dictionary<TKey, TValue>
HashSet<T>
List<T>.Contains()
```

Example:

```csharp
HashSet<ProductCode> codes = new HashSet<ProductCode>();

codes.Add(new ProductCode("NOTE-315"));
codes.Add(new ProductCode("NOTE-315"));

Console.WriteLine(codes.Count);
```

If equality is implemented properly, the result is:

```text
1
```

---

# 11. Safer Equality with `IEquatable<T>`

For strongly typed equality, implement `IEquatable<T>`.

```csharp
public class CustomerId : IEquatable<CustomerId>
{
    public int Value { get; }

    public CustomerId(int value)
    {
        Value = value;
    }

    public bool Equals(CustomerId? other)
    {
        if (other is null)
        {
            return false;
        }

        return Value == other.Value;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as CustomerId);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    public static bool operator ==(CustomerId? left, CustomerId? right)
    {
        return EqualityComparer<CustomerId>.Default.Equals(left, right);
    }

    public static bool operator !=(CustomerId? left, CustomerId? right)
    {
        return !(left == right);
    }

    public override string ToString()
    {
        return $"Customer #{Value}";
    }
}
```

Usage:

```csharp
CustomerId first = new CustomerId(8021);
CustomerId second = new CustomerId(8021);

Console.WriteLine(first == second);
Console.WriteLine(first.Equals(second));
```

Output:

```text
True
True
```

---

# 12. Overloading `<`, `>`, `<=`, and `>=`

For ordering, you can overload comparison operators.

```csharp
public class Rating
{
    public double Value { get; }

    public Rating(double value)
    {
        if (value < 0 || value > 5)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Rating must be between 0 and 5.");
        }

        Value = value;
    }

    public static bool operator <(Rating left, Rating right)
    {
        return left.Value < right.Value;
    }

    public static bool operator >(Rating left, Rating right)
    {
        return left.Value > right.Value;
    }

    public static bool operator <=(Rating left, Rating right)
    {
        return left.Value <= right.Value;
    }

    public static bool operator >=(Rating left, Rating right)
    {
        return left.Value >= right.Value;
    }

    public override string ToString()
    {
        return $"{Value}/5";
    }
}
```

Usage:

```csharp
Rating basic = new Rating(3.2);
Rating premium = new Rating(4.7);

Console.WriteLine(premium > basic);
Console.WriteLine(basic <= premium);
```

Output:

```text
True
True
```

---

# 13. Combining Operators with Interfaces

Operator overloads make syntax nicer, but interfaces make your objects more usable in the .NET ecosystem.

For sortable objects, implement:

```csharp
IComparable<T>
```

Example:

```csharp
public class PackageWeight : IComparable<PackageWeight>
{
    public double Kilograms { get; }

    public PackageWeight(double kilograms)
    {
        if (kilograms < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(kilograms));
        }

        Kilograms = kilograms;
    }

    public int CompareTo(PackageWeight? other)
    {
        if (other is null)
        {
            return 1;
        }

        return Kilograms.CompareTo(other.Kilograms);
    }

    public static bool operator <(PackageWeight left, PackageWeight right)
    {
        return left.CompareTo(right) < 0;
    }

    public static bool operator >(PackageWeight left, PackageWeight right)
    {
        return left.CompareTo(right) > 0;
    }

    public static bool operator <=(PackageWeight left, PackageWeight right)
    {
        return left.CompareTo(right) <= 0;
    }

    public static bool operator >=(PackageWeight left, PackageWeight right)
    {
        return left.CompareTo(right) >= 0;
    }

    public override string ToString()
    {
        return $"{Kilograms} kg";
    }
}
```

Usage:

```csharp
List<PackageWeight> weights = new List<PackageWeight>
{
    new PackageWeight(8.4),
    new PackageWeight(2.1),
    new PackageWeight(5.9)
};

weights.Sort();

foreach (PackageWeight weight in weights)
{
    Console.WriteLine(weight);
}
```

Output:

```text
2.1 kg
5.9 kg
8.4 kg
```

---

# 14. Operator Overloading with `struct`

Operator overloading is very common with `struct` types because structs often represent small value objects.

Example:

```csharp
public readonly struct Distance
{
    public double Meters { get; }

    public Distance(double meters)
    {
        if (meters < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(meters), "Distance cannot be negative.");
        }

        Meters = meters;
    }

    public static Distance operator +(Distance left, Distance right)
    {
        return new Distance(left.Meters + right.Meters);
    }

    public static Distance operator -(Distance left, Distance right)
    {
        if (left.Meters < right.Meters)
        {
            throw new InvalidOperationException("Resulting distance cannot be negative.");
        }

        return new Distance(left.Meters - right.Meters);
    }

    public override string ToString()
    {
        return $"{Meters} m";
    }
}
```

Usage:

```csharp
Distance pathA = new Distance(120.5);
Distance pathB = new Distance(35.25);

Distance total = pathA + pathB;
Distance difference = pathA - pathB;

Console.WriteLine(total);
Console.WriteLine(difference);
```

Output:

```text
155.75 m
85.25 m
```

---

# 15. Immutable Objects and Operators

Operators usually work best with **immutable objects**.

An immutable object cannot be changed after it is created.

Example:

```csharp
public class Points
{
    public int Value { get; }

    public Points(int value)
    {
        Value = value;
    }

    public static Points operator +(Points left, Points right)
    {
        return new Points(left.Value + right.Value);
    }
}
```

The `+` operator returns a new object instead of modifying the existing ones.

```csharp
Points firstRound = new Points(18);
Points secondRound = new Points(24);

Points total = firstRound + secondRound;
```

This is safer than changing one of the existing objects:

```csharp
firstRound.Value += secondRound.Value; // Not allowed because Value has only get;
```

---

# 16. Why Immutability Matters

Operators should usually behave like mathematical operations.

For example:

```csharp
int a = 5;
int b = 7;

int c = a + b;
```

This does not change `a` or `b`.

Likewise:

```csharp
Money wallet = new Money(80, "USD");
Money deposit = new Money(35, "USD");

Money updated = wallet + deposit;
```

The original objects should usually remain unchanged.

| Object | Value |
|---|---|
| `wallet` | `80 USD` |
| `deposit` | `35 USD` |
| `updated` | `115 USD` |

---

# 17. Operator Overloading with Records

C# records are useful for immutable value-like objects.

```csharp
public record GameScore(int Value)
{
    public static GameScore operator +(GameScore left, GameScore right)
    {
        return new GameScore(left.Value + right.Value);
    }

    public static bool operator >(GameScore left, GameScore right)
    {
        return left.Value > right.Value;
    }

    public static bool operator <(GameScore left, GameScore right)
    {
        return left.Value < right.Value;
    }
}
```

Usage:

```csharp
GameScore teamA = new GameScore(42);
GameScore teamB = new GameScore(37);

GameScore combined = teamA + teamB;

Console.WriteLine(combined);
Console.WriteLine(teamA > teamB);
```

Output:

```text
GameScore { Value = 79 }
True
```

---

# 18. Conversion Operators

Conversion operators define how one type converts to another.

There are two types:

| Type | Meaning |
|---|---|
| `implicit` | Automatic conversion |
| `explicit` | Requires a cast |

---

# 19. Implicit Conversion Operator

Use `implicit` when the conversion is safe and does not lose data.

Example:

```csharp
public readonly struct Kilometers
{
    public double Value { get; }

    public Kilometers(double value)
    {
        Value = value;
    }

    public static implicit operator double(Kilometers kilometers)
    {
        return kilometers.Value;
    }

    public override string ToString()
    {
        return $"{Value} km";
    }
}
```

Usage:

```csharp
Kilometers tripLength = new Kilometers(18.6);

double numericValue = tripLength;

Console.WriteLine(numericValue);
```

Output:

```text
18.6
```

Here, C# automatically converts `Kilometers` to `double`.

---

# 20. Explicit Conversion Operator

Use `explicit` when conversion may lose information or needs clear intention.

Example:

```csharp
public readonly struct Miles
{
    public double Value { get; }

    public Miles(double value)
    {
        Value = value;
    }

    public static explicit operator int(Miles miles)
    {
        return (int)miles.Value;
    }

    public override string ToString()
    {
        return $"{Value} mi";
    }
}
```

Usage:

```csharp
Miles route = new Miles(9.8);

int roundedDown = (int)route;

Console.WriteLine(roundedDown);
```

Output:

```text
9
```

Because the decimal part is removed, the conversion must be explicit.

---

# 21. Converting Between Custom Types

You can also convert between your own object types.

```csharp
public readonly struct Celsius
{
    public double Value { get; }

    public Celsius(double value)
    {
        Value = value;
    }

    public static explicit operator Fahrenheit(Celsius celsius)
    {
        return new Fahrenheit((celsius.Value * 9 / 5) + 32);
    }

    public override string ToString()
    {
        return $"{Value}°C";
    }
}

public readonly struct Fahrenheit
{
    public double Value { get; }

    public Fahrenheit(double value)
    {
        Value = value;
    }

    public static explicit operator Celsius(Fahrenheit fahrenheit)
    {
        return new Celsius((fahrenheit.Value - 32) * 5 / 9);
    }

    public override string ToString()
    {
        return $"{Value}°F";
    }
}
```

Usage:

```csharp
Celsius morning = new Celsius(22);

Fahrenheit converted = (Fahrenheit)morning;

Console.WriteLine(converted);
```

Output:

```text
71.6°F
```

---

# 22. Operators That Can Be Overloaded

You can overload many C# operators.

## Unary Operators

```csharp
+  -  !  ~  ++  --  true  false
```

## Binary Operators

```csharp
+  -  *  /  %  &  |  ^  <<  >>
```

## Comparison Operators

```csharp
==  !=  <  >  <=  >=
```

## Conversion Operators

```csharp
implicit
explicit
```

---

# 23. Operators That Cannot Be Overloaded

Some operators cannot be overloaded.

| Operator | Meaning |
|---|---|
| `&&` | Conditional AND |
| `||` | Conditional OR |
| `??` | Null-coalescing |
| `?.` | Null-conditional |
| `?:` | Conditional expression |
| `=` | Assignment |
| `=>` | Lambda operator |
| `is` | Type check |
| `as` | Safe cast |
| `new` | Object creation |
| `sizeof` | Size of type |
| `typeof` | Type object |
| `nameof` | Name as string |
| `.` | Member access |

> Although `&&` and `||` cannot be overloaded directly, they can work indirectly if you overload `&`, `|`, `true`, and `false`.

---

# 24. The `true` and `false` Operators

C# allows custom types to define truth-like behavior using the `true` and `false` operators.

Example:

```csharp
public class ValidationResult
{
    public bool IsSuccessful { get; }
    public string Message { get; }

    public ValidationResult(bool isSuccessful, string message)
    {
        IsSuccessful = isSuccessful;
        Message = message;
    }

    public static bool operator true(ValidationResult result)
    {
        return result.IsSuccessful;
    }

    public static bool operator false(ValidationResult result)
    {
        return !result.IsSuccessful;
    }

    public override string ToString()
    {
        return Message;
    }
}
```

Usage:

```csharp
ValidationResult result = new ValidationResult(true, "Email format is valid.");

if (result)
{
    Console.WriteLine("Accepted");
}
else
{
    Console.WriteLine("Rejected");
}
```

Output:

```text
Accepted
```

---

# 25. Overloading `&` and `|`

You can overload bitwise/logical-style operators for custom objects.

```csharp
public class RuleResult
{
    public bool Passed { get; }
    public string Details { get; }

    public RuleResult(bool passed, string details)
    {
        Passed = passed;
        Details = details;
    }

    public static RuleResult operator &(RuleResult left, RuleResult right)
    {
        return new RuleResult(
            left.Passed && right.Passed,
            $"{left.Details}; {right.Details}"
        );
    }

    public static RuleResult operator |(RuleResult left, RuleResult right)
    {
        return new RuleResult(
            left.Passed || right.Passed,
            $"{left.Details}; {right.Details}"
        );
    }

    public static bool operator true(RuleResult result)
    {
        return result.Passed;
    }

    public static bool operator false(RuleResult result)
    {
        return !result.Passed;
    }

    public override string ToString()
    {
        return $"{Passed}: {Details}";
    }
}
```

Usage:

```csharp
RuleResult hasName = new RuleResult(true, "Name exists");
RuleResult hasPhone = new RuleResult(false, "Phone is missing");

RuleResult finalResult = hasName & hasPhone;

Console.WriteLine(finalResult);
```

Output:

```text
False: Name exists; Phone is missing
```

---

# 26. Operator Overloading in Inheritance

Operators are **static**, so they are not polymorphic like virtual methods.

That means operators do not behave like overridden instance methods.

Example:

```csharp
public class ShapeSize
{
    public int Area { get; }

    public ShapeSize(int area)
    {
        Area = area;
    }

    public static ShapeSize operator +(ShapeSize left, ShapeSize right)
    {
        return new ShapeSize(left.Area + right.Area);
    }
}
```

A derived class cannot override the operator in the same way it overrides a virtual method.

```csharp
public class RectangleSize : ShapeSize
{
    public RectangleSize(int area) : base(area)
    {
    }
}
```

This works:

```csharp
ShapeSize a = new RectangleSize(20);
ShapeSize b = new RectangleSize(30);

ShapeSize result = a + b;
```

But the operator used is the one declared for `ShapeSize`.

> Operators are resolved at compile time, not through runtime polymorphism.

---

# 27. Best Use Cases for Operator Overloading

Operator overloading is best when the operator meaning is obvious.

Good examples:

| Type | Useful Operators |
|---|---|
| `Money` | `+`, `-`, `==`, `!=`, `<`, `>` |
| `Distance` | `+`, `-`, `<`, `>` |
| `Temperature` | `<`, `>`, explicit conversions |
| `Vector` | `+`, `-`, `*` |
| `Matrix` | `+`, `-`, `*` |
| `ComplexNumber` | `+`, `-`, `*`, `/` |
| `Score` | `+`, `>`, `<` |
| `DateRange` | `==`, `!=`, maybe overlap methods instead of operators |

---

# 28. Poor Use Cases for Operator Overloading

Avoid operator overloading when it makes code confusing.

Bad example:

```csharp
public static Invoice operator +(Invoice invoice, Customer customer)
{
    // Unclear meaning
}
```

This is confusing:

```csharp
Invoice updated = invoice + customer;
```

Better:

```csharp
Invoice updated = invoice.AssignTo(customer);
```

---

# 29. Operator Overloading vs Methods

Operators are not always better than methods.

## Use Operators When

- The operation is mathematically or logically natural.
- The meaning is obvious.
- The operation is short and predictable.
- The type behaves like a value.
- The operator does not surprise the reader.

Example:

```csharp
Distance total = firstPath + secondPath;
```

## Use Methods When

- The operation needs a name to be clear.
- The operation has side effects.
- The operation is domain-specific.
- The meaning of an operator would be unclear.

Example:

```csharp
invoice.ApplyDiscount(15);
order.AssignCustomer(customer);
playlist.AddSong(song);
```

---

# 30. Complete Example: A `Vector2D` Type

## Class Definition

```csharp
public readonly struct Vector2D : IEquatable<Vector2D>
{
    public double X { get; }
    public double Y { get; }

    public Vector2D(double x, double y)
    {
        X = x;
        Y = y;
    }

    public static Vector2D operator +(Vector2D left, Vector2D right)
    {
        return new Vector2D(left.X + right.X, left.Y + right.Y);
    }

    public static Vector2D operator -(Vector2D left, Vector2D right)
    {
        return new Vector2D(left.X - right.X, left.Y - right.Y);
    }

    public static Vector2D operator *(Vector2D vector, double scalar)
    {
        return new Vector2D(vector.X * scalar, vector.Y * scalar);
    }

    public static Vector2D operator *(double scalar, Vector2D vector)
    {
        return vector * scalar;
    }

    public static bool operator ==(Vector2D left, Vector2D right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Vector2D left, Vector2D right)
    {
        return !left.Equals(right);
    }

    public bool Equals(Vector2D other)
    {
        return X.Equals(other.X) && Y.Equals(other.Y);
    }

    public override bool Equals(object? obj)
    {
        return obj is Vector2D other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }

    public override string ToString()
    {
        return $"({X}, {Y})";
    }
}
```

## Usage

```csharp
Vector2D start = new Vector2D(3, 4);
Vector2D movement = new Vector2D(6, -2);

Vector2D position = start + movement;
Vector2D reversed = position - movement;
Vector2D enlarged = position * 2;

Console.WriteLine(position);
Console.WriteLine(reversed);
Console.WriteLine(enlarged);
Console.WriteLine(start == reversed);
```

Output:

```text
(9, 2)
(3, 4)
(18, 4)
True
```

---

# 31. Complete Example: A `BankAccountBalance` Type

```csharp
public sealed class BankAccountBalance : IEquatable<BankAccountBalance>, IComparable<BankAccountBalance>
{
    public decimal Amount { get; }
    public string Currency { get; }

    public BankAccountBalance(decimal amount, string currency)
    {
        if (string.IsNullOrWhiteSpace(currency))
        {
            throw new ArgumentException("Currency is required.", nameof(currency));
        }

        Amount = amount;
        Currency = currency.ToUpperInvariant();
    }

    public static BankAccountBalance operator +(BankAccountBalance left, BankAccountBalance right)
    {
        EnsureSameCurrency(left, right);

        return new BankAccountBalance(left.Amount + right.Amount, left.Currency);
    }

    public static BankAccountBalance operator -(BankAccountBalance left, BankAccountBalance right)
    {
        EnsureSameCurrency(left, right);

        return new BankAccountBalance(left.Amount - right.Amount, left.Currency);
    }

    public static bool operator >(BankAccountBalance left, BankAccountBalance right)
    {
        EnsureSameCurrency(left, right);

        return left.Amount > right.Amount;
    }

    public static bool operator <(BankAccountBalance left, BankAccountBalance right)
    {
        EnsureSameCurrency(left, right);

        return left.Amount < right.Amount;
    }

    public static bool operator >=(BankAccountBalance left, BankAccountBalance right)
    {
        EnsureSameCurrency(left, right);

        return left.Amount >= right.Amount;
    }

    public static bool operator <=(BankAccountBalance left, BankAccountBalance right)
    {
        EnsureSameCurrency(left, right);

        return left.Amount <= right.Amount;
    }

    public static bool operator ==(BankAccountBalance? left, BankAccountBalance? right)
    {
        return EqualityComparer<BankAccountBalance>.Default.Equals(left, right);
    }

    public static bool operator !=(BankAccountBalance? left, BankAccountBalance? right)
    {
        return !(left == right);
    }

    public bool Equals(BankAccountBalance? other)
    {
        if (other is null)
        {
            return false;
        }

        return Amount == other.Amount && Currency == other.Currency;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as BankAccountBalance);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Amount, Currency);
    }

    public int CompareTo(BankAccountBalance? other)
    {
        if (other is null)
        {
            return 1;
        }

        EnsureSameCurrency(this, other);

        return Amount.CompareTo(other.Amount);
    }

    private static void EnsureSameCurrency(BankAccountBalance left, BankAccountBalance right)
    {
        if (left.Currency != right.Currency)
        {
            throw new InvalidOperationException("Currencies must match.");
        }
    }

    public override string ToString()
    {
        return $"{Amount:0.00} {Currency}";
    }
}
```

Usage:

```csharp
BankAccountBalance salary = new BankAccountBalance(3200, "eur");
BankAccountBalance rent = new BankAccountBalance(950, "EUR");
BankAccountBalance groceries = new BankAccountBalance(280.75m, "EUR");

BankAccountBalance remaining = salary - rent - groceries;

Console.WriteLine(remaining);
Console.WriteLine(salary > rent);
```

Output:

```text
1969.25 EUR
True
```

---

# 32. Handling `null` in Operator Overloads

For reference types, always consider `null`.

Bad example:

```csharp
public static bool operator ==(UserName left, UserName right)
{
    return left.Value == right.Value;
}
```

This will crash if either side is `null`.

Better:

```csharp
public static bool operator ==(UserName? left, UserName? right)
{
    if (ReferenceEquals(left, right))
    {
        return true;
    }

    if (left is null || right is null)
    {
        return false;
    }

    return left.Value == right.Value;
}
```

Full example:

```csharp
public sealed class UserName : IEquatable<UserName>
{
    public string Value { get; }

    public UserName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("User name cannot be empty.", nameof(value));
        }

        Value = value.Trim();
    }

    public bool Equals(UserName? other)
    {
        return other is not null &&
               string.Equals(Value, other.Value, StringComparison.OrdinalIgnoreCase);
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as UserName);
    }

    public override int GetHashCode()
    {
        return StringComparer.OrdinalIgnoreCase.GetHashCode(Value);
    }

    public static bool operator ==(UserName? left, UserName? right)
    {
        return EqualityComparer<UserName>.Default.Equals(left, right);
    }

    public static bool operator !=(UserName? left, UserName? right)
    {
        return !(left == right);
    }

    public override string ToString()
    {
        return Value;
    }
}
```

---

# 33. Compound Assignment Operators

You cannot directly overload compound assignment operators like:

```csharp
+=
-=
*=
/=
```

But C# uses your existing operators.

If you overload `+`, this works automatically:

```csharp
score += bonus;
```

Because it means:

```csharp
score = score + bonus;
```

Example:

```csharp
public readonly struct Score
{
    public int Value { get; }

    public Score(int value)
    {
        Value = value;
    }

    public static Score operator +(Score left, Score right)
    {
        return new Score(left.Value + right.Value);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}
```

Usage:

```csharp
Score total = new Score(15);
Score bonus = new Score(6);

total += bonus;

Console.WriteLine(total);
```

Output:

```text
21
```

---

# 34. Indexer Operator-Like Functionality

C# does not allow overloading `[]` directly as an operator, but classes can implement **indexers**.

Indexers allow objects to be accessed like arrays.

```csharp
public class WeeklySchedule
{
    private readonly Dictionary<string, string> _tasks = new Dictionary<string, string>
    {
        ["Monday"] = "Design review",
        ["Tuesday"] = "Code cleanup",
        ["Wednesday"] = "Database backup"
    };

    public string this[string day]
    {
        get
        {
            return _tasks.TryGetValue(day, out string? task)
                ? task
                : "No task planned";
        }

        set
        {
            _tasks[day] = value;
        }
    }
}
```

Usage:

```csharp
WeeklySchedule schedule = new WeeklySchedule();

Console.WriteLine(schedule["Monday"]);

schedule["Friday"] = "Release preparation";

Console.WriteLine(schedule["Friday"]);
```

Output:

```text
Design review
Release preparation
```

---

# 35. `is` Pattern Matching with Objects

Although `is` cannot be overloaded, it is often used with OOP to check object types or patterns.

```csharp
object item = new BankAccountBalance(1250, "USD");

if (item is BankAccountBalance balance)
{
    Console.WriteLine(balance.Amount);
}
```

Output:

```text
1250
```

Pattern matching example:

```csharp
if (item is BankAccountBalance { Amount: > 1000, Currency: "USD" })
{
    Console.WriteLine("Large USD balance");
}
```

---

# 36. Null-Conditional Operator `?.`

The `?.` operator safely accesses members when an object may be `null`.

```csharp
CustomerProfile? profile = GetProfile();

string? email = profile?.Email;
```

If `profile` is `null`, `email` becomes `null` instead of throwing an exception.

Example:

```csharp
public class CustomerProfile
{
    public string Email { get; set; } = "";
}

CustomerProfile? GetProfile()
{
    return null;
}

CustomerProfile? profile = GetProfile();

Console.WriteLine(profile?.Email ?? "No email found");
```

Output:

```text
No email found
```

---

# 37. Null-Coalescing Operator `??`

The `??` operator provides a fallback value.

```csharp
string displayName = userName ?? "Guest";
```

Example:

```csharp
public class Account
{
    public string? Nickname { get; set; }
}

Account account = new Account();

string visibleName = account.Nickname ?? "Visitor";

Console.WriteLine(visibleName);
```

Output:

```text
Visitor
```

---

# 38. Null-Coalescing Assignment `??=`

The `??=` operator assigns a value only if the variable is currently `null`.

```csharp
settings.Theme ??= "Light";
```

Example:

```csharp
public class UserSettings
{
    public string? Theme { get; set; }
}

UserSettings settings = new UserSettings();

settings.Theme ??= "Ocean";

Console.WriteLine(settings.Theme);
```

Output:

```text
Ocean
```

---

# 39. Object-Oriented Example Using Several Operators

```csharp
public sealed class InventoryItem : IEquatable<InventoryItem>
{
    public string Sku { get; }
    public int Quantity { get; }

    public InventoryItem(string sku, int quantity)
    {
        if (string.IsNullOrWhiteSpace(sku))
        {
            throw new ArgumentException("SKU is required.", nameof(sku));
        }

        if (quantity < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity));
        }

        Sku = sku.ToUpperInvariant();
        Quantity = quantity;
    }

    public static InventoryItem operator +(InventoryItem left, InventoryItem right)
    {
        EnsureSameSku(left, right);

        return new InventoryItem(left.Sku, left.Quantity + right.Quantity);
    }

    public static InventoryItem operator -(InventoryItem left, InventoryItem right)
    {
        EnsureSameSku(left, right);

        int newQuantity = left.Quantity - right.Quantity;

        if (newQuantity < 0)
        {
            throw new InvalidOperationException("Inventory quantity cannot become negative.");
        }

        return new InventoryItem(left.Sku, newQuantity);
    }

    public static bool operator ==(InventoryItem? left, InventoryItem? right)
    {
        return EqualityComparer<InventoryItem>.Default.Equals(left, right);
    }

    public static bool operator !=(InventoryItem? left, InventoryItem? right)
    {
        return !(left == right);
    }

    public bool Equals(InventoryItem? other)
    {
        return other is not null &&
               Sku == other.Sku &&
               Quantity == other.Quantity;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as InventoryItem);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Sku, Quantity);
    }

    private static void EnsureSameSku(InventoryItem left, InventoryItem right)
    {
        if (left.Sku != right.Sku)
        {
            throw new InvalidOperationException("Only inventory items with the same SKU can be combined.");
        }
    }

    public override string ToString()
    {
        return $"{Sku}: {Quantity} units";
    }
}
```

Usage:

```csharp
InventoryItem shelfStock = new InventoryItem("tea-440", 32);
InventoryItem delivery = new InventoryItem("TEA-440", 18);
InventoryItem sold = new InventoryItem("TEA-440", 7);

InventoryItem updatedStock = shelfStock + delivery - sold;

Console.WriteLine(updatedStock);
```

Output:

```text
TEA-440: 43 units
```

---

# 40. Best Practices

## ✅ Do

- Use operators only when their meaning is obvious.
- Prefer immutable types.
- Return a new object instead of mutating operands.
- Overload operators in logical pairs:
  - `==` and `!=`
  - `<` and `>`
  - `<=` and `>=`
- Override `Equals()` and `GetHashCode()` when overloading equality.
- Implement `IEquatable<T>` for value-like equality.
- Implement `IComparable<T>` for sortable types.
- Handle `null` carefully for reference types.
- Validate invalid operations, such as adding different currencies.
- Keep operator logic short and predictable.

## ❌ Avoid

- Using operators for unclear business actions.
- Giving operators surprising meanings.
- Mutating the left or right operand.
- Throwing exceptions for normal expected comparisons.
- Overloading too many operators unnecessarily.
- Using operators when a named method is clearer.

---

# 41. Quick Reference

| Goal | Use |
|---|---|
| Add two objects | Overload `+` |
| Subtract two objects | Overload `-` |
| Compare equality | Overload `==`, `!=` |
| Sort or order objects | Overload `<`, `>`, `<=`, `>=` and implement `IComparable<T>` |
| Convert object to another type | Use `implicit` or `explicit` |
| Access object like an array | Use an indexer |
| Provide fallback for null | Use `??` |
| Safely access nullable object members | Use `?.` |
| Assign if null | Use `??=` |

---

# 42. Mini Practice Examples

## Practice 1: Create a `BookPrice` Type

Requirements:

- Has `Amount`
- Has `Currency`
- Supports `+`
- Prevents adding different currencies
- Overrides `ToString()`

Starter:

```csharp
public class BookPrice
{
    public decimal Amount { get; }
    public string Currency { get; }

    public BookPrice(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static BookPrice operator +(BookPrice left, BookPrice right)
    {
        // Your code here
    }

    public override string ToString()
    {
        return $"{Amount} {Currency}";
    }
}
```

---

## Practice 2: Create a `StorageSize` Type

Requirements:

- Stores size in megabytes
- Supports `+`
- Supports `>`
- Supports `<`
- Has a readable `ToString()`

Example usage:

```csharp
StorageSize imageFolder = new StorageSize(850);
StorageSize videoFolder = new StorageSize(4200);

StorageSize total = imageFolder + videoFolder;

Console.WriteLine(total);
Console.WriteLine(videoFolder > imageFolder);
```

Expected output:

```text
5050 MB
True
```

---

## Practice 3: Create a `SubscriptionDays` Type

Requirements:

- Stores number of days
- Supports `++`
- Supports `--`
- Prevents negative days
- Overrides `ToString()`

Example usage:

```csharp
SubscriptionDays remaining = new SubscriptionDays(12);

remaining--;
remaining++;

Console.WriteLine(remaining);
```

Expected output:

```text
12 days
```