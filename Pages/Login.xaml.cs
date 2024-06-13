using ItalianPizza.Auxiliary;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
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
using TeamHubServiceProjects.DTOs;
using TeamsHubDesktopClient.DTOs;
using TeamsHubDesktopClient.Gateways.Provider;
using TeamsHubDesktopClient.SinglentonClasses;

namespace TeamsHubDesktopClient.Pages
{
    /// <summary>
    /// Lógica de interacción para Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        private readonly UserIdentityManagerRESTProvider userIdentityManager;
        private readonly StudentManagementRESTProvider studentManager;

        public Login()
        {
            InitializeComponent();
            userIdentityManager = new UserIdentityManagerRESTProvider();
            studentManager = new StudentManagementRESTProvider();
        }

        private async void Button_Login(object sender, RoutedEventArgs e)
        {
            if (AreLoginFieldsValid())
            {
                var loginRequest = new SessionLoginRequest
                {
                    Email = txtEmail.Text,
                    password = txtPassword.Text
                };

                var response = await userIdentityManager.ValidateUserAsync(loginRequest);
                if (response != null && response.IsValid)
                {
                    StudentSinglenton.ID = response.User.ID;
                    StudentSinglenton.FullName = response.User.FullName;
                    StudentSinglenton.Email = response.User.Email;
                    HttpClientSingleton.UserToken = response.Token;
                    NavigationService.Navigate(new Index());
                }
                else
                {
                    MessageBox.Show("El correo y contraseña no coinciden, " +
                        "intentelo de nuevamente");
                }
            }
            else
            {
                MessageBox.Show("Verifique que el correo y " +
                    "contraseña no sea nulo ni tenga espacios en blanco");
            }
        }

        private bool AreLoginFieldsValid()
        {
            bool band = true;

            if(string.IsNullOrEmpty(txtPassword.Text)) band = false;
            if(string.IsNullOrEmpty(txtEmail.Text)) band = false;

            return band;
        }

        private List<string> AreUserFieldsValid()
        {
            List<string> errorMessages = new List<string>();
            if (!RegexChecker.CheckName(txtName.Text))
            {
                txtName.BorderBrush = Brushes.Red;
                errorMessages.Add("'Nombres'");
            }

            if (!RegexChecker.CheckLastName(txtLastName.Text))
            {
                txtLastName.BorderBrush = Brushes.Red;
                errorMessages.Add("'Apellido Paterno'");
            }

            if (!RegexChecker.CheckSecondLastName(txtSurName.Text))
            {
                txtSurName.BorderBrush = Brushes.Red;
                errorMessages.Add("'Apellido Materno'");
            }

            if (!RegexChecker.CheckName(txtNickName.Text))
            {
                txtNickName.BorderBrush = Brushes.Red;
                errorMessages.Add("'Apodo'");
            }

            if (!RegexChecker.CheckEmail(txtEmailRegister.Text))
            {
                txtEmailRegister.BorderBrush = Brushes.Red;
                errorMessages.Add("'Email'");
            }

            if (string.IsNullOrEmpty(pwdPasswordConfirm.Password))
            {
                pwdPasswordConfirm.BorderBrush = Brushes.Red;
                errorMessages.Add("'Confirmacion de contraseña'");
            }

            if (string.IsNullOrEmpty(pwdPasswordRegister.Password))
            {
                pwdPasswordRegister.BorderBrush = Brushes.Red;
                errorMessages.Add("'Contraseña'");
            }

            return errorMessages;
        }

        private void Button_RegisterUser(object sender, RoutedEventArgs e)
        {
            List<string> errorMessages = AreUserFieldsValid();
            if (errorMessages.Count < 1)
            {
                StudentDTO studentDTO = GetUserInfo();
                bool result = studentManager.AddStudent(studentDTO);
                if (result)
                {
                    MessageBox.Show("Se registro correctamente el usuario en el sistema");
                    grdLogin.Visibility = Visibility.Visible;
                    grdRegisterForm.Visibility = Visibility.Hidden;
                    CleanFields();
                }
                else
                {
                    MessageBox.Show("Error en el servidor", "Lo siento hubo un problema con el servidor," +
                        " intentelo mas tarde");
                }
            }
            else
            {
                string WrongFields = "'" + string.Join("', '", errorMessages) + "'";
                MessageBox.Show("Campos invalidos", "Los campos " + WrongFields
                    + " no deben ser nulos, ni debe tener caracteres especiales");
            }
        }

        private StudentDTO GetUserInfo()
        {
            StudentDTO student = new()
            {
                Name = txtName.Text,
                Email = txtEmailRegister.Text,
                MiddleName = txtNickName.Text,
                LastName = txtLastName.Text,
                SurName = txtSurName.Text,
                Password = pwdPasswordConfirm.Password
            };

            return student;
        }

        private void Button_CloseForm(object sender, RoutedEventArgs e)
        {
            grdLogin.Visibility = Visibility.Visible;
            grdRegisterForm.Visibility = Visibility.Hidden;
            CleanFields();
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

        private void Label_ShowForm(object sender, MouseButtonEventArgs e)
        {
            grdLogin.Visibility = Visibility.Hidden;
            grdRegisterForm.Visibility = Visibility.Visible;
            txtEmail.Text = string.Empty;
            txtPassword.Text = string.Empty;
        }
    }
}
