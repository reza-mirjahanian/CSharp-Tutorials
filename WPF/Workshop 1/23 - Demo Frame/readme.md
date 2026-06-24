In WPF, a **Frame** is a control used to **display and navigate between pages**. It’s mainly used in applications that use **Page-based navigation** (similar to a web browser).

Think of it as a **container that loads Page objects**.

Basic idea:
- **Frame** = host container
- **Page** = the content being shown inside the Frame

Simple example:

XAML:
<Window x:Class="FrameExample.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        Title="Frame Example" Height="300" Width="400">

    <Grid>
        <Frame x:Name="MainFrame"/>
    </Grid>

</Window>


Code-behind (C#):
public MainWindow()
{
    InitializeComponent();
    MainFrame.Navigate(new Page1());
}


Here the **Frame loads Page1**.

Example Page:

<Page x:Class="FrameExample.Page1"
      xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
      xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">

    <Grid>
        <TextBlock Text="Hello from Page 1"
                   HorizontalAlignment="Center"
                   VerticalAlignment="Center"
                   FontSize="24"/>
    </Grid>

</Page>


Navigation to another page:

MainFrame.Navigate(new Page2());


or inside a Page:

NavigationService.Navigate(new Page2());


Important properties of Frame:
- **Source** → loads a page by URI  
- **Navigate()** → loads a page programmatically  
- **NavigationUIVisibility** → shows/hides navigation buttons  
- **CanGoBack / GoBack()** → navigation history

Example using Source:

<Frame Source="Page1.xaml"/>


Common use cases:
- Multi-page applications
- Wizards (Next / Previous pages)
- Settings screens
- Navigation-style apps

Difference between **Frame vs Window**:
- **Window** = a separate top-level window
- **Frame** = a container inside a window for pages

Simple structure of a navigation app:
MainWindow
   └── Frame
        ├── Page1
        ├── Page2
        └── Page3


