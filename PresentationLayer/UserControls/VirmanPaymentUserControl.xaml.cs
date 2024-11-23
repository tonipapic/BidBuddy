using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Services;
using EntitiesLayer.Entities;
using EntitiesLayer.Models;
using PresentationLayer.Helpers;
using PresentationLayer.Navigation;
using System;
using System.Drawing;
using System.Windows.Controls;
using ZXing;

namespace PresentationLayer.UserControls {
    /// <summary>
    /// Interaction logic for VirmanPaymentUserControl.xaml
    /// <remark>Josip Mojzeš</remark>
    /// </summary>
    public partial class VirmanPaymentUserControl : UserControl {

        private NavigationControl navigationControl;
        private Auction auction;
        private AuctionBid selectedBid;

        public VirmanPaymentUserControl(NavigationControl navigationControl, Auction auction, AuctionBid selectedBid) {
            InitializeComponent();
            this.navigationControl = navigationControl;
            this.auction = auction;
            this.selectedBid = selectedBid;
        }

        private void ShowPaymentInfo() {

            PaymentService paymentService = new PaymentService();
            VirmanPaymentInfo info = paymentService.GetVirmanPaymentInfoForAuction(auction, selectedBid);

            txtReceiver.Text = info.ReceiverName;
            txtIban.Text = info.ReceiverIBAN;

            txtAmount.Text = $"{info.Amount} €";
            txtDescription.Text = info.Description;

            try {
                string barcodeContent = paymentService.GenerateHUB3PaymentContent(info);
                GenerateBarcode(barcodeContent);
            } catch (HUB3PaymentException e) {
                MessageBoxes.ShowError("Greška", e.Message);
            } catch (WriterException e) {
                MessageBoxes.ShowError("Nepoznata greška" + "\n" + e.Message, "Greška");
            } catch(Exception e) {
                MessageBoxes.ShowError("Nepoznata greška" + "\n" + e.Message, "Greška");
            }

        }

        private void GenerateBarcode(string content) {

            BarcodeGenerator barcodeGenerator = new BarcodeGenerator();
            BitmapImageHelper bitmapImageHelper = new BitmapImageHelper();

            Size barcodeSize = new Size(600, 200);

            using (Bitmap barcode = barcodeGenerator.GeneratePdf417(content, barcodeSize)) {
                imgBarcode.Width = barcodeSize.Width;
                imgBarcode.Height = barcodeSize.Height;
                imgBarcode.Source = bitmapImageHelper.ConvertToBitmapImage(barcode);
            }

        }

        private void UserControl_Loaded(object sender, System.Windows.RoutedEventArgs e) {
            ShowPaymentInfo();
        }
    }
}
