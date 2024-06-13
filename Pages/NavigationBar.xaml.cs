using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using TeamsHubDesktopClient.SinglentonClasses;

namespace TeamsHubDesktopClient.Pages
{
    /// <summary>
    /// Lógica de interacción para NavigationBar.xaml
    /// </summary>
    public partial class NavigationBar : UserControl
    {
        public NavigationBar()
        {
            InitializeComponent();
        }

        private void ChangePage(Page page)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            Frame framePrincipal = mainWindow.FindName("frameContainer") as Frame;
            framePrincipal.Navigate(page);
        }


        private void LogOut(object sender, MouseButtonEventArgs e)
        {
            ChangePage(new Login());
        }

        private void GoToTheModuleActivitiesWindow(object sender, MouseButtonEventArgs e)
        {
            ChangePage(new ActivitiesModule(ProjectSinglenton.projectDTO.IdProject));
        }

        private void GoToTheModuleFilesWindow(object sender, MouseButtonEventArgs e)
        {
            ChangePage(new FileModule());
        }
        private void GoToTheModuleProjectsWindow(object sender, MouseButtonEventArgs e)
        {
            ChangePage(new Index());
        }

        private void GoToTheProjectProgressModule(object sender, MouseButtonEventArgs e)
        {
            ChangePage(new ProjectProgressModule());
        }

        private void GoToTheAddStudentsToProject(object sender, MouseButtonEventArgs e)
        {
            ChangePage(new AddStudentsToProject());
        }


    }
}
