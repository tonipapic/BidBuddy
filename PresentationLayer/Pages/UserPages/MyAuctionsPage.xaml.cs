using BusinessLogicLayer.Services;
using EntitiesLayer.Entities;
using Microsoft.Win32;
using PresentationLayer.Helpers;
using PresentationLayer.Navigation;
using PresentationLayer.Pages.UserPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
namespace PresentationLayer.Pages {
    /// <summary>
    /// Interaction logic for MyAuctionsPage.xaml
    /// </summary>
    public partial class MyAuctionsPage : System.Windows.Controls.UserControl {

        private NavigationControl navigationControl;

        private DataGridHelper<Auction> dghAuctions;

        public MyAuctionsPage(NavigationControl navigationControl) {
            InitializeComponent();
            this.navigationControl = navigationControl;

            dghAuctions = new DataGridHelper<Auction>(dgMyAuctions);
            dghAuctions.AddColumn("Naziv", (e) => e.Name);
            dghAuctions.AddColumn("Stvoreno", (e) => e.CreationDate.ToString("dd.MM.yyyy. HH:mm"));
            dghAuctions.AddColumn("Završava", (e) => e.EndDate.ToString("dd.MM.yyyy. HH:mm"));

            dghAuctions.AddColumn("Stanje", (e) => e.AuctionState.Name);

            dghAuctions.AddColumn("Regija", (e) => e.Region.Name);
            dghAuctions.AddColumn("Kategorija", (e) => e.Category.Name);
            dghAuctions.AddColumn("Proizvod", (e) => e.ProductState.Name);
            dghAuctions.AddColumn("Početna cijena", (e) => e.MinimalBidPrice);
            dghAuctions.AddColumn("Instant cijena", (e) => e.InstantBuyPrice.HasValue ? e.InstantBuyPrice.Value.ToString() : "nema");

        }
        private void Refresh() {

            var user = AuthenticationService.LoggedUser;
            var auctionService = new AuctionService();

            List<Auction> userAuctions = auctionService.GetAuctionsForUser(user.UserId);
            dghAuctions.DataSource = userAuctions;

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            Refresh();
        }

        private void btnCreateAuction_Click(object sender, RoutedEventArgs e) {
            navigationControl.PushPage(new CreateAuctionPage(navigationControl), "Kreiraj aukciju");
        }

        private void btnDeleteAuction_Click(object sender, RoutedEventArgs e) {

            Auction selected = dghAuctions.CurrentItem();
            if (selected == null) return;

            MessageBoxResult result = MessageBoxes.Confirm(
                "Brisanje aukcije",
                "Jeste li sigurni da želite obrisati aukciju? Ova radnja se ne može vratiti!");

            if (result == MessageBoxResult.OK) {
                var auctionService = new AuctionService();
                
                try {
                    auctionService.DeleteAuction(selected);
                    MessageBoxes.ShowInfo("Brisanje aukcije", "Uspješno ste obrisali aukciju!");
                    Refresh();
                } catch(Exception ex) {
                    MessageBoxes.ShowError("Brisanje aukcije", "Brisanje aukcije nije uspjelo! " + ex.Message);
                }
            }

        }

        private void btnDetails_Click(object sender, RoutedEventArgs e) {
            Auction selected = dghAuctions.CurrentItem();
            if(selected != null) {
                navigationControl.PushPage(new AuctionDetails(navigationControl, selected), "");
            }
        }

        private void dgMyAuctions_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lblAuctionName.Content = dghAuctions.CurrentItem()?.Name;
            lblBidAmount.Content = dghAuctions.CurrentItem()?.AuctionBids.Where(p => p.Selected).FirstOrDefault()?.Value+" €";
            lblBidTime.Content = dghAuctions.CurrentItem()?.EndDate.ToString("dd.MM.yyyy. HH:mm");
            lblHighestBidder.Content = dghAuctions.CurrentItem()?.AuctionBids.Where(p=>p.Selected).FirstOrDefault()?.User.FullName;
            stckPnlStats.Visibility = Visibility.Visible;

            if(dghAuctions.CurrentItem() == null) {
                return;
            }

            if(dghAuctions.CurrentItem().AuctionStateId!=AuctionState.Active)
            {
                btnPDFExport.Visibility = Visibility.Visible;

            }
            else
            {
                btnPDFExport.Visibility=Visibility.Collapsed;
            }
        }

        private void btnGetStatistics_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.ShowDialog();
            try { 
            var selectedFolderPath = folderBrowserDialog.SelectedPath;
            if (string.IsNullOrWhiteSpace(selectedFolderPath)) { return; }

            StatisticsService.GeneratePDF(selectedFolderPath,dghAuctions.CurrentItem());
            }
            catch(Exception ex)
            {
                MessageBoxes.ShowError("Greška", "Greška prilikom generiranja PDF-a! " + ex.Message);
            }
        }
    }
}
