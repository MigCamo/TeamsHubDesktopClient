using Microsoft.Extensions.DependencyInjection;
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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TeamHubServiceProjects.DTOs;
using TeamsHubDesktopClient.DTOs;
using TeamsHubDesktopClient.Gateways.Provider;
using TeamsHubDesktopClient.Resources;
using TeamsHubDesktopClient.SinglentonClasses;

namespace TeamsHubDesktopClient.Pages
{
    /// <summary>
    /// Lógica de interacción para AddStudentsToProject.xaml
    /// </summary>
    public partial class AddStudentsToProject : Page
    {
        private List<TaskDTO> _tasks;
        private List<User> users;
        private TaskManagementRESTProvider _TaskManagement;
        private StudentManagementRESTProvider _StudentManager;

        public AddStudentsToProject()
        {
            InitializeComponent();
            _TaskManagement = App.ServiceProvider.GetService<TaskManagementRESTProvider>();
            _StudentManager = App.ServiceProvider.GetService<StudentManagementRESTProvider>();
            InitializeInformation();
        }

        private void InitializeInformation()
        {
            lblProjectName.Content = ProjectSinglenton.projectDTO.Name;
            lblStartDate.Content = "Fecha de inicio: " + ProjectSinglenton.projectDTO.StartDate.ToString();
            lblEndDate.Content = "Fecha de cierre: " + ProjectSinglenton.projectDTO.EndDate.ToString();
            _tasks = _TaskManagement.GetAllTaskByProject(ProjectSinglenton.projectDTO.IdProject);
            users = _StudentManager.GetStudentsByProject(ProjectSinglenton.projectDTO.IdProject);
            ShowTask(_tasks);
            ShowStudents(users);
        }

        private void ShowTask(List<TaskDTO> taskList)
        {
            wpTask.Children.Clear();

            ScrollViewer scrollViewer = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
            };

            StackPanel stackPanelContainer = new StackPanel();
            if(taskList != null)
            {
                foreach (var task in taskList)
                {
                    Grid grdContainer = new Grid
                    {
                        Margin = new Thickness(0, 0, 0, 10),
                    };

                    Rectangle rectBackground = new Rectangle
                    {
                        Height = 65,
                        Width = 650,
                        RadiusX = 15,
                        RadiusY = 15,
                        Fill = new SolidColorBrush(Color.FromRgb(51, 47, 47)),
                        Margin = new Thickness(0, 0, 0, 0),
                    };

                    DropShadowEffect dropShadowEffect = new DropShadowEffect
                    {
                        Color = Colors.Black,
                        Direction = 315,
                        ShadowDepth = 5,
                        Opacity = 0.5,
                    };

                    rectBackground.Effect = dropShadowEffect;
                    grdContainer.Children.Add(rectBackground);

                    Image imgAddProductIcon = new Image
                    {
                        Height = 50,
                        Width = 50,
                        Source = new BitmapImage(new Uri("..\\Resources\\Pictures\\ICON-TASK.png", UriKind.RelativeOrAbsolute)),
                        Stretch = Stretch.Fill,
                        Margin = new Thickness(-565, 0, 0, 0),
                    };
                    grdContainer.Children.Add(imgAddProductIcon);

                    Label lblNameCustomerOrder = new Label
                    {
                        Content = task.Name,
                        Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                        FontWeight = FontWeights.Black,
                        FontSize = 18,
                        Margin = new Thickness(70, 0, 0, 0),
                    };
                    grdContainer.Children.Add(lblNameCustomerOrder);

                    Label lblOrderCostCustomer = new Label
                    {
                        Content = task.Description,
                        Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                        FontWeight = FontWeights.SemiBold,
                        FontSize = 18,
                        Margin = new Thickness(70, 25, 0, 0),
                    };
                    grdContainer.Children.Add(lblOrderCostCustomer);
                    stackPanelContainer.Children.Add(grdContainer);
                }

                scrollViewer.Content = stackPanelContainer;
                wpTask.Children.Add(scrollViewer);
            }
            else
            {
                MessageBox.Show("ha ocurrido un problema con los servidores", "Error en los servidores", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowStudents(List<User> studentList)
        {
            wpTeam.Children.Clear();

            ScrollViewer scrollViewer = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
            };

            StackPanel stackPanelContainer = new StackPanel();
            if (studentList != null)
            {
                foreach (var student in studentList)
                {
                    Grid grdContainer = new Grid
                    {
                        Margin = new Thickness(0, 0, 0, 10),
                    };

                    Rectangle rectBackground = new Rectangle
                    {
                        Height = 65,
                        Width = 650,
                        RadiusX = 15,
                        RadiusY = 15,
                        Fill = new SolidColorBrush(Color.FromRgb(51, 47, 47)),
                        Margin = new Thickness(0, 0, 0, 0),
                    };

                    DropShadowEffect dropShadowEffect = new DropShadowEffect
                    {
                        Color = Colors.Black,
                        Direction = 315,
                        ShadowDepth = 5,
                        Opacity = 0.5,
                    };

                    rectBackground.Effect = dropShadowEffect;
                    grdContainer.Children.Add(rectBackground);

                    Image imgAddProductIcon = new Image
                    {
                        Height = 50,
                        Width = 50,
                        Source = new BitmapImage(new Uri("..\\Resources\\Pictures\\ICON-PROFILE.png", UriKind.RelativeOrAbsolute)),
                        Stretch = Stretch.Fill,
                        Margin = new Thickness(-565, 0, 0, 0),
                    };
                    grdContainer.Children.Add(imgAddProductIcon);

                    Label lblNameCustomerOrder = new Label
                    {
                        Content = student.FullName,
                        Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                        FontWeight = FontWeights.Black,
                        FontSize = 18,
                        Margin = new Thickness(70, 0, 0, 0),
                    };
                    grdContainer.Children.Add(lblNameCustomerOrder);

                    Label lblOrderCostCustomer = new Label
                    {
                        Content = student.Email,
                        Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                        FontWeight = FontWeights.SemiBold,
                        FontSize = 18,
                        Margin = new Thickness(70, 25, 0, 0),
                    };
                    grdContainer.Children.Add(lblOrderCostCustomer);
                    stackPanelContainer.Children.Add(grdContainer);
                }

                scrollViewer.Content = stackPanelContainer;
                wpTeam.Children.Add(scrollViewer);
            }
            else
            {
                MessageBox.Show("ha ocurrido un problema con los servidores", "Error en los servidores", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        private void Button_RegisterUser(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(txtSearch.Text))
            {
                User student = _StudentManager.GetStudentInfo(txtSearch.Text);
                if(student != null)
                {
                    int result = _StudentManager.AddStudentToProject(student.ID, ProjectSinglenton.projectDTO.IdProject);

                    if(result == (int)ServerResponse.SuccessfulRegistration)
                    {
                        InitializeInformation();
                        MessageBox.Show("Se registro correctamente", "Exitoso", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else if (result == (int)ServerResponse.NotRegistered)
                    {
                        MessageBox.Show("Usuario ya existente", "Lo siento, el usuario ya esta registrado", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        MessageBox.Show("Lo siento, hubo un problema con los servidores", "Error en los servidores", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }
            }
        }
    }


}
