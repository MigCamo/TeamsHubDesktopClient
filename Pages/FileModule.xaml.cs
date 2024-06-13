using FilePackage;
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
            FileManagementRESTProvider = new FileManagementRESTProvider();
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
        }

        private async Task LoadFilesVisual()
        {
            var fileList = await FileManagementRESTProvider.GetAllFileByProjectAsync(ProjectSinglenton.projectDTO.IdProject);

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
                        Source = new BitmapImage(new Uri("..\\Resources\\Pictures\\ICON-ARCHIVO.png", UriKind.RelativeOrAbsolute)),
                        Margin = new Thickness(-1370, 0, 0, 0),
                    };
                    grdContainer.Children.Add(imgAddProductIcon);

                    Label lblNameCustomerOrder = new Label
                    {
                        Content = file.Name,
                        Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255)),
                        FontWeight = FontWeights.Black,
                        FontSize = 18,
                        Margin = new Thickness(70, 0, 0, 0),
                    };
                    grdContainer.Children.Add(lblNameCustomerOrder);

                    StackPanel buttonPanel = new StackPanel
                    {
                        Orientation = Orientation.Horizontal,
                        HorizontalAlignment = HorizontalAlignment.Right,
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(0, 0, 10, 0) // Ajustar el margen según sea necesario
                    };

                    Style buttonStyle = new Style(typeof(Button));
                    buttonStyle.Setters.Add(new Setter(Control.BorderThicknessProperty, new Thickness(1))); // Grosor del borde
                    buttonStyle.Setters.Add(new Setter(Control.BorderBrushProperty, new SolidColorBrush(Colors.Black))); // Color del borde
                    buttonStyle.Setters.Add(new Setter(Control.PaddingProperty, new Thickness(10))); // Relleno interno

                    Button btnDelete = new Button
                    {
                        Content = "Eliminar",
                        Margin = new Thickness(0, 0, 5, 0),
                        Background = new SolidColorBrush(Color.FromRgb(255, 69, 58)),
                        Foreground = new SolidColorBrush(Colors.White),
                        Style = buttonStyle
                    };

                    btnDelete.Click += async (sender, e) =>
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
                    };

                    Button btnDownload = new Button
                    {
                        Content = "Descargar",
                        Margin = new Thickness(0, 0, 0, 0),
                        Background = new SolidColorBrush(Color.FromRgb(50, 205, 50)),
                        Foreground = new SolidColorBrush(Colors.White),
                        Style = buttonStyle
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
                                MessageBox.Show("El archivo está vacío o no se pudo descargar correctamente.");
                            }
                        }
                        catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
                        {
                            MessageBox.Show("Archivo no encontrado: " + ex.Message);
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
        }

        private void SaveFileInUserSystem(byte[] fileBytes, string fileName)
        {

            string fileDirectory = "..\\ProjectFile\\" + ProjectSinglenton.projectDTO.Name;
            string filePath = System.IO.Path.Combine(fileDirectory, fileName);
            try
            {
                lblProjectName.Content = "EMTRP AIQO";
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
