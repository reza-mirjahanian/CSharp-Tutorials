# Table of Contents

1. [Clipping and Scrolling in WPF](#clipping-and-scrolling-in-wpf)
2. [What Is Clipping?](#what-is-clipping)
3. [What Is Scrolling?](#what-is-scrolling)
4. [How WPF Layout Affects Clipping and Scrolling](#how-wpf-layout-affects-clipping-and-scrolling)
5. [Clipping in WPF](#clipping-in-wpf)
   1. [Automatic Clipping with `ClipToBounds`](#automatic-clipping-with-cliptobounds)
   2. [Custom Clipping with the `Clip` Property](#custom-clipping-with-the-clip-property)
6. [Scrolling in WPF](#scrolling-in-wpf)
   1. [`ScrollViewer`](#scrollviewer)
   2. [Physical vs Logical Scrolling](#physical-vs-logical-scrolling)
   3. [Scrollbar Visibility](#scrollbar-visibility)
7. [Clipping and Scrolling Together](#clipping-and-scrolling-together)
8. [Common Controls Related to Scrolling](#common-controls-related-to-scrolling)
9. [Examples](#examples)
   1. [Basic Clipping Example](#basic-clipping-example)
   2. [Custom Clip Shape Example](#custom-clip-shape-example)
   3. [Basic Scrolling Example](#basic-scrolling-example)
   4. [Horizontal and Vertical Scrolling Example](#horizontal-and-vertical-scrolling-example)
10. [Important Notes](#important-notes)

---

# Clipping and Scrolling in WPF

In **WPF**, *clipping* and *scrolling* are two important concepts used when content is **larger than the available display area**.

- **Clipping** controls what part of a visual element is allowed to be shown.
- **Scrolling** allows the user to move through hidden content instead of cutting it off permanently.

These concepts are often used together in UI design.

> If content does not fit inside a container, WPF can either:
>
> - hide the extra part (**clip it**), or
> - let the user navigate to it (**scroll it**).

---

# What Is Clipping?

**Clipping** means restricting the visible region of a UI element.

If an element draws outside its allowed area, clipping can prevent those extra parts from being displayed.

## Simple idea

Imagine a large image inside a small box:

- The **box** is the visible area.
- The **image** may be bigger than the box.
- With clipping, only the part inside the box is visible.

## In WPF, clipping is commonly done by:

- `ClipToBounds`
- `Clip`

---

# What Is Scrolling?

**Scrolling** means moving the visible viewport over larger content.

Instead of hiding extra content forever, scrolling lets the user access it by moving up, down, left, or right.

## Example

If a panel contains many controls and they do not fit vertically:

- without scrolling → some controls may not be reachable
- with scrolling → the user can move through the full content

---

# How WPF Layout Affects Clipping and Scrolling

WPF uses a layout system based on two important passes:

1. **Measure**
2. **Arrange**

## Measure pass

Each element tells its parent how much size it wants.

## Arrange pass

The parent gives the child its final size and position.

This matters because:

- some controls try to take as much space as they need
- some parents restrict child size
- clipping and scrolling depend on the final arranged size

> A control may contain large content, but whether it gets clipped or scrolls depends on the container and layout behavior.

---

# Clipping in WPF

## Automatic Clipping with `ClipToBounds`

`ClipToBounds` is a property that determines whether content outside the bounds of an element is visible.

### Values

| Value | Meaning |
|---|---|
| `True` | Content outside the element’s boundary is hidden |
| `False` | Content may render outside the boundary |

### Example idea

A child control is larger than its parent:

- if `ClipToBounds="True"` → extra area is hidden
- if `ClipToBounds="False"` → the child may draw beyond the parent

## Example

```xml
<Border Width="180"
        Height="90"
        Background="LightGray"
        ClipToBounds="True">
    <Button Width="260"
            Height="140"
            Content="Large Button"/>
</Border>
```

### Result

- The `Button` is larger than the `Border`
- Because `ClipToBounds` is enabled, only the part inside the `Border` is shown

---

## Custom Clipping with the `Clip` Property

The `Clip` property allows you to define a **specific geometry** that limits what part of an element is visible.

This is more flexible than `ClipToBounds`.

### Common geometry types

- `RectangleGeometry`
- `EllipseGeometry`
- `PathGeometry`
- `CombinedGeometry`

## Example: rectangular clip

```xml
<Grid>
    <Image Source="sample-banner.png"
           Width="240"
           Height="160">
        <Image.Clip>
            <RectangleGeometry Rect="20,10,140,90"/>
        </Image.Clip>
    </Image>
</Grid>
```

### Meaning of `Rect="20,10,140,90"`

- `20` → X position
- `10` → Y position
- `140` → width
- `90` → height

Only that rectangle region of the image will be visible.

## Example: ellipse clip

```xml
<Image Source="avatar-photo.png"
       Width="180"
       Height="180">
    <Image.Clip>
        <EllipseGeometry Center="90,90"
                         RadiusX="70"
                         RadiusY="70"/>
    </Image.Clip>
</Image>
```

This creates a circular or elliptical visible region.

> `Clip` is useful when you want non-rectangular visual effects.

---

# Scrolling in WPF

## `ScrollViewer`

The main control used for scrolling in WPF is **`ScrollViewer`**.

It provides:

- a viewport
- horizontal and/or vertical scrollbars
- the ability to navigate content larger than the visible area

## Basic structure

```xml
<ScrollViewer Width="220" Height="140">
    <StackPanel>
        <TextBlock Text="Line 1"/>
        <TextBlock Text="Line 2"/>
        <TextBlock Text="Line 3"/>
        <TextBlock Text="Line 4"/>
        <TextBlock Text="Line 5"/>
        <TextBlock Text="Line 6"/>
        <TextBlock Text="Line 7"/>
    </StackPanel>
</ScrollViewer>
```

If the content becomes taller than `140`, vertical scrolling appears.

---

## Physical vs Logical Scrolling

WPF supports two scrolling styles:

### 1. Physical scrolling

Scrolling moves by **pixels** or physical distance.

- smooth movement
- common for general content

### 2. Logical scrolling

Scrolling moves by **items** rather than pixels.

- often used in controls like `ListBox`
- movement happens item by item

Logical scrolling usually depends on panels that implement `IScrollInfo`, such as:

- `VirtualizingStackPanel`

---

## Scrollbar Visibility

You can control when scrollbars appear using:

- `HorizontalScrollBarVisibility`
- `VerticalScrollBarVisibility`

### Common values

| Value | Meaning |
|---|---|
| `Disabled` | No scrolling allowed in that direction |
| `Hidden` | Scrolling possible, scrollbar not shown |
| `Auto` | Show scrollbar only when needed |
| `Visible` | Always show scrollbar |

## Example

```xml
<ScrollViewer Width="260"
              Height="150"
              HorizontalScrollBarVisibility="Auto"
              VerticalScrollBarVisibility="Visible">
    <TextBlock TextWrapping="Wrap"
               FontSize="16"
               Text="This is a long sample paragraph designed to demonstrate how scrolling behaves when the text content exceeds the display area of the container."/>
</ScrollViewer>
```

---

# Clipping and Scrolling Together

Clipping and scrolling are related, but they are **not the same**.

## Key difference

- **Clipping**: hides overflow
- **Scrolling**: provides access to overflow

## Together in practice

A `ScrollViewer` usually displays only a portion of its content at a time.

That means:

- the visible area acts like a **viewport**
- content outside the viewport is effectively not shown at that moment
- scrolling changes which part becomes visible

You can think of scrolling as a **movable clipped window** over larger content.

> The viewport clips what is currently outside view, while the scrollbars allow navigation through the hidden parts.

---

# Common Controls Related to Scrolling

Several WPF controls use scrolling internally or commonly interact with `ScrollViewer`.

## Examples

- `TextBox`
- `ListBox`
- `ListView`
- `TreeView`
- `DataGrid`
- `RichTextBox`

These controls may:

- contain a built-in `ScrollViewer`
- support logical scrolling
- use virtualization for better performance

---

# Examples

## Basic Clipping Example

```xml
<Grid>
    <Border Width="210"
            Height="110"
            Background="#FFD9D9D9"
            BorderBrush="#FF606060"
            BorderThickness="1"
            ClipToBounds="True">
        <Rectangle Width="320"
                   Height="180"
                   Fill="CornflowerBlue"/>
    </Border>
</Grid>
```

### What happens?

1. The `Border` is smaller than the `Rectangle`
2. The rectangle extends beyond the border area
3. Because `ClipToBounds="True"`, only the inside portion is visible

---

## Custom Clip Shape Example

```xml
<Grid>
    <TextBlock Text="WPF Clipping Demo"
               FontSize="30"
               FontWeight="Bold"
               Foreground="DarkBlue">
        <TextBlock.Clip>
            <EllipseGeometry Center="110,25"
                             RadiusX="90"
                             RadiusY="22"/>
        </TextBlock.Clip>
    </TextBlock>
</Grid>
```

### Effect

The text is shown only inside the elliptical area.

---

## Basic Scrolling Example

```xml
<ScrollViewer Width="240"
              Height="130"
              VerticalScrollBarVisibility="Auto">
    <StackPanel Margin="8">
        <Button Content="Option A" Margin="0,4"/>
        <Button Content="Option B" Margin="0,4"/>
        <Button Content="Option C" Margin="0,4"/>
        <Button Content="Option D" Margin="0,4"/>
        <Button Content="Option E" Margin="0,4"/>
        <Button Content="Option F" Margin="0,4"/>
        <Button Content="Option G" Margin="0,4"/>
    </StackPanel>
</ScrollViewer>
```

### Behavior

- The `StackPanel` may become taller than the viewer
- A vertical scrollbar appears automatically when needed

---

## Horizontal and Vertical Scrolling Example

```xml
<ScrollViewer Width="280"
              Height="150"
              HorizontalScrollBarVisibility="Auto"
              VerticalScrollBarVisibility="Auto">
    <Canvas Width="520" Height="360" Background="Beige">
        <Rectangle Canvas.Left="40"
                   Canvas.Top="30"
                   Width="180"
                   Height="90"
                   Fill="Tomato"/>
        
        <Ellipse Canvas.Left="290"
                 Canvas.Top="180"
                 Width="140"
                 Height="140"
                 Fill="MediumSeaGreen"/>
    </Canvas>
</ScrollViewer>
```

### Behavior

Because the `Canvas` is larger than the `ScrollViewer`:

- horizontal scrolling is needed
- vertical scrolling is needed
- both scrollbars can appear

---

# Important Notes

## 1. `ClipToBounds` is not the same as `Clip`

- `ClipToBounds` clips to the element’s rectangular bounds
- `Clip` clips using a custom geometry

## 2. Not every overflowing element scrolls automatically

If content overflows a container:

- overflow may simply be hidden
- or it may still render outside
- scrolling only happens when a scrolling mechanism exists, usually a `ScrollViewer`

## 3. Panels behave differently

Different panels affect overflow differently:

- `Grid`
- `Canvas`
- `StackPanel`
- `WrapPanel`
- `DockPanel`

For example:

- a `Canvas` allows absolute positioning
- a `StackPanel` can grow in one direction
- scrolling behavior can change depending on the panel inside the `ScrollViewer`

## 4. Infinite size can affect scrolling

Some panels, especially `StackPanel`, can measure children in a way that influences scrollbar behavior.

This is why layout issues sometimes appear when nesting controls.

## 5. Performance matters for large content

For large lists or data-heavy UIs, prefer controls and panels that support:

- virtualization
- logical scrolling
- efficient rendering

## 6. Typical attached properties

Some controls expose scrolling-related attached properties such as:

- `ScrollViewer.HorizontalScrollBarVisibility`
- `ScrollViewer.VerticalScrollBarVisibility`
- `ScrollViewer.CanContentScroll`

Example:

```xml
<ListBox ScrollViewer.VerticalScrollBarVisibility="Auto"
         ScrollViewer.CanContentScroll="True"
         Width="220"
         Height="140">
    <ListBoxItem Content="Item 101"/>
    <ListBoxItem Content="Item 102"/>
    <ListBoxItem Content="Item 103"/>
    <ListBoxItem Content="Item 104"/>
    <ListBoxItem Content="Item 105"/>
    <ListBoxItem Content="Item 106"/>
</ListBox>
```

## 7. Mental model

A useful way to remember this:

- **Clipping** = *cut away what does not fit*
- **Scrolling** = *move across what does not fit*

> Clipping controls visibility.  
> Scrolling controls navigation.