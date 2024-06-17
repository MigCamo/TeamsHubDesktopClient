using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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
using TeamsHubDesktopClient.DTOs;
using TeamsHubDesktopClient.Gateways.Provider;
using TeamsHubDesktopClient.SinglentonClasses;

namespace TeamsHubDesktopClient.Pages
{
    /// <summary>
    /// Lógica de interacción para ActivitiesModule.xaml
    /// </summary>
    public partial class ActivitiesModule : Page
    {
        private List<TaskDTO> _tasks;
        private TaskManagementRESTProvider _TaskManagement;

        public ActivitiesModule(int projectID)
        {
            InitializeComponent();
            _TaskManagement = App.ServiceProvider.GetService<TaskManagementRESTProvider>();
            LoadTasksModule(projectID);
        }

        private void LoadTasksModule(int projectID)
        {
            lblProjectName.Content = ProjectSinglenton.projectDTO.Name;
            _tasks = _TaskManagement.GetAllTaskByProject(projectID);

            if( _tasks != null)
            {
                ShowTask(_tasks);
            }
            else
            {
                MessageBox.Show("Lo siento hubo problemas con el servidor, intentelo mas tarde.",
                        "Error con el servidor", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void UpdateDatePickerFieldStartDate(object sender, SelectionChangedEventArgs e)
        {
            DateTime selectedDate = dpStartDate.SelectedDate.Value;
            string formattedDate = selectedDate.ToString("dd/MM/yyyy");
            txtDatePickerStartDate.Text = formattedDate;
        }

        public void UpdateDatePickerFieldEndDate(object sender, SelectionChangedEventArgs e)
        {
            DateTime selectedDate = dpEndDate.SelectedDate.Value;
            string formattedDate = selectedDate.ToString("dd/MM/yyyy");
            txtDatePickerEndDate.Text = formattedDate;
        }

        private void ShowTask(List<TaskDTO> tasks)
        {
            wpActivitiesEnProceso.Children.Clear();
            wpActivitiesPendientes.Children.Clear();
            wpActivitiesFinalizadas.Children.Clear();

            ScrollViewer scrollViewerPendientes = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                Content = new StackPanel()
            };

            ScrollViewer scrollViewerEnProceso = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                Content = new StackPanel()
            };

            ScrollViewer scrollViewerFinalizadas = new ScrollViewer
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                Content = new StackPanel()
            };

            StackPanel stackPanelPendientes = (StackPanel)scrollViewerPendientes.Content;
            StackPanel stackPanelEnProceso = (StackPanel)scrollViewerEnProceso.Content;
            StackPanel stackPanelFinalizadas = (StackPanel)scrollViewerFinalizadas.Content;

            foreach (var task in tasks)
            {
                Grid grdContainer = new Grid
                {
                    Margin = new Thickness(55, 0, 0, 30),
                };

                Rectangle rectBackground = new Rectangle
                {
                    Height = 200,
                    Width = 450,
                    RadiusX = 30,
                    RadiusY = 30,
                    Fill = new SolidColorBrush(Color.FromRgb(16, 18, 4)),
                    Margin = new Thickness(-45, 0, 0, 0),
                    Effect = new DropShadowEffect
                    {
                        Color = Colors.Black,
                        Direction = 215,
                        ShadowDepth = 5,
                        Opacity = 0.5,
                    }
                };
                grdContainer.Children.Add(rectBackground);

                SolidColorBrush solidColorBrush;

                if (task.Status == "Actividad Pendiente")
                {
                    solidColorBrush = new SolidColorBrush(Color.FromRgb(21, 143, 231));
                }
                else if (task.Status == "Actividad en proceso")
                {
                    solidColorBrush = new SolidColorBrush(Color.FromRgb(38, 231, 21));
                }
                else
                {
                    solidColorBrush = new SolidColorBrush(Color.FromRgb(231, 21, 21));
                }

                Rectangle label = new Rectangle
                {
                    Height = 14,
                    Width = 100,
                    RadiusX = 8,
                    RadiusY = 8,
                    Fill = solidColorBrush,
                    Margin = new Thickness(-350, -150, 0, 0)
                };
                grdContainer.Children.Add(label);

                Label lblTaskName = new Label
                {
                    Content = task.Name,
                    Foreground = new SolidColorBrush(Color.FromRgb(152, 162, 174)),
                    FontWeight = FontWeights.Black,
                    FontSize = 18,
                    Margin = new Thickness(-25, 40, 0, 0),
                };
                grdContainer.Children.Add(lblTaskName);

                Label lblTaskStartDate = new Label
                {
                    Content = "Fecha de inicio: " + task.StartDate.ToString("dd/MM/yy"),
                    Foreground = new SolidColorBrush(Color.FromRgb(152, 162, 174)),
                    FontWeight = FontWeights.Bold,
                    FontSize = 18,
                    Margin = new Thickness(-25, 90, 0, 0),
                };
                grdContainer.Children.Add(lblTaskStartDate);

                Label lblTaskEndDate = new Label
                {
                    Content = "Fecha de cierre: " + task.EndDate.ToString("dd/MM/yy"),
                    Foreground = new SolidColorBrush(Color.FromRgb(152, 162, 174)),
                    FontWeight = FontWeights.Bold,
                    FontSize = 18,
                    Margin = new Thickness(-25, 125, 0, 0),
                };
                grdContainer.Children.Add(lblTaskEndDate);

                grdContainer.PreviewMouseLeftButtonDown += (sender, e) => ShowDetailsTask(task);


                if (task.Status == "Actividad Pendiente")
                {
                    stackPanelPendientes.Children.Add(grdContainer);
                }
                else if (task.Status == "Actividad en proceso")
                {
                    stackPanelEnProceso.Children.Add(grdContainer);
                }
                else
                {
                    stackPanelFinalizadas.Children.Add(grdContainer);
                }
            }

            wpActivitiesPendientes.Children.Add(scrollViewerPendientes);
            wpActivitiesEnProceso.Children.Add(scrollViewerEnProceso);
            wpActivitiesFinalizadas.Children.Add(scrollViewerFinalizadas);
        }

        private void ShowDetailsTask(TaskDTO task)
        {
            grdForm.Visibility = Visibility.Visible;
            txtName.Text = task.Name;
            txtDescription.Text = task.Description;
            txtDatePickerStartDate.Text = task.StartDate.ToString("dd/MM/yy");
            txtDatePickerEndDate.Text = task.EndDate.ToString("dd/MM/yy");
            dpStartDate.SelectedDate = task.StartDate;
            dpEndDate.SelectedDate = task.EndDate;
            txtID.Text = task.IdTask.ToString();

            switch (task.Status)
            {
                case "Actividad Pendiente":
                    cboStatus.SelectedIndex = 0;
                    break;
                case "Actividad en proceso":
                    cboStatus.SelectedIndex = 1;
                    break;
                case "Actividad Finalizada":
                    cboStatus.SelectedIndex = 2;
                    break;
            }

            btnDeleteTask.Visibility = Visibility.Visible;
            btnUpdateTask.Visibility = Visibility.Visible;
        }

        private void BackToPreviousWindow(object sender, MouseButtonEventArgs e)
        {
            NavigationService.GoBack(); 
        }

        private TaskDTO GetTaskData()
        {
            TaskDTO taskDTO = new TaskDTO
            {
                Name = txtName.Text,
                Description = txtDescription.Text,
                StartDate = (DateTime)dpStartDate.SelectedDate,
                EndDate = (DateTime)dpEndDate.SelectedDate,
                IdProject = ProjectSinglenton.projectDTO.IdProject
            };

            switch (cboStatus.SelectedIndex)
            {
                case 0:
                    taskDTO.Status = "Actividad Pendiente";
                    break;
                case 1:
                    taskDTO.Status = "Actividad en proceso";
                    break;
                case 2:
                    taskDTO.Status = "Actividad Finalizada";
                    break;
            }

            if(txtID.Text.Length > 0)
            {
                taskDTO.IdTask = int.Parse(txtID.Text);
            }

            return taskDTO;
        }

        private async void Button_RegisterTask(object sender, RoutedEventArgs e)
        {
            if (AreValidFields())
            {
                TaskDTO newTask = GetTaskData();
                if(newTask.StartDate < newTask.EndDate)
                {
                    bool result = await _TaskManagement.AddTaskAsync(newTask);
                    if (result)
                    {
                        MessageBox.Show("La tarea se ha agregado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                        NavigationService.Navigate(new ActivitiesModule(ProjectSinglenton.projectDTO.IdProject));
                    }
                    else
                    {
                        MessageBox.Show("Lo siento hubo problemas con el servidor, intentelo mas tarde.",
                            "Error con el servidor", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Lo siento pero la fecha de inicio no puede ser mayor a la de cierre");
                }
            }
        }

        private async void Button_UpdateTask(object sender, RoutedEventArgs e)
        {
            if (AreValidFields())
            {
                TaskDTO task = GetTaskData();
                if (task.StartDate < task.EndDate)
                {
                    bool result = await _TaskManagement.UpdateTaskAsync(task);

                    if (result)
                    {
                        MessageBox.Show("La tarea se ha modificado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                        NavigationService.Navigate(new ActivitiesModule(ProjectSinglenton.projectDTO.IdProject));
                    }
                    else
                    {
                        MessageBox.Show("Lo siento hubo problemas con el servidor, intentelo mas tarde.",
                            "Error con el servidor", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Lo siento pero la fecha de inicio no puede ser mayor a la de cierre");
                }
            }
        }

        private async void Button_DeleteTask(object sender, RoutedEventArgs e)
        {
            bool result = await _TaskManagement.RemoveTaskAsync(int.Parse(txtID.Text));
            if (result)
            {
                MessageBox.Show("La tarea se ha agregado correctamente.", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new ActivitiesModule(ProjectSinglenton.projectDTO.IdProject));
            }
            else
            {
                MessageBox.Show("Lo siento hubo problemas con el servidor, intentelo mas tarde.",
                        "Error con el servidor", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }


        private bool AreValidFields()
        {
            bool band = true;

            if(txtName.Text.Trim().Length == 0)
            {
                band = false;
            }

            if (txtDescription.Text.Trim().Length == 0)
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

            if (cboStatus.SelectedItem == null)
            {
                band = false;
            }
             
            return band;
        }

        private void Button_ShowForm(object sender, RoutedEventArgs e)
        {
            grdForm.Visibility = Visibility.Visible;
        }

        private void CloseForm(object sender, MouseButtonEventArgs e)
        {
            grdForm.Visibility = Visibility.Hidden;
        }
    }
}
