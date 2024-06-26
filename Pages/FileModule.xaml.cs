﻿using FilePackage;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using TeamsHubDesktopClient.SinglentonClasses;
using TeamsHubDesktopClient.Gateways.Provider;
using System.Windows.Media.Effects;
using Environment = System.Environment;
using Microsoft.Extensions.DependencyInjection;
using TeamsHubDesktopClient.DTOs;

namespace TeamsHubDesktopClient.Pages
{
    /// <summary>
    /// Lógica de interacción para FileModule.xaml
    /// </summary>
    public partial class FileModule : Page
    {

        private FileManagement.FileManagementClient? client;
        private FileManagementRESTProvider FileManagementRESTProvider;

        public FileModule()
        {
            InitializeComponent();
            lblProjectName.Content = ProjectSinglenton.projectDTO.Name;
            FileManagementRESTProvider = App.ServiceProvider.GetService<FileManagementRESTProvider>();
            _ = LoadFilesVisual();
        }

        private void BackToPreviousWindow(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Index());
        }

        private void UploadFileFromSystem(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string selectedFileName = openFileDialog.FileName;
                FileInfo fileInfo = new FileInfo(selectedFileName);

                if (fileInfo.Length > 4000 * 1024)
                {
                    MessageBox.Show("El archivo seleccionado es mayor a 4,000 KB. Por favor, seleccione un archivo más pequeño.", "Archivo demasiado grande", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else
                {
                    string selectedFilePath = openFileDialog.FileName;
                    string fileName = System.IO.Path.GetFileName(selectedFilePath);
                    string extension = System.IO.Path.GetExtension(selectedFilePath);
                    byte[] fileBytes = File.ReadAllBytes(selectedFilePath);
                    _ = SendFIleBygRPC(fileBytes, fileName, extension);
                    _ = LoadFilesVisual();
                }
            }
        }

        private async Task SendFIleBygRPC(byte[] fileContent, string fileName, string extension)
        {
            try
            {
                var channelOptions = new GrpcChannelOptions
                {
                    Credentials = ChannelCredentials.Insecure
                };

                using var channel = GrpcChannel.ForAddress("http://localhost:5001", channelOptions);
                client = new FileManagement.FileManagementClient(channel);
                var reply = await client.SaveFileAsync(new FileRequest
                {
                    ProjectName = ProjectSinglenton.projectDTO.IdProject,
                    FileName = fileName,
                    Extension = extension,
                    FileString = Google.Protobuf.ByteString.CopyFrom(fileContent)
                });
                MessageBox.Show("Se ha registrado el archivos correctamente", "Exito", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                NavigationService.Navigate(new FileModule());
            }
            catch (RpcException ex)
            {
                MessageBox.Show("Error al subir el archivo", "Error al subir el archivo", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private async Task LoadFilesVisual()
        {
            List<DocumentDTO> fileList = null;

            fileList = await FileManagementRESTProvider.GetAllFileByProjectAsync(ProjectSinglenton.projectDTO.IdProject);

            if (fileList != null)
            {
                wpProjectFiles.Children.Clear();
                ScrollViewer scrollViewer = new ScrollViewer
                {
                    VerticalScrollBarVisibility = ScrollBarVisibility.Hidden,
                    HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
                };
                StackPanel stackPanelContainer = new StackPanel();

                foreach (var file in  fileList)
                {
                    Grid grdContainer = new Grid
                    {
                        Margin = new Thickness(0, 0, 0, 10),
                    };

                    Rectangle rectBackground = new Rectangle
                    {
                        Height = 65,
                        Width = 1450,
                        RadiusX = 30,
                        RadiusY = 30,
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
                        Source = new BitmapImage(new Uri("..\\Resources\\Pictures\\ICON-ARCHIVO.png", UriKind.RelativeOrAbsolute)),
                        Margin = new Thickness(-1350, 0, 0, 0),
                    };
                    grdContainer.Children.Add(imgAddProductIcon);

                    Label lblNameCustomerOrder = new Label
                    {
                        Content = file.Name,
                        Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                        FontWeight = FontWeights.Black,
                        FontSize = 18,
                        Margin = new Thickness(90, 0, 0, 0),
                    };
                    grdContainer.Children.Add(lblNameCustomerOrder);

                    StackPanel buttonPanel = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        HorizontalAlignment = HorizontalAlignment.Right,
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(0, 0, 10, 0)
                    };

                    Style buttonStyle = new Style(typeof(Button));
                    buttonStyle.Setters.Add(new Setter(Control.BorderThicknessProperty, new Thickness(1))); 
                    buttonStyle.Setters.Add(new Setter(Control.BorderBrushProperty, new SolidColorBrush(Colors.Black))); // Color del borde
                    buttonStyle.Setters.Add(new Setter(Control.PaddingProperty, new Thickness(10))); // Relleno interno

                    Button btnDelete = new Button
                    {
                        Content = "Eliminar",
                        Margin = new Thickness(-95, 0, 5, 0),
                        Background = new SolidColorBrush(Color.FromRgb(255, 69, 58)),
                        Foreground = new SolidColorBrush(Colors.White),
                        FontWeight = FontWeights.Bold,
                        Width = 100,
                        Height = 40,
                    };

                    btnDelete.Click += async (sender, e) =>
                    {
                        try
                        {
                            var channelOptions = new GrpcChannelOptions
                            {
                                Credentials = ChannelCredentials.Insecure
                            };
                            using var channel = GrpcChannel.ForAddress("http://localhost:5001", channelOptions);
                            client = new FileManagement.FileManagementClient(channel);
                            var reply = await client.DeleteFileAsync(new DeleteRequest { IdFile = file.IdDocument });
                            MessageBox.Show("Se eliminará el archivo: " + file.Name);
                            _ = LoadFilesVisual();
                        }
                        catch (RpcException ex)
                        {
                            MessageBox.Show("Error al borrar el archivo", "Error al borrar el archivo", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    };

                    Button btnDownload = new Button
                    {
                        Content = "Descargar",
                        Margin = new Thickness(-15, 0, 0, 0),
                        Background = new SolidColorBrush(Color.FromRgb(50, 205, 50)),
                        Foreground = new SolidColorBrush(Colors.White),
                        FontWeight = FontWeights.Bold,
                        Width = 100,
                        Height = 40,
                    };

                    btnDownload.Click += async (sender, e) =>
                    {
                        var channelOptions = new GrpcChannelOptions
                        {
                            Credentials = ChannelCredentials.Insecure
                        };
                        using var channel = GrpcChannel.ForAddress("http://localhost:5001", channelOptions);
                        client = new FileManagement.FileManagementClient(channel);
                        try
                        {
                            var reply = await client.DownloadFileAsync(new DownloadRequest { IdFile = file.IdDocument });

                            if (reply.FileContent.Length > 0)
                            {
                                SaveFileInUserSystem(reply.FileContent.ToByteArray(), file.Name);
                                MessageBox.Show("Se descargará el archivo: " + file.Name);
                            }
                            else
                            {
                                MessageBox.Show("El archivo está vacío o no se pudo descargar correctamente.", "Error al descargar archivo", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                        catch (RpcException ex)
                        {
                            MessageBox.Show("El archivo no se ha encontrado", "Error al descargar archivo", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    };

                    buttonPanel.Children.Add(btnDelete);
                    buttonPanel.Children.Add(btnDownload);

                    grdContainer.Children.Add(buttonPanel);
                    stackPanelContainer.Children.Add(grdContainer);

                }

                scrollViewer.Content = stackPanelContainer;               
                wpProjectFiles.Children.Add(scrollViewer);
            }
            else
            {
                MessageBox.Show("Error al concetarse al servidor, intente nuevamente mas tarde", "Error de conexion", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveFileInUserSystem(byte[] fileBytes, string fileName)
        {

            string fileDirectory = "..\\ProjectFile\\" + ProjectSinglenton.projectDTO.Name;
            string filePath = System.IO.Path.Combine(fileDirectory, fileName);
            try
            {
                if (!Directory.Exists(fileDirectory))
                {
                    Directory.CreateDirectory(fileDirectory);
                }

                File.WriteAllBytes(filePath, fileBytes);
                Console.WriteLine($"Archivo guardado en: {filePath}");
            }
            catch (Exception ex)
            {
                lblProjectName.Content = "HAY UN PUTO ERROR";
                Console.WriteLine($"Error al guardar el archivo: {ex.Message}");
            }
        }
    }
}
