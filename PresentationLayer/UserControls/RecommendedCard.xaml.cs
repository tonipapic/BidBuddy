using BusinessLogicLayer.Services;
using EntitiesLayer.Entities;
using PresentationLayer.Helpers;
using PresentationLayer.Navigation;
using PresentationLayer.Pages.UserPages;
using System;
using System.Collections.Generic;
using System.Drawing;
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

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for RecommendedCard.xaml
    /// </summary>
    public partial class RecommendedCard : UserControl
    {
        private AuctionImageService auctionImageService;
        private BitmapImage noImagesImage;
        public Auction RecommendedAuction
        {
            get { return (Auction)GetValue(RecommendedAuctionProperty); }
            set { SetValue(RecommendedAuctionProperty, value); }
        }

        public static readonly DependencyProperty RecommendedAuctionProperty =
            DependencyProperty.Register(
                "RecommendedAuction",
                typeof(Auction),
                typeof(RecommendedCard),
                new PropertyMetadata(default(Auction), RecommendedAuctionPropertyChanged));

        public NavigationControl NavigationControl
        {
            get { return (NavigationControl)GetValue(NavigationControlProperty); }
            set { SetValue(NavigationControlProperty, value); }
        }

        public static readonly DependencyProperty NavigationControlProperty =
            DependencyProperty.Register(
                "NavigationControl",
                typeof(NavigationControl),
                typeof(RecommendedCard),
                new PropertyMetadata(default(NavigationControl)));

        public RecommendedCard()
        {
            auctionImageService = new AuctionImageService();
            InitializeComponent();
        }

        private static void RecommendedAuctionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var card = d as RecommendedCard;
            card.lblName.Text = card.RecommendedAuction.Name;
            //BitmapImage bitmapImage = card.auctionImageService.GetImageForAuction(card.RecommendedAuction.AuctionId);
            //if (bitmapImage != null)
            //    card.imgAuctionImage.Source = new BitmapImageHelper().ConvertToBitmapImage(bitmapImage);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            noImagesImage = new BitmapImage(new Uri(@"\Resources\Images\nema-slike.png", UriKind.Relative));
            imgAuctionImage.Source = noImagesImage;
        }

        private void AuctionClicked(object sender, MouseButtonEventArgs e)
        {
            NavigationControl.PushPage(new AuctionDetails(NavigationControl, RecommendedAuction), RecommendedAuction.Name);
        }
    }
}
