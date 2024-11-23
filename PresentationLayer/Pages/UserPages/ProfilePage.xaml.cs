using BusinessLogicLayer.Services;
using EntitiesLayer.Entities;
using PresentationLayer.Navigation;
using PresentationLayer.Pages.Misc;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Windows;
using System.Windows.Controls;

namespace PresentationLayer.Pages {
    /// <summary>
    /// Interaction logic for ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : UserControl {

        private NavigationControl navigationControl;

        /// <summary>
        /// Initializes a new instance of the ProfilePage class.
        /// </summary>
        /// <param name="navigationControl"></param>
        /// <remarks>Dorian Rušak</remarks>
        public ProfilePage(NavigationControl navigationControl) {
            InitializeComponent();
            this.navigationControl = navigationControl;
        }

        /// <summary>
        /// Handles the Loaded event of the UserControl.
        /// Loads user profile information upon page load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>Dorian Rušak</remarks>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        { 

            User loggedUser = AuthenticationService.LoggedUser;

            if (loggedUser != null)
            {
                UserService userService = new UserService();
                AuctionService auctionService = new AuctionService();
                int userId = loggedUser.UserId;

                User user = userService.GetUserProfile(userId);

                if (user != null)
                {
                    txtUsername.Text = user.Username;
                    txtFirstName.Text = user.FirstName;
                    txtLastName.Text = user.LastName;
                    txtEmail.Text = user.Email;
                    txtPhoneNumber.Text = user.PhoneNumber;
                    txtIban.Text = user.IBAN;

                    List<Auction> userAuctions = auctionService.GetAuctionsForUser(userId);
                    dataGridAuctions.ItemsSource = userAuctions;
                }
                else
                {
                    MessageBox.Show("Greška");
                }
            }
            else
            {
                MessageBox.Show("Nema prijavljenog korisnika");
            }
        }

        /// <summary>
        /// Handles the Click event of the "Update" button.
        /// Updates user profile information.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>Dorian Rušak</remarks>
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {

            User loggedUser = AuthenticationService.LoggedUser;

            if (loggedUser != null)
            {
                UserService userService = new UserService();
                int userId = loggedUser.UserId;

                string newFirstName = txtFirstName.Text;
                string newLastName = txtLastName.Text;
                string newEmail = txtEmail.Text;
                string newPhoneNumber = txtPhoneNumber.Text;
                string newIban = txtIban.Text;

                userService.UpdateUserProfile(userId, newFirstName, newLastName, newEmail, newPhoneNumber, newIban);
            }
            else
            {
                MessageBox.Show("Nema prijavljenog korisnika");
            }
        }

        /// <summary>
        /// Handles the Click event of the "Logout" button.
        /// Logs the user out and navigates to the login page.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>Dorian Rušak, Josip Mojzeš</remarks>
        private void btnLogut_Click(object sender, RoutedEventArgs e)
        {
            AuthenticationService authenticationService = new AuthenticationService();
            authenticationService.Logout();

            navigationControl.HideNavigationUI();
            navigationControl.SetPage(new LoginPage(navigationControl), "Prijava");

        }
    }
}
