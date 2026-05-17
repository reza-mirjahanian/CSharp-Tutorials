using System.Windows;

namespace RezaClassExample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Btn_Clicked(object sender, RoutedEventArgs e)
        {
            Item em = new Item();
            em.Run();
        }
    }
}
