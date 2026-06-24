# Table of Contents

1. [What Are Panels in WPF?](#what-are-panels-in-wpf)
2. [Why Panels Matter](#why-panels-matter)
3. [How WPF Layout Works](#how-wpf-layout-works)
4. [Common WPF Panels](#common-wpf-panels)
   1. [Grid](#grid)
   2. [StackPanel](#stackpanel)
   3. [WrapPanel](#wrappanel)
   4. [DockPanel](#dockpanel)
   5. [Canvas](#canvas)
   6. [UniformGrid](#uniformgrid)
5. [Panel Comparison Table](#panel-comparison-table)
6. [Attached Properties in Panels](#attached-properties-in-panels)
7. [Sizing, Alignment, and Margins](#sizing-alignment-and-margins)
8. [Choosing the Right Panel](#choosing-the-right-panel)
9. [Nested Layouts](#nested-layouts)
10. [Performance and Best Practices](#performance-and-best-practices)
11. [Code Examples](#code-examples)

---

# What Are Panels in WPF?

In **WPF**, a **panel** is a layout container that arranges child elements on the screen.

Think of a panel as a *manager* for UI elements like:

- buttons
- text boxes
- labels
- images
- lists

A panel decides things such as:

- **where** child elements appear
- **how much space** they get
- **how they resize** when the window changes size

> In short: **Panels control layout**.

Most visible WPF interfaces are built by placing controls inside one or more panels.

---

# Why Panels Matter

Without panels, every control would need to be positioned manually. That quickly becomes difficult when:

- the window is resized
- content length changes
- font sizes increase
- the app runs on different screen sizes

Panels make layouts:

- **flexible**
- **responsive**
- **easier to maintain**
- **more readable**

For example:

- a login form may use a **Grid**
- a toolbar may use a **StackPanel**
- a drawing surface may use a **Canvas**

---

# How WPF Layout Works

WPF layout mainly works in **two phases**:

## 1. Measure phase

Each parent asks its children:

> “How much space do you need?”

Each child reports its preferred size.

## 2. Arrange phase

The parent decides:

> “Here is the final space you will use.”

Then the child is positioned and rendered.

This layout system allows controls to adapt naturally.

## Important layout-related properties

Some properties often work together with panels:

- `Width`
- `Height`
- `MinWidth`
- `MinHeight`
- `MaxWidth`
- `MaxHeight`
- `Margin`
- `HorizontalAlignment`
- `VerticalAlignment`

---

# Common WPF Panels

## Grid

`Grid` is one of the most important and commonly used panels in WPF.

It arranges content in:

- **rows**
- **columns**

This is ideal for:

- forms
- dashboards
- data entry screens
- general application windows

### Defining rows and columns

You define structure using:

- `RowDefinitions`
- `ColumnDefinitions`

Each row or column can use different sizing styles:

- **fixed size** → `120`
- **auto size** → `Auto`
- **star sizing** → `*`, `2*`, `3*`

### Example

```xml
<Grid Margin="12">
    <Grid.RowDefinitions>
        <RowDefinition Height="Auto"/>
        <RowDefinition Height="*"/>
        <RowDefinition Height="Auto"/>
    </Grid.RowDefinitions>

    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="Auto"/>
        <ColumnDefinition Width="2*"/>
    </Grid.ColumnDefinitions>

    <TextBlock Grid.Row="0" Grid.Column="0"
               Margin="6"
               VerticalAlignment="Center"
               Text="Full Name:"/>

    <TextBox Grid.Row="0" Grid.Column="1"
             Margin="6"
             MinWidth="180"/>

    <ListBox Grid.Row="1" Grid.Column="0"
             Grid.ColumnSpan="2"
             Margin="6"/>

    <Button Grid.Row="2" Grid.Column="1"
            Margin="6"
            Width="110"
            HorizontalAlignment="Right"
            Content="Save Record"/>
</Grid>
```

### Star sizing

`*` means “take a proportional share of available space.”

Examples:

- `*` → one share
- `2*` → two shares
- `4*` → four shares

If three columns are defined as:

```xml
<ColumnDefinition Width="*"/>
<ColumnDefinition Width="2*"/>
<ColumnDefinition Width="*"/>
```

Then available width is split into:

- 1 part
- 2 parts
- 1 part

So the middle column gets more space.

### Useful Grid attached properties

- `Grid.Row`
- `Grid.Column`
- `Grid.RowSpan`
- `Grid.ColumnSpan`

---

## StackPanel

`StackPanel` arranges child elements in a **single line**:

- vertically
- or horizontally

By default, it stacks items **vertically**.

### Good use cases

- menus
- toolbars
- simple groups of controls
- settings sections

### Vertical StackPanel example

```xml
<StackPanel Margin="14">
    <TextBlock Margin="0,0,0,8"
               FontSize="18"
               Text="Account Settings"/>

    <CheckBox Margin="0,0,0,6" Content="Enable alerts"/>
    <CheckBox Margin="0,0,0,6" Content="Use dark theme"/>
    <CheckBox Margin="0,0,0,6" Content="Remember last page"/>
</StackPanel>
```

### Horizontal StackPanel example

```xml
<StackPanel Orientation="Horizontal" Margin="10">
    <Button Margin="4" Padding="12,6" Content="New"/>
    <Button Margin="4" Padding="12,6" Content="Open"/>
    <Button Margin="4" Padding="12,6" Content="Export"/>
</StackPanel>
```

### Important note

A `StackPanel` gives children as much space as they need in the stacking direction.

That means it is **not always the best choice** when you want controls to stretch evenly.

> For form-like layouts, `Grid` is often better than `StackPanel`.

---

## WrapPanel

`WrapPanel` arranges items in a line and automatically moves them to the next row or column when there is not enough space.

### Good use cases

- tag lists
- thumbnail galleries
- button collections
- responsive item groups

### Example

```xml
<WrapPanel Margin="10">
    <Button Margin="5" Content="C#"/>
    <Button Margin="5" Content="WPF"/>
    <Button Margin="5" Content="XAML"/>
    <Button Margin="5" Content=".NET"/>
    <Button Margin="5" Content="UI Design"/>
    <Button Margin="5" Content="Accessibility"/>
</WrapPanel>
```

If the window becomes narrower, items wrap automatically.

### Key property

- `Orientation`
  - `Horizontal` → wraps to the next row
  - `Vertical` → wraps to the next column

---

## DockPanel

`DockPanel` attaches child elements to one of the edges of the available space:

- top
- bottom
- left
- right

It is useful when building layouts like:

- tool windows
- sidebars
- header/content/footer areas

### Example

```xml
<DockPanel LastChildFill="True">
    <Border DockPanel.Dock="Top"
            Background="LightSteelBlue"
            Padding="10">
        <TextBlock FontSize="20" Text="Inventory Manager"/>
    </Border>

    <StackPanel DockPanel.Dock="Left"
                Width="140"
                Background="Beige">
        <Button Margin="8" Content="Overview"/>
        <Button Margin="8" Content="Products"/>
        <Button Margin="8" Content="Reports"/>
    </StackPanel>

    <Border Background="WhiteSmoke" Padding="14">
        <TextBlock Text="Main workspace area"/>
    </Border>
</DockPanel>
```

### Important property

- `DockPanel.Dock`

Possible values:

- `Top`
- `Bottom`
- `Left`
- `Right`

### `LastChildFill`

By default, `LastChildFill="True"`.

That means the last child automatically fills the remaining space.

---

## Canvas

`Canvas` places elements using **absolute coordinates**.

Unlike most other panels, it does **not** automatically organize children in rows, columns, or stacks.

### Positioning properties

- `Canvas.Left`
- `Canvas.Top`
- `Canvas.Right`
- `Canvas.Bottom`

### Example

```xml
<Canvas Background="AliceBlue" Width="320" Height="180">
    <Button Canvas.Left="18"
            Canvas.Top="22"
            Width="90"
            Height="34"
            Content="Start"/>

    <Ellipse Canvas.Left="150"
             Canvas.Top="35"
             Width="70"
             Height="70"
             Fill="Coral"/>

    <TextBlock Canvas.Left="120"
               Canvas.Top="125"
               FontSize="16"
               Text="Sketch Area"/>
</Canvas>
```

### When to use Canvas

Use `Canvas` when you need:

- exact positioning
- drawing surfaces
- drag-and-drop design areas
- game-like UI scenes

### Warning

`Canvas` is usually **not ideal** for resizable business forms because absolute positions do not adapt well.

---

## UniformGrid

`UniformGrid` arranges all child elements into a grid where every cell has the **same size**.

This makes it very useful for:

- icon menus
- keypad layouts
- game boards
- equal-sized button groups

### Example

```xml
<UniformGrid Rows="2" Columns="3" Margin="10">
    <Button Content="7" Margin="4"/>
    <Button Content="8" Margin="4"/>
    <Button Content="9" Margin="4"/>
    <Button Content="4" Margin="4"/>
    <Button Content="5" Margin="4"/>
    <Button Content="6" Margin="4"/>
</UniformGrid>
```

### Key features

- simple to use
- equal cell sizes
- less flexible than `Grid`
- great when uniformity matters more than custom structure

---

# Panel Comparison Table

| Panel | Main Layout Style | Best For | Strength | Limitation |
|---|---|---|---|---|
| `Grid` | Rows and columns | Forms, structured screens | Very flexible | More setup needed |
| `StackPanel` | Single vertical/horizontal stack | Simple grouped controls | Easy to use | Weak for complex resizing |
| `WrapPanel` | Flow + wrapping | Tags, thumbnails, button clouds | Adapts to width/height | Not ideal for strict alignment |
| `DockPanel` | Edge docking | App shells, sidebars | Great for outer layout | Less precise for detailed forms |
| `Canvas` | Absolute positioning | Drawing, custom surfaces | Exact placement | Poor responsiveness |
| `UniformGrid` | Equal-sized grid cells | Keypads, tiled menus | Very consistent | Limited customization |

---

# Attached Properties in Panels

Some panels use **attached properties**.

An attached property is a property defined by one class but used on child elements inside it.

Examples:

- `Grid.Row`
- `Grid.Column`
- `DockPanel.Dock`
- `Canvas.Left`
- `Canvas.Top`

### Example

```xml
<Grid>
    <Grid.RowDefinitions>
        <RowDefinition Height="Auto"/>
        <RowDefinition Height="*"/>
    </Grid.RowDefinitions>

    <TextBlock Grid.Row="0" Text="Header"/>
    <TextBox Grid.Row="1" Text="Editable content"/>
</Grid>
```

Here:

- the `TextBlock` does not own a `Row` property by itself
- `Grid` provides `Grid.Row`
- the child uses it so the parent knows where to place it

> Attached properties are a core part of how WPF panels control child layout.

---

# Sizing, Alignment, and Margins

Panels work closely with element sizing rules.

## Width and Height

You can set explicit size values:

```xml
<Button Width="130" Height="36" Content="Submit"/>
```

But in many cases, letting WPF size controls automatically is better.

## Margin

`Margin` adds outer spacing around an element.

```xml
<TextBox Margin="8"/>
```

This means extra space outside the control.

## Alignment

Common alignment properties:

- `HorizontalAlignment`
  - `Left`
  - `Center`
  - `Right`
  - `Stretch`

- `VerticalAlignment`
  - `Top`
  - `Center`
  - `Bottom`
  - `Stretch`

### Example

```xml
<Button Width="120"
        Height="36"
        Margin="10"
        HorizontalAlignment="Right"
        VerticalAlignment="Center"
        Content="Apply"/>
```

## Padding vs Margin

| Property | Meaning |
|---|---|
| `Margin` | Space outside the control |
| `Padding` | Space inside the control, between content and border |

Example:

```xml
<Button Margin="8"
        Padding="14,7"
        Content="Continue"/>
```

---

# Choosing the Right Panel

Choosing a panel depends on the layout goal.

## Use `Grid` when:

- you need rows and columns
- controls must align cleanly
- the layout is form-like
- resizing should behave well

## Use `StackPanel` when:

- items appear in one direction
- layout is simple
- content is grouped linearly

## Use `WrapPanel` when:

- items should flow naturally
- screen width may change
- wrapping is desired

## Use `DockPanel` when:

- you are building a main application shell
- content belongs to edges and center regions

## Use `Canvas` when:

- exact coordinates matter
- freeform placement is required

## Use `UniformGrid` when:

- all cells should be equal
- a tiled layout is needed

---

# Nested Layouts

In real applications, one panel is often placed inside another.

This is called **nested layout**.

For example:

- outer `DockPanel` for app structure
- inner `Grid` for form fields
- inner `StackPanel` for button groups

### Example

```xml
<DockPanel>
    <Border DockPanel.Dock="Top"
            Background="#D9E7FF"
            Padding="12">
        <TextBlock FontSize="22" Text="Customer Editor"/>
    </Border>

    <Grid Margin="14">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
            <RowDefinition Height="Auto"/>
        </Grid.RowDefinitions>

        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="Auto"/>
            <ColumnDefinition Width="*"/>
        </Grid.ColumnDefinitions>

        <TextBlock Grid.Row="0" Grid.Column="0" Margin="6" Text="Email:"/>
        <TextBox Grid.Row="0" Grid.Column="1" Margin="6"/>

        <TextBlock Grid.Row="1" Grid.Column="0" Margin="6" Text="Phone:"/>
        <TextBox Grid.Row="1" Grid.Column="1" Margin="6"/>

        <TextBox Grid.Row="2"
                 Grid.Column="0"
                 Grid.ColumnSpan="2"
                 Margin="6"
                 AcceptsReturn="True"
                 VerticalScrollBarVisibility="Auto"/>

        <StackPanel Grid.Row="3"
                    Grid.Column="1"
                    Orientation="Horizontal"
                    HorizontalAlignment="Right">
            <Button Margin="6" Padding="14,6" Content="Cancel"/>
            <Button Margin="6" Padding="14,6" Content="Update"/>
        </StackPanel>
    </Grid>
</DockPanel>
```

### Why nesting is useful

- each panel solves a different layout problem
- the UI becomes more organized
- complex windows become easier to design

> Good WPF layout often comes from combining panels, not relying on only one.

---

# Performance and Best Practices

## 1. Prefer `Grid` for structured forms

A `Grid` usually gives better alignment and resizing behavior than a deeply nested group of `StackPanel` elements.

## 2. Avoid unnecessary nesting

Too many nested panels can make XAML harder to read and maintain.

## 3. Do not overuse `Canvas`

`Canvas` is powerful, but fixed coordinates often break responsive layout behavior.

## 4. Use `Margin` consistently

Consistent spacing makes the UI look cleaner and more professional.

## 5. Let controls size naturally when possible

Avoid hardcoding sizes unless necessary.

## 6. Use star sizing for flexible windows

`*` sizing helps layouts adapt better to available space.

## 7. Pick the simplest panel that solves the problem

A more advanced panel is not always better.

---

# Code Examples

## Example 1: Simple login form with `Grid`

```xml
<Grid Margin="18">
    <Grid.RowDefinitions>
        <RowDefinition Height="Auto"/>
        <RowDefinition Height="Auto"/>
        <RowDefinition Height="Auto"/>
    </Grid.RowDefinitions>

    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="Auto"/>
        <ColumnDefinition Width="*"/>
    </Grid.ColumnDefinitions>

    <TextBlock Grid.Row="0"
               Grid.Column="0"
               Margin="6"
               VerticalAlignment="Center"
               Text="User ID:"/>

    <TextBox Grid.Row="0"
             Grid.Column="1"
             Margin="6"
             MinWidth="190"/>

    <TextBlock Grid.Row="1"
               Grid.Column="0"
               Margin="6"
               VerticalAlignment="Center"
               Text="Passcode:"/>

    <PasswordBox Grid.Row="1"
                 Grid.Column="1"
                 Margin="6"/>

    <Button Grid.Row="2"
            Grid.Column="1"
            Width="96"
            Margin="6"
            HorizontalAlignment="Right"
            Content="Sign In"/>
</Grid>
```

## Example 2: Toolbar with `StackPanel`

```xml
<StackPanel Orientation="Horizontal" Margin="10">
    <Button Margin="4" Padding="10,5" Content="Add"/>
    <Button Margin="4" Padding="10,5" Content="Edit"/>
    <Button Margin="4" Padding="10,5" Content="Remove"/>
    <Button Margin="4" Padding="10,5" Content="Refresh"/>
</StackPanel>
```

## Example 3: Responsive tag area with `WrapPanel`

```xml
<WrapPanel Margin="12">
    <Border Margin="4" Padding="8,4" Background="#E8F1FF">
        <TextBlock Text="Desktop"/>
    </Border>
    <Border Margin="4" Padding="8,4" Background="#E8F1FF">
        <TextBlock Text="Mobile"/>
    </Border>
    <Border Margin="4" Padding="8,4" Background="#E8F1FF">
        <TextBlock Text="Cloud"/>
    </Border>
    <Border Margin="4" Padding="8,4" Background="#E8F1FF">
        <TextBlock Text="Security"/>
    </Border>
</WrapPanel>
```

## Example 4: Sidebar layout with `DockPanel`

```xml
<DockPanel>
    <Border DockPanel.Dock="Top" Background="#DDEBF7" Padding="10">
        <TextBlock FontSize="18" Text="Project Workspace"/>
    </Border>

    <StackPanel DockPanel.Dock="Left" Width="150" Background="#F8F4E8">
        <Button Margin="8" Content="Home"/>
        <Button Margin="8" Content="Tasks"/>
        <Button Margin="8" Content="Files"/>
    </StackPanel>

    <Border Padding="12" Background="White">
        <TextBlock Text="Editor content appears here."/>
    </Border>
</DockPanel>
```

## Example 5: Free positioning with `Canvas`

```xml
<Canvas Width="360" Height="220" Background="#F3FAFF">
    <Rectangle Canvas.Left="24"
               Canvas.Top="28"
               Width="110"
               Height="60"
               Fill="#7FB3FF"/>

    <Ellipse Canvas.Left="180"
             Canvas.Top="34"
             Width="85"
             Height="85"
             Fill="#FFB38A"/>

    <TextBlock Canvas.Left="118"
               Canvas.Top="150"
               FontSize="17"
               Text="Layout Sandbox"/>
</Canvas>
```

## Example 6: Equal cells with `UniformGrid`

```xml
<UniformGrid Rows="3" Columns="3" Margin="10">
    <Button Margin="4" Content="1"/>
    <Button Margin="4" Content="2"/>
    <Button Margin="4" Content="3"/>
    <Button Margin="4" Content="4"/>
    <Button Margin="4" Content="5"/>
    <Button Margin="4" Content="6"/>
    <Button Margin="4" Content="7"/>
    <Button Margin="4" Content="8"/>
    <Button Margin="4" Content="9"/>
</UniformGrid>
```