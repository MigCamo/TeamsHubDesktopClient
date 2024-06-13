using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TeamHubServiceProjects.DTOs;
using TeamsHubDesktopClient.Gateways.Provider;
using TeamsHubDesktopClient.SinglentonClasses;

namespace TeamsHubDesktopClient.Pages
{
    /// <summary>
    /// Lógica de interacción para Index.xaml
    /// </summary>
    /// 
    public partial class Index : Page
    {
        ProjectManagementRESTProvider projectManagementRESTProvider;
        private TranslateTransform butProfileTranslateTransform;
        private TranslateTransform butLogOutTranslateTransform;
        private bool areButtonsVisible = false;
        List<ProjectDTO> projectList;

        public Index()
        {
            InitializeComponent();
            lblStudentName.Content = StudentSinglenton.FullName;
            projectManagementRESTProvider = new ProjectManagementRESTProvider();
            InitializeProjectsAsync();
            InitializeAnimation();
        }

        public void UpdateDatePickerFieldStartDate(object sender, SelectionChangedEventArgs e)
        {
            DateTime selectedDate = dpStartDate.SelectedDate.Value;
            string formattedDate = selectedDate.ToString("dd/MM/yyyy");
            txtStartDate.Text = formattedDate;
        }

        public void UpdateDatePickerFieldEndDate(object sender, SelectionChangedEventArgs e)
        {
            DateTime selectedDate = dpEndDate.SelectedDate.Value;
            string formattedDate = selectedDate.ToString("dd/MM/yyyy");
            txtEndDate.Text = formattedDate;
        }

        private void ShowMyProjects(List<ProjectDTO> myProjects)
        {
            wpMyProjects.Children.Clear();

            ScrollViewer scrollViewer = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
            };

            StackPanel stackPanelContainer = new StackPanel();

            foreach (var project in myProjects)
            {
                Grid grdContainer = new Grid
                {
                    Margin = new Thickness(55, 0, 0, 30),
                };

                Rectangle rectBackground = new Rectangle
                {
                    Height = 195,
                    Width = 1430,
                    RadiusX = 40,
                    RadiusY = 40,
                    Fill = new SolidColorBrush(Color.FromRgb(77, 97, 123)),
                    Margin = new Thickness(-45, 0, 0, 0),
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
                    Height = 160,
                    Width = 160,
                    Source = new BitmapImage(new Uri("D:\\Workspace-C#\\TeamsHubDesktopClient\\Resources\\Pictures\\ICON-PROJECT.png", UriKind.RelativeOrAbsolute)),
                    Stretch = Stretch.Fill,
                    Margin = new Thickness(-1240, 0, 0, 0),
                };
                grdContainer.Children.Add(imgAddProductIcon);

                Label lblNameCustomerOrder = new Label
                {
                    Content = project.Name,
                    Foreground = new SolidColorBrush(Color.FromRgb(33, 37, 41)),
                    FontWeight = FontWeights.Black,
                    FontSize = 30,
                    Margin = new Thickness(190, 25, 0, 0),
                };
                grdContainer.Children.Add(lblNameCustomerOrder);


                string dateAux = $"{project.StartDate:yyyy-MM-dd}";
                Label lblOrderCostCustomer = new Label
                {
                    Content = "Fecha de inicio: " + dateAux,
                    Foreground = new SolidColorBrush(Color.FromRgb(33, 37, 41)),
                    FontWeight = FontWeights.SemiBold,
                    FontSize = 20,
                    Margin = new Thickness(190, 85, 0, 0),
                };
                grdContainer.Children.Add(lblOrderCostCustomer);

                dateAux = $"{project.EndDate:yyyy-MM-dd}";
                Label lblOrderCostCustomer12 = new Label
                {
                    Content = "Fecha de cierre: " + dateAux,
                    Foreground = new SolidColorBrush(Color.FromRgb(33, 37, 41)),
                    FontWeight = FontWeights.SemiBold,
                    FontSize = 20,
                    Margin = new Thickness(190, 120, 0, 0),
                };
                grdContainer.Children.Add(lblOrderCostCustomer12);

                Label lblProjectStatus = new Label
                {
                    Content = "Estado: " + project.Status,
                    Foreground = new SolidColorBrush(Color.FromRgb(33, 37, 41)),
                    FontWeight = FontWeights.Black,
                    FontSize = 30,
                    Margin = new Thickness(1050, 25, 0, 0),
                };
                grdContainer.Children.Add(lblProjectStatus);

                Button button = new Button
                {
                    Content = "Ver detalles",
                    Height = 50,
                    Width = 250,
                    Background = new SolidColorBrush(Color.FromRgb(0, 0, 0)),
                    Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                    FontFamily = new FontFamily("Arial Black"),
                    FontSize = 22,
                    Margin = new Thickness(1050, 90, 0, 0),
                };

                button.Click += (sender, e) => GoToTaskWindow(project);
                grdContainer.Children.Add(button);

                Image btnDeleteProject = new Image
                {
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Height = 45,
                    Width = 45,
                    Margin = new Thickness(960, 115, 0, 0),
                    Source = new BitmapImage(new Uri("D:\\Workspace-C#\\TeamsHubDesktopClient\\Resources\\Pictures\\ICON-DELETEPROJECT.png", UriKind.RelativeOrAbsolute)),
                };
                btnDeleteProject.MouseLeftButtonUp += (sender, e) => DeleteProjectAsync(project);
                grdContainer.Children.Add(btnDeleteProject);

                Image btnUpdateProject = new Image
                {
                    HorizontalAlignment = System.Windows.HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    Height = 50,
                    Width = 50,
                    Margin = new Thickness(1025, 115, 0, 0),
                    Source = new BitmapImage(new Uri("D:\\Workspace-C#\\TeamsHubDesktopClient\\Resources\\Pictures\\ICON-UPDATEPROJECT.png", UriKind.RelativeOrAbsolute)),
                };
                btnUpdateProject.MouseLeftButtonUp += (sender, e) => UpdateProject(project);
                grdContainer.Children.Add(btnUpdateProject);

                stackPanelContainer.Children.Add(grdContainer);
            }

            scrollViewer.Content = stackPanelContainer;
            wpMyProjects.Children.Add(scrollViewer);
        }

        private async Task InitializeProjectsAsync()
        {
            projectList = await projectManagementRESTProvider.GetAllMyProjectsAsync(StudentSinglenton.ID);
            ShowMyProjects(projectList);
        }

        private void Button_ShowForm(object sender, RoutedEventArgs e)
        {
            grdForm.Visibility = Visibility.Visible;
        }
    
        private void GoToTaskWindow(ProjectDTO projectDTO)
        {
            ProjectSinglenton.projectDTO = projectDTO;
            NavigationService.Navigate(new ActivitiesModule(projectDTO.IdProject));
        }

        private void Button_RegisterProject(object sender, RoutedEventArgs e)
        {
            if (!AreValidFields())
            {
                ProjectDTO projectDTO = GetProjectInfo();
                bool result = projectManagementRESTProvider.AddProject(projectDTO, StudentSinglenton.ID);
                string message;
                if (result)
                {
                    message = "Se ha registrado correctamente el proyecto";
                    MessageBox.Show(message);
                    NavigationService.Navigate(new Index());
                }
                else
                {
                    message = "Lo siento se ha presentado un problema en el servidor, intentelo nuevamente, mas tarde";
                    MessageBox.Show(message);
                }
                MessageBox.Show(message);
            }
            else
            {
                MessageBox.Show("Lo siento, verifique que los campos del fomulario no sean nulos, ni que fueran espacios en blancos");
            }
        }

        private async void Button_UpdateProject(object sender, RoutedEventArgs e)
        {
            if (!AreValidFields())
            {
                ProjectDTO projectDTO = GetProjectInfo();
                bool result = await projectManagementRESTProvider.UpdateProjectAsync(projectDTO);
                string message;
                if (result)
                {
                    message = "Se ha modificado correctamente el proyecto";
                    MessageBox.Show(message);
                    NavigationService.Navigate(new Index());
                }
                else
                {
                    message = "Lo siento se ha presentado un problema en el servidor, intentelo nuevamente, mas tarde";
                    MessageBox.Show(message);
                }
            }
            else
            {
                MessageBox.Show("Lo siento, verifique que los campos del fomulario no sean nulos, ni que fueran espacios en blancos");
            }
        }

        public void CleanFields()
        {
            txtName.Text = string.Empty;
            txtEndDate.Text = string.Empty;
            txtStartDate.Text = string.Empty;
            cboStatus.SelectedIndex = -1;
        }
        
        private bool AreValidFields()
        {
            bool band = true;

            if (txtName.Text.Length == 0)
            {
                band = false;
            }

            if (cboStatus.SelectedIndex != -1)
            {
                band = false;
            }

            if (dpStartDate.SelectedDate == null)
            {
                band = false;
            }

            if (dpEndDate.SelectedDate == null)
            {
                band = false;
            }

            return band;
        }

        private ProjectDTO GetProjectInfo()
        {
            ProjectDTO projectDTO = new ProjectDTO();
            projectDTO.Name = txtName.Text;
            projectDTO.StartDate = dpStartDate.SelectedDate;
            projectDTO.EndDate = dpEndDate.SelectedDate;
            if(cboStatus.SelectedIndex == 0)
            {
                projectDTO.Status = "En proceso";
            }
            else
            {
                projectDTO.Status = "Finalizado";
            }

            if(txtID.Text.Length != 0)
            {
                projectDTO.IdProject = int.Parse(txtID.Text);
            }

            return projectDTO;
        }

        private void Button_CloseForm(object sender, RoutedEventArgs e)
        {
            grdForm.Visibility = Visibility.Hidden;
            CleanFields();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void UpdateProject(ProjectDTO projectDTO)
        {
            grdForm.Visibility = Visibility.Visible;
            txtName.Text = projectDTO.Name;
            string dateAux = $"{projectDTO.StartDate:yyyy-MM-dd}";
            txtStartDate.Text = dateAux;
            dateAux = $"{projectDTO.EndDate:yyyy-MM-dd}";
            txtEndDate.Text = dateAux;
            dpStartDate.SelectedDate = projectDTO.StartDate;
            dpEndDate.SelectedDate = projectDTO.EndDate;
            if(projectDTO.Status == "En proceso")
            {
                cboStatus.SelectedIndex = 0;
            }
            else
            {
                cboStatus.SelectedIndex = 1;
            }
            txtID.Text = projectDTO.IdProject.ToString();
            btnUpdateProject.Visibility = Visibility.Visible;
        }

        private async Task DeleteProjectAsync(ProjectDTO projectDTO)
        {
            await projectManagementRESTProvider.RemoveProjectAsync(projectDTO);
            MessageBox.Show("Se ha eliminado el proyecto en la base de datos");
            NavigationService.Navigate(new Index());
        }

        private void InitializeAnimation()
        {
            butProfileTranslateTransform = new TranslateTransform();
            butProfile.RenderTransform = butProfileTranslateTransform;
            butLogOutTranslateTransform = new TranslateTransform();
            butLogOut.RenderTransform = butLogOutTranslateTransform;
        }

        private void OpenSlidingMenu(object sender, MouseButtonEventArgs e)
        {
            if (areButtonsVisible)
            {
                HideButtons();
            }
            else
            {
                ShowButtons();
            }
        }

        private void ShowButtons()
        {
            DoubleAnimation moveButProfile = new DoubleAnimation(0, 66, TimeSpan.FromSeconds(0.5));
            butProfileTranslateTransform.BeginAnimation(TranslateTransform.YProperty, moveButProfile);
            DoubleAnimation moveButLogOut = new DoubleAnimation(0, 135, TimeSpan.FromSeconds(0.5));
            butLogOutTranslateTransform.BeginAnimation(TranslateTransform.YProperty, moveButLogOut);
            areButtonsVisible = true;
        }

        private void HideButtons()
        {
            DoubleAnimation moveButProfile = new DoubleAnimation(63, 0, TimeSpan.FromSeconds(0.5));
            butProfileTranslateTransform.BeginAnimation(TranslateTransform.YProperty, moveButProfile);
            DoubleAnimation moveButLogOut = new DoubleAnimation(126, 0, TimeSpan.FromSeconds(0.5));
            butLogOutTranslateTransform.BeginAnimation(TranslateTransform.YProperty, moveButLogOut);
            areButtonsVisible = false;
        }

        private void LogOut(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Login());
        }

        private void GoToMyProfile(object sender, RoutedEventArgs e)
        {
            DetailsProfileModule detailsProfileModule = new DetailsProfileModule()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                Margin = new Thickness(100, 115, 0, 0)
            };
            Grid.SetColumn(detailsProfileModule, 0);
            Background.Children.Add(detailsProfileModule);
        }
    }
}
