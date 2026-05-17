# XAML in WPF

## What XAML Is

**XAML** stands for **Extensible Application Markup Language**.  
In **WPF** (*Windows Presentation Foundation*), XAML is used to describe a user interface in a readable, declarative way.

Instead of building every control in C# manually, you can define windows, buttons, text boxes, layouts, and styles directly in markup.

> Think of XAML as the language for describing **what the UI looks like and how it is arranged**, while C# usually handles **behavior and logic**.

---

## Why WPF Uses XAML

XAML makes desktop UI development easier because it separates:

- **UI structure**
- **UI appearance**
- **application logic**

This separation gives several benefits:

- **Cleaner code**
- **Easier maintenance**
- **Better collaboration** between designers and developers
- **Reusable styles and templates**
- **Less repetitive C# code**

---

## XAML and Code-Behind

In a WPF application, a window or user control usually has two related files:

| File | Purpose |
|---|---|
| `MainWindow.xaml` | Describes the UI |
| `MainWindow.xaml.cs` | Contains C# event handlers and logic |

### Example Structure

```xaml
<Window x:Class="SampleApp.DashboardWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Dashboard"
        Height="280"
        Width="420">
    <Grid>
        <Button Content="Press Here"
                Width="110"
                Height="36"
                HorizontalAlignment="Center"
                VerticalAlignment="Center"
                Click="HandlePress"/>
    </Grid>
</Window>
```

```csharp
using System.Windows;

namespace SampleApp
{
    public partial class DashboardWindow : Window
    {
        public DashboardWindow()
        {
            InitializeComponent();
        }

        private void HandlePress(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Welcome!");
        }
    }
}
```

### Key Idea

- **XAML** creates and configures controls
- **Code-behind** responds to events and performs logic

---

## Basic Syntax of XAML

XAML is based on **XML**, so it uses:

- **elements**
- **attributes**
- **nested tags**

### Simple Example

```xaml
<Button Content="Save" Width="120" Height="34"/>
```

This creates a button and sets its properties.

### Nested Example

```xaml
<StackPanel>
    <TextBlock Text="Username"/>
    <TextBox Width="220"/>
</StackPanel>
```

Here:

- `StackPanel` is a container
- `TextBlock` displays text
- `TextBox` allows user input

---

## Objects and Properties in XAML

Each XAML element usually represents a **.NET object**.

For example:

```xaml
<TextBox Width="180" />
```

This is similar in idea to:

```csharp
var input = new TextBox();
input.Width = 180;
```

### Attribute Syntax

Properties can be set as attributes:

```xaml
<Label Content="Email" FontSize="16"/>
```

### Property Element Syntax

Some properties are better written using nested elements:

```xaml
<Button>
    <Button.Content>
        Send
    </Button.Content>
</Button>
```

This is functionally similar to:

```xaml
<Button Content="Send"/>
```

> **Property element syntax** is especially useful for complex content like layouts, templates, brushes, or collections.

---

## XAML Namespaces

Most WPF XAML files include at least two namespaces:

```xaml
<Window
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
</Window>
```

### What They Mean

| Namespace | Role |
|---|---|
| `xmlns="..."` | Default WPF controls and classes |
| `xmlns:x="..."` | Special XAML features such as `x:Name` |

### Common `x:` Members

- `x:Name`
- `x:Class`
- `x:Key`

Example:

```xaml
<TextBox x:Name="SearchBox" Width="200"/>
```

This gives the control a name so it can be accessed in code.

---

## Common WPF Elements in XAML

### Controls

These are visible UI elements:

- `Button`
- `TextBox`
- `TextBlock`
- `Label`
- `CheckBox`
- `ComboBox`
- `ListBox`

Example:

```xaml
<StackPanel Margin="16">
    <TextBlock Text="City" Margin="0,0,0,6"/>
    <TextBox Width="220"/>
    <CheckBox Content="Set as default" Margin="0,10,0,0"/>
</StackPanel>
```

### Containers and Layout Panels

These organize controls on the screen:

- `Grid`
- `StackPanel`
- `WrapPanel`
- `DockPanel`
- `Canvas`

> In WPF, layout is very important. You usually place controls inside layout panels instead of positioning everything manually.

---

## The `Grid` Layout

`Grid` is one of the most important WPF layout containers.

It arranges content in **rows** and **columns**.

### Example

```xaml
<Grid Margin="18">
    <Grid.RowDefinitions>
        <RowDefinition Height="Auto"/>
        <RowDefinition Height="Auto"/>
        <RowDefinition Height="*"/>
    </Grid.RowDefinitions>

    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="Auto"/>
        <ColumnDefinition Width="*"/>
    </Grid.ColumnDefinitions>

    <TextBlock Grid.Row="0"
               Grid.Column="0"
               Margin="0,0,10,10"
               VerticalAlignment="Center"
               Text="Full Name:"/>

    <TextBox Grid.Row="0"
             Grid.Column="1"
             Margin="0,0,0,10"/>

    <TextBlock Grid.Row="1"
               Grid.Column="0"
               Margin="0,0,10,10"
               VerticalAlignment="Top"
               Text="Notes:"/>

    <TextBox Grid.Row="1"
             Grid.Column="1"
             Height="70"
             AcceptsReturn="True"/>

    <Button Grid.Row="2"
            Grid.Column="1"
            Width="100"
            Height="34"
            Margin="0,12,0,0"
            HorizontalAlignment="Right"
            Content="Submit"/>
</Grid>
```

### Important Sizing Values

| Value | Meaning |
|---|---|
| `Auto` | Size based on content |
| `*` | Use remaining available space |
| `2*` | Take twice as much proportional space as `*` |

---

## The `StackPanel` Layout

`StackPanel` places child elements in a **vertical** or **horizontal** line.

### Vertical Example

```xaml
<StackPanel Margin="14">
    <TextBlock Text="Login" FontSize="20" Margin="0,0,0,12"/>
    <TextBox Margin="0,0,0,8"/>
    <PasswordBox Margin="0,0,0,8"/>
    <Button Content="Sign In" Width="90"/>
</StackPanel>
```

### Horizontal Example

```xaml
<StackPanel Orientation="Horizontal" Margin="12">
    <Button Content="Back" Margin="0,0,8,0"/>
    <Button Content="Next"/>
</StackPanel>
```

### When to Use It

Use `StackPanel` when:

- controls should flow in one direction
- you do not need row/column precision
- the layout is simple

---

## Setting Properties

WPF controls expose many properties.

### Common Property Examples

| Property | Example | Purpose |
|---|---|---|
| `Width` | `Width="160"` | Sets width |
| `Height` | `Height="40"` | Sets height |
| `Margin` | `Margin="8"` | Outer spacing |
| `Padding` | `Padding="6"` | Inner spacing |
| `Background` | `Background="LightBlue"` | Background color |
| `Foreground` | `Foreground="DarkSlateGray"` | Text color |
| `FontSize` | `FontSize="18"` | Text size |
| `HorizontalAlignment` | `HorizontalAlignment="Center"` | Horizontal positioning |
| `VerticalAlignment` | `VerticalAlignment="Top"` | Vertical positioning |

### Example

```xaml
<Button Content="Checkout"
        Width="140"
        Height="38"
        Margin="10"
        Padding="8"
        Background="SteelBlue"
        Foreground="White"
        FontSize="15"/>
```

---

## Content Controls and Container Controls

Some WPF controls can hold a **single piece of content**, while others can hold **multiple children**.

### Content Controls

Examples:

- `Button`
- `Label`
- `ContentControl`
- `Window`

```xaml
<Button Content="Refresh"/>
```

### Container Controls

Examples:

- `Grid`
- `StackPanel`
- `WrapPanel`

```xaml
<StackPanel>
    <TextBlock Text="Item A"/>
    <TextBlock Text="Item B"/>
</StackPanel>
```

---

## Attached Properties

An **attached property** is a property defined by one class but set on child elements.

This is very common in WPF layout.

### Example with `Grid.Row` and `Grid.Column`

```xaml
<Grid>
    <Grid.RowDefinitions>
        <RowDefinition/>
        <RowDefinition/>
    </Grid.RowDefinitions>

    <TextBlock Grid.Row="0" Text="Phone"/>
    <TextBox Grid.Row="1"/>
</Grid>
```

Here:

- `Grid` defines the attached properties
- child controls use them to tell the `Grid` where they belong

### Other Common Attached Properties

- `DockPanel.Dock`
- `Canvas.Left`
- `Canvas.Top`

---

## Events in XAML

You can attach event handlers directly in XAML.

### Example

```xaml
<Button Content="Calculate"
        Click="CalculateButton_Click"/>
```

Then in C#:

```csharp
private void CalculateButton_Click(object sender, RoutedEventArgs e)
{
    MessageBox.Show("Calculation complete.");
}
```

### Common Events

| Control Type | Common Events |
|---|---|
| `Button` | `Click` |
| `TextBox` | `TextChanged` |
| `Window` | `Loaded` |
| `ComboBox` | `SelectionChanged` |

---

## Naming Controls with `x:Name`

Use `x:Name` when you want to access a control in code.

```xaml
<TextBox x:Name="QuantityInput" Width="120"/>
<Button Content="Check" Click="Check_Click"/>
```

```csharp
private void Check_Click(object sender, RoutedEventArgs e)
{
    string value = QuantityInput.Text;
    MessageBox.Show(value);
}
```

---

## Resources in XAML

Resources let you define reusable objects such as:

- colors
- brushes
- styles
- templates
- strings

### Example

```xaml
<Window.Resources>
    <SolidColorBrush x:Key="PrimaryBrush" Color="#2E6FA3"/>
</Window.Resources>
```

Then use it:

```xaml
<Button Content="Open"
        Background="{StaticResource PrimaryBrush}"
        Foreground="White"/>
```

### Why Resources Matter

They help with:

- consistency
- reuse
- easier updates

> Change the resource once, and every control using it can update automatically depending on how it is referenced.

---

## `StaticResource` and `DynamicResource`

These are used to retrieve resources.

### `StaticResource`

- resolved once when the UI loads
- faster in many cases
- commonly used

```xaml
<TextBlock Foreground="{StaticResource PrimaryBrush}"
           Text="Inventory"/>
```

### `DynamicResource`

- resolved at runtime
- useful when resources may change after loading

```xaml
<TextBlock Foreground="{DynamicResource PrimaryBrush}"
           Text="Inventory"/>
```

### Comparison Table

| Feature | `StaticResource` | `DynamicResource` |
|---|---|---|
| Resolution time | Load time | Runtime |
| Performance | Usually faster | Usually slightly slower |
| Best for | Fixed resources | Theme/runtime changes |

---

## Data Binding

**Data binding** connects UI elements to data.

This is one of the most powerful features of WPF.

### Example

```xaml
<TextBox Text="{Binding CustomerName}" Width="220"/>
```

This means the `TextBox` is connected to a property named `CustomerName`.

### Why Binding Is Useful

- reduces manual UI update code
- keeps UI and data synchronized
- supports MVVM very well

### Binding Example with a Slider

```xaml
<StackPanel Margin="12">
    <Slider x:Name="SizeSlider" Minimum="8" Maximum="28" Value="14"/>
    <TextBlock Text="{Binding ElementName=SizeSlider, Path=Value}"
               FontSize="16"/>
</StackPanel>
```

Here, the `TextBlock` displays the slider’s current value.

---

## Binding Modes

Bindings can work in different directions.

| Mode | Meaning |
|---|---|
| `OneWay` | Data goes from source to UI |
| `TwoWay` | Source and UI update each other |
| `OneTime` | Data is copied once |

### Example

```xaml
<TextBox Text="{Binding EmailAddress, Mode=TwoWay}" Width="240"/>
```

For editable fields, `TwoWay` is often used.

---

## The Data Context

Bindings usually look for data in the current **DataContext**.

```xaml
<TextBlock Text="{Binding ProductTitle}"/>
```

This works if the current `DataContext` contains a property named `ProductTitle`.

### Typical Idea

- set a data object as the `DataContext`
- child elements inherit it
- bindings use it automatically

---

## Commands

In WPF, commands are often preferred over directly handling button clicks, especially in patterns like **MVVM**.

### Example

```xaml
<Button Content="Remove"
        Command="{Binding RemoveCommand}"/>
```

Instead of calling an event handler in code-behind, the button triggers a command from the bound object.

### Why Commands Are Useful

- cleaner separation of UI and logic
- easier testing
- fits MVVM architecture

---

## Styles

A **Style** lets you define reusable property settings for controls.

### Example

```xaml
<Window.Resources>
    <Style x:Key="PrimaryButtonStyle" TargetType="Button">
        <Setter Property="Width" Value="130"/>
        <Setter Property="Height" Value="36"/>
        <Setter Property="Margin" Value="6"/>
        <Setter Property="Background" Value="Teal"/>
        <Setter Property="Foreground" Value="White"/>
    </Style>
</Window.Resources>
```

Use it like this:

```xaml
<Button Content="Export" Style="{StaticResource PrimaryButtonStyle}"/>
<Button Content="Import" Style="{StaticResource PrimaryButtonStyle}"/>
```

### Benefits of Styles

- consistent appearance
- less repeated markup
- easier design updates

---

## Implicit Styles

If a style has no `x:Key`, it can apply automatically to all controls of a type.

```xaml
<Window.Resources>
    <Style TargetType="TextBox">
        <Setter Property="Margin" Value="5"/>
        <Setter Property="Width" Value="210"/>
    </Style>
</Window.Resources>
```

Now every `TextBox` inside that scope gets those settings unless overridden.

---

## Control Templates

A **ControlTemplate** changes the visual structure of a control.

This is more powerful than simply setting colors or font sizes.

### Example Idea

You can redefine how a `Button` is drawn:

```xaml
<ControlTemplate TargetType="Button">
    <Border Background="DarkOrange" CornerRadius="10" Padding="10">
        <ContentPresenter HorizontalAlignment="Center"
                          VerticalAlignment="Center"/>
    </Border>
</ControlTemplate>
```

### Difference Between Style and Template

| Feature | Style | ControlTemplate |
|---|---|---|
| Changes property values | ✅ | Sometimes |
| Rebuilds visual structure | ❌ | ✅ |
| Used for appearance reuse | ✅ | ✅ |

---

## Templates and Content Presenters

When making templates, `ContentPresenter` is important.

It displays the control’s content inside the template.

```xaml
<Border Background="SlateGray" Padding="8">
    <ContentPresenter/>
</Border>
```

Without it, a content control like a `Button` may not show its text or child content.

---

## Brushes and Colors

WPF uses brushes to paint UI surfaces.

### Common Brush Types

- `SolidColorBrush`
- `LinearGradientBrush`
- `RadialGradientBrush`
- `ImageBrush`

### Solid Color Example

```xaml
<TextBlock Text="Active"
           Foreground="ForestGreen"/>
```

### Gradient Example

```xaml
<Button Content="Launch" Foreground="White">
    <Button.Background>
        <LinearGradientBrush StartPoint="0,0" EndPoint="0,1">
            <GradientStop Color="#4A90E2" Offset="0"/>
            <GradientStop Color="#245D99" Offset="1"/>
        </LinearGradientBrush>
    </Button.Background>
</Button>
```

---

## Common Markup Extensions

A **markup extension** is a special XAML expression inside `{}`.

### Common Examples

| Syntax | Purpose |
|---|---|
| `{Binding ...}` | Creates a binding |
| `{StaticResource Key}` | Gets a static resource |
| `{DynamicResource Key}` | Gets a dynamic resource |
| `{x:Static ...}` | Uses a static .NET value |

### Example

```xaml
<TextBlock Text="{Binding CurrentStatus}"/>
```

---

## Collections in XAML

Some properties accept collections of child items.

### Example with `ListBox`

```xaml
<ListBox Width="180" Height="110">
    <ListBoxItem Content="North"/>
    <ListBoxItem Content="South"/>
    <ListBoxItem Content="East"/>
</ListBox>
```

The `ListBox` contains multiple items.

### Another Example with a Gradient Collection

```xaml
<LinearGradientBrush>
    <GradientStop Color="#FFE08A" Offset="0"/>
    <GradientStop Color="#FF9F43" Offset="1"/>
</LinearGradientBrush>
```

---

## Window Structure Example

A more realistic WPF XAML window might look like this:

```xaml
<Window x:Class="ClientApp.ProfileWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Profile Editor"
        Height="340"
        Width="460">
    <Grid Margin="18">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
            <RowDefinition Height="Auto"/>
        </Grid.RowDefinitions>

        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="Auto"/>
            <ColumnDefinition Width="*"/>
        </Grid.ColumnDefinitions>

        <TextBlock Grid.Row="0"
                   Grid.Column="0"
                   Margin="0,0,10,10"
                   VerticalAlignment="Center"
                   Text="First Name:"/>

        <TextBox Grid.Row="0"
                 Grid.Column="1"
                 Margin="0,0,0,10"/>

        <TextBlock Grid.Row="1"
                   Grid.Column="0"
                   Margin="0,0,10,10"
                   VerticalAlignment="Center"
                   Text="Last Name:"/>

        <TextBox Grid.Row="1"
                 Grid.Column="1"
                 Margin="0,0,0,10"/>

        <TextBlock Grid.Row="2"
                   Grid.Column="0"
                   Margin="0,0,10,10"
                   VerticalAlignment="Top"
                   Text="Bio:"/>

        <TextBox Grid.Row="2"
                 Grid.Column="1"
                 Height="90"
                 AcceptsReturn="True"
                 TextWrapping="Wrap"/>

        <StackPanel Grid.Row="4"
                    Grid.Column="1"
                    Orientation="Horizontal"
                    HorizontalAlignment="Right"
                    Margin="0,14,0,0">
            <Button Content="Save" Width="90" Margin="0,0,8,0"/>
            <Button Content="Cancel" Width="90"/>
        </StackPanel>
    </Grid>
</Window>
```

---

## Special Features of XAML in WPF

### What Makes It Powerful

- **declarative UI design**
- **strong support for data binding**
- **rich styling and templating**
- **resource reuse**
- **animation support**
- **integration with C# and .NET objects**

### Practical Impact

With XAML, you can:

1. define a complete desktop window visually
2. bind it to application data
3. restyle controls without changing business logic
4. build reusable UI pieces

---

## Common Mistakes Beginners Make

### 1. Using the Wrong Layout Panel

- using `Canvas` for forms that should resize
- using `StackPanel` where a `Grid` is more appropriate

### 2. Confusing `Margin` and `Padding`

| Property | Meaning |
|---|---|
| `Margin` | Space outside the control |
| `Padding` | Space inside the control |

### 3. Forgetting the `DataContext`

A binding like this:

```xaml
<TextBlock Text="{Binding OrderId}"/>
```

will show nothing if the `DataContext` does not provide `OrderId`.

### 4. Overusing Code-Behind

Putting too much UI logic in `.xaml.cs` can make the app harder to maintain.

### 5. Misunderstanding `StaticResource` vs `DynamicResource`

Use the right one depending on whether values stay fixed or may change at runtime.

---

## A Quick Mental Model

You can think of WPF XAML like this:

- **Controls** = UI building blocks
- **Panels** = layout organizers
- **Properties** = configuration
- **Resources** = reusable values
- **Bindings** = connection to data
- **Styles/Templates** = appearance customization
- **Code-behind / Commands** = behavior

> If you understand those seven ideas, you understand the foundation of XAML in WPF.

---

## Mini Example: Login Form

```xaml
<Window x:Class="DesktopSuite.LoginWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Member Login"
        Height="230"
        Width="340">
    <Grid Margin="18">
        <Grid.RowDefinitions>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="Auto"/>
            <RowDefinition Height="*"/>
        </Grid.RowDefinitions>

        <Grid.ColumnDefinitions>
            <ColumnDefinition Width="Auto"/>
            <ColumnDefinition Width="*"/>
        </Grid.ColumnDefinitions>

        <TextBlock Grid.Row="0"
                   Grid.Column="0"
                   Margin="0,0,12,10"
                   VerticalAlignment="Center"
                   Text="User ID:"/>

        <TextBox x:Name="UserIdBox"
                 Grid.Row="0"
                 Grid.Column="1"
                 Margin="0,0,0,10"/>

        <TextBlock Grid.Row="1"
                   Grid.Column="0"
                   Margin="0,0,12,10"
                   VerticalAlignment="Center"
                   Text="Password:"/>

        <PasswordBox x:Name="SecretBox"
                     Grid.Row="1"
                     Grid.Column="1"
                     Margin="0,0,0,10"/>

        <CheckBox Grid.Row="2"
                  Grid.Column="1"
                  Margin="0,0,0,10"
                  Content="Remember me"/>

        <Button Grid.Row="3"
                Grid.Column="1"
                Width="96"
                Height="34"
                HorizontalAlignment="Right"
                Content="Login"/>
    </Grid>
</Window>
```