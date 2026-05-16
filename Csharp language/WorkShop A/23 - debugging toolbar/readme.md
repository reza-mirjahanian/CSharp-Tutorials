# Navigating with the Debugging Toolbar in Visual Studio

## 🧭 What the Debugging Toolbar Is

Visual Studio gives you toolbar buttons to control a running program while debugging.

There are usually **two places** where debugging controls appear:

1. **Standard toolbar**
   - Contains the main debug start/continue button
   - Contains the **Hot Reload** button

2. **Debug toolbar**
   - Contains more detailed debugging controls
   - Used for pausing, stopping, restarting, and stepping through code

> The **Standard toolbar** is for starting or continuing debugging quickly.  
> The **Debug toolbar** is for controlling exactly how the program runs during debugging.

---

# 1. Standard Toolbar Debug Buttons

The **Standard toolbar** usually appears near the top of Visual Studio.

It contains two important debug-related buttons:

| Button | Common Icon | Purpose |
|---|---:|---|
| **Start Debugging / Continue** | ▶️ | Starts debugging or continues after a breakpoint |
| **Hot Reload** | 🔥 | Applies supported code changes while the app is still running |

---

## ▶️ Start Debugging / Continue Button

The green play button is one of the most important debugging buttons.

Depending on the current situation, it can mean either:

| Situation | Button Meaning |
|---|---|
| The app is not running | **Start Debugging** |
| The app is paused at a breakpoint | **Continue** |
| The app is paused after an exception | **Continue** |
| The debugger is attached to a process | **Continue execution** |

---

## Example: Start Debugging

Suppose you have this code:

```csharp
using System;

public class Program
{
    public static void Main()
    {
        string customerName = "Nora";
        int rewardPoints = 42;

        Console.WriteLine($"Customer: {customerName}");
        Console.WriteLine($"Points: {rewardPoints}");
    }
}
```
Clicking **Start Debugging** runs the program under the debugger.


Keyboard shortcut:

```text
F5
```

---

## Example: Continue After a Breakpoint

Imagine you place a breakpoint on this line:

```csharp
Console.WriteLine($"Points: {rewardPoints}");
```

When the debugger reaches that line, execution pauses.

At that moment, the green button changes behavior from:

```text
Start Debugging
```

to:

```text
Continue
```

Clicking it tells Visual Studio:

> Keep running the program until another breakpoint, exception, or program end is reached.

Keyboard shortcut:

```text
F5
```

---

# 2. 🔥 Hot Reload Button

The **Hot Reload** button lets you apply certain code changes while the program is still running.

This is useful when you want to make small changes without restarting the whole application.

---

## What Hot Reload Can Do

Hot Reload can often apply changes such as:

- Updating method bodies
- Changing string values
- Adjusting calculations
- Modifying some UI code
- Updating Razor pages in some ASP.NET Core projects
- Changing logic inside existing methods

---

## Example: Hot Reload

Original code:

```csharp
Console.WriteLine("Welcome to the billing tool.");
```

You change it to:

```csharp
Console.WriteLine("Welcome to the invoice manager.");
```

Then click:

```text
Hot Reload
```

Visual Studio attempts to apply the change while the app is still running.

---

## Hot Reload Limitations

Hot Reload cannot apply every kind of change.

Some changes may require restarting the application.

| Change Type | Usually Supported? |
|---|---:|
| Change text inside a method | ✅ Often |
| Change a calculation inside a method | ✅ Often |
| Add a new method | ⚠️ Sometimes |
| Rename a class | ❌ Often requires restart |
| Change project references | ❌ Requires restart |
| Change startup configuration | ❌ Requires restart |

---

# 3. The Separate Debug Toolbar

The **Debug toolbar** contains more advanced debugging commands.

It is especially useful after the program has started and is paused at a breakpoint.

Common buttons include:

| Command | Icon | Shortcut | Purpose |
|---|---:|---:|---|
| Break All | ⏸️ | Ctrl + Alt + Break | Pause the running program |
| Stop Debugging | ⏹️ | Shift + F5 | Stop the debugging session |
| Restart | 🔁 | Ctrl + Shift + F5 | Stop and start debugging again |
| Show Next Statement | ➡️ | Alt + Num * | Show the next line to execute |
| Step Into | ⬇️ | F11 | Enter a method call |
| Step Over | ↷ | F10 | Run a method without entering it |
| Step Out | ⬆️ | Shift + F11 | Finish the current method and return |
| Run to Cursor | 🎯 | Ctrl + F10 | Run until the cursor line is reached |

---

# 4. Starting a Debug Session

## Step 1: Set a Breakpoint

Click in the left margin beside a line of code.

Example:

```csharp
decimal subtotal = 85.50m;
decimal taxRate = 0.08m;
decimal total = subtotal + subtotal * taxRate;

Console.WriteLine($"Total: {total}");
```

Set a breakpoint on:

```csharp
decimal total = subtotal + subtotal * taxRate;
```

A red dot appears beside the line.

---

## Step 2: Start Debugging

Click the green button:

```text
Start Debugging
```

Or press:

```text
F5
```

---

## Step 3: Wait for the Breakpoint

When Visual Studio reaches the breakpoint:

- The program pauses
- The current line is highlighted
- Variables can be inspected
- Debug toolbar commands become useful

---

# 5. Continuing Program Execution

When paused at a breakpoint, click:

```text
Continue
```

or press:

```text
F5
```

The program continues running until one of these happens:

1. Another breakpoint is reached  
2. An exception occurs  
3. The program finishes  
4. You manually pause or stop debugging  

---

# 6. Pausing a Running Program

If your program is running and you want to inspect its current state, use:

```text
Break All
```

Icon:

```text
⏸️
```

This pauses execution wherever the program currently is.

---

## Example Scenario

Your code is stuck in a long loop:

```csharp
int counter = 0;

while (counter < 500000)
{
    counter++;

    if (counter % 10000 == 0)
    {
        Console.WriteLine($"Processed item {counter}");
    }
}
```

Clicking **Break All** lets you pause the program and inspect:

- Current value of counter
- Current call stack
- Running thread
- Current line of execution

---

# 7. Stopping Debugging

Use:

```text
Stop Debugging
```

Icon:

```text
⏹️
```

Shortcut:

```text
Shift + F5
```

This completely ends the debugging session.

---

## Stop Debugging vs Continue

| Command | What It Does |
|---|---|
| Continue | Keeps the program running |
| Stop Debugging | Ends the debugging session |
| Break All | Pauses the running program |
| Restart | Stops and starts again |

---

# 8. Restarting Debugging

Use:

```text
Restart
```

Icon:

```text
🔁
```

Shortcut:

```text
Ctrl + Shift + F5
```

Restarting is useful when:

- You changed startup logic
- Hot Reload cannot apply a change
- You want to test from the beginning
- The program state is no longer useful
- You need to reproduce a bug again

---

# 9. Stepping Through Code

Stepping lets you run code line by line.

---

## Example Code

```csharp
public class Program
{
    public static void Main()
    {
        decimal itemPrice = 120m;
        int quantity = 3;

        decimal subtotal = CalculateSubtotal(itemPrice, quantity);
        decimal finalTotal = AddHandlingFee(subtotal);

        Console.WriteLine($"Final total: {finalTotal}");
    }

    private static decimal CalculateSubtotal(decimal price, int count)
    {
        return price * count;
    }

    private static decimal AddHandlingFee(decimal amount)
    {
        return amount + 12m;
    }
}
```

Debugger paused here:

```csharp
decimal subtotal = CalculateSubtotal(itemPrice, quantity);
```

---

# 10. Step Over

Use:

```text
Step Over
```

Shortcut:

```text
F10
```

---

## Example

Current line:

```csharp
decimal subtotal = CalculateSubtotal(itemPrice, quantity);
```

Pressing F10 moves to:

```csharp
decimal finalTotal = AddHandlingFee(subtotal);
```

---

# 11. Step Into

Use:

```text
Step Into
```

Shortcut:

```text
F11
```

---

## Example

```csharp
decimal subtotal = CalculateSubtotal(itemPrice, quantity);
```

Step Into goes into:

```csharp
private static decimal CalculateSubtotal(decimal price, int count)
{
    return price * count;
}
```

---

# 12. Step Out

Use:

```text
Step Out
```

Shortcut:

```text
Shift + F11
```

---

## Example

Inside:

```csharp
private static decimal AddHandlingFee(decimal amount)
{
    return amount + 12m;
}
```

Step Out returns to:

```csharp
decimal finalTotal = AddHandlingFee(subtotal);
```

---

# 13. Run to Cursor

Use:

```text
Run to Cursor
```

Shortcut:

```text
Ctrl + F10
```

---

## Example

```csharp
decimal basePrice = 240m;
decimal shipping = 18m;
decimal discount = 30m;

decimal beforeDiscount = basePrice + shipping;
decimal afterDiscount = beforeDiscount - discount;

Console.WriteLine($"Amount due: {afterDiscount}");
```

Paused at:

```csharp
decimal basePrice = 240m;
```

Cursor on:

```csharp
Console.WriteLine($"Amount due: {afterDiscount}");
```

---

# 14. Show Next Statement

Use:

```text
Show Next Statement
```

---

# 15. Reading the Current Execution Line

Example:

```csharp
decimal discount = CalculateDiscount(orderAmount);
```

---

# 16. Debug Toolbar Drop-Down Lists

| Drop-Down | Purpose |
|---|---|
| Process | Process being debugged |
| Thread | Switch threads |
| Stack Frame | Move through call stack |
| Debug Target | Select startup target |

---

# 17. Process Selector

Examples:

```text
Inventory.Api.exe
```

```text
dotnet.exe
```

```text
ReportWorker.exe
```

---

# 18. Thread Selector

Used to inspect different threads.

---

# 19. Stack Frame Selector

Example call stack:

```text
ValidateOrder
ProcessOrder
Main
```

---

# 20. Debug Target Selector

Examples:

```text
https
```

```text
http
```

```text
IIS Express
```

---

# 21. Typical Debugging Workflow

```csharp
var invoiceTotal = CalculateInvoiceTotal(320m, 0.10m);
```

```text
▶️ Start Debugging
```

Inspect:

```csharp
invoiceTotal
```

```text
F10
F11
Shift + F11
```

```text
F5
Shift + F5
```

---

# 22. Choosing the Right Debug Button

| Goal | Use |
|---|---|
| Start debugging | Start Debugging |
| Continue | Continue |
| Apply changes | Hot Reload |
| Pause | Break All |
| Stop | Stop Debugging |
| Restart | Restart |
| Step | Step Over / Step Into / Step Out |
| Jump to cursor | Run to Cursor |
| Show execution line | Show Next Statement |

---

# 23. Start Debugging vs Start Without Debugging

| Command | Shortcut | Meaning |
|---|---:|---|
| Start Debugging | F5 | Debugger attached |
| Start Without Debugging | Ctrl + F5 | Runs normally |

---

# 24. Debugging Toolbar States

| State | Actions |
|---|---|
| Not running | Start Debugging |
| Running | Break All, Stop, Hot Reload |
| Paused | Continue, Step, Stop |
| Exception | Continue, Stop |
| Finished | Start Debugging |

---

# 25. Practical Example: Finding a Calculation Bug

```csharp
using System;

public class Program
{
    public static void Main()
    {
        decimal productPrice = 75m;
        int itemCount = 4;
        decimal couponAmount = 20m;

        decimal total = CalculateTotal(productPrice, itemCount, couponAmount);

        Console.WriteLine($"Total: {total}");
    }

    private static decimal CalculateTotal(decimal price, int quantity, decimal discount)
    {
        decimal subtotal = price * quantity;
        decimal finalAmount = subtotal + discount;

        return finalAmount;
    }
}
```

Bug:

```csharp
decimal finalAmount = subtotal + discount;
```

Fix:

```csharp
decimal finalAmount = subtotal - discount;
```

---

# 26. Useful Debug Keyboard Shortcuts

| Shortcut | Command |
|---:|---|
| F5 | Start / Continue |
| Ctrl + F5 | Start Without Debugging |
| Shift + F5 | Stop |
| Ctrl + Shift + F5 | Restart |
| F10 | Step Over |
| F11 | Step Into |
| Shift + F11 | Step Out |
| Ctrl + F10 | Run to Cursor |
| Ctrl + Alt + Break | Break All |

---
