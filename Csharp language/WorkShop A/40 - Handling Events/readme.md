# C# Raising and Handling Events

Events are C#’s built-in way for one object to **notify** other objects when something important happens.

They are commonly used when:

- a button is clicked
- a file finishes downloading
- a value changes
- a timer reaches its interval
- a process completes

You can think of an event as:

> “Something happened here — anyone interested can react.”

---

# Why Events Exist

Without events, one object would need to constantly check another object to see whether something changed.

That approach is called **polling**, and it is usually inefficient.

With events:

- the **publisher** announces that something happened
- one or more **subscribers** respond
- the publisher does **not need to know** who is listening

This creates **loose coupling** between classes.

---

# Core Event Terminology

## 1. Publisher

The class that **raises** the event.

## 2. Subscriber

The class or method that **handles** the event.

## 3. Event Handler

A method that runs when the event occurs.

## 4. Event Data

Optional information passed along with the event.

---

# Basic Event Model in C#

The standard event pattern in C# uses:

- `delegate`
- `event`
- a method that raises the event
- an event handler method in the subscriber

---

# Delegates and Events

An event is based on a **delegate**.

A delegate defines the **shape** of methods that can respond to the event.

For example, a handler may need to accept:

- the object that raised the event
- extra event information

This is why many events use the pattern:

```csharp
void Handler(object? sender, EventArgs e)
```

Or, when custom data is needed:

```csharp
void Handler(object? sender, StatusChangedEventArgs e)
```

---

# The Standard Event Signature

## Built-in delegate: `EventHandler`

Use this when no custom data is required.

```csharp
public event EventHandler? Started;
```

This corresponds to handlers like:

```csharp
void OnStarted(object? sender, EventArgs e)
{
    Console.WriteLine("Started event received.");
}
```

---

## Generic version: `EventHandler<TEventArgs>`

Use this when you want to send additional data.

```csharp
public event EventHandler<ProgressUpdatedEventArgs>? ProgressUpdated;
```

---

# Creating a Simple Event

## Example: A notifier that announces completion

```csharp
using System;

public class JobRunner
{
    public event EventHandler? Finished;

    public void Execute()
    {
        Console.WriteLine("Job is running...");
        RaiseFinished();
    }

    protected virtual void RaiseFinished()
    {
        Finished?.Invoke(this, EventArgs.Empty);
    }
}
```

## Subscriber

```csharp
using System;

public class ConsoleListener
{
    public void Subscribe(JobRunner runner)
    {
        runner.Finished += WhenFinished;
    }

    private void WhenFinished(object? sender, EventArgs e)
    {
        Console.WriteLine("Listener noticed that the job is done.");
    }
}
```

## Using both classes

```csharp
var runner = new JobRunner();
var listener = new ConsoleListener();

listener.Subscribe(runner);
runner.Execute();
```

### Flow

1. `ConsoleListener` subscribes to `runner.Finished`
2. `runner.Execute()` is called
3. `RaiseFinished()` invokes the event
4. `WhenFinished(...)` runs

---

# Raising an Event

To raise an event, call it like a delegate:

```csharp
Finished?.Invoke(this, EventArgs.Empty);
```

## Why `?.Invoke`?

Because an event may have **no subscribers**.

Using:

```csharp
Finished?.Invoke(...)
```

means:

> “Invoke the event only if at least one handler is attached.”

Without this null check, invoking the event with no subscribers would throw an exception.

---

# Sending Custom Event Data

Often you want to provide more information than just “something happened.”

For that, create a class that inherits from `EventArgs`.

---

## Custom `EventArgs`

```csharp
using System;

public class TemperatureChangedEventArgs : EventArgs
{
    public int PreviousValue { get; }
    public int CurrentValue { get; }

    public TemperatureChangedEventArgs(int previousValue, int currentValue)
    {
        PreviousValue = previousValue;
        CurrentValue = currentValue;
    }
}
```

---

## Publisher with custom event data

```csharp
using System;

public class Thermometer
{
    private int _temperature;

    public event EventHandler<TemperatureChangedEventArgs>? TemperatureChanged;

    public void SetTemperature(int newTemperature)
    {
        if (_temperature == newTemperature)
            return;

        int oldTemperature = _temperature;
        _temperature = newTemperature;

        RaiseTemperatureChanged(oldTemperature, newTemperature);
    }

    protected virtual void RaiseTemperatureChanged(int oldTemperature, int newTemperature)
    {
        var args = new TemperatureChangedEventArgs(oldTemperature, newTemperature);
        TemperatureChanged?.Invoke(this, args);
    }
}
```

---

## Subscriber

```csharp
using System;

public class TemperatureDisplay
{
    public void Register(Thermometer thermometer)
    {
        thermometer.TemperatureChanged += OnTemperatureChanged;
    }

    private void OnTemperatureChanged(object? sender, TemperatureChangedEventArgs e)
    {
        Console.WriteLine($"Temperature changed from {e.PreviousValue} to {e.CurrentValue}.");
    }
}
```

---

# Subscribing and Unsubscribing

## Subscribe with `+=`

```csharp
thermometer.TemperatureChanged += OnTemperatureChanged;
```

## Unsubscribe with `-=`

```csharp
thermometer.TemperatureChanged -= OnTemperatureChanged;
```

This is important when:

- the subscriber should stop listening
- you want to avoid duplicate handling
- you want to prevent memory leaks in long-lived applications

---

# Multiple Subscribers

An event can have **many subscribers**.

```csharp
runner.Finished += HandlerOne;
runner.Finished += HandlerTwo;
runner.Finished += HandlerThree;
```

When the event is raised, all attached handlers are called.

> Handlers are usually called in the order they were added.

---

# Example with Multiple Subscribers

```csharp
using System;

public class Alarm
{
    public event EventHandler? Activated;

    public void Trigger()
    {
        Console.WriteLine("Alarm triggered.");
        Activated?.Invoke(this, EventArgs.Empty);
    }
}
```

```csharp
using System;

var alarm = new Alarm();

alarm.Activated += (sender, e) =>
{
    Console.WriteLine("Security team notified.");
};

alarm.Activated += (sender, e) =>
{
    Console.WriteLine("Warning light switched on.");
};

alarm.Activated += (sender, e) =>
{
    Console.WriteLine("Activity recorded.");
};

alarm.Trigger();
```

---

# Anonymous Methods and Lambda Expressions

You do not always need a named method.

You can subscribe using a **lambda expression**:

```csharp
runner.Finished += (sender, e) =>
{
    Console.WriteLine("Lambda received the finished event.");
};
```

This is useful for short event responses.

---

# A More Realistic Example

## Publisher: Download tracker

```csharp
using System;

public class DownloadTracker
{
    public event EventHandler<DownloadProgressEventArgs>? ProgressChanged;
    public event EventHandler? Completed;

    public void Start()
    {
        for (int percent = 20; percent <= 100; percent += 20)
        {
            OnProgressChanged(percent);
        }

        OnCompleted();
    }

    protected virtual void OnProgressChanged(int percent)
    {
        ProgressChanged?.Invoke(this, new DownloadProgressEventArgs(percent));
    }

    protected virtual void OnCompleted()
    {
        Completed?.Invoke(this, EventArgs.Empty);
    }
}
```

## Custom event data

```csharp
using System;

public class DownloadProgressEventArgs : EventArgs
{
    public int Percentage { get; }

    public DownloadProgressEventArgs(int percentage)
    {
        Percentage = percentage;
    }
}
```

## Subscriber

```csharp
using System;

public class DownloadMonitor
{
    public void Attach(DownloadTracker tracker)
    {
        tracker.ProgressChanged += HandleProgressChanged;
        tracker.Completed += HandleCompleted;
    }

    private void HandleProgressChanged(object? sender, DownloadProgressEventArgs e)
    {
        Console.WriteLine($"Download progress: {e.Percentage}%");
    }

    private void HandleCompleted(object? sender, EventArgs e)
    {
        Console.WriteLine("Download completed.");
    }
}
```

## Usage

```csharp
var tracker = new DownloadTracker();
var monitor = new DownloadMonitor();

monitor.Attach(tracker);
tracker.Start();
```

---

# Encapsulation with `event`

Why not expose the delegate directly?

Because `event` protects it.

If you wrote this:

```csharp
public EventHandler? Finished;
```

outside code could:

- invoke it directly
- replace all handlers
- clear all subscriptions

That is dangerous.

Using `event` instead:

```csharp
public event EventHandler? Finished;
```

allows outside code to:

- subscribe with `+=`
- unsubscribe with `-=`

But **not** raise the event directly.

---

# Recommended Pattern: Protected Virtual `On...` Method

A common practice is to wrap event invocation in a method named `OnEventName`.

Example:

```csharp
protected virtual void OnFinished()
{
    Finished?.Invoke(this, EventArgs.Empty);
}
```

## Why this pattern is useful

- improves readability
- keeps raising logic in one place
- allows derived classes to customize behavior
- matches common .NET design style

---

# Full Example Following the Common Pattern

```csharp
using System;

public class CounterReachedEventArgs : EventArgs
{
    public int Count { get; }

    public CounterReachedEventArgs(int count)
    {
        Count = count;
    }
}

public class Counter
{
    public event EventHandler<CounterReachedEventArgs>? ThresholdReached;

    private readonly int _threshold;
    private int _current;

    public Counter(int threshold)
    {
        _threshold = threshold;
    }

    public void Add(int amount)
    {
        _current += amount;
        Console.WriteLine($"Current count: {_current}");

        if (_current >= _threshold)
        {
            OnThresholdReached(new CounterReachedEventArgs(_current));
        }
    }

    protected virtual void OnThresholdReached(CounterReachedEventArgs e)
    {
        ThresholdReached?.Invoke(this, e);
    }
}
```

## Subscriber

```csharp
using System;

public class CounterObserver
{
    public void Connect(Counter counter)
    {
        counter.ThresholdReached += Counter_ThresholdReached;
    }

    private void Counter_ThresholdReached(object? sender, CounterReachedEventArgs e)
    {
        Console.WriteLine($"Threshold reached at value {e.Count}.");
    }
}
```

## Usage

```csharp
var counter = new Counter(12);
var observer = new CounterObserver();

observer.Connect(counter);

counter.Add(3);
counter.Add(4);
counter.Add(6);
```

---

# Event Accessors

In advanced scenarios, you can manually control subscription behavior using `add` and `remove`.

```csharp
private EventHandler? _updated;

public event EventHandler Updated
{
    add
    {
        Console.WriteLine("A handler was added.");
        _updated += value;
    }
    remove
    {
        Console.WriteLine("A handler was removed.");
        _updated -= value;
    }
}
```

You would then invoke `_updated` internally:

```csharp
_updated?.Invoke(this, EventArgs.Empty);
```

This is useful when you need:

- custom logging
- thread-safe storage
- specialized subscription rules

---

# Important Rules and Best Practices

## 1. Use `EventArgs.Empty` when no data is needed

```csharp
Completed?.Invoke(this, EventArgs.Empty);
```

---

## 2. Derive custom event data from `EventArgs`

```csharp
public class ScoreChangedEventArgs : EventArgs
{
    public int Score { get; }

    public ScoreChangedEventArgs(int score)
    {
        Score = score;
    }
}
```

---

## 3. Name events with verbs or action-like names

Good names:

- `Completed`
- `ValueChanged`
- `ThresholdReached`
- `ItemAdded`

Less helpful names:

- `DoEvent`
- `StuffHappened`

---

## 4. Raise events only from inside the declaring class

That is one of the main reasons to use `event`.

---

## 5. Unsubscribe when appropriate

Particularly important for long-lived publishers.

Example:

```csharp
tracker.ProgressChanged -= HandleProgressChanged;
tracker.Completed -= HandleCompleted;
```

---

## 6. Keep handlers small when possible

Event handlers should usually:

- react quickly
- avoid unrelated responsibilities
- avoid heavy blocking work unless necessary

---

# Common Mistake: Forgetting to Unsubscribe

If a long-lived object publishes events and a short-lived object subscribes without unsubscribing, the subscriber may stay in memory longer than expected.

## Example

```csharp
public class NewsFeed
{
    public event EventHandler? Updated;

    public void Refresh()
    {
        Updated?.Invoke(this, EventArgs.Empty);
    }
}
```

```csharp
public class PopupWidget
{
    private readonly NewsFeed _feed;

    public PopupWidget(NewsFeed feed)
    {
        _feed = feed;
        _feed.Updated += OnFeedUpdated;
    }

    private void OnFeedUpdated(object? sender, EventArgs e)
    {
        Console.WriteLine("Widget refreshed.");
    }

    public void Dispose()
    {
        _feed.Updated -= OnFeedUpdated;
    }
}
```

If `Dispose()` is never called, the `NewsFeed` may continue holding a reference to the widget through the event subscription.

---

# Event Handler Signature Breakdown

Consider this method:

```csharp
private void OnTemperatureChanged(object? sender, TemperatureChangedEventArgs e)
{
    Console.WriteLine($"Old: {e.PreviousValue}, New: {e.CurrentValue}");
}
```

## Meaning of each part

| Part | Meaning |
|---|---|
| `private` | The method is only used inside the class |
| `void` | Event handlers usually return no value |
| `OnTemperatureChanged` | The handler’s name |
| `object? sender` | The object that raised the event |
| `TemperatureChangedEventArgs e` | Additional event data |

---

# `sender` and `e`

## `sender`

Usually the publisher object.

```csharp
if (sender is Thermometer thermometer)
{
    Console.WriteLine("Event came from a thermometer instance.");
}
```

## `e`

Contains the event-specific data.

```csharp
Console.WriteLine(e.CurrentValue);
```

---

# Minimal Pattern Reference

## Publisher

```csharp
public class Machine
{
    public event EventHandler? Started;

    public void Start()
    {
        Started?.Invoke(this, EventArgs.Empty);
    }
}
```

## Subscriber

```csharp
public class MachineListener
{
    public void Attach(Machine machine)
    {
        machine.Started += OnMachineStarted;
    }

    private void OnMachineStarted(object? sender, EventArgs e)
    {
        Console.WriteLine("Machine started.");
    }
}
```

---

# Step-by-Step Mental Model

## When defining an event

1. Decide **what happened**
2. Decide whether extra data is needed
3. Declare the event
4. Raise it at the right moment

## When handling an event

1. Get a reference to the publisher
2. Subscribe using `+=`
3. Write a compatible handler method
4. Optionally unsubscribe using `-=`

---

# Quick Comparison: Delegate vs Event

| Feature | Delegate Field | Event |
|---|---|---|
| Can subscribe with `+=` | Yes | Yes |
| Can unsubscribe with `-=` | Yes | Yes |
| Can invoke from outside the class | Yes | No |
| Can overwrite all handlers from outside | Yes | No |
| Recommended for notifications | No | Yes |

---

# Compact End-to-End Example

```csharp
using System;

public class MessageArrivedEventArgs : EventArgs
{
    public string Text { get; }

    public MessageArrivedEventArgs(string text)
    {
        Text = text;
    }
}

public class Inbox
{
    public event EventHandler<MessageArrivedEventArgs>? MessageArrived;

    public void Receive(string text)
    {
        OnMessageArrived(new MessageArrivedEventArgs(text));
    }

    protected virtual void OnMessageArrived(MessageArrivedEventArgs e)
    {
        MessageArrived?.Invoke(this, e);
    }
}

public class InboxViewer
{
    public void Watch(Inbox inbox)
    {
        inbox.MessageArrived += Inbox_MessageArrived;
    }

    private void Inbox_MessageArrived(object? sender, MessageArrivedEventArgs e)
    {
        Console.WriteLine($"New message: {e.Text}");
    }
}
```

## Usage

```csharp
var inbox = new Inbox();
var viewer = new InboxViewer();

viewer.Watch(inbox);
inbox.Receive("Your order is ready.");
```

---

# Key Pattern to Remember

```csharp
public event EventHandler<SomethingHappenedEventArgs>? SomethingHappened;

protected virtual void OnSomethingHappened(SomethingHappenedEventArgs e)
{
    SomethingHappened?.Invoke(this, e);
}
```

And in the subscriber:

```csharp
publisher.SomethingHappened += HandleSomethingHappened;

private void HandleSomethingHappened(object? sender, SomethingHappenedEventArgs e)
{
    // React here
}
```