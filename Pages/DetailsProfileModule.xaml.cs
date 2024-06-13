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

namespace TeamsHubDesktopClient.Pages
{
    /// <summary>
    /// Lógica de interacción para DetailsProfileModule.xaml
    /// </summary>
    public partial class DetailsProfileModule : UserControl
    {
        public DetailsProfileModule()
        {
            InitializeComponent();
        }

        private void Button_CancelRegister(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            Frame framePrincipal = mainWindow.FindName("frameContainer") as Frame;
            framePrincipal.Navigate(new Index());
        }
    }
}
