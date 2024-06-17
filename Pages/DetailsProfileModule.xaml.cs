using ItalianPizza.Auxiliary;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using TeamsHubDesktopClient.DTOs;
using TeamsHubDesktopClient.Gateways.Provider;
using TeamsHubDesktopClient.Resources;
using TeamsHubDesktopClient.SinglentonClasses;
using static System.Net.Mime.MediaTypeNames;

namespace TeamsHubDesktopClient.Pages
{
    /// <summary>
    /// Lógica de interacción para DetailsProfileModule.xaml
    /// </summary>
    public partial class DetailsProfileModule : UserControl
    {
        StudentManagementRESTProvider studentManagement;
        StudentDTO myUser;
        String password;

        public DetailsProfileModule()
        {
            InitializeComponent();
            studentManagement = App.ServiceProvider.GetService<StudentManagementRESTProvider>();
            myUser = studentManagement.GetUserPersonalData(StudentSinglenton.ID);
            password = myUser.Password;
            InitializeUserData(myUser);

        }

        private void Button_CancelRegister(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
            Frame framePrincipal = mainWindow.FindName("frameContainer") as Frame;
            framePrincipal.Navigate(new Index());
        }

        private void InitializeUserData(StudentDTO studentDTO)
        {
            txtName.Text = studentDTO.Name;
            txtLastName.Text = studentDTO.LastName;
            txtSurName.Text = studentDTO.SurName;
            txtEmailRegister.Text = studentDTO.Email;
            txtNickName.Text = studentDTO.MiddleName; 
            pwdPasswordRegister.Password = studentDTO.Password;
        }

        private StudentDTO GetUserInfo()
        {
            StudentDTO student = new()
            {
                IdStudent = StudentSinglenton.ID,
                Name = txtName.Text,
                Email = txtEmailRegister.Text,
                MiddleName = txtNickName.Text,
                LastName = txtLastName.Text,
                SurName = txtSurName.Text,
                Password = pwdPasswordRegister.Password
            };

            return student;
        }

        private async void Button_EditUser(object sender, RoutedEventArgs e)
        { 
            if (AreUserFieldsValid())
            {
                if(pwdPasswordConfirm.Password == password)
                {
                    StudentDTO studentDTO = GetUserInfo();
                    int result = await studentManagement.EditStudent(studentDTO);

                    if (result == (int)ServerResponse.SuccessfulRegistration)
                    {
                        StudentSinglenton.FullName = txtLastName.Text + " " + txtSurName.Text + " " + txtName.Text + " " + txtNickName.Text;
                        MessageBox.Show("Modificacion de usuario correcta", "Se modifico correctamente la modificacion del usuario",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                        MainWindow mainWindow = Window.GetWindow(this) as MainWindow;
                        Frame framePrincipal = mainWindow.FindName("frameContainer") as Frame;
                        framePrincipal.Navigate(new Index());
                    }
                    else if (result == (int)ServerResponse.NotRegistered)
                    {
                        MessageBox.Show("Usuario no encontrado", "Lo siento, el usuario a modificar no se encuentra",
                            MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Lo siento, hubo un problema con los servidores", "Error en los servidores",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Contraseña incorrecta", "Lo siento pero la contraseña de confirmacion no coincide con la registrada",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void CleanFields()
        {
            txtEmailRegister.Text = string.Empty;
            txtEmailRegister.BorderBrush = Brushes.White;
            txtNickName.Text = string.Empty;
            txtNickName.BorderBrush = Brushes.White;
            txtEmailRegister.Text = string.Empty;
            txtEmailRegister.BorderBrush = Brushes.White;
            txtName.Text = string.Empty;
            txtName.BorderBrush = Brushes.White;
            txtLastName.Text = string.Empty;
            txtLastName.BorderBrush = Brushes.White;
            txtSurName.Text = string.Empty;
            txtSurName.BorderBrush = Brushes.White;
            pwdPasswordConfirm.Password = string.Empty;
            pwdPasswordConfirm.BorderBrush = Brushes.White;
            pwdPasswordRegister.Password = string.Empty;
            pwdPasswordRegister.BorderBrush = Brushes.White;
        }

        private bool AreUserFieldsValid()
        {
            bool areFieldsValid = true;

            List<string> errorMessages = new List<string>();
            if (!RegexChecker.CheckName(txtName.Text))
            {
                txtName.BorderBrush = Brushes.Red;
                errorMessages.Add("'Nombres'");
                areFieldsValid = false;
            }

            if (!RegexChecker.CheckLastName(txtLastName.Text))
            {
                txtLastName.BorderBrush = Brushes.Red;
                errorMessages.Add("'Apellido Paterno'");
                areFieldsValid = false;
            }

            if (!RegexChecker.CheckSecondLastName(txtSurName.Text))
            {
                txtSurName.BorderBrush = Brushes.Red;
                errorMessages.Add("'Apellido Materno'");
                areFieldsValid = false;
            }

            if (!RegexChecker.CheckName(txtNickName.Text))
            {
                txtNickName.BorderBrush = Brushes.Red;
                errorMessages.Add("'Apodo'");
                areFieldsValid = false;
            }

            if (!RegexChecker.CheckEmail(txtEmailRegister.Text))
            {
                txtEmailRegister.BorderBrush = Brushes.Red;
                errorMessages.Add("'Email'");
                areFieldsValid = false;
            }

            if (string.IsNullOrEmpty(pwdPasswordConfirm.Password))
            {
                pwdPasswordConfirm.BorderBrush = Brushes.Red;
                errorMessages.Add("'Confirmacion de contraseña'");
                areFieldsValid = false;
            }

            if (string.IsNullOrEmpty(pwdPasswordRegister.Password))
            {
                pwdPasswordRegister.BorderBrush = Brushes.Red;
                errorMessages.Add("'Contraseña'");
                areFieldsValid = false;
            }

            if (!areFieldsValid)
            {
                string WrongFields = "'" + string.Join("', '", errorMessages) + "'";
                MessageBox.Show("Campos invalidos", "Los campos " + WrongFields
                    + " no deben ser nulos, ni debe tener caracteres especiales");
            }

            return areFieldsValid;
        }

    }
}
