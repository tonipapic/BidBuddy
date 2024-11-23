using BusinessLogicLayer.Exceptions;
using BusinessLogicLayer.Payment;
using EntitiesLayer.Entities;
using EntitiesLayer.Models;

namespace BusinessLogicLayer.Services {

    /// <summary>
    /// <remark>Josip Mojzeš</remark>
    /// </summary>
    public class PaymentService {

        /// <summary>
        /// Try to make a card payment for the auction.
        /// </summary>
        /// <param name="auction"></param>
        /// <param name="selectedBid"></param>
        /// <param name="info"></param>
        /// <exception cref="PaymentException"></exception>
        public void MakeCardPayment(Auction auction, AuctionBid selectedBid, CardPaymentInfo info) {

            if (auction.AuctionStateId != AuctionState.PaymentProcessing) {
                throw new PaymentException("Za plaćanje, aukcija mora biti u stanju obrade uplate!");
            }

            IBank bank = new FakeBank();

            // throws CardPaymentException in case of unsuccessful payment
            bank.MakeCardPayment(auction, selectedBid, info);

            // card payment was successfully completed
            ConfirmPayment(auction, selectedBid);

        }

        /// <summary>
        /// Confirms payment for given auction.
        /// Also, sets and updates auction state to InDelivery.
        /// Also, generates receipt for successful payment.
        /// </summary>
        /// <param name="auction"></param>
        /// <param name="selectedBid"></param>
        /// <exception cref="PaymentException"></exception>
        public void ConfirmPayment(Auction auction, AuctionBid selectedBid) {

            if(auction.AuctionStateId != AuctionState.PaymentProcessing) {
                throw new PaymentException("Za potvrdu plaćanja, aukcija mora biti u stanju obrade uplate!");
            }

            AuctionService auctionService = new AuctionService();
            ReceiptService receiptService = new ReceiptService();

            // generate receipt
            receiptService.GenerateReceiptForAuction(auction, selectedBid);

            // udpate auction state to InDelivery
            auction.AuctionStateId = AuctionState.InDelivery;
            auctionService.UpdateAuction(auction);

        }

        /// <summary>
        /// Generates informations used to virman payment.
        /// </summary>
        /// <param name="auction"></param>
        /// <param name="selectedBid"></param>
        /// <returns></returns>
        public VirmanPaymentInfo GetVirmanPaymentInfoForAuction(Auction auction, AuctionBid selectedBid) {

            VirmanPaymentInfo info = new VirmanPaymentInfo();
            User auctionCreator = auction.User;

            info.ReceiverName = auctionCreator.FullName;
            info.ReceiverIBAN = auctionCreator.IBAN;

            info.Amount = selectedBid.Value;
            info.Currency = "EUR";
            info.Description = $"BidBuddy plaćanje aukcije {auction.AuctionId}";

            return info;
        }

        /// <summary>
        /// Generates content for HUB3 payment format.
        /// Returns a string that can be used as a PDF417 barcode payload.
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public string GenerateHUB3PaymentContent(VirmanPaymentInfo info) {

            var builder = new HUB3PaymentBuilder()
                .SetRecipientName(info.ReceiverName)
                .SetRecipientIBAN(info.ReceiverIBAN)
                .SetAmount(info.Amount)
                .SetCurrency(info.Currency)
                .SetPaymentDescription(info.Description);

            return builder.Build();
        }

    }
}
