using BusinessLogicLayer.Services;
using EntitiesLayer.Entities;
using PresentationLayer.ApplicationTheme;
using PresentationLayer.Helpers;
using PresentationLayer.Navigation;
using PresentationLayer.Pages.UserPages;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace PresentationLayer.UserControls
{
    /// <summary>
    /// Interaction logic for AukcijaCardView.xaml
    /// </summary>
    public partial class AuctionCardView : UserControl
    {
        private const int ANIMATION_TIME = 250;

        public Auction Auction
        {
            get { return (Auction)GetValue(AuctionProperty); }
            set { SetValue(AuctionProperty, value); }
        }

        public static readonly DependencyProperty AuctionProperty =
            DependencyProperty.Register(
                "Auction",
                typeof(Auction),
                typeof(AuctionCardView),
                new PropertyMetadata(default(Auction), OnAuctionPropertyChanged));
        public NavigationControl NavigationControl
        {
            get { return (NavigationControl)GetValue(NavigationControlProperty); }
            set { SetValue(NavigationControlProperty, value); }
        }

        public static readonly DependencyProperty NavigationControlProperty =
            DependencyProperty.Register(
                "NavigationControl",
                typeof(NavigationControl),
                typeof(AuctionCardView),
                new PropertyMetadata(default(NavigationControl)));

        public AuctionCardView()
        {
            InitializeComponent();
        }
        private static void OnAuctionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            AuctionCardView source = d as AuctionCardView;


            source.txtBlockEndDate.Text = $"Završava: {source.Auction.EndDate:dd.MM.yyyy u HH:mm:ss}";
            source.HightlightCurrentBid();
            source.GetAuctionImages();
        }

        private void HightlightCurrentBid()
        {
            Run startRun = new Run();
            Run highlightRun = new Run();

            highlightRun.Foreground = new SolidColorBrush(AppColors.Emerald);
            highlightRun.FontWeight = FontWeights.Bold;
            highlightRun.FontSize = 14;

            FlowDocument flowDocument = new FlowDocument();
            Paragraph paragraph = new Paragraph();
            decimal currentBid = Auction.GetCurrentBid();

            startRun.Text = currentBid == Auction.MinimalBidPrice ?
                "Minimalna" :
                "Trenutna";
            startRun.Text += " ponuda: ";

            var value = Auction.GetCurrentBid();
            highlightRun.Text = $"{value}€";

            paragraph.Inlines.Add(startRun);
            paragraph.Inlines.Add(highlightRun);
            flowDocument.Blocks.Add(paragraph);
            flowDocument.TextAlignment = TextAlignment.Center;
            txtBlockCurrentBid.Document = flowDocument;
        }

        private void PointerEnter(object sender, MouseEventArgs e)
        {
            AuctionCardView source = sender as AuctionCardView;
            AnimateBrushColor(source, AppColors.BackgroundWhite, AppColors.BackgroundPurple);
            AnimateTextColor(source, AppColors.BackgroundBlack, AppColors.BackgroundWhite);
        }

        private void PointerLeave(object sender, MouseEventArgs e)
        {
            AuctionCardView source = sender as AuctionCardView;
            AnimateBrushColor(source, AppColors.BackgroundPurple, AppColors.BackgroundWhite);
            AnimateTextColor(source, AppColors.BackgroundWhite, AppColors.BackgroundBlack);
        }

        private void AnimateBrushColor(AuctionCardView card, Color from, Color to)
        {
            AppAnimations.AnimateBrushColor(card.bdrBackground, x => x.Background, from, to);
            AppAnimations.AnimateBrushColor(card.aivImages, x => x.Background, from, to);
        }

        private void AnimateTextColor(AuctionCardView card, Color from, Color to)
        {
            foreach (TextBlock tb in DescendantFinder.FindVisualChildren<TextBlock>(card))
            {
                if (AncestorFinder.FindAncestor<Button>(tb) != null) continue;
                AppAnimations.AnimateBrushColor(tb, x => x.Foreground, from, to);
            }
        }

        private void OpenAuctionDetails(object sender, EventArgs e)
        {

            if (sender is AuctionCardView)
            {
                var card = (AuctionCardView)sender;
                NavigateToDetails(card);
            }
            if (sender is Button)
            {
                var button = (Button)sender;
                var card = AncestorFinder.FindAncestor<AuctionCardView>(button);
                NavigateToDetails(card);
            }
        }

        private static void NavigateToDetails(AuctionCardView card)
        {
            if (card.NavigationControl == null || card.Auction == null) return;

            card.NavigationControl.PushPage(new AuctionDetails(card.NavigationControl, card.Auction), card.Auction.Name);
        }

        private void GetAuctionImages()
        {
            AuctionImageService auctionImageService = new AuctionImageService();
            BitmapImageHelper bitmapImageHelper = new BitmapImageHelper();

            var auctionImages = auctionImageService.GetImagesForAuction(Auction.AuctionId);

            var bitmapImages = bitmapImageHelper.ConvertToBitmapImages(auctionImages);
            this.aivImages.Images = bitmapImages;
        }
    }
}
