# Storing Values with `object`, `dynamic`, and `ExpandoObject`

## 1) Using `object` to Store Any Type

The `object` type can hold **any kind of value**.

> `object` is the base type of all types in C#.

That sounds convenient, but it has two important drawbacks:

- **Less clear code**
- **Possible performance cost**

Because of that, you should **avoid `object` unless you truly need it**, such as when working with older APIs or third-party libraries that require it.

---

## 2) Example: Storing Different Values in `object`

Create a console app and replace the contents of `Program.cs` with code like this:

```csharp
object elevation = 1.75; // double stored in object
object firstName = "Nima"; // string stored in object

Console.WriteLine($"{firstName} is {elevation} meters tall.");

// int size1 = firstName.Length; // Compile-time error!

int size2 = ((string)firstName).Length; // Cast required
Console.WriteLine($"{firstName} has {size2} characters.");
```

---

## 3) Why `firstName.Length` Fails

This line does **not** compile:

```csharp
int size1 = firstName.Length;
```

### Why?

Because the compiler only knows that `firstName` is an `object`.

- `object` does **not** define a `Length` property
- Even though the actual value is a string, the compiler cannot assume that

### Fix

You must explicitly tell the compiler what type is actually inside the variable:

```csharp
int size2 = ((string)firstName).Length;
```

This is called a **cast**.

---

## 4) Expected Output

```text
Nima is 1.75 meters tall.
Nima has 4 characters.
```

---

## 5) Key Idea About `object`

### With `object`:

- You can store **anything**
- But you often need to **cast back** to the real type before using type-specific members

### Example

| Variable Type | Stored Value | Can access `.Length` directly? |
|---|---:|---|
| `object` | `"Nima"` | ❌ No |
| `string` | `"Nima"` | ✅ Yes |

---

## 6) Good Practice

> **Avoid `object` whenever possible.**

Better alternatives usually exist, especially:

- **Specific types** like `string`, `int`, `double`
- **Generics**, when you need flexibility without losing type safety

---

# Storing Values with `dynamic`

## 7) What `dynamic` Does

The `dynamic` type can also store **any kind of value**.

Unlike `object`, you can access members on it **without casting**.

```csharp
dynamic item;
```

That makes it feel more flexible, but there is a trade-off:

- **Even weaker compile-time checking**
- **More runtime errors**
- **Performance overhead**

---

## 8) Example: Reassigning a `dynamic` Variable

```csharp
dynamic item;

// An array has a Length property.
item = new[] { 4, 8, 12 };

// int does not have a Length property.
item = 27;

// string has a Length property.
item = "Parsa";

// This compiles, but may fail at runtime depending on the actual value.
Console.WriteLine($"The length of item is {item.Length}");

// Show the real runtime type.
Console.WriteLine($"item is a {item.GetType()}");
```

---

## 9) Why This Compiles

This line compiles:

```csharp
Console.WriteLine($"The length of item is {item.Length}");
```

The compiler allows it because `item` is `dynamic`.

> With `dynamic`, member checking is delayed until **runtime**.

So the compiler says, in effect:

> “I’ll allow it now, and the runtime can figure it out later.”

---

## 10) Output When the Last Value Is a String

Since the last assigned value is `"Parsa"`, the code works:

```text
The length of item is 5
item is a System.String
```

---

## 11) Runtime Failure Example

If you comment out the string assignment:

```csharp
// item = "Parsa";
```

then the last assigned value becomes:

```csharp
item = 27;
```

Now this line will fail at runtime:

```csharp
Console.WriteLine($"The length of item is {item.Length}");
```

Because `int` has **no** `Length` property.

### Typical runtime error

```text
Unhandled exception. Microsoft.CSharp.RuntimeBinder.RuntimeBinderException:
'int' does not contain a definition for 'Length'
```

---

## 12) Output When the Last Value Is an Array

If you also comment out the integer assignment:

```csharp
// item = 27;
```

then the last assigned value is the array:

```csharp
item = new[] { 4, 8, 12 };
```

Arrays do have a `Length` property, so the program works:

```text
The length of item is 3
item is a System.Int32[]
```

---

## 13) `object` vs `dynamic`

## Comparison Table

| Feature | `object` | `dynamic` |
|---|---|---|
| Can store any type | ✅ Yes | ✅ Yes |
| Compile-time type checking | ✅ More checking | ❌ Very limited |
| Need cast to use type-specific members | ✅ Usually yes | ❌ No |
| Risk of runtime errors | ⚠️ Lower | ⚠️ Higher |
| IntelliSense support | ✅ Better | ❌ Often limited |
| Performance | Better than `dynamic` | Usually worse |

---

## 14) Important Limitation of `dynamic`

### Code editors often cannot help much

When using `dynamic`:

- IntelliSense may be missing or reduced
- The compiler cannot verify member names at build time
- Errors are discovered **only when the program runs**

That means code like this:

```csharp
dynamic data = "hello";
Console.WriteLine(data.Lenght); // Misspelled!
```

will compile, but fail at runtime because `Lenght` is not a valid member.

---

## 15) When `dynamic` Is Useful

`dynamic` is most useful when working with systems where types are not known clearly at compile time, such as:

- **COM interop**
  - e.g. automating Excel or Word
- **Non-.NET languages**
  - e.g. Python or JavaScript integrations
- **Loosely typed APIs**
- **Scripting scenarios**

> Use `dynamic` when flexibility is required, not as the default choice.

---

# Dynamic Objects with `ExpandoObject`

## 16) What `ExpandoObject` Is

`ExpandoObject` is a special dynamic object from the `System.Dynamic` namespace.

It allows you to:

- **Add properties at runtime**
- **Remove properties at runtime**
- Access those properties using **dot notation**

It behaves somewhat like a lightweight object whose shape can change while the program runs.

---

## 17) Namespace Import

At the top of `Program.cs`, add:

```csharp
using System.Dynamic;
```

---

## 18) Creating an `ExpandoObject`

```csharp
using System.Dynamic;

dynamic profile = new ExpandoObject();

// Add properties at runtime
profile.GivenName = "Sara";
profile.FamilyName = "Rahimi";
profile.YearsOld = 28;

Console.WriteLine($"{profile.GivenName} {profile.FamilyName} is {profile.YearsOld} years old.");
```

### Output

```text
Sara Rahimi is 28 years old.
```

---

## 19) Reading `ExpandoObject` as a Dictionary

Internally, `ExpandoObject` stores data like key-value pairs.

You can cast it to `IDictionary<string, object>` to inspect its contents.

```csharp
var values = (IDictionary<string, object>)profile;

foreach (var entry in values)
{
    Console.WriteLine($"{entry.Key} = {entry.Value}");
}
```

### Output

```text
GivenName = Sara
FamilyName = Rahimi
YearsOld = 28
```

---

## 20) Full Example

```csharp
using System.Dynamic;

dynamic profile = new ExpandoObject();

profile.GivenName = "Sara";
profile.FamilyName = "Rahimi";
profile.YearsOld = 28;

Console.WriteLine($"{profile.GivenName} {profile.FamilyName} is {profile.YearsOld} years old.");

var values = (IDictionary<string, object>)profile;

foreach (var entry in values)
{
    Console.WriteLine($"{entry.Key} = {entry.Value}");
}
```

---

## 21) Why `ExpandoObject` Can Be Useful

`ExpandoObject` is handy when you do **not** want a fixed class definition.

### Common use cases

- **Scripting**
- **Temporary data containers**
- **JSON-style flexible data**
- **Dynamic APIs**
- **Prototype-style development**

If you know JavaScript objects, this may feel familiar.

---

## 22) Simulating Methods

Because `ExpandoObject` can store delegates, you can attach behavior too:

```csharp
using System.Dynamic;

dynamic robot = new ExpandoObject();

robot.Name = "Robo";
robot.Speak = (Action)(() => Console.WriteLine($"Hello, I am {robot.Name}."));

robot.Speak();
```

### Output

```text
Hello, I am Robo.
```

---

## 23) When to Avoid `ExpandoObject`

Avoid it in code where these matter a lot:

- **High performance**
- **Strong type safety**
- **Reliable refactoring support**
- **Full IntelliSense and compile-time validation**

---

## 24) Choosing Between These Options

## Quick Guide

### Use a normal type when possible

```csharp
string title = "CSharp";
int count = title.Length;
```

This is the safest and clearest approach.

### Use `object` when:

- An API forces you to accept many types
- You are okay with explicit casting

### Use `dynamic` when:

- Type details are only known at runtime
- You are integrating with dynamic or external systems

### Use `ExpandoObject` when:

- You need an object with properties added dynamically
- A fixed class would be unnecessary or too rigid

---

## 25) Mental Model

### Think of them like this:

- **Specific type** → “I know exactly what this is.”
- **`object`** → “This can be anything, but I must cast it before using special members.”
- **`dynamic`** → “This can be anything, and I’ll try to use it directly; runtime will decide if that works.”
- **`ExpandoObject`** → “This is a dynamic object whose properties can be created on the fly.”

---

## 26) Small Side-by-Side Example

```csharp
object alpha = "code";
dynamic beta = "code";

int a = ((string)alpha).Length; // cast required
int b = beta.Length;            // no cast required
```

### Difference

- `alpha.Length` → compile-time error
- `beta.Length` → compiles, runtime checks it later

---

## 27) Best Practice Notes

> ✅ Prefer **strongly typed variables** whenever possible.

> ⚠️ Use `object` only when necessary.

> ⚠️ Use `dynamic` carefully, because errors move from compile time to runtime.

> ✅ Use `ExpandoObject` for flexible object-shaped data, not for strongly typed domain models.