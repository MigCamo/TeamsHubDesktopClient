using Google.Protobuf.WellKnownTypes;
using Microsoft.Extensions.DependencyInjection;
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
            projectManagementRESTProvider = App.ServiceProvider.GetService<ProjectManagementRESTProvider>();
            _ = InitializeProjectsAsync();
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

                Image imgProjectIcon = new Image
                {
                    Height = 160,
                    Width = 160,
                    Source = new BitmapImage(new Uri("..\\Resources\\Pictures\\ICON-PROJECT.png", UriKind.RelativeOrAbsolute)),
                    Stretch = Stretch.Fill,
                    Margin = new Thickness(-1240, 0, 0, 0),
                };
                grdContainer.Children.Add(imgProjectIcon);

                Label lblProjectName = new Label
                {
                    Content = project.Name,
                    Foreground = new SolidColorBrush(Color.FromRgb(33, 37, 41)),
                    FontWeight = FontWeights.Black,
                    FontSize = 30,
                    Margin = new Thickness(190, 25, 0, 0),
                };
                grdContainer.Children.Add(lblProjectName);


                string dateAux = $"{project.StartDate:yyyy-MM-dd}";
                Label lblStartDate = new Label
                {
                    Content = "Fecha de inicio: " + dateAux,
                    Foreground = new SolidColorBrush(Color.FromRgb(33, 37, 41)),
                    FontWeight = FontWeights.SemiBold,
                    FontSize = 20,
                    Margin = new Thickness(190, 85, 0, 0),
                };
                grdContainer.Children.Add(lblStartDate);

                dateAux = $"{project.EndDate:yyyy-MM-dd}";
                Label lblEndDate = new Label
                {
                    Content = "Fecha de cierre: " + dateAux,
                    Foreground = new SolidColorBrush(Color.FromRgb(33, 37, 41)),
                    FontWeight = FontWeights.SemiBold,
                    FontSize = 20,
                    Margin = new Thickness(190, 120, 0, 0),
                };
                grdContainer.Children.Add(lblEndDate);

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
                    Source = new BitmapImage(new Uri("..\\Resources\\Pictures\\ICON-DELETEPROJECT.png", UriKind.RelativeOrAbsolute)),
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
                    Source = new BitmapImage(new Uri("..\\Resources\\Pictures\\ICON-UPDATEPROJECT.png", UriKind.RelativeOrAbsolute)),
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
            if(projectList != null)
            {
                ShowMyProjects(projectList);
            }
            else
            {
                MessageBox.Show("Lo siento hubo una problema con los servidores, intentelo mas tarde");
            }
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
            if (AreValidFields())
            {
                ProjectDTO projectDTO = GetProjectInfo();
                if (projectDTO.StartDate < projectDTO.EndDate)
                {
                    bool result = projectManagementRESTProvider.AddProject(projectDTO, StudentSinglenton.ID);
                    if (result)
                    {
                        MessageBox.Show("Se ha registrado correctamente el proyecto");
                        NavigationService.Navigate(new Index());
                    }
                    else
                    {
                        MessageBox.Show("Lo siento se ha presentado un problema en el servidor, intentelo nuevamente, mas tarde");
                    }
                }
                else
                {
                    MessageBox.Show("Lo siento pero la fecha de inicio no puede ser mayor a la de cierre");
                }
            }
            else
            {
                MessageBox.Show("Lo siento, verifique que los campos del fomulario no sean nulos, ni que fueran espacios en blancos");
            }
        }

        private async void Button_UpdateProject(object sender, RoutedEventArgs e)
        {
            if (AreValidFields())
            {
                ProjectDTO projectDTO = GetProjectInfo();

                if(projectDTO.StartDate < projectDTO.EndDate)
                {
                    bool result = await projectManagementRESTProvider.UpdateProjectAsync(projectDTO);
                    if (result)
                    {
                        MessageBox.Show("Se ha modificado correctamente el proyecto");
                        NavigationService.Navigate(new Index());
                    }
                    else
                    {
                        MessageBox.Show("Lo siento se ha presentado un problema en el servidor, intentelo nuevamente, mas tarde");
                    }
                }
                else
                {
                    MessageBox.Show("Lo siento pero la fecha de inicio no puede ser mayor a la de cierre");
                }

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
            bool areValidFields = true;

            if (txtName.Text.Trim().Length == 0)
            {
                areValidFields = false;
            }

            if(txtID.Text.Trim().Length > 0)
            {
                if (cboStatus.SelectedIndex == -1)
                {
                    areValidFields = false;
                }
            }
            
            if (dpStartDate.SelectedDate == null)
            {
                areValidFields = false;
            }

            if (dpEndDate.SelectedDate == null)
            {
                areValidFields = false;
            }

            if (!areValidFields)
            {
                MessageBox.Show("Lo siento, verifique que los campos del " +
                    "fomulario no sean nulos, ni que fueran espacios en blancos");
            }

            return areValidFields;
        }

        private ProjectDTO GetProjectInfo()
        {
            ProjectDTO projectDTO = new ProjectDTO();
            projectDTO.Name = txtName.Text;
            projectDTO.StartDate = dpStartDate.SelectedDate;
            projectDTO.EndDate = dpEndDate.SelectedDate;

            if(txtID.Text.Length != 0)
            {
                projectDTO.IdProject = int.Parse(txtID.Text);
                if (cboStatus.SelectedIndex == 0)
                {
                    projectDTO.Status = "En proceso";
                }
                else
                {
                    projectDTO.Status = "Finalizado";
                }
            }
            else
            {
                projectDTO.Status = "No iniciado";
            }

            return projectDTO;
        }

        private void Button_CloseForm(object sender, RoutedEventArgs e)
        {
            grdForm.Visibility = Visibility.Hidden;
            CleanFields();
        }

        private void UpdateProject(ProjectDTO projectDTO)
        {
            grdForm.Visibility = Visibility.Visible;
            btnUpdateProject.Visibility = Visibility.Visible;
            cboStatus.IsEnabled = true;
            FillFieldsWithInformation(projectDTO);
        }

        private void FillFieldsWithInformation(ProjectDTO projectDTO)
        {
            txtName.Text = projectDTO.Name;
            string dateAux = $"{projectDTO.StartDate:yyyy-MM-dd}";
            txtStartDate.Text = dateAux;
            dateAux = $"{projectDTO.EndDate:yyyy-MM-dd}";
            txtEndDate.Text = dateAux;
            dpStartDate.SelectedDate = projectDTO.StartDate;
            dpEndDate.SelectedDate = projectDTO.EndDate;
            txtID.Text = projectDTO.IdProject.ToString();
        }

        private async Task DeleteProjectAsync(ProjectDTO projectDTO)
        {
            bool result = await projectManagementRESTProvider.RemoveProjectAsync(projectDTO);

            if (result)
            {
                MessageBox.Show("Se ha eliminado el proyecto en la base de datos");
                NavigationService.Navigate(new Index());
            }
            else
            {
                MessageBox.Show("Lo siento se ha presentado un problema en el servidor, intentelo nuevamente, mas tarde");
            }
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
