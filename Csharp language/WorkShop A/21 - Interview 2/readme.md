**1. What is the difference between a verbatim string and an interpolated string?**  
- **Verbatim string (`@`)**: Treats escape characters literally and allows multi‑line text.  
  ```csharp
  string path = @"C:\Users\John\Documents\file.txt";
  ```
- **Interpolated string (`$`)**: Allows embedding expressions directly inside `{}`.  
  ```csharp
  string name = "John";
  string message = $"Hello {name}";
  ```
They can also be combined:  
```csharp
string path = $@"C:\Users\{name}\Documents";
```

---

**2. Why should you be careful when using the `dynamic` type?**  
`dynamic` bypasses **compile‑time type checking**. Errors that would normally be caught during compilation are instead caught **at runtime**, which can cause runtime exceptions and make debugging harder.

---

**3. What is the newest syntax to create an instance of a class like `XmlDocument`?**  
**Target‑typed `new` expression** (C# 9+):

```csharp
XmlDocument doc = new();
```

The compiler infers the type from the variable declaration.

---

**4. What happens when you divide a `double` variable by 0?**  
No exception occurs. Instead, IEEE floating‑point rules apply:
- `x / 0.0` → `Infinity` or `-Infinity`
- `0.0 / 0.0` → `NaN`

Example:
```csharp
double x = 5.0 / 0.0;  // Infinity
```

---

**5. What happens when you divide an `int` variable by 0?**  
A **`DivideByZeroException`** is thrown at runtime.

```csharp
int x = 5 / 0;  // throws DivideByZeroException
```

---

**6. What interface must an object implement to be enumerated with `foreach`?**  
`IEnumerable` (or the generic `IEnumerable<T>`).

This interface provides a `GetEnumerator()` method that returns an enumerator used by `foreach`.

---

**7. What does the underscore (`_`) represent in a switch expression?**  
It is the **discard pattern** or **default case**.  
It matches **any value not matched by previous patterns**.

```csharp
var result = number switch
{
    1 => "One",
    2 => "Two",
    _ => "Other"
};
```

---

**8. What is the difference between the `=` and `==` operators?**  
- `=` → **Assignment operator** (assigns a value to a variable).  
  ```csharp
  int x = 5;
  ```
- `==` → **Equality comparison operator** (checks if two values are equal).  
  ```csharp
  if (x == 5)
  {
      // true if x equals 5
  }
  ```