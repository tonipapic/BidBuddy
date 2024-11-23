using BusinessLogicLayer.Services;
using EntitiesLayer.Entities;
using PresentationLayer.Helpers;
using PresentationLayer.Navigation;
using PresentationLayer.Pages.UserPages;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PresentationLayer.Pages {
    /// <summary>
    /// Interaction logic for AllAuctionsPage.xaml
    /// </summary>
    public partial class AllAuctionsPage : UserControl {

        private NavigationControl navigationControl;
        private DataGridHelper<Auction> dghAuctions;

        public AllAuctionsPage(NavigationControl navigationControl) {
            InitializeComponent();
            this.navigationControl = navigationControl;

            dghAuctions = new DataGridHelper<Auction>(dgAllAuctions);
            dghAuctions.AddColumn("Naziv", (e) => e.Name);
            dghAuctions.AddColumn("Stvoreno", (e) => e.CreationDate.ToString("dd.MM.yyyy. HH:mm"));
            dghAuctions.AddColumn("Završava", (e) => e.EndDate.ToString("dd.MM.yyyy. HH:mm"));

            dghAuctions.AddColumn("Stanje", (e) => e.AuctionState.Name);

            dghAuctions.AddColumn("Regija", (e) => e.Region.Name);
            dghAuctions.AddColumn("Kategorija", (e) => e.Category.Name);
            dghAuctions.AddColumn("Proizvod", (e) => e.ProductState.Name);
            dghAuctions.AddColumn("Početna cijena", (e) => e.MinimalBidPrice + " €");
            dghAuctions.AddColumn("Instant cijena", (e) => e.InstantBuyPrice.HasValue ? e.InstantBuyPrice.Value.ToString() + " €" : "nema");

        }

        private void Refresh() {
            var auctionService = new AuctionService();
            List<Auction> userAuctions = auctionService.GetAllAuctions();
            dghAuctions.DataSource = userAuctions;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            Refresh();
        }

        private void btnDetails_Click(object sender, RoutedEventArgs e) {
            Auction selected = dghAuctions.CurrentItem();
            if (selected != null) {
                navigationControl.PushPage(new AuctionDetails(navigationControl, selected), "");
            }
        }

        private void btnDeleteAuction_Click(object sender, RoutedEventArgs e) {
            Auction selected = dghAuctions.CurrentItem();
            if (selected == null) return;

            MessageBoxResult result = MessageBoxes.Confirm(
                "Brisanje aukcije",
                "Jeste li sigurni da želite obrisati aukciju? Ova radnja se ne može vratiti!");

            if (result == MessageBoxResult.OK)
            {
                var auctionService = new AuctionService();

                try
                {
                    auctionService.DeleteAuction(selected);
                    MessageBoxes.ShowInfo("Brisanje aukcije", "Uspješno ste obrisali aukciju!");
                    Refresh();
                }
                catch (Exception ex)
                {
                    MessageBoxes.ShowError("Brisanje aukcije", "Brisanje aukcije nije uspjelo! " + ex.Message);
                }
            }
        }
    }
}
