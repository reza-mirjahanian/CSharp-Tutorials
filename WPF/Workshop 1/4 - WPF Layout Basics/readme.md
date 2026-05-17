# WPF Layout Basics

## 1. What Is Layout in WPF?

In **WPF**, layout is the system that decides:

- **Where** controls appear
- **How large** controls should be
- **How controls react** when the window is resized
- **How parent containers arrange their child elements**

WPF layout is based on a **container-child model**.

> A layout container holds child controls and decides how those controls are measured and arranged.

Example:

```xml
<Window x:Class="LayoutDemo.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Layout Practice"
        Width="520"
        Height="320">

    <Grid>
        <Button Content="Save Profile" />
    </Grid>

</Window>
```

Here:

| Element | Role |
|---|---|
| `Window` | Main application window |
| `Grid` | Layout container |
| `Button` | Child control |

---

# 2. The WPF Layout Process

WPF layout usually happens in two main steps:

## 2.1 Measure Pass

During the **measure pass**, each control asks:

> “How much space do I need?”

For example, a `Button` calculates its desired size based on:

- Its text
- Font size
- Padding
- Margin
- Available space from the parent

## 2.2 Arrange Pass

During the **arrange pass**, the parent container decides:

> “Where should each child go, and how much space should it actually get?”

The parent may give the child:

- Exactly the size it requested
- More space than requested
- Less space than requested

---

# 3. Common WPF Layout Containers

WPF provides several layout panels.

| Panel | Purpose |
|---|---|
| `Grid` | Arranges controls in rows and columns |
| `StackPanel` | Stacks controls vertically or horizontally |
| `DockPanel` | Docks controls to edges |
| `WrapPanel` | Wraps controls to a new line when space runs out |
| `Canvas` | Uses fixed coordinates |
| `UniformGrid` | Places controls in equal-sized cells |

---

# 4. `Grid`

## 4.1 What Is `Grid`?

`Grid` is one of the most commonly used WPF layout containers.

It arranges controls using:

- **Rows**
- **Columns**
- **Cells**

A `Grid` is useful for building structured interfaces such as forms, dashboards, and settings pages.

---

## 4.2 Basic `Grid` Example

```xml
<Grid>
    <Button Content="Create Account" />
</Grid>
```

By default, a `Grid` has:

- One row
- One column

So the button is placed in the only available cell.

---

## 4.3 Defining Rows and Columns

Use:

- `Grid.RowDefinitions`
- `Grid.ColumnDefinitions`

Example:

```xml
<Grid>
    <Grid.RowDefinitions>
        <RowDefinition Height="Auto" />
        <RowDefinition Height="*" />
        <RowDefinition Height="60" />
    </Grid.RowDefinitions>

    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="160" />
        <ColumnDefinition Width="*" />
    </Grid.ColumnDefinitions>

    <TextBlock Grid.Row="0"
               Grid.Column="0"
               Text="Username:"
               Margin="8" />

    <TextBox Grid.Row="0"
             Grid.Column="1"
             Margin="8" />

    <ListBox Grid.Row="1"
             Grid.Column="0"
             Grid.ColumnSpan="2"
             Margin="8" />

    <Button Grid.Row="2"
            Grid.Column="1"
            Content="Continue"
            Width="120"
            Height="34"
            HorizontalAlignment="Right"
            Margin="8" />
</Grid>
```

---

## 4.4 Row and Column Sizing

WPF supports three common sizing modes.

| Size Type | Example | Meaning |
|---|---|---|
| Fixed | `Height="80"` | Uses exactly 80 device-independent pixels |
| Auto | `Height="Auto"` | Sizes to fit content |
| Star | `Height="*"` | Takes remaining available space |

---

## 4.5 Fixed Size

```xml
<RowDefinition Height="70" />
<ColumnDefinition Width="180" />
```

This creates:

- A row that is exactly `70` units high
- A column that is exactly `180` units wide

Use fixed sizes carefully because they may not resize well on different screens.

---

## 4.6 Auto Size

```xml
<RowDefinition Height="Auto" />
<ColumnDefinition Width="Auto" />
```

`Auto` means the row or column becomes just large enough to fit its content.

Example:

```xml
<Grid>
    <Grid.RowDefinitions>
        <RowDefinition Height="Auto" />
        <RowDefinition Height="*" />
    </Grid.RowDefinitions>

    <TextBlock Grid.Row="0"
               Text="Application Settings"
               FontSize="22"
               FontWeight="Bold"
               Margin="12" />

    <TextBox Grid.Row="1"
             Text="Settings content goes here..."
             Margin="12" />
</Grid>
```

The first row takes only as much height as the `TextBlock` needs.

---

## 4.7 Star Size

```xml
<RowDefinition Height="*" />
<ColumnDefinition Width="*" />
```

`*` means “take the remaining available space.”

Example:

```xml
<Grid>
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="*" />
        <ColumnDefinition Width="2*" />
    </Grid.ColumnDefinitions>

    <Border Grid.Column="0"
            Background="LightSteelBlue"
            Margin="6" />

    <Border Grid.Column="1"
            Background="LightGoldenrodYellow"
            Margin="6" />
</Grid>
```

Here:

| Column | Width |
|---|---|
| First column | 1 share |
| Second column | 2 shares |

So the second column is twice as wide as the first.

---

# 5. Placing Controls in a `Grid`

## 5.1 `Grid.Row`

Use `Grid.Row` to choose the row.

```xml
<TextBlock Grid.Row="1"
           Text="This is in row 1" />
```

> Row numbers start from `0`.

---

## 5.2 `Grid.Column`

Use `Grid.Column` to choose the column.

```xml
<TextBlock Grid.Column="2"
           Text="This is in column 2" />
```

> Column numbers start from `0`.

---

## 5.3 `Grid.RowSpan`

Use `Grid.RowSpan` when a control should cover multiple rows.

```xml
<Button Grid.Row="0"
        Grid.RowSpan="2"
        Content="Tall Button" />
```

This button spans two rows.

---

## 5.4 `Grid.ColumnSpan`

Use `Grid.ColumnSpan` when a control should cover multiple columns.

```xml
<TextBox Grid.Column="0"
         Grid.ColumnSpan="3"
         Text="This text box spans three columns" />
```

---

# 6. `StackPanel`

## 6.1 What Is `StackPanel`?

A `StackPanel` arranges its children in a single line:

- Vertically
- Horizontally

It is useful for simple lists of controls.

---

## 6.2 Vertical `StackPanel`

By default, `StackPanel` uses vertical orientation.

```xml
<StackPanel Margin="16">
    <TextBlock Text="Profile Details"
               FontSize="20"
               FontWeight="Bold"
               Margin="0,0,0,12" />

    <TextBox Text="Name"
             Margin="0,0,0,8" />

    <TextBox Text="Email"
             Margin="0,0,0,8" />

    <Button Content="Update"
            Width="100" />
</StackPanel>
```

The controls appear from top to bottom.

---

## 6.3 Horizontal `StackPanel`

```xml
<StackPanel Orientation="Horizontal"
            Margin="16">
    <Button Content="Back"
            Width="90"
            Margin="0,0,8,0" />

    <Button Content="Next"
            Width="90"
            Margin="0,0,8,0" />

    <Button Content="Finish"
            Width="90" />
</StackPanel>
```

The controls appear from left to right.

---

## 6.4 Important `StackPanel` Behavior

A `StackPanel` gives its children unlimited space in the direction it stacks.

For example:

```xml
<StackPanel Orientation="Vertical">
    <TextBox Height="40" />
    <ListBox />
</StackPanel>
```

The `ListBox` may not resize as expected because the vertical `StackPanel` allows unlimited vertical space.

For resizable layouts, prefer `Grid`.

---

# 7. `DockPanel`

## 7.1 What Is `DockPanel`?

A `DockPanel` docks child elements to the edges of the container.

Available dock positions:

- `Top`
- `Bottom`
- `Left`
- `Right`

The final child can fill the remaining space.

---

## 7.2 Basic `DockPanel` Example

```xml
<DockPanel>
    <Border DockPanel.Dock="Top"
            Height="50"
            Background="LightBlue">
        <TextBlock Text="Header Area"
                   VerticalAlignment="Center"
                   Margin="12" />
    </Border>

    <Border DockPanel.Dock="Bottom"
            Height="40"
            Background="LightGray">
        <TextBlock Text="Status: Ready"
                   VerticalAlignment="Center"
                   Margin="12" />
    </Border>

    <Border DockPanel.Dock="Left"
            Width="140"
            Background="Honeydew">
        <TextBlock Text="Navigation"
                   Margin="12" />
    </Border>

    <Grid Background="WhiteSmoke">
        <TextBlock Text="Main Content"
                   HorizontalAlignment="Center"
                   VerticalAlignment="Center" />
    </Grid>
</DockPanel>
```

The last child fills the remaining space by default.

---

## 7.3 `LastChildFill`

`DockPanel.LastChildFill` controls whether the last child fills the remaining space.

```xml
<DockPanel LastChildFill="False">
    <Button DockPanel.Dock="Top"
            Content="Top Button" />

    <Button DockPanel.Dock="Bottom"
            Content="Bottom Button" />

    <Button Content="Normal Button" />
</DockPanel>
```

If `LastChildFill="False"`, the last child does not automatically fill the remaining space.

---

# 8. `WrapPanel`

## 8.1 What Is `WrapPanel`?

A `WrapPanel` arranges controls in a row or column and wraps them when there is not enough space.

It is useful for:

- Toolbars
- Tags
- Thumbnails
- Icon lists
- Dynamic button groups

---

## 8.2 Basic `WrapPanel` Example

```xml
<WrapPanel Margin="12">
    <Button Content="Tag One"
            Width="90"
            Margin="4" />

    <Button Content="Tag Two"
            Width="90"
            Margin="4" />

    <Button Content="Tag Three"
            Width="90"
            Margin="4" />

    <Button Content="Tag Four"
            Width="90"
            Margin="4" />

    <Button Content="Tag Five"
            Width="90"
            Margin="4" />
</WrapPanel>
```

When the window becomes too narrow, buttons move to the next line.

---

## 8.3 Horizontal and Vertical Wrapping

Default orientation is horizontal:

```xml
<WrapPanel Orientation="Horizontal">
    <Button Content="Alpha" />
    <Button Content="Beta" />
    <Button Content="Gamma" />
</WrapPanel>
```

Vertical wrapping:

```xml
<WrapPanel Orientation="Vertical">
    <Button Content="North" />
    <Button Content="East" />
    <Button Content="South" />
</WrapPanel>
```

---

# 9. `Canvas`

## 9.1 What Is `Canvas`?

A `Canvas` places controls using fixed coordinates.

It uses attached properties such as:

- `Canvas.Left`
- `Canvas.Top`
- `Canvas.Right`
- `Canvas.Bottom`

---

## 9.2 Basic `Canvas` Example

```xml
<Canvas Background="WhiteSmoke">
    <Button Content="Move"
            Canvas.Left="40"
            Canvas.Top="30"
            Width="100"
            Height="35" />

    <TextBlock Text="Positioned Text"
               Canvas.Left="180"
               Canvas.Top="40"
               FontSize="16" />
</Canvas>
```

The button is placed:

- `40` units from the left
- `30` units from the top

---

## 9.3 When to Use `Canvas`

Use `Canvas` for:

- Drawing tools
- Diagrams
- Games
- Custom visual editors
- Absolute positioning scenarios

Avoid using `Canvas` for normal application layout because it does not resize naturally.

---

# 10. `UniformGrid`

## 10.1 What Is `UniformGrid`?

A `UniformGrid` arranges children in equal-sized cells.

Unlike `Grid`, you usually do not define each row and column separately.

---

## 10.2 Basic `UniformGrid` Example

```xml
<UniformGrid Rows="2"
             Columns="3"
             Margin="12">
    <Button Content="One" />
    <Button Content="Two" />
    <Button Content="Three" />
    <Button Content="Four" />
    <Button Content="Five" />
    <Button Content="Six" />
</UniformGrid>
```

This creates a layout like this:

| Column 1 | Column 2 | Column 3 |
|---|---|---|
| One | Two | Three |
| Four | Five | Six |

---

# 11. Alignment

## 11.1 Horizontal Alignment

`HorizontalAlignment` controls how an element is positioned horizontally inside its parent.

Common values:

| Value | Meaning |
|---|---|
| `Left` | Aligns to the left |
| `Center` | Aligns to the center |
| `Right` | Aligns to the right |
| `Stretch` | Fills available width |

Example:

```xml
<Button Content="Submit"
        Width="120"
        HorizontalAlignment="Right" />
```

---

## 11.2 Vertical Alignment

`VerticalAlignment` controls how an element is positioned vertically inside its parent.

Common values:

| Value | Meaning |
|---|---|
| `Top` | Aligns to the top |
| `Center` | Aligns to the center |
| `Bottom` | Aligns to the bottom |
| `Stretch` | Fills available height |

Example:

```xml
<Button Content="Start"
        Height="40"
        VerticalAlignment="Center" />
```

---

## 11.3 Stretch Behavior

By default, many controls use:

```xml
HorizontalAlignment="Stretch"
VerticalAlignment="Stretch"
```

However, if you set an explicit `Width` or `Height`, stretching may no longer have the same effect.

Example:

```xml
<Button Content="Wide Button"
        HorizontalAlignment="Stretch"
        Margin="12" />
```

This button stretches horizontally.

But this one does not fully stretch because it has a fixed width:

```xml
<Button Content="Fixed Button"
        Width="140"
        HorizontalAlignment="Stretch"
        Margin="12" />
```

---

# 12. Margin

## 12.1 What Is `Margin`?

`Margin` creates space **outside** an element.

Example:

```xml
<Button Content="Save"
        Margin="12" />
```

This adds `12` units of space around all sides of the button.

---

## 12.2 Margin Syntax

| Syntax | Meaning |
|---|---|
| `Margin="10"` | 10 on all sides |
| `Margin="10,20"` | 10 left/right, 20 top/bottom |
| `Margin="10,20,30,40"` | left 10, top 20, right 30, bottom 40 |

Example:

```xml
<TextBlock Text="Account Name"
           Margin="8,12,8,4" />
```

This means:

| Side | Value |
|---|---|
| Left | `8` |
| Top | `12` |
| Right | `8` |
| Bottom | `4` |

---

# 13. Padding

## 13.1 What Is `Padding`?

`Padding` creates space **inside** a control, between the control’s border and its content.

Example:

```xml
<Button Content="Login"
        Padding="16,8" />
```

The button text has:

- `16` units of horizontal inner space
- `8` units of vertical inner space

---

## 13.2 Margin vs Padding

| Feature | `Margin` | `Padding` |
|---|---|---|
| Space location | Outside the control | Inside the control |
| Affects distance from other controls | Yes | No |
| Affects distance from content | No | Yes |
| Common use | Separating controls | Making content more comfortable |

Example:

```xml
<StackPanel Margin="20">
    <Button Content="Small Padding"
            Padding="4"
            Margin="0,0,0,10" />

    <Button Content="Large Padding"
            Padding="18"
            Margin="0,0,0,10" />
</StackPanel>
```

---

# 14. Width and Height

## 14.1 Explicit Size

You can set fixed sizes with:

- `Width`
- `Height`

Example:

```xml
<Button Content="Upload"
        Width="130"
        Height="38" />
```

---

## 14.2 Minimum and Maximum Size

Use:

- `MinWidth`
- `MinHeight`
- `MaxWidth`
- `MaxHeight`

Example:

```xml
<TextBox MinWidth="180"
         MaxWidth="420"
         Height="32" />
```

This `TextBox` can resize horizontally, but it will not become smaller than `180` or larger than `420`.

---

## 14.3 Prefer Flexible Layout

Instead of fixed size:

```xml
<Button Content="Confirm"
        Width="140" />
```

Prefer flexible sizing:

```xml
<Button Content="Confirm"
        MinWidth="100"
        Padding="14,6" />
```

Flexible layouts usually adapt better to:

- Different screen sizes
- Different text lengths
- Localization
- Accessibility scaling

---

# 15. Device-Independent Pixels

WPF uses **device-independent pixels**, often called **DIPs**.

> One WPF unit is usually treated as `1/96` of an inch.

This helps WPF applications scale across screens with different DPI settings.

Example:

```xml
<Button Width="96"
        Height="32"
        Content="DPI Friendly" />
```

A width of `96` means 96 WPF units, not necessarily 96 physical screen pixels.

---

# 16. Nested Layouts

## 16.1 What Is Nesting?

You can place layout containers inside other layout containers.

Example:

```xml
<Grid Margin="16">
    <Grid.RowDefinitions>
        <RowDefinition Height="Auto" />
        <RowDefinition Height="*" />
        <RowDefinition Height="Auto" />
    </Grid.RowDefinitions>

    <TextBlock Grid.Row="0"
               Text="Customer Form"
               FontSize="22"
               FontWeight="Bold"
               Margin="0,0,0,16" />

    <Grid Grid.Row="1">
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="140" />
            <ColumnDefinition Width="*" />
        </Grid.ColumnDefinitions>

        <Grid.RowDefinitions>
            <RowDefinition Height="Auto" />
            <RowDefinition Height="Auto" />
            <RowDefinition Height="Auto" />
        </Grid.RowDefinitions>

        <TextBlock Grid.Row="0"
                   Grid.Column="0"
                   Text="First Name:"
                   Margin="0,0,8,10" />

        <TextBox Grid.Row="0"
                 Grid.Column="1"
                 Margin="0,0,0,10" />

        <TextBlock Grid.Row="1"
                   Grid.Column="0"
                   Text="Last Name:"
                   Margin="0,0,8,10" />

        <TextBox Grid.Row="1"
                 Grid.Column="1"
                 Margin="0,0,0,10" />

        <TextBlock Grid.Row="2"
                   Grid.Column="0"
                   Text="Phone:"
                   Margin="0,0,8,10" />

        <TextBox Grid.Row="2"
                 Grid.Column="1"
                 Margin="0,0,0,10" />
    </Grid>

    <StackPanel Grid.Row="2"
                Orientation="Horizontal"
                HorizontalAlignment="Right"
                Margin="0,16,0,0">
        <Button Content="Cancel"
                Width="90"
                Margin="0,0,8,0" />

        <Button Content="Save"
                Width="90" />
    </StackPanel>
</Grid>
```

This layout uses:

| Container | Purpose |
|---|---|
| Outer `Grid` | Creates header, content, and footer areas |
| Inner `Grid` | Aligns labels and text boxes |
| `StackPanel` | Places buttons side by side |

---

# 17. Attached Properties

## 17.1 What Are Attached Properties?

Attached properties are properties defined by a parent layout container but used on child elements.

Example:

```xml
<TextBox Grid.Row="1"
         Grid.Column="2" />
```

`Grid.Row` and `Grid.Column` are attached properties.

They belong to `Grid`, but they are written on the `TextBox`.

---

## 17.2 Common Attached Properties

| Container | Attached Properties |
|---|---|
| `Grid` | `Grid.Row`, `Grid.Column`, `Grid.RowSpan`, `Grid.ColumnSpan` |
| `DockPanel` | `DockPanel.Dock` |
| `Canvas` | `Canvas.Left`, `Canvas.Top`, `Canvas.Right`, `Canvas.Bottom` |

Examples:

```xml
<Button Grid.Row="2"
        Grid.Column="1"
        Content="Apply" />
```

```xml
<TextBlock DockPanel.Dock="Top"
           Text="Toolbar" />
```

```xml
<Ellipse Canvas.Left="60"
         Canvas.Top="40"
         Width="80"
         Height="80"
         Fill="CornflowerBlue" />
```

---

# 18. Layout Properties Cheat Sheet

| Property | Applies To | Purpose |
|---|---|---|
| `Margin` | Most elements | Space outside an element |
| `Padding` | Many controls | Space inside an element |
| `Width` | Most elements | Fixed width |
| `Height` | Most elements | Fixed height |
| `MinWidth` | Most elements | Minimum width |
| `MinHeight` | Most elements | Minimum height |
| `MaxWidth` | Most elements | Maximum width |
| `MaxHeight` | Most elements | Maximum height |
| `HorizontalAlignment` | Most elements | Horizontal placement |
| `VerticalAlignment` | Most elements | Vertical placement |

---

# 19. Choosing the Right Layout Panel

## 19.1 Quick Decision Table

| Goal | Recommended Panel |
|---|---|
| Form with labels and inputs | `Grid` |
| Header, sidebar, footer, main area | `DockPanel` or `Grid` |
| Simple vertical list of controls | `StackPanel` |
| Toolbar that wraps | `WrapPanel` |
| Fixed drawing surface | `Canvas` |
| Equal-size buttons or tiles | `UniformGrid` |

---

## 19.2 Practical Recommendations

### ✅ Use `Grid` when:

- You need rows and columns
- Controls should resize cleanly
- You are building forms
- You need flexible layouts

```xml
<Grid>
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="Auto" />
        <ColumnDefinition Width="*" />
    </Grid.ColumnDefinitions>

    <TextBlock Grid.Column="0"
               Text="Search:"
               VerticalAlignment="Center"
               Margin="8" />

    <TextBox Grid.Column="1"
             Margin="8" />
</Grid>
```

---

### ✅ Use `StackPanel` when:

- You need a simple one-direction layout
- Controls should appear one after another
- The layout is small and predictable

```xml
<StackPanel>
    <TextBlock Text="Options" />
    <CheckBox Content="Enable notifications" />
    <CheckBox Content="Use compact mode" />
</StackPanel>
```

---

### ✅ Use `DockPanel` when:

- You need edge-based layout
- You want top/bottom/left/right regions
- You are building a classic application shell

```xml
<DockPanel>
    <TextBlock DockPanel.Dock="Top"
               Text="Menu Area"
               Background="Gainsboro"
               Padding="8" />

    <TextBlock DockPanel.Dock="Bottom"
               Text="Ready"
               Background="Gainsboro"
               Padding="8" />

    <TextBox Text="Main editor area" />
</DockPanel>
```

---

### ✅ Use `WrapPanel` when:

- Items should automatically move to the next line
- The number of items is dynamic
- You are displaying tags, chips, or thumbnails

```xml
<WrapPanel>
    <Button Content="Red" Margin="4" />
    <Button Content="Green" Margin="4" />
    <Button Content="Blue" Margin="4" />
    <Button Content="Yellow" Margin="4" />
</WrapPanel>
```

---

### ✅ Use `Canvas` when:

- You need exact positions
- You are drawing shapes
- You are building a diagram editor

```xml
<Canvas>
    <Rectangle Canvas.Left="30"
               Canvas.Top="40"
               Width="120"
               Height="70"
               Fill="LightSalmon" />

    <Ellipse Canvas.Left="190"
             Canvas.Top="50"
             Width="80"
             Height="80"
             Fill="MediumSeaGreen" />
</Canvas>
```

---

# 20. Building a Complete Responsive Layout

## 20.1 Example: Simple Settings Window

```xml
<Window x:Class="LayoutBasicsDemo.SettingsWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Settings"
        Width="640"
        Height="420"
        MinWidth="480"
        MinHeight="320">

    <Grid Margin="18">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto" />
            <RowDefinition Height="*" />
            <RowDefinition Height="Auto" />
        </Grid.RowDefinitions>

        <TextBlock Grid.Row="0"
                   Text="Application Settings"
                   FontSize="24"
                   FontWeight="Bold"
                   Margin="0,0,0,18" />

        <Grid Grid.Row="1">
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="170" />
                <ColumnDefinition Width="*" />
            </Grid.ColumnDefinitions>

            <Grid.RowDefinitions>
                <RowDefinition Height="Auto" />
                <RowDefinition Height="Auto" />
                <RowDefinition Height="Auto" />
                <RowDefinition Height="*" />
            </Grid.RowDefinitions>

            <TextBlock Grid.Row="0"
                       Grid.Column="0"
                       Text="Display Name:"
                       VerticalAlignment="Center"
                       Margin="0,0,12,10" />

            <TextBox Grid.Row="0"
                     Grid.Column="1"
                     Margin="0,0,0,10"
                     MinWidth="220" />

            <TextBlock Grid.Row="1"
                       Grid.Column="0"
                       Text="Theme:"
                       VerticalAlignment="Center"
                       Margin="0,0,12,10" />

            <ComboBox Grid.Row="1"
                      Grid.Column="1"
                      Margin="0,0,0,10"
                      SelectedIndex="0">
                <ComboBoxItem Content="Light" />
                <ComboBoxItem Content="Dark" />
                <ComboBoxItem Content="System Default" />
            </ComboBox>

            <TextBlock Grid.Row="2"
                       Grid.Column="0"
                       Text="Startup:"
                       VerticalAlignment="Center"
                       Margin="0,0,12,10" />

            <CheckBox Grid.Row="2"
                      Grid.Column="1"
                      Content="Open dashboard after login"
                      Margin="0,0,0,10" />

            <GroupBox Grid.Row="3"
                      Grid.Column="0"
                      Grid.ColumnSpan="2"
                      Header="Notes"
                      Margin="0,8,0,0">
                <TextBox AcceptsReturn="True"
                         TextWrapping="Wrap"
                         VerticalScrollBarVisibility="Auto"
                         Margin="8" />
            </GroupBox>
        </Grid>

        <StackPanel Grid.Row="2"
                    Orientation="Horizontal"
                    HorizontalAlignment="Right"
                    Margin="0,18,0,0">
            <Button Content="Cancel"
                    MinWidth="90"
                    Padding="12,6"
                    Margin="0,0,8,0" />

            <Button Content="Apply"
                    MinWidth="90"
                    Padding="12,6" />
        </StackPanel>
    </Grid>

</Window>
```

---

## 20.2 Layout Breakdown

| Area | Layout Technique |
|---|---|
| Main window structure | Outer `Grid` with three rows |
| Title area | `Auto` row |
| Form area | Inner `Grid` |
| Notes area | Star-sized row |
| Buttons area | Horizontal `StackPanel` |
| Resizing behavior | `*`, `MinWidth`, `MinHeight` |

---

# 21. Common Layout Mistakes

## 21.1 Using `Canvas` for Everything

❌ Poor choice for normal forms:

```xml
<Canvas>
    <TextBlock Canvas.Left="20"
               Canvas.Top="20"
               Text="Email:" />

    <TextBox Canvas.Left="100"
             Canvas.Top="18"
             Width="200" />
</Canvas>
```

This layout does not adapt well to resizing.

✅ Better:

```xml
<Grid Margin="16">
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="Auto" />
        <ColumnDefinition Width="*" />
    </Grid.ColumnDefinitions>

    <TextBlock Grid.Column="0"
               Text="Email:"
               VerticalAlignment="Center"
               Margin="0,0,10,0" />

    <TextBox Grid.Column="1" />
</Grid>
```

---

## 21.2 Overusing Fixed Width and Height

❌ Less flexible:

```xml
<Button Content="Download Report"
        Width="120"
        Height="30" />
```

Text may be clipped if the content changes.

✅ More flexible:

```xml
<Button Content="Download Report"
        MinWidth="120"
        Padding="12,6" />
```

---

## 21.3 Forgetting Margins

❌ Cramped layout:

```xml
<StackPanel>
    <TextBlock Text="Full Name" />
    <TextBox />
    <Button Content="Save" />
</StackPanel>
```

✅ More readable layout:

```xml
<StackPanel Margin="18">
    <TextBlock Text="Full Name"
               Margin="0,0,0,6" />

    <TextBox Margin="0,0,0,12" />

    <Button Content="Save"
            HorizontalAlignment="Right"
            Padding="14,6" />
</StackPanel>
```

---

## 21.4 Using `StackPanel` When You Need Resizing

❌ Problematic for large resizable areas:

```xml
<StackPanel>
    <TextBlock Text="Messages" />
    <ListBox />
</StackPanel>
```

✅ Better with `Grid`:

```xml
<Grid>
    <Grid.RowDefinitions>
        <RowDefinition Height="Auto" />
        <RowDefinition Height="*" />
    </Grid.RowDefinitions>

    <TextBlock Grid.Row="0"
               Text="Messages"
               Margin="8" />

    <ListBox Grid.Row="1"
             Margin="8" />
</Grid>
```

---

# 22. Practical Exercise

## 22.1 Goal

Create a WPF layout with:

- A title at the top
- A two-column form
- A large notes area
- Buttons at the bottom right

---

## 22.2 Starter XAML

```xml
<Window x:Class="PracticeLayout.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Contact Editor"
        Width="600"
        Height="430">

    <Grid Margin="20">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto" />
            <RowDefinition Height="Auto" />
            <RowDefinition Height="*" />
            <RowDefinition Height="Auto" />
        </Grid.RowDefinitions>

        <TextBlock Grid.Row="0"
                   Text="Edit Contact"
                   FontSize="24"
                   FontWeight="Bold"
                   Margin="0,0,0,18" />

        <Grid Grid.Row="1">
            <Grid.ColumnDefinitions>
                <ColumnDefinition Width="120" />
                <ColumnDefinition Width="*" />
            </Grid.ColumnDefinitions>

            <Grid.RowDefinitions>
                <RowDefinition Height="Auto" />
                <RowDefinition Height="Auto" />
            </Grid.RowDefinitions>

            <TextBlock Grid.Row="0"
                       Grid.Column="0"
                       Text="Name:"
                       VerticalAlignment="Center"
                       Margin="0,0,10,10" />

            <TextBox Grid.Row="0"
                     Grid.Column="1"
                     Margin="0,0,0,10" />

            <TextBlock Grid.Row="1"
                       Grid.Column="0"
                       Text="Email:"
                       VerticalAlignment="Center"
                       Margin="0,0,10,10" />

            <TextBox Grid.Row="1"
                     Grid.Column="1"
                     Margin="0,0,0,10" />
        </Grid>

        <GroupBox Grid.Row="2"
                  Header="Notes"
                  Margin="0,10,0,0">
            <TextBox AcceptsReturn="True"
                     TextWrapping="Wrap"
                     VerticalScrollBarVisibility="Auto"
                     Margin="8" />
        </GroupBox>

        <StackPanel Grid.Row="3"
                    Orientation="Horizontal"
                    HorizontalAlignment="Right"
                    Margin="0,18,0,0">
            <Button Content="Discard"
                    MinWidth="90"
                    Padding="12,6"
                    Margin="0,0,8,0" />

            <Button Content="Save"
                    MinWidth="90"
                    Padding="12,6" />
        </StackPanel>
    </Grid>

</Window>
```

---

# 23. Key Terms

| Term | Meaning |
|---|---|
| Layout container | A control that arranges child elements |
| Child element | A control placed inside a parent container |
| Measure pass | Step where elements calculate desired size |
| Arrange pass | Step where elements receive actual size and position |
| Attached property | A property owned by one class but set on another element |
| Star sizing | Flexible sizing using `*` |
| `Auto` sizing | Size based on content |
| Fixed sizing | Exact size such as `100` |
| Margin | Space outside a control |
| Padding | Space inside a control |