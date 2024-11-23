using EntitiesLayer.Entities;
using PresentationLayer.Navigation;
using PresentationLayer.UserControls;
using System.Windows;
using System.Windows.Controls;

namespace PresentationLayer.Pages.UserPages {
    /// <summary>
    /// Interaction logic for MakePaymentPage.xaml
    /// <remark>Josip Mojzeš</remark>
    /// </summary>
    public partial class MakePaymentPage : UserControl {

        private NavigationControl navigationControl;
        private Auction auction;
        private AuctionBid selectedBid;

        public MakePaymentPage(NavigationControl navigationControl, Auction auction, AuctionBid selectedBid) {
            InitializeComponent();
            this.navigationControl = navigationControl;
            this.auction = auction;
            this.selectedBid = selectedBid;
        }

        private void UpdatePaymentMethod() {
            if(rbVirmanPayment.IsChecked == true) {
                gbPayment.Header = "Virmansko plaćanje";
                gbPayment.Visibility = Visibility.Visible;
                ccPayment.Content = new VirmanPaymentUserControl(navigationControl, auction, selectedBid);
            }else if (rbCardPayment.IsChecked == true) {
                gbPayment.Header = "Kartično plaćanje";
                gbPayment.Visibility = Visibility.Visible;
                ccPayment.Content = new CardPaymentUserControl(navigationControl, auction, selectedBid);
            } else {
                gbPayment.Visibility = Visibility.Collapsed;
                ccPayment.Content = null;
            }
        }

        private void rbVirmanPayment_Checked(object sender, RoutedEventArgs e) {
            UpdatePaymentMethod();
        }

        private void rbVirmanPayment_Unchecked(object sender, RoutedEventArgs e) {
            UpdatePaymentMethod();
        }

        private void rbCardPayment_Checked(object sender, RoutedEventArgs e) {
            UpdatePaymentMethod();
        }

        private void rbCardPayment_Unchecked(object sender, RoutedEventArgs e) {
            UpdatePaymentMethod();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e) {
            UpdatePaymentMethod();
        }

    }
}
