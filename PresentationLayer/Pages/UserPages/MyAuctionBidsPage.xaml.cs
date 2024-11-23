using BusinessLogicLayer.Services;
using EntitiesLayer.Entities;
using PresentationLayer.Helpers;
using PresentationLayer.Navigation;
using PresentationLayer.Pages.UserPages;
using System.Windows;
using System.Windows.Controls;

namespace PresentationLayer.Pages {
    /// <summary>
    /// Interaction logic for MyAuctionBidsPage.xaml
    /// </summary>
    public partial class MyAuctionBidsPage : UserControl {

        private NavigationControl navigationControl;
        private DataGridHelper<AuctionBid> dataGridHelper;
        public MyAuctionBidsPage(NavigationControl navigationControl) {
            InitializeComponent();
            this.navigationControl = navigationControl;
            dataGridHelper=new DataGridHelper<AuctionBid>(dgUserBids);

            dataGridHelper.AddColumn("Naziv aukcije", (r) => r.Auction.Name);
            dataGridHelper.AddColumn("Vrijednost ponude (€)", (r) => r.Value);
            dataGridHelper.AddColumn("Datum", (r) => r.Date.ToString("dd.MM.yyyy. u HH:mm"));
            dataGridHelper.AddColumn("Najveći ponuditelj", (r) => r.Selected ? "Da":"Ne");
            dataGridHelper.AddColumn("Stanje aukcije", (r) => r.Auction.AuctionState.Name.ToString());
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {

        }

        private void dgUserBids_Loaded(object sender, RoutedEventArgs e)
        {
          
            dataGridHelper.DataSource = new AuctionBidService().GetUserBids();
            
        }

        private void btnOpenAuction_Click(object sender, RoutedEventArgs e)
        {
            navigationControl.PushPage(new AuctionDetails(navigationControl, dataGridHelper.CurrentItem().Auction),"");
        }
    }
}
