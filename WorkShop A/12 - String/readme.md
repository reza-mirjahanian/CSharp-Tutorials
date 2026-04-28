# Literal Values in C#

When you assign something to a variable, you often give it a **literal value**.

## What is a literal?

A **literal** is a fixed value written directly in code.

### Examples

```csharp
int age = 25;
double price = 19.99;
char grade = 'A';
string name = "Sara";
```

In these examples:

- `25` is an **integer literal**
- `19.99` is a **floating-point literal**
- `'A'` is a **character literal**
- `"Sara"` is a **string literal**

Different data types use different literal notations.

---

# Character Literals

A **character literal** represents a single `char` value.

## Syntax

Character literals are written inside **single quotes**:

```csharp
char letter = 'A';
char digit = '7';
char symbol = '#';
```

## Important idea

A `char` stores a single **UTF-16 code unit**, not always a full human-readable letter.

> Do **not** always assume that one `char` equals one visible character.

Some Unicode characters need **two `char` values** to be represented. These are called **surrogate pairs**.

## Example

Egyptian Hieroglyph A002 (`U+13001`) requires two `char` values:

```csharp
char high = '\uD80C';
char low = '\uDC01';
```

### Why this matters

If your code assumes:

- 1 `char` = 1 letter

you may create subtle bugs when working with Unicode text.

---

# Escape Sequences in Character and String Literals

Some characters are hard to type directly or have special meaning. For these, C# uses **escape sequences**.

## Common escape sequences

| Escape | Meaning |
|---|---|
| `\t` | Tab |
| `\n` | New line |
| `\"` | Double quote |
| `\\` | Backslash |
| `\'` | Single quote |
| `\uXXXX` | Unicode character |

---

# Representing the ESC Character

The **ESC** character is Unicode `U+001B`.

## In C# 13 and later

You can use the shorter escape sequence:

```csharp
char esc = '\e';
```

## In C# 12 or earlier

Use the Unicode escape:

```csharp
char esc = '\u001b';
```

## Older alternative

You may also have seen:

```csharp
char esc = '\x1b';
```

### Why `\x1b` is not recommended

The `\x` form can be confusing because C# may continue reading more hexadecimal digits than you intended.

For example, if valid hex digits come after `1b`, they may be treated as part of the same escape sequence.

> Safer choices are **`\e`** in C# 13+ or **`\u001b`** in earlier versions.

---

# String Literals

A **string literal** is text enclosed in **double quotes**.

## Basic syntax

```csharp
string message = "Hello";
```

## Escape sequences inside normal string literals

Regular string literals can use escape characters:

```csharp
string text = "Line1\nLine2";
string tabbed = "Name\tAge";
string path = "C:\\Files\\Data";
```

### Key rule

To write a backslash in a normal string literal, use:

```csharp
\\
```

---

# Verbatim String Literals

A **verbatim string literal** is prefixed with `@`.

## Syntax

```csharp
string path = @"C:\Files\Data";
```

## What it does

Using `@` changes how the string is interpreted:

- escape sequences are **disabled**
- backslashes are treated as normal characters
- the string can span **multiple lines**

## Example

```csharp
string folder = @"C:\Users\Admin\Documents";
```

Here, the backslashes do **not** need to be doubled.

## Multiline example

```csharp
string poem = @"Roses are red
Violets are blue
C# is powerful
And flexible too";
```

Because it is verbatim:

- line breaks are preserved
- whitespace is treated as actual content

> You must prefix the string with `@` to make it a verbatim string literal.

---

# Raw String Literals

**Raw string literals** were introduced in **C# 11**.

They are designed to make it easy to write text **exactly as it appears**, without escaping.

## Syntax

A raw string literal starts and ends with **three or more double quotes**:

```csharp
string text = """
Hello
World
""";
```

## Why raw string literals are useful

They are especially convenient for text that contains:

- backslashes
- quotes
- JSON
- XML
- HTML
- code snippets

## Example: JSON

```csharp
string json = """
{
  "name": "Sara",
  "age": 25
}
""";
```

No escaping is needed for the quotes inside the JSON.

## Example: HTML

```csharp
string html = """
<div class="card">
  <h1>Hello</h1>
</div>
""";
```

## Main benefit

With raw string literals, the contents are written more naturally and are easier to read.

---

# Comparing String Literal Types

## 1. Regular string literal

Uses double quotes and supports escape sequences.

```csharp
string s = "Hello\tWorld";
```

### Features

- enclosed in `"` ... `"`
- supports escape sequences like `\t`, `\n`, `\\`

---

## 2. Verbatim string literal

Uses `@` before the string.

```csharp
string s = @"C:\Temp\Files";
```

### Features

- prefixed with `@`
- backslashes are treated literally
- can span multiple lines
- escape sequences are disabled

---

## 3. Raw string literal

Uses three or more double quotes.

```csharp
string s = """
C:\Temp\Files
""";
```

### Features

- enclosed in `"""` ... `"""`
- no need to escape most content
- great for multiline text
- ideal for embedded JSON, XML, or HTML

---

## 4. Interpolated string

Uses `$` before the string.

```csharp
string name = "Sara";
string message = $"Hello, {name}";
```

### Features

- prefixed with `$`
- allows embedded expressions inside `{ }`
- useful for building dynamic text

> You will often combine interpolation with other string styles.

---

# Quick Comparison Table

| Type | Syntax | Escape Sequences | Multiline | Best Use |
|---|---|---:|---:|---|
| Regular string literal | `"text"` | ✅ Yes | ❌ No | Simple text |
| Verbatim string literal | `@"text"` | ❌ No | ✅ Yes | File paths, multiline text |
| Raw string literal | `"""text"""` | Usually unnecessary | ✅ Yes | JSON, XML, HTML, exact text |
| Interpolated string | `$"text {value}"` | ✅ Yes | Usually no* | Dynamic text |

\* Interpolated strings can also be combined with verbatim or raw strings.

---

# Examples Side by Side

## Regular string

```csharp
string regular = "C:\\Users\\Ali\\Desktop";
```

## Verbatim string

```csharp
string verbatim = @"C:\Users\Ali\Desktop";
```

## Raw string

```csharp
string raw = """
C:\Users\Ali\Desktop
""";
```

All three can represent similar text, but the syntax and readability differ.

---

# Character vs String

| Type | Example | Meaning |
|---|---|---|
| `char` | `'A'` | A single character value |
| `string` | `"A"` | A sequence of characters |

## Example

```csharp
char letter = 'A';
string text = "A";
```

Even though they look similar, they are **different types**.

- `'A'` uses **single quotes** and is a `char`
- `"A"` uses **double quotes** and is a `string`

---

# Key Rules to Remember

## Character literals

- use **single quotes**
- example: `'A'`
- represent one `char`

## String literals

- use **double quotes**
- example: `"Hello"`
- represent text

## Verbatim strings

- prefix with `@`
- example: `@"C:\Temp\File.txt"`

## Raw strings

- wrap with `"""` or more double quotes
- useful for text that should stay exactly as written

## Interpolated strings

- prefix with `$`
- example: `$"Hello, {name}"`

## Escape character for backslash in normal strings

```csharp
\\
```

## ESC character

- C# 13+: `'\e'`
- Earlier C#: `'\u001b'`

---

# Mini Examples

## Character literal

```csharp
char initial = 'B';
```

## Regular string literal

```csharp
string greeting = "Hello\nWorld";
```

## Verbatim string literal

```csharp
string filePath = @"C:\Projects\App";
```

## Raw string literal

```csharp
string xml = """
<book>
  <title>C# Basics</title>
</book>
""";
```

## Interpolated string

```csharp
string user = "Mina";
string welcome = $"Welcome, {user}!";
```