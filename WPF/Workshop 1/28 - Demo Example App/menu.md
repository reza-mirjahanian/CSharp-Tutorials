In WPF, a menu is usually built with the `Menu` control and `MenuItem` elements.

### Basic example

```xml
<Menu>
    <MenuItem Header="_File">
        <MenuItem Header="_New" Click="New_Click"/>
        <MenuItem Header="_Open" Click="Open_Click"/>
        <Separator/>
        <MenuItem Header="E_xit" Click="Exit_Click"/>
    </MenuItem>

    <MenuItem Header="_Edit">
        <MenuItem Header="_Copy"/>
        <MenuItem Header="_Paste"/>
    </MenuItem>
</Menu>
```

### Code-behind

```csharp
private void New_Click(object sender, RoutedEventArgs e)
{
    MessageBox.Show("New clicked");
}

private void Open_Click(object sender, RoutedEventArgs e)
{
    MessageBox.Show("Open clicked");
}

private void Exit_Click(object sender, RoutedEventArgs e)
{
    Close();
}
```

### Key ideas

- `Menu` is the top-level container.
- `MenuItem` represents each menu option.
- Nested `MenuItem`s create submenus.
- `Header` is the displayed text.
- `_File` means `Alt + F` activates it.
- `Click` handles user selection.
- `Separator` adds a dividing line.

### With commands

WPF menus often use built-in commands:

```xml
<MenuItem Header="Copy" Command="ApplicationCommands.Copy"/>
<MenuItem Header="Paste" Command="ApplicationCommands.Paste"/>
```

### Context menu example

Right-click menu:

```xml
<TextBox>
    <TextBox.ContextMenu>
        <ContextMenu>
            <MenuItem Header="Clear" Click="Clear_Click"/>
        </ContextMenu>
    </TextBox.ContextMenu>
</TextBox>
```

So in short: use `Menu`, add `MenuItem`s, handle clicks or bind commands.