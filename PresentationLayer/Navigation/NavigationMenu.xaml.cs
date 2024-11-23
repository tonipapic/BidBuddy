using EntitiesLayer.Entities;
using PresentationLayer.Pages;
using PresentationLayer.Pages.UserPages;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PresentationLayer.Navigation {
    /// <summary>
    /// Interaction logic for AdminNavigationMenu.xaml
    /// <remark>Josip Mojzeš</remark>
    /// </summary>
    public partial class NavigationMenu : UserControl {

        private NavigationControl navigationControl;

        private Control[] adminOnlyItems;

        public NavigationMenu(NavigationControl navigationControl) {
            InitializeComponent();
            this.navigationControl = navigationControl;
            adminOnlyItems = new Control[] {btnAllAuctions, btnUsers, btnCategories, btnRegions};
        }

        /// <summary>
        /// Show or hide items in navigation that only admin can see.
        /// </summary>
        /// <param name="show"></param>
        public void ShowAdminItems(bool show) {
            foreach(var item in adminOnlyItems) {
                item.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public void UpdateProfileButton(User user) {
            lblUser.Content = user.FullName;
        }

        private async void btnAuctions_Click(object sender, RoutedEventArgs e) {
            var loadingPage = new AuctionsLoadingPage(navigationControl);
            navigationControl.SetPage(loadingPage, "Aukcije");

            Task<AuctionsPage> pageTask = AuctionsPage.CreateAuctionsPageAsync(navigationControl);
            AuctionsPage page = await pageTask;
            navigationControl.SetPage(page, "Aukcije");
        }

        private void btnMyAuctions_Click(object sender, RoutedEventArgs e) {
            navigationControl.SetPage(new MyAuctionsPage(navigationControl), "Moje aukcije");
        }

        private void btnMyBids_Click(object sender, RoutedEventArgs e) {
            navigationControl.SetPage(new MyAuctionBidsPage(navigationControl), "Moje ponude");
        }

        private void btnProfile_Click(object sender, RoutedEventArgs e) {
            navigationControl.SetPage(new ProfilePage(navigationControl), "Profil");
        }

        private void btnCategories_Click(object sender, RoutedEventArgs e) {
            navigationControl.SetPage(new CategoriesPage(navigationControl), "Kategorije");
        }

        private void btnRegions_Click(object sender, RoutedEventArgs e) {
            navigationControl.SetPage(new RegionsPage(navigationControl), "Regije");
        }

        private void btnUsers_Click(object sender, RoutedEventArgs e) {
            navigationControl.SetPage(new UsersPage(navigationControl), "Korisnici");
        }

        private void btnAllAuctions_Click(object sender, RoutedEventArgs e) {
            navigationControl.SetPage(new AllAuctionsPage(navigationControl), "Sve aukcije");
        }
    }
}
