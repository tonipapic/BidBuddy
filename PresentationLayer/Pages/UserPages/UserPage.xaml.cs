using BusinessLogicLayer.Services;
using EntitiesLayer.Entities;
using PresentationLayer.Helpers;
using PresentationLayer.Navigation;
using System;
using System.Windows;
using System.Windows.Controls;

namespace PresentationLayer.Pages.UserPages {
    /// <summary>
    /// Interaction logic for UserPage.xaml
    /// </summary>
    public partial class UserPage : UserControl {

        private NavigationControl navigationControl;
        private int userId;
        private DataGridHelper<Review> dghSellerReviews;

        public UserPage(NavigationControl navigationControl, int userId)
        {
            InitializeComponent();
            this.navigationControl = navigationControl;
            this.userId = userId;

            DataGridHelper<Review> dgHelper = new DataGridHelper<Review>(dgSellerReviews);
            dghSellerReviews = new DataGridHelper<Review>(dgSellerReviews);

            dghSellerReviews.AddColumn("Auction Id", (review) => review.AuctionId);
            dghSellerReviews.AddColumn("Naziv aukcije", (review) => GetAuctionNameFromReview(review));
            dghSellerReviews.AddColumn("Ocjena", (review) => review.Rating);
            dghSellerReviews.AddColumn("Komentar", (review) => review.Comment);
            dghSellerReviews.AddColumn("Writer Id", typeof(int), (review) => review.WriterId);
            dghSellerReviews.AddColumn("Napisao recenziju", (review) => GetWriterNameFromReview(review));
        }

        private void Refresh()
        {

            var userService = new UserService();
            var user = userService.GetUserById(userId);

            txtUsername.Text = user.Username;
            txtFirstName.Text = user.FirstName;
            txtLastName.Text = user.LastName;
            txtEmail.Text = user.Email;
            txtPhoneNumber.Text = user.PhoneNumber;
            txtIban.Text = user.IBAN;

            if (user.IsVerified)
            {
                txtVerified.Text = "(korisnik je provjeren)";
            }
            else
            {
                txtVerified.Text = "(korisnik nije provjeren)";
            }
            LoadSellerReviews();

        }

        /// <summary>
        /// Loads and displays reviews for the seller identified by the 'userId'.
        /// </summary>
        /// <remarks>Dorian Rušak</remarks>
        private void LoadSellerReviews()
        {
            var userService = new UserService();
            var user = userService.GetUserById(userId);

            var reviewService = new ReviewService();
            var sellerReviews = reviewService.GetReviewsForSeller(user.UserId);

            dghSellerReviews.DataSource = sellerReviews;
        }

        private string GetAuctionNameFromReview(Review review)
        {
            if (review != null )
            {
                var auctionService = new AuctionService();
                var auction = auctionService.GetAuctionById(review.AuctionId);
                return auction?.Name ?? "N/A";
            }
            return "N/A";
        }

        private string GetWriterNameFromReview(Review review)
        {
            if (review != null)
            {
                var userService = new UserService();
                var user = userService.GetUserById(review.WriterId);
                return user?.FullName ?? "N/A";
            }
            return "N/A";
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh();
        }
    }
}

