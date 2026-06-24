# C# Indexers

## What Is an Indexer?

An **indexer** allows an object to be accessed using array-like syntax:

```csharp
objectName[index]
```

Instead of calling a method like this:

```csharp
playlist.GetSong(0);
```

You can write this:

```csharp
playlist[0];
```

> An indexer makes a class or struct behave like a collection.

---

## Basic Indexer Syntax

An indexer looks similar to a property, but it uses the keyword `this`.

```csharp
public returnType this[parameterType parameterName]
{
    get
    {
        // return a value
    }

    set
    {
        // assign a value
    }
}
```

### Important parts

| Part | Meaning |
|---|---|
| `this` | Refers to the current object |
| `[parameter]` | The index used to access data |
| `get` | Runs when reading a value |
| `set` | Runs when assigning a value |
| `value` | The value being assigned inside `set` |

---

# Simple Indexer Example

```csharp
using System;

public class Playlist
{
    private string[] _songs = new string[3];

    public string this[int index]
    {
        get
        {
            return _songs[index];
        }

        set
        {
            _songs[index] = value;
        }
    }
}

public class Program
{
    public static void Main()
    {
        Playlist playlist = new Playlist();

        playlist[0] = "Morning Lights";
        playlist[1] = "Silver Road";
        playlist[2] = "Ocean Echo";

        Console.WriteLine(playlist[1]);
    }
}
```

## How It Works

```csharp
playlist[1]
```

calls the indexer:

```csharp
public string this[int index]
```

So this line:

```csharp
Console.WriteLine(playlist[1]);
```

is similar in meaning to:

```csharp
Console.WriteLine(playlist.GetItem(1));
```

except the indexer gives cleaner syntax.

---

# Indexers Are Like Properties

Indexers are often described as **parameterized properties**.

## Property

```csharp
public string Name { get; set; }
```

Used like this:

```csharp
user.Name = "Mina";
```

## Indexer

```csharp
public string this[int index] { get; set; }
```

Used like this:

```csharp
items[0] = "Notebook";
```

| Feature | Property | Indexer |
|---|---|---|
| Has a name | Yes | Usually uses `this` |
| Can have `get` and `set` | Yes | Yes |
| Takes parameters | No | Yes |
| Access syntax | `obj.Name` | `obj[index]` |

---

# Read-Only Indexer

An indexer can be **read-only** by providing only a `get` accessor.

```csharp
using System;

public class WeekSchedule
{
    private string[] _days =
    {
        "Saturday",
        "Sunday",
        "Monday",
        "Tuesday",
        "Wednesday",
        "Thursday",
        "Friday"
    };

    public string this[int index]
    {
        get
        {
            return _days[index];
        }
    }
}

public class Program
{
    public static void Main()
    {
        WeekSchedule schedule = new WeekSchedule();

        Console.WriteLine(schedule[2]);
    }
}
```

This works:

```csharp
Console.WriteLine(schedule[2]);
```

This does **not** work:

```csharp
schedule[2] = "Holiday";
```

Because the indexer has no `set`.

---

# Write-Only Indexer

A write-only indexer has only a `set` accessor.

```csharp
using System;

public class SecretStore
{
    private string[] _entries = new string[5];

    public string this[int index]
    {
        set
        {
            _entries[index] = $"Stored: {value}";
        }
    }
}
```

Write-only indexers are uncommon because reading data is usually needed too.

---

# Indexer with Validation

Indexers can validate indexes before reading or writing.

```csharp
using System;

public class ScoreBoard
{
    private int[] _scores = new int[4];

    public int this[int index]
    {
        get
        {
            ValidateIndex(index);
            return _scores[index];
        }

        set
        {
            ValidateIndex(index);

            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Score cannot be negative.");

            _scores[index] = value;
        }
    }

    private void ValidateIndex(int index)
    {
        if (index < 0 || index >= _scores.Length)
            throw new IndexOutOfRangeException($"Index {index} is outside the scoreboard range.");
    }
}
```

## Usage

```csharp
ScoreBoard board = new ScoreBoard();

board[0] = 15;
board[1] = 24;

Console.WriteLine(board[1]);
```

> Use validation when your class needs to protect its internal data.

---

# Indexers with Different Parameter Types

An indexer does not have to use `int`.

You can use other parameter types such as:

- `string`
- `char`
- `Guid`
- custom types
- multiple parameters

---

## String Indexer Example

```csharp
using System;
using System.Collections.Generic;

public class SettingsBag
{
    private Dictionary<string, string> _settings = new();

    public string this[string key]
    {
        get
        {
            return _settings[key];
        }

        set
        {
            _settings[key] = value;
        }
    }
}

public class Program
{
    public static void Main()
    {
        SettingsBag settings = new SettingsBag();

        settings["theme"] = "dark";
        settings["language"] = "en-US";

        Console.WriteLine(settings["theme"]);
    }
}
```

Here, the object behaves like a dictionary:

```csharp
settings["theme"]
```

---

# Safer String Indexer

The previous example throws an exception if the key does not exist.

A safer version can return a default value:

```csharp
using System.Collections.Generic;

public class TranslationTable
{
    private Dictionary<string, string> _words = new();

    public string this[string key]
    {
        get
        {
            if (_words.TryGetValue(key, out string? result))
                return result;

            return "[missing translation]";
        }

        set
        {
            _words[key] = value;
        }
    }
}
```

## Usage

```csharp
TranslationTable table = new TranslationTable();

table["hello"] = "salaam";

Console.WriteLine(table["hello"]);
Console.WriteLine(table["goodbye"]);
```

Output:

```text
salaam
[missing translation]
```

---

# Multiple Indexer Parameters

Indexers can accept more than one parameter.

This is useful for matrix-like or grid-like objects.

```csharp
using System;

public class GameMap
{
    private string[,] _tiles = new string[3, 3];

    public string this[int row, int column]
    {
        get
        {
            return _tiles[row, column];
        }

        set
        {
            _tiles[row, column] = value;
        }
    }
}

public class Program
{
    public static void Main()
    {
        GameMap map = new GameMap();

        map[0, 0] = "Start";
        map[1, 2] = "River";
        map[2, 1] = "Tower";

        Console.WriteLine(map[1, 2]);
    }
}
```

The syntax:

```csharp
map[1, 2]
```

passes two arguments to the indexer:

```csharp
public string this[int row, int column]
```

---

# Indexer Overloading

A class can have multiple indexers if their parameter lists are different.

```csharp
using System;
using System.Collections.Generic;

public class LibraryShelf
{
    private List<string> _books = new()
    {
        "Cedar Valley",
        "Glass Harbor",
        "Quiet Machine"
    };

    public string this[int index]
    {
        get
        {
            return _books[index];
        }
    }

    public string this[string startsWith]
    {
        get
        {
            foreach (string book in _books)
            {
                if (book.StartsWith(startsWith, StringComparison.OrdinalIgnoreCase))
                    return book;
            }

            return "No matching book";
        }
    }
}
```

## Usage

```csharp
LibraryShelf shelf = new LibraryShelf();

Console.WriteLine(shelf[0]);
Console.WriteLine(shelf["Glass"]);
```

Output:

```text
Cedar Valley
Glass Harbor
```

---

# Expression-Bodied Indexers

For simple indexers, you can use expression-bodied syntax.

```csharp
public class NameList
{
    private string[] _names =
    {
        "Nika",
        "Arman",
        "Lina"
    };

    public string this[int index] => _names[index];
}
```

This is equivalent to:

```csharp
public string this[int index]
{
    get
    {
        return _names[index];
    }
}
```

---

# Expression-Bodied `get` and `set`

```csharp
public class TemperatureLog
{
    private double[] _values = new double[7];

    public double this[int day]
    {
        get => _values[day];
        set => _values[day] = value;
    }
}
```

---

# Indexers in Interfaces

Interfaces can declare indexers.

```csharp
public interface ITextStorage
{
    string this[int index] { get; set; }
}
```

A class can implement the indexer:

```csharp
public class NoteStorage : ITextStorage
{
    private string[] _notes = new string[10];

    public string this[int index]
    {
        get => _notes[index];
        set => _notes[index] = value;
    }
}
```

---

# Indexers in Abstract Classes

An abstract class can define an abstract indexer.

```csharp
public abstract class DataGrid
{
    public abstract string this[int row, int column] { get; set; }
}
```

A derived class must implement it:

```csharp
public class SimpleDataGrid : DataGrid
{
    private string[,] _cells = new string[4, 4];

    public override string this[int row, int column]
    {
        get => _cells[row, column];
        set => _cells[row, column] = value;
    }
}
```

---

# Indexers with Access Modifiers

The `get` and `set` accessors can have different accessibility.

```csharp
public class RankingTable
{
    private int[] _ranks = new int[5];

    public int this[int index]
    {
        get
        {
            return _ranks[index];
        }

        private set
        {
            _ranks[index] = value;
        }
    }

    public void UpdateRank(int index, int rank)
    {
        this[index] = rank;
    }
}
```

## Meaning

| Accessor | Accessibility |
|---|---|
| `get` | Public |
| `set` | Private |

External code can read:

```csharp
int rank = table[0];
```

But external code cannot assign:

```csharp
table[0] = 3;
```

Only the class itself can assign through the private setter.

---

# Indexers and Encapsulation

Indexers help hide internal storage.

Your class may internally use:

- an array
- a `List<T>`
- a `Dictionary<TKey, TValue>`
- a database call
- calculated values

The caller does not need to know.

```csharp
public class PriceCatalog
{
    private Dictionary<string, decimal> _prices = new()
    {
        ["tea"] = 2.50m,
        ["coffee"] = 3.20m,
        ["juice"] = 2.90m
    };

    public decimal this[string itemName]
    {
        get
        {
            return _prices.TryGetValue(itemName, out decimal price)
                ? price
                : 0m;
        }

        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Price cannot be negative.");

            _prices[itemName] = value;
        }
    }
}
```

---

# When to Use Indexers

## ✅ Good Use Cases

Use indexers when your type naturally behaves like:

- a collection
- a table
- a grid
- a lookup object
- a sequence
- a dictionary-like object

Examples:

```csharp
playlist[0]
```

```csharp
matrix[2, 3]
```

```csharp
settings["fontSize"]
```

---

## ⚠️ Avoid Indexers When

Avoid indexers if the operation is not naturally an indexing operation.

For example, this is unclear:

```csharp
customer[0]
```

What does `0` mean?

- first order?
- first address?
- first invoice?
- customer ID?

In that case, named methods or properties are clearer:

```csharp
customer.GetOrder(0);
customer.PrimaryAddress;
customer.FindInvoice(invoiceNumber);
```

---

# Indexer Rules

## Key rules

1. An indexer must use the `this` keyword.
2. An indexer can have `get`, `set`, or both.
3. An indexer can take one or more parameters.
4. An indexer parameter can be many types, not only `int`.
5. Indexers can be overloaded.
6. Indexers cannot be `static`.
7. Indexers are usually used to expose collection-like access.

---

# C# `field` Keyword

## What Is the `field` Keyword?

The `field` keyword gives access to the **compiler-generated backing field** of an auto-implemented property.

It is used inside property accessors:

```csharp
public string Name
{
    get;
    set
    {
        field = value;
    }
}
```

Normally, an auto-property hides its backing field from you.

```csharp
public string Name { get; set; }
```

The compiler secretly creates a field behind the scenes.

With `field`, you can access that hidden field directly inside the property.

---

# Why the `field` Keyword Is Useful

Before `field`, if you wanted custom logic in a property setter, you usually had to write a manual backing field.

## Traditional approach

```csharp
private string _displayName = "Guest";

public string DisplayName
{
    get
    {
        return _displayName;
    }

    set
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Display name cannot be empty.");

        _displayName = value;
    }
}
```

## With `field`

```csharp
public string DisplayName
{
    get;

    set
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Display name cannot be empty.");

        field = value;
    }
} = "Guest";
```

The `field` keyword removes the need to manually declare:

```csharp
private string _displayName;
```

---

# Basic `field` Example

```csharp
using System;

public class UserProfile
{
    public string Username
    {
        get;

        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Username is required.");

            field = value.Trim();
        }
    } = "visitor";
}
```

## Usage

```csharp
UserProfile profile = new UserProfile();

profile.Username = "  arya  ";

Console.WriteLine(profile.Username);
```

Output:

```text
arya
```

The setter receives:

```csharp
"  arya  "
```

Then stores:

```csharp
"arya"
```

using:

```csharp
field = value.Trim();
```

---

# `field` in a Getter

You can also use `field` in a getter.

```csharp
public class Product
{
    public string Code
    {
        get
        {
            return field.ToUpperInvariant();
        }

        set
        {
            field = value.Trim();
        }
    } = "x-100";
}
```

## Usage

```csharp
Product product = new Product();

product.Code = " p-204 ";

Console.WriteLine(product.Code);
```

Output:

```text
P-204
```

## Important detail

The stored value is:

```text
p-204
```

But the getter returns:

```text
P-204
```

because it transforms the value when reading.

---

# `field` with Validation

The most common use of `field` is validation.

```csharp
using System;

public class BankAccount
{
    public decimal Balance
    {
        get;

        set
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value), "Balance cannot be negative.");

            field = value;
        }
    }
}
```

This prevents invalid data:

```csharp
account.Balance = -50m;
```

---

# `field` with Normalization

You can use `field` to clean or normalize data before storing it.

```csharp
public class Contact
{
    public string Email
    {
        get;

        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Email is required.");

            field = value.Trim().ToLowerInvariant();
