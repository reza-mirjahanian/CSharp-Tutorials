using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CheckBox_Reza
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

        private void choc_Checked(object sender, RoutedEventArgs e)
        {
            Label1.Content = "Extra chocolate";
        }

        private void choc_Unchecked(object sender, RoutedEventArgs e)
        {
            Label1.Content = " ";
        }

        private void sugar_Checked(object sender, RoutedEventArgs e)
        {
            Label2.Content = "No sugar";
        }

        private void sugar_Unchecked(object sender, RoutedEventArgs e)
        {
            Label2.Content = " ";
        }
    }
}
