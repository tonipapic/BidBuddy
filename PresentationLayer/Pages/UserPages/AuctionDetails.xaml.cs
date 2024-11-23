using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Services;
using EntitiesLayer.Entities;
using PresentationLayer.Helpers;
using PresentationLayer.Navigation;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace PresentationLayer.Pages.UserPages {
    /// <summary>
    /// Interaction logic for AuctionDetails.xaml
    /// <remark>Josip Mojzeš</remark>
    /// </summary>
    public partial class AuctionDetails : UserControl {

        private NavigationControl navigationControl;
        private AuctionService auctionService;
        private AuctionBidService bidService;

        private DataGridHelper<AuctionBid> dghBids;

        private int auctionId;
        private Auction auction;
        private List<AuctionBid> auctionBids;
        private AuctionBid currentBid;

        public AuctionDetails(NavigationControl navigationControl, Auction auction) {
            InitializeComponent();
            this.navigationControl = navigationControl;
            auctionId = auction.AuctionId;
            this.auction = auction;
            auctionService = new AuctionService();
            bidService = new AuctionBidService();

            dghBids = new DataGridHelper<AuctionBid>(dgBids);

            dghBids.AddColumn("Korisnik", (e) => e.User.FullName);
            dghBids.AddColumn("Datum", (e) => e.Date.ToString("dd.MM.yyyy. HH:mm"));
            dghBids.AddColumn("Ponuda", (e) => $"{e.Value} €");

            if(auction.AuctionStateId == AuctionState.Active)
            {
                stckPnlAddOffer.Visibility = Visibility.Visible;
            }

            CheckAuctionState();
        }

        private void CheckAuctionState(bool hide = false)
        {
            if (auction.AuctionStateId != AuctionState.Active || hide)
            {
                stckPnlAddOffer.Visibility = Visibility.Collapsed;
                chkBoxRecieveEmails.Visibility = Visibility.Collapsed;
                spReviews.Visibility = Visibility.Visible;
            }
        }

        private void Refresh() {

            auction = auctionService.GetAuctionById(auctionId);

            auctionBids = bidService.GetBidsForAuction(auction.AuctionId);
            currentBid = bidService.GetCurrentBid(auctionBids);

            navigationControl.UpdatePageTitle(this, auction.Name);

            txtCreator.Text = auction.User.FullName;
            txtCreationDate.Content = auction.CreationDate.ToString("dd.MM.yyyy. HH:mm");
            txtEndDate.Content = auction.EndDate.ToString("dd.MM.yyyy. HH:mm");

            txtRegion.Content = auction.Region.Name;
            txtCategory.Content = auction.Category.Name;
            txtProductState.Content = auction.ProductState.Name;

            txtMinimalBidPrice.Content = $"{auction.MinimalBidPrice} €";
            if(auction.InstantBuyPrice.HasValue) {
                txtInstantBuyPrice.Content = $"{auction.InstantBuyPrice} €";
            } else {
                txtInstantBuyPrice.Content = "nema";
            }

            txtAuctionState.Content = auction.AuctionState.Name;

            txtDescription.Text = auction.Description;
            LoadAuctionImages();
            LoadAuctionBids();

            HideSpecificControls();

            User currentUser = AuthenticationService.LoggedUser;
            if(currentUser.UserId == auction.CreatorId) {
                ShowControlsForAuctionCreator();
            } else {
                ShowControlsForNonAuctionCreator();
            }
        }

        /// <summary>
        /// Hide specific controls so they can be manually displayed as needed.
        /// </summary>
        private void HideSpecificControls() {

            spMakePayment.Visibility = Visibility.Collapsed;
            btnMakePayment.Visibility = Visibility.Collapsed;

            spManageAuction.Visibility = Visibility.Collapsed;
            btnEditAuction.Visibility = Visibility.Collapsed;
            btnConfirmAuctionEnd.Visibility = Visibility.Collapsed;
            btnConfirmPayment.Visibility = Visibility.Collapsed;
            btnConfirmDelivery.Visibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Show controls that are related to auction creator.
        /// </summary>
        private void ShowControlsForAuctionCreator() {

            // enable auction management for auction creator
            spManageAuction.Visibility = Visibility.Visible;
            int auctionState = auction.AuctionStateId;

            // enable editing of the auction if the auction is still active or not sold
            if (auctionState == AuctionState.Active || auctionState == AuctionState.NotSold) {
                btnEditAuction.Visibility = Visibility.Visible;
            }

            //enable confirmation of the end of auction
            if (auctionState == AuctionState.Finished) {
                btnConfirmAuctionEnd.Visibility = Visibility.Visible;
            }

            // show option to confirm payment if auction is in payment processing state
            if (auctionState == AuctionState.PaymentProcessing) {
                btnConfirmPayment.Visibility = Visibility.Visible;
            }

            // show option to confirm delivery if auction is in delivery state
            if(auctionState == AuctionState.InDelivery) {
                btnConfirmDelivery.Visibility = Visibility.Visible;
            }

        }

        /// <summary>
        /// Show controls that are related to auction bidder
        /// </summary>
        private void ShowControlsForNonAuctionCreator() {

            User currentUser = AuthenticationService.LoggedUser;
            int auctionState = auction.AuctionStateId;

            // enable payment for winner
            if (auctionState == AuctionState.PaymentProcessing && currentUser.UserId == currentBid?.BidderId) {
                spMakePayment.Visibility = Visibility.Visible;
                btnMakePayment.Visibility = Visibility.Visible;
            }

        }


        private void ConfirmAuctionEnd() {
            try {
                auctionService.ConfirmAuctionEnd(auction);
                MessageBoxes.ShowInfo("Potvrda završetka", "Potvrdili ste završetak aukcije!");
                Refresh();
            } catch (Exception e) {
                MessageBoxes.ShowError("Potvrda završetka", "Potvrda nije uspjela! " + e.Message);
            }
        }

        private void ConfirmPayment() {

            MessageBoxResult result = MessageBoxes.Confirm(
                "Potvrda plaćanja",
                "Jeste li sigurni da želite potvrditi plaćanje korisnika?");

            if(result != MessageBoxResult.OK) {
                return;
            }

            try {

                var paymentService = new PaymentService();
                paymentService.ConfirmPayment(auction, currentBid);

                MessageBoxes.ShowInfo("Potvrda plaćanja", "Uspješno ste potvrdili uplatu korisnika!");
                Refresh();

            }catch(PaymentException e) {
                MessageBoxes.ShowError("Potvrda plaćanja", e.Message);
            }catch(Exception e) {
                MessageBoxes.ShowError("Potvrda plaćanja", "Potvrda plaćanja nije uspjelo! " + e.Message);
            }

        }

        private void MakePayment() {
            navigationControl.PushPage(new MakePaymentPage(navigationControl, auction, currentBid), "Plaćanje");
        }

        private void ConfirmDelivery() {

            MessageBoxResult result = MessageBoxes.Confirm(
                "Potvrda dostave",
                "Jeste li sigurni da želite potvrditi dostavu proizvoda?");

            if (result != MessageBoxResult.OK) {
                return;
            }

            try {

                auctionService.ConfirmDelivery(auction);

                MessageBoxes.ShowInfo("Potvrda dostave", "Uspješno ste potvrdili dostavu proizvoda!");
                Refresh();

            } catch (Exception e) {
                MessageBoxes.ShowError("Potvrda dostave", "Potvrda dostave nije uspjela! " + e.Message);
            }

        }

        private void LoadAuctionImages() {

            AuctionImageService auctionImageService = new AuctionImageService();
            BitmapImageHelper bitmapImageHelper = new BitmapImageHelper();

            var auctionImages = auctionImageService.GetImagesForAuction(auction.AuctionId);

            var bitmapImages = bitmapImageHelper.ConvertToBitmapImages(auctionImages);
            aivImages.Images = bitmapImages;

        }

        private void LoadAuctionBids() {
            
            if(currentBid != null) {
                txtCurrentBidder.Text = currentBid.User.FullName;
                txtCurrentPrice.Content = $"{currentBid.Value} €";
            } else {
                txtCurrentBidder.Text = "nema";
                txtCurrentPrice.Content = "nema";
                linkCurrentBidder.IsEnabled = false;
            }

            dghBids.DataSource = auctionBids;
        }

        private void EditAuction() {
            navigationControl.PushPage(new CreateAuctionPage(navigationControl, auction), "Uredi aukciju");
        }

        
        private void btnAddBid_Click(object sender, RoutedEventArgs e)
        {
           
            try 
            {
            

            if( bidService.AddNewBid(auction, txtOfferValue.Text, (bool)chkBoxRecieveEmails.IsChecked, currentBid) == 1)
            {
                btnAddBid.IsEnabled = false;
                MessageBox.Show("Uspješno ste kupili proizvod");
                CheckAuctionState(true);
            }
            
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
            Refresh();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            Refresh();
        }

        private void btnEditAuction_Click(object sender, RoutedEventArgs e) {
            EditAuction();
        }

        private void btnConfirmPayment_Click(object sender, RoutedEventArgs e) {
            ConfirmPayment();
        }

        private void btnMakePayment_Click(object sender, RoutedEventArgs e) {
            MakePayment();
        }

        private void btnConfirmDelivery_Click(object sender, RoutedEventArgs e) {
            ConfirmDelivery();
        }

        private void btnConfirmAuctionEnd_Click(object sender, RoutedEventArgs e) {
            ConfirmAuctionEnd();
        }

        private void ShowUserInfo(User user) {
            navigationControl.PushPage(new UserPage(navigationControl, user.UserId), user.FullName);
        }

        private void linkCreator_Click(object sender, RoutedEventArgs e) {
            ShowUserInfo(auction.User);
        }

        private void linkCurrentBidder_Click(object sender, RoutedEventArgs e) {
            if(currentBid != null) {
                ShowUserInfo(currentBid.User);
            }
        }

        /// <summary>
        /// Handles the Click event of the "Submit Review" button.
        /// Submits a review for an auction if the conditions are met.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>Dorian Rušak</remarks>
        private void btnSubmitReview_Click(object sender, RoutedEventArgs e)
        {
            if((auction.AuctionStateId == AuctionState.Finished || auction.AuctionStateId == AuctionState.Sold) &&
                currentBid.BidderId == AuthenticationService.LoggedUser.UserId)

            {
                if (cmbRating.SelectedItem is ComboBoxItem selectedRatingItem)
                {
                    string selectedRating = selectedRatingItem.Content.ToString();

                    if (int.TryParse(selectedRating, out int rating))
                    {
                        string comment = txtComment.Text;

                        Review review = new Review
                        {
                            WriterId = AuthenticationService.LoggedUser.UserId,
                            Rating = rating,
                            Comment = comment,
                            AuctionId = auction.AuctionId,
                            Date = DateTime.Now
                        };

                        ReviewService reviewService = new ReviewService();
                        reviewService.AddReview(review);

                        MessageBox.Show("Recenzija uspješno dodata!");

                        txtComment.Text = string.Empty;
                        cmbRating.SelectedIndex = -1;
                    }
                }

            }
            else if(auction.AuctionStateId == AuctionState.NotSold)
            {
                MessageBox.Show("Možete dodavati recenzije samo na uspješno završene aukcije");
            }
            else
            {
                MessageBox.Show("Možete dodavati recenzije samo ako ste pobjedili na aukciji!");
            }
            
        }
    }
}
