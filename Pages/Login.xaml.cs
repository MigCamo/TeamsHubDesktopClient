using ItalianPizza.Auxiliary;
using Microsoft.Extensions.DependencyInjection;
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
using TeamsHubDesktopClient.Resources;
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
            userIdentityManager = App.ServiceProvider.GetService<UserIdentityManagerRESTProvider>();
            studentManager = App.ServiceProvider.GetService<StudentManagementRESTProvider>();
        }

        private async void Button_Login(object sender, RoutedEventArgs e)
        {
            if (AreLoginFieldsValid())
            {
                var loginRequest = new SessionLoginRequest
                {
                    Email = txtEmail.Text,
                    password = pwdPassword.Password
                };

                var response = await userIdentityManager.ValidateUserAsync(loginRequest);
                if(response != null)
                {
                    if (response.IsValid)
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
                    MessageBox.Show("Lo siento hubo un problema con el servidor, intentelo mas tarde");
                }
            }
        }

        private void ViewPassword_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            txtPassword.Text = pwdPassword.Password;
            pwdPassword.Visibility = Visibility.Collapsed;
            txtPassword.Visibility = Visibility.Visible;
            lblHidePassword.Visibility = Visibility.Visible;
            lblViewPassword.Visibility = Visibility.Collapsed;
        }

        private void HidePassword_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            pwdPassword.Password = txtPassword.Text;
            txtPassword.Visibility = Visibility.Collapsed;
            pwdPassword.Visibility = Visibility.Visible;
            lblHidePassword.Visibility = Visibility.Collapsed;
            lblViewPassword.Visibility = Visibility.Visible;
        }

        private void TxtPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            pwdPassword.Password = txtPassword.Text;
        }

        private bool AreLoginFieldsValid()
        {
            bool areFieldsValid = true;

            if(string.IsNullOrEmpty(pwdPassword.Password)) areFieldsValid = false;
            if(string.IsNullOrEmpty(txtEmail.Text)) areFieldsValid = false;

            if(!areFieldsValid)
            {
                MessageBox.Show("Los campos no debe ser ni blancos ni nulos",
                    "Campos invalidos", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            if (pwdPassword.Password.Length < 8)
            {
                MessageBox.Show("La contraseña debe tener al menos 8 caracteres como minimo",
                    "Contraseña invalida", MessageBoxButton.OK, MessageBoxImage.Information );
                areFieldsValid = false;
            }

            return areFieldsValid;
        }

        private bool AreUserFieldsValid()
        {
            bool areUserFieldsValid = true;
            List<string> errorMessages = new List<string>();

            if (!RegexChecker.CheckName(txtName.Text))
            {
                txtName.BorderBrush = Brushes.Red;
                errorMessages.Add("'Nombres'");
                areUserFieldsValid = false;
            }

            if (!RegexChecker.CheckLastName(txtLastName.Text))
            {
                txtLastName.BorderBrush = Brushes.Red;
                errorMessages.Add("'Apellido Paterno'");
                areUserFieldsValid = false;
            }

            if (!RegexChecker.CheckSecondLastName(txtSurName.Text))
            {
                txtSurName.BorderBrush = Brushes.Red;
                errorMessages.Add("'Apellido Materno'");
                areUserFieldsValid = false;
            }

            if (!RegexChecker.CheckName(txtNickName.Text))
            {
                txtNickName.BorderBrush = Brushes.Red;
                errorMessages.Add("'Apodo'");
                areUserFieldsValid = false;
            }

            if (!RegexChecker.CheckEmail(txtEmailRegister.Text))
            {
                txtEmailRegister.BorderBrush = Brushes.Red;
                errorMessages.Add("'Email'");
                areUserFieldsValid = false;
            }

            if (string.IsNullOrEmpty(pwdPasswordConfirm.Password))
            {
                pwdPasswordConfirm.BorderBrush = Brushes.Red;
                errorMessages.Add("'Confirmacion de contraseña'");
                areUserFieldsValid = false;
            }

            if (string.IsNullOrEmpty(pwdPasswordRegister.Password))
            {
                pwdPasswordRegister.BorderBrush = Brushes.Red;
                errorMessages.Add("'Contraseña'");
                areUserFieldsValid = false;
            }

            if(!areUserFieldsValid)
            {
                string WrongFields = "'" + string.Join("', '", errorMessages) + "'";
                MessageBox.Show("Campos invalidos", "Los campos " + WrongFields
                    + " no deben ser nulos, ni debe tener caracteres especiales");
            }

            if (pwdPasswordRegister.Password.Length < 8)
            {
                MessageBox.Show("La contraseña debe tener al menos 8 caracteres como minimo",
                    "Contraseña invalida", MessageBoxButton.OK, MessageBoxImage.Information);
                areUserFieldsValid = false;
            }

            return areUserFieldsValid;
        }

        private void Button_RegisterUser(object sender, RoutedEventArgs e)
        {
            if (AreUserFieldsValid())
            {
                if(pwdPasswordRegister.Password == pwdPasswordConfirm.Password)
                {
                    StudentDTO studentDTO = GetUserInfo();
                    int result = studentManager.AddStudent(studentDTO);
                    if (result == (int)ServerResponse.SuccessfulRegistration)
                    {
                        MessageBox.Show("Se registro correctamente el usuario en el sistema");
                        grdLogin.Visibility = Visibility.Visible;
                        grdRegisterForm.Visibility = Visibility.Hidden;
                        CleanFields();
                    }
                    else if (result == (int)ServerResponse.NotRegistered)
                    {
                        MessageBox.Show("Lo siento, el usuario ya esta registrado", "Usuario ya existente", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        MessageBox.Show("Lo siento, hubo un problema con los servidores", "Error en los servidores", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Lo siento, pero la contraseña de registro y la contraseña de confirmacion, no coinciden, verifiquelas y intente de nuevo, por favor",
                        "contraseñas invalidas", MessageBoxButton.OK, MessageBoxImage.Error);
                }
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

        private void Label_ShowRegisterForm(object sender, MouseButtonEventArgs e)
        {
            grdLogin.Visibility = Visibility.Hidden;
            grdRegisterForm.Visibility = Visibility.Visible;
            txtEmail.Text = string.Empty;
            txtPassword.Text = string.Empty;
        }

        private void Label_ShowRecoverPasswordForm(object sender, MouseButtonEventArgs e)
        {
            grdLogin.Visibility = Visibility.Hidden;
            grdRecoverPasswordForm.Visibility = Visibility.Visible;
            txtEmail.Text = string.Empty;
            txtPassword.Text = string.Empty;
        }

        private void Button_PasswordRecovery(object sender, RoutedEventArgs e)
        {
            bool result;
            if(RegexChecker.CheckEmail(txtRecoverPassword.Text.Trim()))
            {
                result = userIdentityManager.PasswordRecovery(txtRecoverPassword.Text.Trim());
                if(result)
                {
                    MessageBox.Show("Se envio exitosamente la contraseña asu correo, verifiquelo para que recupere la contraseña",
                        "Envio Existoso", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Lo siento, pero no existe ningun correo que nos proporciono, en la base de datos",
                        "Correo no existente", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Lo siento, pero debe enviar un correo valido",
                    "Correo no valido", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Button_CloseRecoverPasswordForm(object sender, RoutedEventArgs e)
        {
            grdRecoverPasswordForm.Visibility = Visibility.Hidden;
            grdLogin.Visibility = Visibility.Visible;
            txtRecoverPassword.Text = string.Empty;
        }
    }
}
