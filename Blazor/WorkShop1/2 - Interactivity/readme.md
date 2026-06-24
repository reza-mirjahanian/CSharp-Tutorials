# Choices of Interactivity in Blazor

## Table of Contents

1. Introduction to Blazor Interactivity
2. Understanding Rendering Modes
3. Static Server-Side Rendering (Static SSR)
4. Server Interactivity
5. WebAssembly Interactivity
6. Comparing Interactivity Modes
7. How the Browser and Server Communicate
8. Choosing the Right Rendering Strategy
9. Practical Architecture Examples
10. Mixing Rendering Modes in Real Applications
11. Example Components
12. Common Mistakes and Best Practices

---

# Introduction to Blazor Interactivity

Blazor allows developers to build interactive web applications using C# instead of JavaScript.

One of the most important concepts in modern Blazor applications is **interactivity mode**.

Interactivity determines:

* Where code executes
* How UI updates happen
* Whether JavaScript is required
* How fast pages become interactive
* How much work is done on the server or client

In modern Blazor applications, components can use different rendering strategies depending on the application's needs.

---

# Understanding Rendering Modes

Blazor applications commonly use three major rendering approaches:

| Mode                      | Execution Location | Interactive | Requires Constant Server Connection |
| ------------------------- | ------------------ | ----------- | ----------------------------------- |
| Static SSR                | Server             | ❌ No        | ❌ No                                |
| Server Interactivity      | Server             | ✅ Yes       | ✅ Yes                               |
| WebAssembly Interactivity | Browser            | ✅ Yes       | ❌ No                                |

These modes can even be combined inside the same application.

---

# Static Server-Side Rendering (Static SSR)

## What Is Static SSR?

Static SSR means:

* The server generates HTML
* The browser receives a fully rendered page
* No interactive runtime is attached afterward

The page behaves like a traditional website.

## Characteristics

### ✅ Advantages

* Extremely fast initial page load
* SEO-friendly
* Minimal browser resource usage
* Works well for static content

### ❌ Limitations

* No live UI updates
* No event handling after rendering
* Buttons and forms require full page reloads

---

## Static SSR Flow

```text
User Request
      ↓
Server Generates HTML
      ↓
Browser Displays HTML
```

There is no persistent connection after rendering.

---

## Example: Static Component

```razor
<h2>Product Information</h2>

<p>This page is rendered on the server.</p>
```

This component displays content but has no client-side interaction.

---

## When to Use Static SSR

Use Static SSR for:

* Landing pages
* Blog articles
* Documentation pages
* Marketing websites
* Read-only dashboards

---

# Server Interactivity

## What Is Server Interactivity?

In Server Interactivity mode:

* Components execute on the server
* The browser communicates with the server continuously
* UI updates happen through a real-time connection

Blazor typically uses **SignalR** for this communication.

---

## Interaction Flow

```text
User Action
      ↓
Browser Sends Event
      ↓
Server Executes C# Code
      ↓
Updated UI Sent Back
      ↓
Browser Updates DOM
```

---

## Characteristics

### ✅ Advantages

* Small client download size
* Fast startup time
* Full C# capabilities on the server
* Easy access to databases and services

### ❌ Limitations

* Requires active network connection
* Increased server memory usage
* Scalability considerations for many users
* Higher latency compared to local execution

---

## Example: Interactive Counter

```razor
@rendermode InteractiveServer

<h3>Server Counter</h3>

<p>Current value: @currentValue</p>

<button @onclick="IncreaseValue">
    Increase
</button>

@code {
    private int currentValue = 10;

    private void IncreaseValue()
    {
        currentValue += 1;
    }
}
```

### What Happens Internally?

1. User clicks the button
2. Event travels to the server
3. C# method executes
4. Updated UI is returned
5. Browser refreshes only the changed section

---

# WebAssembly Interactivity

## What Is WebAssembly Interactivity?

With WebAssembly Interactivity:

* The application runs directly inside the browser
* C# code executes locally on the client
* No constant server connection is required

This mode uses:

* `.NET runtime`
* `WebAssembly (WASM)`

---

## Execution Flow

```text
Browser Downloads App
        ↓
.NET Runtime Starts
        ↓
C# Executes Inside Browser
        ↓
UI Updates Locally
```

---

## Characteristics

### ✅ Advantages

* Offline-friendly
* Very responsive UI
* Reduced server workload
* Local execution speed

### ❌ Limitations

* Larger initial download
* Slower first startup
* Limited direct access to server resources
* Browser memory constraints

---

## Example: WebAssembly Counter

```razor
@rendermode InteractiveWebAssembly

<h3>Client Counter</h3>

<p>Total clicks: @clicks</p>

<button @onclick="AddClick">
    Click Me
</button>

@code {
    private int clicks = 0;

    private void AddClick()
    {
        clicks++;
    }
}
```

All logic runs directly inside the browser.

---

# Comparing Interactivity Modes

## Feature Comparison

| Feature             | Static SSR | Interactive Server | Interactive WebAssembly |
| ------------------- | ---------- | ------------------ | ----------------------- |
| Interactive UI      | ❌          | ✅                  | ✅                       |
| Executes on Server  | ✅          | ✅                  | ❌                       |
| Executes in Browser | ❌          | ❌                  | ✅                       |
| Requires SignalR    | ❌          | ✅                  | ❌                       |
| Offline Support     | ❌          | ❌                  | ✅                       |
| Initial Load Speed  | Very Fast  | Fast               | Slower                  |
| Server Load         | Low        | High               | Low                     |
| SEO Friendly        | Excellent  | Good               | Moderate                |

---

# How the Browser and Server Communicate

## Server Interactivity Communication

In Interactive Server mode:

```text
Browser ↔ SignalR ↔ ASP.NET Core Server
```

The browser sends events such as:

* Button clicks
* Keyboard input
* Form submissions

The server processes these events and returns UI updates.

---

## WebAssembly Communication

In Interactive WebAssembly mode:

```text
Browser → Local .NET Runtime
```

The browser handles:

* UI rendering
* State management
* Event execution

Server communication only happens when APIs are called.

---

# Choosing the Right Rendering Strategy

## Use Static SSR When

* Content rarely changes
* SEO is important
* Maximum performance is required
* Interaction is minimal

---

## Use Interactive Server When

* Fast startup matters
* Application logic should stay on the server
* Users have reliable internet
* Centralized state management is preferred

---

## Use Interactive WebAssembly When

* Rich client-side interaction is needed
* Offline support is useful
* Reducing server cost is important
* Users interact heavily with the UI

---

# Practical Architecture Examples

## Example 1: Company Website

| Section          | Recommended Mode        |
| ---------------- | ----------------------- |
| Home Page        | Static SSR              |
| Product Catalog  | Static SSR              |
| Admin Dashboard  | Interactive Server      |
| Analytics Charts | Interactive WebAssembly |

---

## Example 2: Online Management System

| Feature                 | Recommended Mode        |
| ----------------------- | ----------------------- |
| Login Page              | Static SSR              |
| Data Entry Forms        | Interactive Server      |
| Real-Time Notifications | Interactive Server      |
| Interactive Graphs      | Interactive WebAssembly |

---

# Mixing Rendering Modes in Real Applications

Modern Blazor applications can combine multiple rendering modes.

For example:

```text
Application
├── Static Marketing Pages
├── Server Interactive Admin Area
└── WebAssembly Reporting Dashboard
```

This hybrid approach allows developers to optimize:

* Performance
* Scalability
* User experience
* Infrastructure cost

---

# Example Components

## Static Page Component

```razor
@page "/about"

<h1>About Us</h1>

<p>
    This content is rendered once on the server.
</p>
```

---

## Interactive Server Component

```razor
@rendermode InteractiveServer

<h3>Temperature Monitor</h3>

<p>Current temperature: @temperature°C</p>

<button @onclick="IncreaseTemperature">
    Increase
</button>

@code {
    private int temperature = 22;

    private void IncreaseTemperature()
    {
        temperature++;
    }
}
```

---

## Interactive WebAssembly Component

```razor
@rendermode InteractiveWebAssembly

<h3>Shopping Cart</h3>

<p>Items in cart: @itemCount</p>

<button @onclick="AddItem">
    Add Item
</button>

@code {
    private int itemCount = 3;

    private void AddItem()
    {
        itemCount += 1;
    }
}
```

---

# Common Mistakes and Best Practices

## ⚠️ Common Mistakes

### Using Server Interactivity Everywhere

This can:

* Increase server memory usage
* Reduce scalability
* Create unnecessary network traffic

---

### Using WebAssembly for Large Enterprise Logic

This may:

* Expose sensitive logic
* Increase application download size
* Reduce startup performance

---

## ✅ Best Practices

### Prefer Static SSR for Public Content

Use static rendering whenever interaction is unnecessary.

---

### Keep Interactive Areas Focused

Only enable interactivity where users actually need it.

---

### Use Hybrid Architectures

Combine rendering strategies for the best balance.

---

## 📌 Tip

Think of rendering modes as performance tools.

Different sections of an application may require different interactivity strategies.

Choosing the correct mode improves:

* User experience
* Server scalability
* Application responsiveness
* Hosting cost efficiency
