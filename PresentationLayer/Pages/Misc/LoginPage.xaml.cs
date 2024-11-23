using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Services;
using EntitiesLayer.Entities;
using PresentationLayer.Helpers;
using PresentationLayer.Navigation;
using PresentationLayer.Pages.Misc;
using PresentationLayer.Pages.UserPages;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PresentationLayer.Pages {
    /// <summary>
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : UserControl {

        private NavigationControl navigationControl;

        /// <summary>
        /// Initializes a new instance of the LoginPage class
        /// </summary>
        /// <param name="navigationControl"></param>
        public LoginPage(NavigationControl navigationControl) {
            InitializeComponent();
            this.navigationControl = navigationControl;
        }

        /// <summary>
        /// Completes the user login process, sets the logged-in user, and navigates to the auctions page.
        /// </summary>
        /// <param name="user"></param>
        /// <remarks>Dorian Rušak, Josip Mojzeš</remarks>
        private async void FinishLogin(User user) {

            AuthenticationService.LoggedUser = user;
            navigationControl.NavigationMenu.UpdateProfileButton(user);
            var loadingPage = new AuctionsLoadingPage(navigationControl);

            Task<AuctionsPage> pageTask = AuctionsPage.CreateAuctionsPageAsync(navigationControl);
            AuctionsPage page = await pageTask;

            if (cbStayLoggedIn.IsChecked == true) {
                try {
                    var authService = new AuthenticationService();
                    authService.SaveLoggedUser(user);
                }catch(Exception e) {
                    MessageBoxes.ShowError("Ostani prijavljen", "Nije moguće spremiti podatke da ostanete prijavljeni! " + e.Message);
                }
            }

            if (user.UserRoleId == UserRole.Admin) {
                navigationControl.SetPage(loadingPage, "Aukcije");
                navigationControl.SetPage(page, "Aukcije");
                navigationControl.NavigationMenu.ShowAdminItems(true);
                navigationControl.DisplayNavigationUI();
            } else if (user.UserRoleId == UserRole.Basic) {
                navigationControl.SetPage(loadingPage, "Aukcije");
                navigationControl.SetPage(page, "Aukcije");
                navigationControl.NavigationMenu.ShowAdminItems(false);
                navigationControl.DisplayNavigationUI();
            }
        }
        
        private async void LoadSavedUser() {

            gridMain.Visibility = Visibility.Collapsed;
            gridLoading.Visibility = Visibility.Visible;

            AuthenticationService authService = new AuthenticationService();

            User user = await authService.LoadSavedUserAsync();
            if (user != null) {
                FinishLogin(user);
            }

            gridMain.Visibility = Visibility.Visible;
            gridLoading.Visibility = Visibility.Collapsed;

        }

        /// <summary>
        /// Handles the Click event of the "Login" button.
        /// Attempts to log in the user using provided credentials.
        /// Displays appropriate error messages in case of failures.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, RoutedEventArgs e) {
            string username = txtUsername.Text;
            string password = pbPassword.Password;

            UserService userService = new UserService();
            
            try
            {
                User user = userService.LoginUser(username, password);
                FinishLogin(user);
            }
            catch(UserNotFoundException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch(InvalidPasswordException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch(ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Handles the Click event of the "Registration" button.
        /// Navigates to the registration page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRegistration_Click(object sender, RoutedEventArgs e)
        {
            RegistrationPage registrationPage = new RegistrationPage(navigationControl);
            navigationControl.SetPage(registrationPage, "Registracija");
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            LoadSavedUser();
        }
    }
}
