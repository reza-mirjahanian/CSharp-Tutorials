# 🔍 Difference Between These Two Statements


# 📌 Core difference

| Statement | Kind | Stored? | Same object each time? |
|---|---|---:|---:|
| `public List<Person> Children = new();` | Field | Yes | Yes |
| `public List<Person> Children => new();` | Property | No | No |

---


## 1) Field

```csharp
public List<Person> Children = new();
```

This declares a **field** named `Children`.

- It creates **one list**
- That list is stored in the object
- Every time you access `Children`, you get the **same list instance**

### Example

```csharp
public class Family
{
    public List<Person> Children = new();
}
```

```csharp
var family = new Family();

family.Children.Add(new Person("Ava"));
family.Children.Add(new Person("Liam"));

Console.WriteLine(family.Children.Count); // 2
```

Why `2`?

Because the list is stored once, and you keep adding to that same list.

---

## 2) Expression-bodied property

```csharp
public List<Person> Children => new();
```

This declares a **read-only property** with an expression body.

It means:

> every time someone reads `Children`, create and return a **new list**

It is basically like this:

```csharp
public List<Person> Children
{
    get
    {
        return new List<Person>();
    }
}
```

So:

- no list is stored
- a fresh list is created on every access
- changes made to one returned list are lost the next time you access the property

---

# 🧪 Example of the second version

```csharp
public class Family
{
    public List<Person> Children => new();
}
```

```csharp
var family = new Family();

family.Children.Add(new Person("Ava"));
Console.WriteLine(family.Children.Count); // 0
```

Why `0`?

Because:

1. `family.Children` creates a new list
2. `"Ava"` is added to that temporary list
3. that list is not stored anywhere
4. `family.Children` is accessed again
5. a brand-new empty list is returned
6. count is `0`

---


# 🧠 Mental model

## First one

```csharp
public List<Person> Children = new();
```

Means:

> “This object has a list of children.”

---

## Second one

```csharp
public List<Person> Children => new();
```

Means:

> “Whenever you ask for children, I will hand you a brand-new empty list.”

That is usually **not** what you want for a collection property.

---

# ⚠️ Why the second version is often a bug

If your intention is to keep a collection as part of the object’s state, this is wrong:

```csharp
public List<Person> Children => new();
```

Because the object never actually remembers any children.

So code like this becomes useless:

```csharp
family.Children.Add(new Person("Noah"));
family.Children.Add(new Person("Emma"));
```

Each call works on a different temporary list.

---

# ✅ If you want a property instead of a field

A better version is:

```csharp
public List<Person> Children { get; } = new();
```

This gives you:

- a **property**
- one stored list
- the same list every time
- no public setter

### Example

```csharp
public class Family
{
    public List<Person> Children { get; } = new();
}
```

```csharp
var family = new Family();
family.Children.Add(new Person("Ava"));
Console.WriteLine(family.Children.Count); // 1
```

---

# 🆚 Field vs property vs computed property

## Field

```csharp
public List<Person> Children = new();
```

- direct storage
- publicly exposed field
- usually not recommended in public API design

---

## Auto-property with stored value

```csharp
public List<Person> Children { get; } = new();
```

- stored value
- better encapsulation
- common and recommended

---

## Computed property

```csharp
public List<Person> Children => new();
```

- computed every time
- creates a new object on each access
- usually wrong for mutable collections

---

# 🧪 Tiny demonstration

```csharp
var a = family.Children;
var b = family.Children;
```

## With field

```csharp
public List<Person> Children = new();
```

Then:

- `a` and `b` refer to the **same list**

So:

```csharp
Console.WriteLine(object.ReferenceEquals(a, b)); // True
```

---

## With expression-bodied property

```csharp
public List<Person> Children => new();
```

Then:

- `a` and `b` are **different lists**

So:

```csharp
Console.WriteLine(object.ReferenceEquals(a, b)); // False
```

---

# 🚫 Important practical consequence

This fails in spirit:

```csharp
family.Children.Add(new Person("Mina"));
family.Children.Add(new Person("Sara"));
Console.WriteLine(family.Children.Count);
```

With:

```csharp
public List<Person> Children => new();
```

The output is:

```csharp
0
```

Because each access gets a new empty list.

---

# ✅ Short version of the real difference

```csharp
public List<Person> Children = new();
```

means:

> create one list and store it in the object

while

```csharp
public List<Person> Children => new();
```

means:

> do not store anything; create a new list every time the property is read

---

# 📍 Most likely intended version

If you want a read-only property that still keeps the same collection:

```csharp
public List<Person> Children { get; } = new();
```

That is usually the best choice here.