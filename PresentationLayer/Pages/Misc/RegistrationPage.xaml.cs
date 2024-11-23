using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Services;
using PresentationLayer.Navigation;
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

namespace PresentationLayer.Pages.Misc
{
    /// <summary>
    /// Interaction logic for RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : UserControl
    {
        private NavigationControl navigationControl;
        public RegistrationPage(NavigationControl navigationControl)
        {
            InitializeComponent();
            this.navigationControl = navigationControl;
        }

        /// <summary>
        /// Handles the click event when the user clicks the "Register" button.
        /// Attempts to register a new user with the provided information and displays a success message.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnRegistration_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string username = txtUsername.Text;
                string email = txtEmail.Text;
                string password = pbPassword.Password;
                string phoneNumber = txtPhoneNumber.Text;
                string firstName = txtName.Text;
                string lastName = txtSurname.Text;
                DateTime? birthDate = birthDatePicker.SelectedDate;
                string iban = txtIban.Text;

                UserService userService = new UserService();

                if(birthDate !=  null)
                {
                    userService.RegisterUser(username, email, password, phoneNumber, firstName, lastName, birthDate.Value, iban);
                }
                else
                {
                    userService.RegisterUser(username, email, password, phoneNumber, firstName, lastName, new DateTime(), iban);
                }

                

                successMessage.Text = "Registracija usjpešna";

                await Task.Delay(1000);

                LoginPage loginPage = new LoginPage(navigationControl);
                navigationControl.SetPage(loginPage, "Prijava");
            }
            catch (UsernameTakenException ex) 
            {
                MessageBox.Show(ex.Message);
            }
            catch(EmailTakenException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch (ArgumentNullException ex)
            { 
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Handles the click event when the user clicks the "Cancel" button.
        /// Navigates back to the login page.
        /// </summary>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            LoginPage loginPage = new LoginPage(navigationControl);
            navigationControl.SetPage(loginPage, "Prijava");
        }
    }
}
