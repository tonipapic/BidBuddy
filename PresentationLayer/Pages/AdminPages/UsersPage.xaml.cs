using BusinessLogicLayer.Services;
using EntitiesLayer.Entities;
using PresentationLayer.ApplicationTheme;
using PresentationLayer.Helpers;
using PresentationLayer.Navigation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PresentationLayer.Pages
{
    /// <summary>
    /// <remarks>Karlo Jačmenjak</remarks>
    /// </summary>
    public partial class UsersPage : UserControl
    {

        private ComboBoxHelper<UserRole> cbhUserRoles;
        private NavigationControl navigationControl;
        private DataGridHelper<Auction> dghUserAuctions;
        private DataGridHelper<User> dghUsers;
        private AuctionService auctionService;
        private UserService userService;
        private List<UserRole> roles;
        private List<User> users;
        private User editingUser;

        public UsersPage(NavigationControl navigationControl)
        {
            this.roles = new UserRoleService().GetUserRoles();
            this.navigationControl = navigationControl;
            this.auctionService = new AuctionService();
            this.userService = new UserService();
            InitializeComponent();
            InitializeUsersGrid();
            InitializeUserAuctionsGrid();
        }

        private void InitializeUsersGrid()
        {
            dghUsers = new DataGridHelper<User>(dgUsers);
            dghUsers.AddColumn("ID", (e) => e.UserId);
            dghUsers.AddColumn("Korisničko ime", (e) => e.Username);
            dghUsers.AddColumn("Ime", (e) => e.FirstName);
            dghUsers.AddColumn("Prezime", (e) => e.LastName);
            dghUsers.AddColumn("Email", (e) => e.Email);
            dghUsers.AddColumn("Broj telefona", (e) => e.PhoneNumber);
            dghUsers.AddColumn("Tip korisnika", (e) => e.UserRole.Name);
            dghUsers.AddColumn("Provjereni", typeof(bool), (e) => e.IsVerified);
            dghUsers.AddColumn("IBAN", (e) => e.IBAN);
            dghUsers.AddColumn("Broj ocjena", (e) => e.ReviewCount());
            dghUsers.AddColumn("Razlog", (e) => e.ReasonMessage());
        }

        private void InitializeUserAuctionsGrid()
        {
            this.dghUserAuctions = new DataGridHelper<Auction>(dgUserAuctions);
            dghUserAuctions.AddColumn("ID", (e) => e.AuctionId);
            dghUserAuctions.AddColumn("Aukcija", (e) => e.Name);
            dghUserAuctions.AddColumn("Kategorija", (e) => e.Category.Name);
            dghUserAuctions.AddColumn("Opis", (e) => e.Description);
            dghUserAuctions.AddColumn("Trenutna ponuda", (e) => e.GetCurrentBid());
            dghUserAuctions.AddColumn("Broj ponuda", (e) => e.AuctionBids.Count());
            dghUserAuctions.AddColumn("Preostalo vrijeme", (e) => Auction.TimeLeft(e));
            dghUserAuctions.AddColumn("Aukcija gotova", typeof(bool), (e) => Auction.IsAuctionOver(e));
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            FetchUsers();
            RefreshUsers();
            cbhUserRoles = new ComboBoxHelper<UserRole>(cmbUserRoles, (r) => r.Name);

            cbhUserRoles.DataSource = roles;
            tcUserOptions.Visibility = Visibility.Collapsed;
        }

        private void FetchUsers()
        {
            users = userService.GetAllUsers();
        }

        private void RefreshUsers(List<User> newUsers = null)
        {
            dghUsers.ClearData();
            if (newUsers != null)
                dghUsers.DataSource = newUsers;
            else
                dghUsers.DataSource = users;
        }
        private void FilterUsers()
        {
            var filtered = users.Where(u => u.Username.ToLower().Contains(tbUsername.Text.ToLower())).ToList();
            filtered = filtered.Where(u => u.Email.ToLower().Contains(tbEmail.Text.ToLower())).ToList();
            RefreshUsers(filtered);
        }

        private void FiltersChanged(object sender, TextChangedEventArgs e)
        {
            var page = AncestorFinder.FindAncestor<UsersPage>(sender as TextBox);
            page.FilterUsers();
        }

        private void ShowUserOptions()
        {

            editingUser = dghUsers.CurrentItem();
            if (editingUser != null)
            {
                cbhUserRoles.SelectItem(x => x.RoleId == editingUser.UserRoleId);
                chkBoxVerified.IsChecked = editingUser.IsVerified;
                txtBlockUsername.Text = editingUser.Username;
                txtBoxReason.Text = editingUser.BanMessage;
            }
        }

        private void SaveUserProperties(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            btn.IsEnabled = !btn.IsEnabled;
            if (editingUser != null)
                userService.UpdateUserProfile(
                    editingUser.UserId,
                    chkBoxVerified.IsChecked ?? false,
                    cbhUserRoles.CurrentItem().RoleId,
                    txtBoxReason.Text);
            FetchUsers();
            RefreshUsers();
            btn.IsEnabled = !btn.IsEnabled;
        }

        private void UserGridSelectedChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowUserOptions();
            _ = FetchUserAuctions();
        }

        private void EditClicked(object sender, RoutedEventArgs e)
        {
            ShowUserOptions();
            tcUserOptions.Visibility = tcUserOptions.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }

        private async void tabChanged(object sender, SelectionChangedEventArgs e)
        {
            if(e.Source is TabControl)
            {
                if(tiUserAuctions.IsSelected)
                {
                    await FetchUserAuctions();
                }
            }
        }

        private async Task FetchUserAuctions()
        {
            Filter<Auction> filter = new Filter<Auction>();
            filter.AddFilter(a => a.CreatorId == editingUser.UserId);

            Task<List<Auction>> auctionTask = auctionService.GetAuctionsFilteredAsync(filter);
            List<Auction> auctions = await auctionTask;

            dghUserAuctions.DataSource = auctions.OrderBy(x => x.EndDate).ToList();
        }
    }
}
