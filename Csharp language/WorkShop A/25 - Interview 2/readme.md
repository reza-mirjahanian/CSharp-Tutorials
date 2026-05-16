# Answers

## 1. Where does the `Trace.WriteLine` method write its output to?

`Trace.WriteLine` writes output to **trace listeners**.

Examples include:

- **Visual Studio Output window**
- **Console** *(if a console trace listener is configured)*
- **Text files or log files** *(if a file listener is added)*
- **Custom listeners**

> By default, it commonly appears in the **Output** window while debugging, depending on configured listeners.

---

## 2. What are the five trace levels?

The five common trace levels are:

1. **Verbose**
2. **Information**
3. **Warning**
4. **Error**
5. **Critical**

---

## 3. What is the difference between the `Debug` and `Trace` classes?

| Class | Purpose |
|---|---|
| **`Debug`** | Used mainly during **development and debugging** |
| **`Trace`** | Used for **debugging and release scenarios**, especially application tracing |

### Key difference

- **`Debug`** statements usually work only in **Debug builds**
- **`Trace`** statements can work in **Debug and Release builds**

> Use `Debug` for developer-only diagnostic messages, and `Trace` for broader application tracing.

---

## 4. When writing a unit test, what are the three “A”s?

The three “A”s are:

1. **Arrange** — set up the test data and objects
2. **Act** — run the code being tested
3. **Assert** — verify the result

Example structure:

```csharp
// Arrange
var calculator = new PriceCalculator();

// Act
var total = calculator.AddTax(100m, 10m);

// Assert
Assert.Equal(110m, total);
```

---

## 5. When writing a unit test using xUnit, which attribute must you decorate the test methods with?

You decorate test methods with:

- **`[Fact]`** for a normal test
- **`[Theory]`** for a parameterized test

If the question asks for the standard required attribute for a basic test method, the answer is:

```csharp
[Fact]
```

---

## 6. What `dotnet` command executes xUnit tests?

Use:

```bash
dotnet test
```

If you want to run tests for a specific test project:

```bash
dotnet test MyProject.Tests
```