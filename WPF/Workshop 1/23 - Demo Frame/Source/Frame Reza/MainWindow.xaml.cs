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

namespace Frame_Reza
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
  
        
        public MainWindow()
        {
            InitializeComponent();

            // Frame.NavigationFailed += MyFrame_NavigationFailed;
            // Frame1.NavigationFailed += MyFrame_NavigationFailed;
        }

    

        private void btnMsft_Click(object sender, RoutedEventArgs e)
        {
            Frame1.Visibility = System.Windows.Visibility.Hidden;
            Frame.Visibility =  System.Windows.Visibility.Visible;
           
        }

        private void btnIntel_Click(object sender, RoutedEventArgs e)
        {
            Frame.Visibility =  System.Windows.Visibility.Hidden;
            Frame1.Visibility =  System.Windows.Visibility.Visible;
        }
    }
}

