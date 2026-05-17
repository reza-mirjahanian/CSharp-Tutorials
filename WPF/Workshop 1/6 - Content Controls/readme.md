# Table of Contents

1. [What Are Content Controls in WPF?](#what-are-content-controls-in-wpf)
2. [Why Content Controls Matter](#why-content-controls-matter)
3. [The `ContentControl` Class](#the-contentcontrol-class)
4. [Common WPF Content Controls](#common-wpf-content-controls)
   1. [Label](#label)
   2. [Button](#button)
   3. [CheckBox](#checkbox)
   4. [RadioButton](#radiobutton)
   5. [GroupBox](#groupbox)
   6. [Expander](#expander)
   7. [ScrollViewer](#scrollviewer)
   8. [Window](#window)
   9. [ToolTip](#tooltip)
5. [How Content Works in WPF](#how-content-works-in-wpf)
   1. [Text Content](#text-content)
   2. [UI Element Content](#ui-element-content)
   3. [Complex Content](#complex-content)
6. [ContentControl Properties](#contentcontrol-properties)
7. [Using Content in XAML](#using-content-in-xaml)
8. [Using Content in C#](#using-content-in-c)
9. [Content Templates](#content-templates)
10. [Content vs Headered Content Controls](#content-vs-headered-content-controls)
11. [Nested Content Examples](#nested-content-examples)
12. [Best Practices](#best-practices)

---

# What Are Content Controls in WPF?

In **WPF**, a **content control** is a control designed to display **a single piece of content**.

That content can be:

- plain text
- an image
- another UI element
- a layout container
- a more complex visual structure

> A content control can hold **one object** in its `Content` property.

This single object can still be very powerful, because that one object may itself contain many child elements.

---

# Why Content Controls Matter

Content controls are important because they make WPF interfaces:

- **flexible**
- **composable**
- **easy to customize**
- **template-friendly**

For example, a `Button` does not have to show only text. It can contain:

- a `StackPanel`
- an icon
- formatted text
- multiple visual elements together

That means you can build rich interfaces without creating custom controls from scratch.

---

# The `ContentControl` Class

The base class behind many WPF controls is `ContentControl`.

It provides the core ability to store and show a single `Content` object.

## Basic idea

```xml
<ContentControl Content="Hello WPF!" />
```

This works because the control simply displays the value assigned to `Content`.

## Equivalent expanded form

```xml
<ContentControl>
    <ContentControl.Content>
        Hello WPF!
    </ContentControl.Content>
</ContentControl>
```

Both forms are valid.

---

# Common WPF Content Controls

## Label

A `Label` displays descriptive content, often for another control.

```xml
<Label Content="Email Address:" />
```

### Notes

- Commonly used next to input controls
- Can display more than just text
- Supports access keys with `_`

Example:

```xml
<Label Content="_Username" Target="{Binding ElementName=txtUser}" />
```

---

## Button

A `Button` is one of the most common content controls.

```xml
<Button Content="Save" Width="110" Height="36" />
```

### Button with complex content

```xml
<Button Padding="10,6">
    <StackPanel Orientation="Horizontal">
        <TextBlock Text="💾" Margin="0,0,6,0" />
        <TextBlock Text="Store Data" />
    </StackPanel>
</Button>
```

---

## CheckBox

A `CheckBox` can contain text or richer UI content.

```xml
<CheckBox Content="Enable notifications" />
```

### Example with custom content

```xml
<CheckBox>
    <StackPanel Orientation="Horizontal">
        <TextBlock Text="🔔" Margin="0,0,6,0"/>
        <TextBlock Text="Receive alerts"/>
    </StackPanel>
</CheckBox>
```

---

## RadioButton

A `RadioButton` is also a content control.

```xml
<RadioButton GroupName="ThemeChoice" Content="Light Mode" />
<RadioButton GroupName="ThemeChoice" Content="Dark Mode" />
```

---

## GroupBox

A `GroupBox` is used to visually group related controls.

```xml
<GroupBox Header="Profile Settings" Margin="10">
    <StackPanel Margin="8">
        <CheckBox Content="Show online status" />
        <CheckBox Content="Allow direct messages" />
    </StackPanel>
</GroupBox>
```

### Important

`GroupBox` is often discussed with content controls because it displays one main child inside it, though it also has a separate `Header`.

---

## Expander

An `Expander` shows content that can be expanded or collapsed.

```xml
<Expander Header="Advanced Filters" IsExpanded="True" Margin="10">
    <StackPanel Margin="8">
        <CheckBox Content="Include archived items" />
        <CheckBox Content="Only favorites" />
    </StackPanel>
</Expander>
```

---

## ScrollViewer

A `ScrollViewer` wraps one content element and adds scrolling.

```xml
<ScrollViewer Height="140" VerticalScrollBarVisibility="Auto">
    <TextBlock TextWrapping="Wrap">
        This is a long paragraph placed inside a scrollable container so the user can read more content without expanding the layout too much.
    </TextBlock>
</ScrollViewer>
```

---

## Window

A `Window` can also be treated as a content control because it hosts one root content element.

```xml
<Window x:Class="SampleApp.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Dashboard"
        Height="320"
        Width="520">
    <Grid>
        <TextBlock Text="Welcome!" HorizontalAlignment="Center" VerticalAlignment="Center" />
    </Grid>
</Window>
```

---

## ToolTip

A `ToolTip` can show rich content, not just plain text.

```xml
<Button Content="Hover me">
    <Button.ToolTip>
        <ToolTip>
            <StackPanel>
                <TextBlock Text="Detailed help" FontWeight="Bold" />
                <TextBlock Text="Click to start the process." />
            </StackPanel>
        </ToolTip>
    </Button.ToolTip>
</Button>
```

---

# How Content Works in WPF

## Text Content

The simplest content is text.

```xml
<Button Content="Continue" />
```

WPF automatically displays the string.

---

## UI Element Content

The content can be a UI element.

```xml
<Button>
    <Image Width="20" Height="20" Source="Assets/icon-save.png" />
</Button>
```

Here, the button content is an `Image`.

---

## Complex Content

The content can be a container that holds multiple elements.

```xml
<Button Padding="8">
    <DockPanel>
        <TextBlock Text="📁" Margin="0,0,8,0" />
        <TextBlock Text="Open Folder" />
    </DockPanel>
</Button>
```

Even though the button only has **one** content object, that object is a `DockPanel`, which can contain many children.

> **Single content object** does **not** mean a single visual item on screen.

---

# ContentControl Properties

Here are some important properties related to content controls:

| Property | Description |
|---|---|
| `Content` | The object displayed inside the control |
| `ContentTemplate` | A `DataTemplate` used to control how content is rendered |
| `ContentTemplateSelector` | Chooses a template dynamically |
| `ContentStringFormat` | Formats string output |
| `HorizontalContentAlignment` | Aligns content horizontally |
| `VerticalContentAlignment` | Aligns content vertically |
| `Padding` | Adds inner spacing around content |

## Example

```xml
<Button Content="Submit"
        Padding="12,6"
        HorizontalContentAlignment="Center"
        VerticalContentAlignment="Center" />
```

---

# Using Content in XAML

## Simple syntax

In many controls, the `Content` property is the **content property**, so you can write:

```xml
<Button>Send</Button>
```

This is the same as:

```xml
<Button Content="Send" />
```

## With nested elements

```xml
<Button Padding="8">
    <StackPanel Orientation="Horizontal">
        <TextBlock Text="📨" Margin="0,0,6,0" />
        <TextBlock Text="Send Message" />
    </StackPanel>
</Button>
```

---

# Using Content in C#

You can also assign content in code.

## Simple example

```csharp
var btn = new Button();
btn.Content = "Run Task";
```

## Using a visual element as content

```csharp
var panel = new StackPanel
{
    Orientation = Orientation.Horizontal
};

panel.Children.Add(new TextBlock
{
    Text = "✅",
    Margin = new Thickness(0, 0, 6, 0)
});

panel.Children.Add(new TextBlock
{
    Text = "Completed"
});

var btn = new Button
{
    Content = panel,
    Padding = new Thickness(10, 6, 10, 6)
};
```

---

# Content Templates

A content control becomes even more useful when it displays data objects instead of hardcoded UI elements.

You can define a `DataTemplate` to control how the content appears.

## Example model display

```xml
<Window.Resources>
    <DataTemplate x:Key="ProductCardTemplate">
        <Border BorderBrush="DarkGray" BorderThickness="1" Padding="10" CornerRadius="6">
            <StackPanel>
                <TextBlock Text="{Binding Title}" FontWeight="Bold" FontSize="15" />
                <TextBlock Text="{Binding Cost, StringFormat=Price: {0:C}}" />
            </StackPanel>
        </Border>
    </DataTemplate>
</Window.Resources>

<ContentControl Content="{Binding SelectedProduct}"
                ContentTemplate="{StaticResource ProductCardTemplate}" />
```

## Why use templates?

- **separates data from presentation**
- **keeps XAML cleaner**
- **supports reuse**
- **works well with MVVM**

---

# Content vs Headered Content Controls

Some controls have both a main content area and a header.

Examples include:

- `GroupBox`
- `Expander`
- `TabItem`

These are often based on **headered content** behavior rather than plain `ContentControl`.

## Comparison table

| Type | Main Property | Can Also Have Header? | Example |
|---|---|---|---|
| `ContentControl` | `Content` | No | `Button`, `Label` |
| `HeaderedContentControl` | `Content` | Yes | `GroupBox`, `Expander`, `TabItem` |

## Example

```xml
<Expander Header="More Details">
    <TextBlock Text="This area becomes visible when expanded." />
</Expander>
```

---

# Nested Content Examples

## Button containing a layout panel

```xml
<Button Width="160" Height="48">
    <Grid>
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="Auto"/>
            <ColumnDefinition Width="*"/>
        </Grid.ColumnDefinitions>

        <TextBlock Text="⭐" Margin="6,0,10,0" VerticalAlignment="Center"/>
        <TextBlock Grid.Column="1"
                   Text="Mark as Favorite"
                   VerticalAlignment="Center"/>
    </Grid>
</Button>
```

## Label containing formatted text

```xml
<Label>
    <TextBlock>
        <Run Text="Account status: " />
        <Run Text="Active" FontWeight="Bold" Foreground="Green" />
    </TextBlock>
</Label>
```

## ScrollViewer containing a complex layout

```xml
<ScrollViewer VerticalScrollBarVisibility="Auto" Height="180">
    <StackPanel Margin="12">
        <TextBlock Text="Release Notes" FontSize="18" FontWeight="Bold" Margin="0,0,0,10"/>
        <TextBlock TextWrapping="Wrap" Margin="0,0,0,8"
                   Text="This version introduces a refreshed layout, faster loading, and improved validation messages."/>
        <TextBlock TextWrapping="Wrap"
                   Text="Users can now customize notifications and manage saved filters more easily."/>
    </StackPanel>
</ScrollViewer>
```

---

# Best Practices

## 1. Keep content meaningful

Use content controls for what they are designed for:

- `Label` for descriptions
- `Button` for actions
- `CheckBox` for on/off choices

---

## 2. Prefer templates for data display

If the control displays business data, prefer:

- `ContentTemplate`
- `DataTemplate`
- binding

instead of manually building visuals in code.

---

## 3. Avoid putting too much UI inside simple controls

A `Button` can hold a full layout, but avoid making it unnecessarily complicated.

### Better approach

- keep content readable
- use styles and templates when complexity grows

---

## 4. Remember the “single child” rule

A content control accepts only **one direct content object**.

If you need multiple visual elements, wrap them in a container like:

- `StackPanel`
- `Grid`
- `DockPanel`
- `Border`

Example:

```xml
<Button>
    <StackPanel Orientation="Horizontal">
        <TextBlock Text="🔍" Margin="0,0,6,0" />
        <TextBlock Text="Search" />
    </StackPanel>
</Button>
```

---

## 5. Use alignment and padding carefully

These properties strongly affect appearance:

- `Padding`
- `HorizontalContentAlignment`
- `VerticalContentAlignment`

Example:

```xml
<Button Content="Upload"
        Padding="14,8"
        HorizontalContentAlignment="Center"
        VerticalContentAlignment="Center" />
```

---

## 6. Understand implicit content syntax

When a property is marked as the content property, this:

```xml
<Label>Project Name</Label>
```

means the same as:

```xml
<Label Content="Project Name" />
```

This shorthand is very common in WPF XAML.

---

## 7. Use rich content when it improves usability

Good uses of complex content include:

- icons beside text
- emphasized labels
- richer tooltips
- structured button content

Avoid complexity if it makes the UI harder to maintain.

---

# C# Example: Building a Content Control Dynamically

```csharp
var container = new ContentControl();

var card = new Border
{
    BorderBrush = Brushes.SlateGray,
    BorderThickness = new Thickness(1),
    Padding = new Thickness(12),
    CornerRadius = new CornerRadius(5)
};

var layout = new StackPanel();

layout.Children.Add(new TextBlock
{
    Text = "Server Status",
    FontSize = 16,
    FontWeight = FontWeights.Bold
});

layout.Children.Add(new TextBlock
{
    Text = "All services are running normally.",
    Margin = new Thickness(0, 6, 0, 0)
});

card.Child = layout;
container.Content = card;
```

---

# XAML Example: Displaying Data with `ContentTemplate`

```xml
<Window.Resources>
    <DataTemplate x:Key="UserBadgeTemplate">
        <Border Background="#F3F6FA"
                BorderBrush="#C8D2E1"
                BorderThickness="1"
                Padding="10"
                CornerRadius="6">
            <StackPanel Orientation="Horizontal">
                <TextBlock Text="👤" FontSize="18" Margin="0,0,8,0"/>
                <StackPanel>
                    <TextBlock Text="{Binding FullName}" FontWeight="Bold"/>
                    <TextBlock Text="{Binding Role}" Foreground="DimGray"/>
                </StackPanel>
            </StackPanel>
        </Border>
    </DataTemplate>
</Window.Resources>

<ContentControl Content="{Binding ActiveUser}"
                ContentTemplate="{StaticResource UserBadgeTemplate}" />
```