using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Services;
using EntitiesLayer.Entities;
using EntitiesLayer.Models;
using PresentationLayer.Helpers;
using PresentationLayer.Navigation;
using System.Windows.Controls;

namespace PresentationLayer.UserControls {
    /// <summary>
    /// Interaction logic for CardPaymentUserControl.xaml
    /// <remark>Josip Mojzeš</remark>
    /// </summary>
    public partial class CardPaymentUserControl : UserControl {

        private NavigationControl navigationControl;
        private Auction auction;
        private AuctionBid selectedBid;

        public CardPaymentUserControl(NavigationControl navigationControl, Auction auction, AuctionBid selectedBid) {
            InitializeComponent();
            this.navigationControl = navigationControl;
            this.auction = auction;
            this.selectedBid = selectedBid;
        }

        private void Pay() {

            try {

                CardPaymentInfo info = new CardPaymentInfo();

                string firstName = txtFirstName.Text.Trim();
                string lastName = txtLastName.Text.Trim();
                string address = txtAddress.Text.Trim();

                string postalCode = txtPostalCode.Text.Trim();
                string city = txtCity.Text.Trim();
                string country = txtCountry.Text.Trim();

                string cardNumber = txtCardNumber.Text.Trim();
                string cardExpireDate = txtExpireDate.Text.Trim();
                string cardCvvText = txtCvv.Text.Trim();
                
                if (firstName == "") throw new InvalidInputException("Unesite ime!");
                if (lastName == "") throw new InvalidInputException("Unesite prezime!");
                if (address == "") throw new InvalidInputException("Unesite adresu!");
                if (postalCode == "") throw new InvalidInputException("Unesite poštanski broj!");
                if (city == "") throw new InvalidInputException("Unesite mjesto stanovanja!");
                if (country == "") throw new InvalidInputException("Unesite državu!");

                if (cardNumber == "") throw new InvalidInputException("Unesite broj kartice!");
                if (cardExpireDate == "") throw new InvalidInputException("Unesite datum isteka kartice!");
                if (cardCvvText == "") throw new InvalidInputException("Unesite CVV kartice!");

                if(int.TryParse(cardCvvText, out int cardCvv) == false) {
                    throw new InvalidInputException("CVV kartice treba biti broj!");
                }

                info.FirstName = firstName;
                info.LastName = lastName;
                info.Address = address;
                info.PostalCode = postalCode;
                info.City = city;
                info.Country = country;

                info.CardNumber = cardNumber;
                info.ExpireDate = cardExpireDate;
                info.CVV = cardCvv;

                PaymentService paymentService = new PaymentService();
                paymentService.MakeCardPayment(auction, selectedBid, info);

                MessageBoxes.ShowInfo("Uspješno plaćanje", "Uspješno ste izvršili plaćanje.");
                navigationControl.PopPage();

            } catch (InvalidInputException e) {
                MessageBoxes.ShowWarning("Plaćanje", e.Message);
            } catch (CardPaymentException e) {
                MessageBoxes.ShowWarning("Plaćanje nije uspjelo", e.Message);
            }

        }

        private void btnPay_Click(object sender, System.Windows.RoutedEventArgs e) {
            Pay();
        }
    }
}
